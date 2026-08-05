/**
 * Ao iniciar o front do Orquestrador, sobe também os Angular dos monitores
 * (1-Receptor :4200, 2-Arquivador :4210, N-* futuros).
 *
 * Windows: grava um .cmd que redireciona sozinho para o log e dispara com
 * spawn(windowsHide+detached+stdio ignore) — sem janela preta e sem depender
 * do fd do prestart (que morre antes do nx escrever).
 */
const { spawn, spawnSync } = require('child_process');
const http = require('http');
const fs = require('fs');
const path = require('path');
const { freePort, pidsListeningOnPort } = require('./free-port.cjs');

const frontendRoot = path.resolve(__dirname, '..');
const cteRoot = path.resolve(frontendRoot, '..', '..');
const isWin = process.platform === 'win32';

function sleep(ms) {
  return new Promise((r) => setTimeout(r, ms));
}

function httpUp(port) {
  return new Promise((resolve) => {
    const req = http.get(
      { host: '127.0.0.1', port, path: '/', timeout: 900 },
      (res) => {
        res.resume();
        resolve(true);
      }
    );
    req.on('error', () => resolve(false));
    req.on('timeout', () => {
      req.destroy();
      resolve(false);
    });
  });
}

function readServePort(frontDir) {
  const appsDir = path.join(frontDir, 'apps');
  if (!fs.existsSync(appsDir)) {
    return null;
  }
  for (const name of fs.readdirSync(appsDir)) {
    if (name.endsWith('-e2e')) continue;
    const pj = path.join(appsDir, name, 'project.json');
    if (!fs.existsSync(pj)) continue;
    try {
      const json = JSON.parse(fs.readFileSync(pj, 'utf8'));
      const port =
        json?.targets?.serve?.options?.port ??
        json?.targets?.serve?.configurations?.development?.port ??
        json?.targets?.['serve-static']?.options?.port;
      if (typeof port === 'number') {
        return port;
      }
    } catch {
      /* ignore */
    }
  }
  return null;
}

function discoverMonitorFronts() {
  if (!fs.existsSync(cteRoot)) {
    return [];
  }
  const list = [];
  for (const ent of fs.readdirSync(cteRoot, { withFileTypes: true })) {
    if (!ent.isDirectory()) continue;
    if (!/^\d+-/.test(ent.name)) continue;
    if (ent.name.startsWith('0-')) continue;
    const frontDir = path.join(cteRoot, ent.name, 'Frontend');
    if (!fs.existsSync(path.join(frontDir, 'package.json'))) continue;
    const port = readServePort(frontDir);
    if (!port) {
      console.warn(`[chain-fronts] sem porta em ${ent.name}/Frontend — pulando`);
      continue;
    }
    list.push({ id: ent.name, dir: frontDir, port });
  }
  list.sort((a, b) => a.id.localeCompare(b.id, 'en'));
  return list;
}

function readDefaultAppName(frontDir) {
  const appsDir = path.join(frontDir, 'apps');
  if (!fs.existsSync(appsDir)) {
    return 'cte-receptor';
  }
  const apps = fs
    .readdirSync(appsDir)
    .filter((n) => !n.endsWith('-e2e') && fs.existsSync(path.join(appsDir, n, 'project.json')));
  return apps[0] || 'cte-receptor';
}

function safeId(id) {
  return id.replace(/[^\w-]+/g, '_');
}

function logTail(logPath, maxChars = 800) {
  try {
    const text = fs.readFileSync(logPath, 'utf8');
    return text.length <= maxChars ? text : text.slice(-maxChars);
  } catch {
    return '(log ilegível)';
  }
}

/**
 * Garante node_modules íntegro no Frontend do monitor (igual Receptor/Arquivador).
 * Se faltar nx ou @angular/common (install interrompido), reinstala.
 */
function angularCommonOk(frontDir) {
  return fs.existsSync(
    path.join(frontDir, 'node_modules', '@angular', 'common', 'fesm2022', 'common.mjs')
  );
}

function removeDirSafe(dir) {
  try {
    fs.rmSync(dir, { recursive: true, force: true, maxRetries: 3 });
  } catch (err) {
    console.warn(`[chain-fronts] falha ao limpar ${dir}: ${err.message}`);
  }
}

function ensureNpmInstall(front) {
  const nxJs = path.join(front.dir, 'node_modules', 'nx', 'bin', 'nx.js');
  const nmDir = path.join(front.dir, 'node_modules');
  const healthy = fs.existsSync(nxJs) && angularCommonOk(front.dir);

  if (healthy) {
    return true;
  }

  if (fs.existsSync(nmDir) && !healthy) {
    console.warn(
      `[chain-fronts] ${front.id}: node_modules incompleto/corrompido — reinstalando`
    );
    removeDirSafe(nmDir);
  }

  console.log(
    `[chain-fronts] ${front.id}: instalando dependências (npm install)… pode levar alguns minutos`
  );
  const npmCmd = isWin ? 'npm.cmd' : 'npm';
  const result = spawnSync(npmCmd, ['install', '--no-fund', '--no-audit'], {
    cwd: front.dir,
    stdio: 'inherit',
    env: { ...process.env, CI: 'true', NG_CLI_ANALYTICS: 'false' },
    shell: isWin,
  });

  if (result.status !== 0 || !fs.existsSync(nxJs) || !angularCommonOk(front.dir)) {
    console.warn(
      `[chain-fronts] ${front.id}: npm install falhou — rode manualmente:\n` +
        `  cd "${front.dir}"\n` +
        `  rmdir /s /q node_modules\n` +
        `  npm.cmd install`
    );
    return false;
  }

  console.log(`[chain-fronts] ${front.id}: npm install OK`);
  return true;
}

