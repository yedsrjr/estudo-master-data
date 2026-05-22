CREATE TABLE [dbo].[TabelaPrecos] (
    [CodClient]    INT             NOT NULL,
    [CodProduto]   INT             NOT NULL,
    [ValorUnit]    DECIMAL (10, 2) NULL,
    [DataInclusao] DATETIME2 (0)   NULL,
    [Id]           INT             IDENTITY (1, 1) NOT NULL
);

