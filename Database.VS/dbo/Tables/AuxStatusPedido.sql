CREATE TABLE [dbo].[AuxStatusPedido] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Descricao] VARCHAR (20) NULL,
    [Icone]     VARCHAR (20) NULL,
    [Cor]       VARCHAR (10) NULL,
    CONSTRAINT [PK_AuxStatusPedido] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