/**
 * Sobe o nx serve independente do prestart.
 * O .cmd abre o log com >> — o parent pode sair sem matar a saída do nx.
 */
function startNpm(front) {
  const tempDir = process.env.TEMP || process.env.TMPDIR || '/tmp';
  const logPath = path.join(tempDir, `cte-front-${safeId(front.id)}.log`);
  const runnerPath = path.join(tempDir, `cte-run-${safeId(front.id)}.cmd`);
  const nxJs = path.join(front.dir, 'node_modules', 'nx', 'bin', 'nx.js');
  const appName = readDefaultAppName(front.dir);
  const nodeExe = process.execPath;

  fs.writeFileSync(
    logPath,
    `=== cte chain-fronts ${front.id} @ ${new Date().toISOString()} ===\r\n` +
      `dir=${front.dir}\r\nport=${front.port}\r\nnx=${fs.existsSync(nxJs)}\r\nnode=${nodeExe}\r\n\r\n`,
    'utf8'
  );

  if (!fs.existsSync(nxJs)) {
    console.warn(
      `[chain-fronts] ${front.id}: nx.js ausente após ensure — pulando`
    );
    return { logPath, ok: false };
  }

  if (isWin) {
    // Echoes vão para o log ANTES do nx — se só o header aparecer, o .cmd nem rodou.
    const runner = [
      '@echo off',
      `cd /d "${front.dir}"`,
      'if errorlevel 1 (',
      `  echo [runner] FALHA cd >> "${logPath}"`,
      '  exit /b 1',
      ')',
      'set CI=true',
      'set NG_CLI_ANALYTICS=false',
      `echo [runner] start %DATE% %TIME% >> "${logPath}"`,
      `echo [runner] cwd=%CD% >> "${logPath}"`,
      `echo [runner] cmd="${nodeExe}" "${nxJs}" serve ${appName} --port=${front.port} >> "${logPath}"`,
      `"${nodeExe}" "${nxJs}" serve ${appName} --port=${front.port} >> "${logPath}" 2>&1`,
      `echo [runner] nx exit=%ERRORLEVEL% at %TIME% >> "${logPath}"`,
      '',
    ].join('\r\n');
    fs.writeFileSync(runnerPath, runner, 'utf8');

    // Dispara o .cmd direto (não PowerShell ArgumentList — era o bug do log vazio).
    const child = spawn('cmd.exe', ['/d', '/c', 'call', runnerPath], {
      cwd: front.dir,
      detached: true,
      stdio: 'ignore',
      windowsHide: true,
      env: {
        ...process.env,
        CI: 'true',
        NG_CLI_ANALYTICS: 'false',
      },
    });
    child.on('error', (err) => {
      try {
        fs.appendFileSync(logPath, `[spawn error] ${err.message}\r\n`);
      } catch {
        /* ignore */
      }
    });
    child.unref();
  } else {
    const child = spawn(
      'bash',
      [
        '-lc',
        `cd "${front.dir}" && echo "[runner] start $(date)" >> "${logPath}" && CI=true NG_CLI_ANALYTICS=false "${nodeExe}" "${nxJs}" serve ${appName} --port=${front.port} >> "${logPath}" 2>&1; echo "[runner] exit=$?" >> "${logPath}"`,
      ],
      {
        detached: true,
        stdio: 'ignore',
        env: { ...process.env, CI: 'true', NG_CLI_ANALYTICS: 'false' },
      }
    );
    child.unref();
  }

  console.log(
    `[chain-fronts] iniciando ${front.id} → http://localhost:${front.port} (log: ${logPath})`
  );
  return { logPath, ok: true };
}

async function main() {
  console.log(`[chain-fronts] CT_e: ${cteRoot}`);
  const fronts = discoverMonitorFronts();
  if (fronts.length === 0) {
    console.warn('[chain-fronts] nenhum Frontend de monitor encontrado');
    return;
  }

  const started = [];

  for (const front of fronts) {
    // Já respondendo HTTP → mantém (não derruba Angular só para subir de novo).
    if (await httpUp(front.port)) {
      console.log(
        `[chain-fronts] já online: ${front.id} http://localhost:${front.port}`
      );
      continue;
    }

    // Porta ocupada sem HTTP (órfão / compile travado) → libera forçado e sobe.
    const listeners = pidsListeningOnPort(front.port);
    if (listeners.size > 0) {
      console.log(
        `[chain-fronts] ${front.id}: porta ${front.port} ocupada sem resposta` +
          ` (pid ${[...listeners].join(',')}) — encerrando forçado`
      );
    }
    freePort(front.port);

    if (!ensureNpmInstall(front)) {
      continue;
    }
    const result = startNpm(front);
    if (result?.ok) {
      started.push(front);
    }
  }

  // Confirma em ~4s se o runner escreveu no log (senão o spawn falhou de novo).
  if (started.length > 0) {
    await sleep(4000);
    for (const front of started) {
      const logPath = path.join(
        process.env.TEMP || process.env.TMPDIR || '/tmp',
        `cte-front-${safeId(front.id)}.log`
      );
      const body = logTail(logPath, 1200);
      if (!body.includes('[runner]')) {
        console.warn(
          `[chain-fronts] ${front.id}: runner não escreveu no log — spawn falhou. Veja ${logPath}`
        );
      } else if (await httpUp(front.port)) {
        console.log(
          `[chain-fronts] ${front.id} já respondendo em http://localhost:${front.port}`
        );
      } else {
        console.log(
          `[chain-fronts] ${front.id} compilando em background (aguarde 1–3 min)…`
        );
      }
    }
  }
}

main().catch((err) => {
  console.error('[chain-fronts]', err);
  process.exitCode = 0;
});
