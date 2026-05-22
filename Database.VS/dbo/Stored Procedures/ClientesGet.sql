CREATE PROCEDURE [dbo].[ClientesGet]
    @orderby    VARCHAR(MAX),
    @Id         INT,
    @NomeCliente VARCHAR(100),
    @NumCPF     VARCHAR(14),
    @regporpag  INT,
    @pag        INT,
    @qtdtotal   INT OUTPUT
AS
BEGIN

    DECLARE @sqlColumn  NVARCHAR(MAX)
    DECLARE @sqlTable   NVARCHAR(MAX)
    DECLARE @sqlWhere   NVARCHAR(MAX)
    DECLARE @sqlOrderBy NVARCHAR(MAX)
    DECLARE @sqlOffset  NVARCHAR(MAX)
    DECLARE @query      NVARCHAR(MAX)

    -- CASE inline: sem chamada de função extra por linha
    SET @sqlColumn = '
        [Id],
        [NomeAbreviado],
        [NomeCliente],
        [NumCPF],
        [UpdatedAt],
        [Status]
    '

    SET @sqlTable = 'FROM [dbo].[Clientes] WITH (NOLOCK)'

    SET @sqlWhere = ' WHERE 1=1 '
    IF @Id IS NOT NULL
        SET @sqlWhere = @sqlWhere + ' AND [Id] = @Id'
    IF @NomeCliente IS NOT NULL
        SET @sqlWhere = @sqlWhere + ' AND [NomeCliente] LIKE ''%'' + RTRIM(@NomeCliente) + ''%'' '
    IF @NumCPF IS NOT NULL
        SET @sqlWhere = @sqlWhere + ' AND [NumCPF] LIKE ''%'' + RTRIM(@NumCPF) + ''%'' '

    SET @sqlOrderBy = ' ORDER BY [Id]'
    IF @orderby IS NOT NULL AND @orderby <> ''
        SET @sqlOrderBy = ' ORDER BY ' + @orderby

    IF @pag < 1
        SET @pag = 1

    SET @sqlOffset =
        ' OFFSET ((@pag - 1) * @regporpag) ROWS FETCH NEXT @regporpag ROWS ONLY '

    -- Contagem total
    IF @qtdtotal IS NULL OR @qtdtotal = 0
    BEGIN
        SET @qtdtotal = 0
        SET @query = N'SELECT @count = COUNT(*) ' + @sqlTable + @sqlWhere

        EXECUTE sp_executesql @query,
            N'@Id INT, @NomeCliente VARCHAR(100), @NumCPF VARCHAR(14), @count INT OUTPUT',
            @Id, @NomeCliente, @NumCPF,
            @count = @qtdtotal OUTPUT
    END

    -- Resultado paginado
    SET @query = N'SELECT ' + @sqlColumn + @sqlTable + @sqlWhere + @sqlOrderBy + @sqlOffset

    EXECUTE sp_executesql @query,
        N'@Id INT, @NomeCliente VARCHAR(100), @NumCPF VARCHAR(14), @regporpag INT, @pag INT, @qtdtotal INT OUTPUT',
        @Id, @NomeCliente, @NumCPF, @regporpag, @pag, @qtdtotal

END