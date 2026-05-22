CREATE TABLE [dbo].[tb_masterdata_auditlog] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [dictionary] NVARCHAR (64) NOT NULL,
    [actionType] INT           NOT NULL,
    [modified]   DATETIME      NOT NULL,
    [userId]     VARCHAR (30)  NULL,
    [ip]         VARCHAR (45)  NULL,
    [browser]    VARCHAR (100) NULL,
    [origin]     INT           NOT NULL,
    [recordKey]  VARCHAR (100) NOT NULL,
    [json]       TEXT          NULL,
    CONSTRAINT [PK_tb_masterdata_auditlog] PRIMARY KEY NONCLUSTERED ([id] ASC)
);

