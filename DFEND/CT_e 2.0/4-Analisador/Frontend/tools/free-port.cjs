/**
 * Libera uma porta TCP local (Windows/Linux) antes do ng serve.
 * Uso: node free-port.cjs 4200
 * Evita o prompt "Port already in use. Would you like to use a different port?"
 */
const { execSync, spawnSync } = require('child_process');

function sleepSync(ms) {
  const end = Date.now() + ms;
  while (Date.now() < end) {
    /* busy wait — script curto, sem dependências */
  }
}

function pidsListeningOnPort(p) {
  const pids = new Set();
  if (process.platform === 'win32') {
    let out = '';
    try {
      out = execSync('netstat -ano -p tcp', { encoding: 'utf8', windowsHide: true });
    } catch {
      return pids;
    }
    // Evita casar :4200 com :42001 — exige fim de token após a porta
    const re = new RegExp(`:${p}(?:\\s|$)`, 'i');
    for (const line of out.split(/\r?\n/)) {
      if (!/LISTENING/i.test(line) || !re.test(line)) continue;
      const m = line.trim().match(/(\d+)\s*$/);
      if (m) pids.add(Number(m[1]));
    }
    return pids;
  }

  try {
    const out = execSync(`lsof -tiTCP:${p} -sTCP:LISTEN`, {
      encoding: 'utf8',
      stdio: ['ignore', 'pipe', 'ignore'],
    });
    for (const part of out.split(/\s+/)) {
      const n = Number(part.trim());
      if (n > 0) pids.add(n);
    }
  } catch {
    /* nada escutando */
  }
  return pids;
}

function killPid(pid) {
  if (!pid || pid === process.pid || pid === process.ppid) {
    return;
  }
  if (process.platform === 'win32') {
    spawnSync('taskkill', ['/PID', String(pid), '/T', '/F'], {
      stdio: 'ignore',
      windowsHide: true,
    });
    return;
  }
  try {
    process.kill(pid, 'SIGKILL');
  } catch {
    /* already gone */
  }
}

function freePort(p, { quiet = false } = {}) {
  let killed = [];
  for (let attempt = 0; attempt < 3; attempt++) {
    const pids = [...pidsListeningOnPort(p)].filter((x) => x > 0);
    if (pids.length === 0) {
      break;
    }
    for (const pid of pids) {
      killPid(pid);
      killed.push(pid);
    }
    sleepSync(400);
  }
  const still = [...pidsListeningOnPort(p)];
  if (!quiet) {
    if (killed.length) {
      console.log(
        `[free-port] porta ${p}: encerrado pid(s) ${[...new Set(killed)].join(', ')}` +
          (still.length ? ` — ainda ocupada por ${still.join(', ')}` : ' — livre')
      );
    } else {
      console.log(`[free-port] porta ${p}: já livre`);
    }
  }
  return still.length === 0;
}

module.exports = { freePort, pidsListeningOnPort };

if (require.main === module) {
  const port = Number(process.argv[2]);
  if (!Number.isInteger(port) || port <= 0 || port > 65535) {
    console.error('[free-port] informe uma porta válida, ex: node free-port.cjs 4200');
    process.exit(1);
  }
  process.exit(freePort(port) ? 0 : 2);
}
