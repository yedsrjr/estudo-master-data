CREATE FUNCTION dbo.obter_peso_item
(
    @ItemId    NVARCHAR(50),
    @TipoPeso  NVARCHAR(20),  -- 'BRUTO' ou 'LIQUIDO'
    @Quantidade DECIMAL(10, 3)
)
RETURNS DECIMAL(10, 3)
AS
BEGIN
    DECLARE @Peso DECIMAL(10, 3);

    SELECT @Peso = CASE @TipoPeso
                       WHEN 'BRUTO'   THEN PesoBruto
                       WHEN 'LIQUIDO' THEN PesoLiquido
                   END
    FROM [Produtos]
    WHERE CodSku = @ItemId;

    RETURN @Peso * @Quantidade;
END;