CREATE   PROCEDURE [dbo].[PedidosGet]
@orderby VARCHAR(MAX), 
@Id Int, 
@DataCriacao_from DateTime2(0),
@DataCriacao_to DateTime2(0),
@regporpag INT, 
@pag INT, 
@qtdtotal INT OUTPUT 
AS 
BEGIN 
	DECLARE @sqlColumn   NVARCHAR(MAX)
	DECLARE @sqlTable    NVARCHAR(MAX)
	DECLARE @sqlWhere     NVARCHAR(MAX)
	DECLARE @sqlOrderBy    NVARCHAR(MAX)
	DECLARE @sqlOffset   NVARCHAR(MAX)
	DECLARE @query       NVARCHAR(MAX)
	DECLARE @count       INT
	--COLUMNS
	SET @sqlColumn = '
		[Id], 
		[CodClient], 
		[Status], 
		[Total], 
		[DataCriacao], 
		[Anexo], 
		[ObservacaoNF]
	 '
	--TABLES
	SET @sqlTable = 'FROM [dbo].[Pedidos] WITH (NOLOCK)'
	--CONDITIONALS
	SET @sqlWhere = ' WHERE 1=1 '
	IF @Id IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND [Id] = @Id'
	IF @DataCriacao_from IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND CONVERT(DATE, [DataCriacao]) BETWEEN CONVERT(VARCHAR(10), @DataCriacao_from, 112) AND CONVERT(VARCHAR(10), @DataCriacao_to, 112) '
	--ORDER BY
	SET @sqlOrderBy  = ' ORDER BY [DataCriacao] DESC'
	IF @orderby IS NOT NULL AND @orderby <> ''
	BEGIN
		SET @sqlOrderBy  = ' ORDER BY ' + @orderby
	END
	--PAGINATION
	IF @pag < 1
		SET @pag = 1
	SET @sqlOffset = ' '
	SET @sqlOffset = @sqlOffset + ' OFFSET ('
	SET @sqlOffset = @sqlOffset + '(@pag - 1)'
	SET @sqlOffset = @sqlOffset + ' * '
	SET @sqlOffset = @sqlOffset + '@regporpag'
	SET @sqlOffset = @sqlOffset + ') ROWS FETCH NEXT '
	SET @sqlOffset = @sqlOffset + '@regporpag'
	SET @sqlOffset = @sqlOffset + ' ROWS ONLY '
	--TOTAL OF RECORDS
	IF @qtdtotal is null or @qtdtotal = 0
	BEGIN
		SET @qtdtotal = 0;
		SET @query = N'SELECT @count = COUNT(*) ' + @sqlTable + @sqlWhere
		EXECUTE sp_executesql @query,
		N'@Id Int, 
				@DataCriacao_from DateTime2(0),
		@DataCriacao_to DateTime2(0),
		@count int output',
		@Id,
		@DataCriacao_from,
		@DataCriacao_to,
		@count = @qtdtotal output
	END
	--DATASET RESULT
	SET @query = N'SELECT ' + @sqlColumn + @sqlTable + @sqlWhere + @sqlOrderBy + @sqlOffset
	EXECUTE sp_executesql @query,
	N'@Id Int, 
		@DataCriacao_from DateTime2(0),
	@DataCriacao_to DateTime2(0),
	@regporpag INT, 
	@pag INT, 
	@qtdtotal INT OUTPUT ',
	
	@Id,
	@DataCriacao_from,
	@DataCriacao_to,
	@regporpag,
	@pag,
	@qtdtotal
	
END