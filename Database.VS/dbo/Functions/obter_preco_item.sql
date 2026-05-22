CREATE FUNCTION dbo.obter_preco_item
(
    @ClienteId INT,
    @ItemId    NVARCHAR(50)
)
RETURNS DECIMAL(10, 2)
AS
BEGIN
    DECLARE @ValorUnit DECIMAL(10, 2);

    SELECT TOP 1
        @ValorUnit = ValorUnit
    FROM [TabelaPrecos]
    WHERE
        CodClient = @ClienteId
        AND CodProduto = @ItemId
    ORDER BY DataInclusao DESC;

    RETURN @ValorUnit;
END;