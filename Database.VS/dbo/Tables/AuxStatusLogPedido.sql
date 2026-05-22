CREATE TABLE [dbo].[AuxStatusLogPedido] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Descricao] VARCHAR (20) NULL,
    [Icone]     INT          NULL,
    [Cor]       VARCHAR (10) NULL,
    CONSTRAINT [PK_AuxStatusLogPedido] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

