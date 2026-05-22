CREATE   PROCEDURE [dbo].[PedidoItemGet]
@orderby VARCHAR(MAX), 
@Id Int, 
@PedidoId Int, 
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
		[PedidoId], 
		[ItemId], 
		[PesoBruto], 
		[PesoLiquido], 
		[Quantidade], 
		[ValorUnit], 
		[ValorTotal], 
		[Status]
	 '
	--TABLES
	SET @sqlTable = 'FROM [dbo].[PedidoItem] WITH (NOLOCK)'
	--CONDITIONALS
	SET @sqlWhere = ' WHERE 1=1 '
	IF @Id IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND [Id] = @Id'
	IF @PedidoId IS NOT NULL
		SET @sqlWhere = @sqlWhere + ' AND [PedidoId] = @PedidoId'
	--ORDER BY
	SET @sqlOrderBy  = ' ORDER BY [Id]'
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
		@PedidoId Int, 
		@count int output',
		@Id,
		@PedidoId,
		@count = @qtdtotal output
	END
	--DATASET RESULT
	SET @query = N'SELECT ' + @sqlColumn + @sqlTable + @sqlWhere + @sqlOrderBy + @sqlOffset
	EXECUTE sp_executesql @query,
	N'@Id Int, 
	@PedidoId Int, 
	@regporpag INT, 
	@pag INT, 
	@qtdtotal INT OUTPUT ',
	
	@Id,
	@PedidoId,
	@regporpag,
	@pag,
	@qtdtotal
	
END