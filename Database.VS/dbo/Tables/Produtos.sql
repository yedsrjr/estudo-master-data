CREATE TABLE [dbo].[Produtos] (
    [CodSku]      INT             NOT NULL,
    [Descricao]   VARCHAR (100)   NULL,
    [PesoBruto]   DECIMAL (10, 3) NULL,
    [PesoLiquido] DECIMAL (10, 3) NULL,
    [Quantidade]  INT             NULL,
    [UpdatedAt]   DATETIME2 (7)   NULL,
    [Status]      VARCHAR (20)    NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY NONCLUSTERED ([CodSku] ASC)
);

