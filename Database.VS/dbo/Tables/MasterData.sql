CREATE TABLE [dbo].[MasterData] (
    [type]      VARCHAR (1)    NOT NULL,
    [name]      NVARCHAR (64)  NOT NULL,
    [tablename] NVARCHAR (MAX) NULL,
    [json]      NVARCHAR (MAX) NULL,
    [info]      NVARCHAR (150) NULL,
    [owner]     NVARCHAR (64)  NULL,
    [modified]  DATETIME       NOT NULL,
    [sync]      BIT            NULL,
    CONSTRAINT [PK_MasterData] PRIMARY KEY NONCLUSTERED ([type] ASC, [name] ASC)
);

