CREATE TABLE [dbo].[LogPedido] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [IdPedido]    INT           NULL,
    [Status]      INT           NULL,
    [DataCriacao] DATETIME2 (7) NULL,
    [Observacao]  VARCHAR (200) NULL,
    CONSTRAINT [PK_LogPedido] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LogPedido_Pedidos] FOREIGN KEY ([IdPedido]) REFERENCES [dbo].[Pedidos] ([Id])
);

