-- 001_create_resgate_tables.sql
-- Feature: resgate-cte-an-orquestrador
-- LEGADO: modo atual do Resgate NÃO usa estas tabelas.
-- Resgate informa chaves via cte.tmp_integracao_… + fila_alvo_cte_integrador (Download/Carga).
-- Script mantido só por histórico / rollback do desenho antigo com lote próprio.
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'cte')
    EXEC('CREATE SCHEMA cte');
GO

IF OBJECT_ID('cte.lote_resgate_cte', 'U') IS NULL
BEGIN
    CREATE TABLE cte.lote_resgate_cte (
        id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        usuario NVARCHAR(100) NOT NULL,
        criado_em DATETIME2 NOT NULL CONSTRAINT DF_lote_resgate_criado DEFAULT SYSUTCDATETIME(),
        status NVARCHAR(40) NOT NULL,
        total INT NOT NULL,
        recuperados INT NOT NULL CONSTRAINT DF_lote_rec DEFAULT 0,
        existentes INT NOT NULL CONSTRAINT DF_lote_exi DEFAULT 0,
        nao_localizados INT NOT NULL CONSTRAINT DF_lote_nao DEFAULT 0,
        erros INT NOT NULL CONSTRAINT DF_lote_err DEFAULT 0,
        chave_atual CHAR(44) NULL,
        passo_atual_lote NVARCHAR(10) NULL,
        correlation_id UNIQUEIDENTIFIER NOT NULL
    );
END
GO

IF OBJECT_ID('cte.item_resgate_cte', 'U') IS NULL
BEGIN
    CREATE TABLE cte.item_resgate_cte (
        id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        lote_id BIGINT NOT NULL,
        chave CHAR(44) NOT NULL,
        status NVARCHAR(40) NOT NULL,
        passo_atual NVARCHAR(10) NOT NULL,
        motivo NVARCHAR(500) NULL,
        tentativas INT NOT NULL CONSTRAINT DF_item_tent DEFAULT 0,
        atualizado_em DATETIME2 NOT NULL CONSTRAINT DF_item_upd DEFAULT SYSUTCDATETIME(),
        tempo_ms INT NULL,
        CONSTRAINT FK_item_lote FOREIGN KEY (lote_id) REFERENCES cte.lote_resgate_cte(id),
        CONSTRAINT UQ_item_lote_chave UNIQUE (lote_id, chave)
    );
    CREATE INDEX IX_item_status ON cte.item_resgate_cte(status, id);
END
GO

IF OBJECT_ID('cte.evento_resgate_cte', 'U') IS NULL
BEGIN
    CREATE TABLE cte.evento_resgate_cte (
        id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        lote_id BIGINT NOT NULL,
        item_id BIGINT NULL,
        horario DATETIME2 NOT NULL CONSTRAINT DF_evt_hor DEFAULT SYSUTCDATETIME(),
        mensagem NVARCHAR(1000) NOT NULL,
        passo NVARCHAR(10) NULL,
        CONSTRAINT FK_evt_lote FOREIGN KEY (lote_id) REFERENCES cte.lote_resgate_cte(id)
    );
    CREATE INDEX IX_evt_lote ON cte.evento_resgate_cte(lote_id, id DESC);
END
GO
