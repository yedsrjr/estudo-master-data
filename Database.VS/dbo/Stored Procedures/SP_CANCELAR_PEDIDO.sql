CREATE   PROCEDURE dbo.SP_CANCELAR_PEDIDO
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Valida se o pedido existe
    IF NOT EXISTS (SELECT 1 FROM dbo.Pedidos WHERE Id = @ID)
    BEGIN
        RAISERROR('Pedido não encontrado.', 16, 1);
        RETURN;
    END

    -- Valida se já está cancelado
    IF EXISTS (SELECT 1 FROM dbo.Pedidos WHERE Id = @ID AND Status = 3)
    BEGIN
        RAISERROR('Pedido já está cancelado.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

            UPDATE dbo.Pedidos
            SET Status = 3
            WHERE Id = @ID;

            UPDATE dbo.PedidoItem
            SET Status = 3
            WHERE PedidoId = @ID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        DECLARE @erro NVARCHAR(500) = ERROR_MESSAGE();
        RAISERROR(@erro, 16, 1);
    END CATCH
END
