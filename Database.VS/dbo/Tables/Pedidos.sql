CREATE TABLE [dbo].[Pedidos] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [DataCriacao]  DATETIME2 (0)   NULL,
    [CodClient]    INT             NULL,
    [Total]        DECIMAL (15, 2) NULL,
    [Status]       VARCHAR (3)     NULL,
    [Anexo]        VARCHAR (100)   NULL,
    [ObservacaoNF] VARCHAR (200)   NULL,
    CONSTRAINT [PK_Pedidos] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

