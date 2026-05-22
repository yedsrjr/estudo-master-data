CREATE TABLE [dbo].[PedidoItem] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [ItemId]      INT             NOT NULL,
    [PedidoId]    INT             NULL,
    [Quantidade]  INT             NULL,
    [PesoBruto]   DECIMAL (10, 3) NULL,
    [PesoLiquido] DECIMAL (10, 3) NULL,
    [ValorUnit]   DECIMAL (10, 2) NULL,
    [ValorTotal]  DECIMAL (10, 2) NULL,
    [Status]      INT             NULL,
    CONSTRAINT [PK_PedidoItem] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PedidoItem_Pedidos] FOREIGN KEY ([PedidoId]) REFERENCES [dbo].[Pedidos] ([Id])
);

