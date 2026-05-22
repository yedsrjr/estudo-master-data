CREATE   PROCEDURE [dbo].[SP_COUNT_RECORD]
    @TABLE SYSNAME
AS
BEGIN
    SET NOCOUNT ON;

    IF @TABLE NOT IN ('Clientes', 'Produtos', 'Pedidos')
    BEGIN
        RAISERROR('Tabela inválida.', 16, 1);
        RETURN;
    END

    DECLARE @sql NVARCHAR(MAX);

    -- Tabela que filtra por 'PEE'
    IF @TABLE = 'Pedidos'
    BEGIN
        SET @sql = N'SELECT COUNT(*) FROM dbo.' + QUOTENAME(@TABLE) +
                   N' WHERE [Status] = 1';
    END
    ELSE
    BEGIN
        -- Demais tabelas filtram por Status = 1
        SET @sql = N'SELECT COUNT(*) FROM dbo.' + QUOTENAME(@TABLE) +
                   N' WHERE [Status] = 1';
    END

    EXEC sp_executesql @sql;
END
