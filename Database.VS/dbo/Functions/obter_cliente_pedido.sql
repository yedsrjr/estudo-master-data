CREATE   FUNCTION dbo.obter_cliente_pedido(@p_pedido INT)
RETURNS VARCHAR(50)
AS
BEGIN
    DECLARE @v_CodCliente VARCHAR(50);

    SELECT @v_CodCliente = CodClient
    FROM [JJMasterData].[dbo].[Pedidos]
    WHERE Id = @p_pedido;

    RETURN @v_CodCliente;
END;