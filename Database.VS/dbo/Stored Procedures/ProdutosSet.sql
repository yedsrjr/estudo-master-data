CREATE   PROCEDURE [dbo].[ProdutosSet]
@action varchar(1), 
@CodSku Int, 
@Descricao Varchar(100) = NULL, 
@PesoBruto Decimal(10,3) = NULL, 
@PesoLiquido Decimal(10,3) = NULL, 
@Quantidade Int = NULL, 
@Status Varchar(20) = NULL, 
@RET INT OUTPUT 
AS 
BEGIN 
	DECLARE @TYPEACTION VARCHAR(1) 
	SET @TYPEACTION = @action 
	IF @TYPEACTION = ' ' 
	BEGIN 
		SET @TYPEACTION = 'I' 
		DECLARE @NCOUNT INT 
 
		SELECT @NCOUNT = COUNT(*) 
		FROM [dbo].[Produtos] WITH (NOLOCK) 
		WHERE [CodSku] = @CodSku
 
		IF @NCOUNT > 0 
		BEGIN 
			SET @TYPEACTION = 'A'
		END 
	END 
 
	IF @TYPEACTION = 'I' 
	BEGIN 
		INSERT INTO [dbo].[Produtos] (
			[Descricao],
			[PesoBruto],
			[PesoLiquido],
			[Quantidade],
			[Status])
		OUTPUT Inserted.CodSku
		VALUES (
			@Descricao,
			@PesoBruto,
			@PesoLiquido,
			@Quantidade,
			@Status)
		SET @RET = 0; 
	END 
	ELSE IF @TYPEACTION = 'A' 
	BEGIN 
		UPDATE [dbo].[Produtos] SET 
			[Descricao] = @Descricao, 
			[PesoBruto] = @PesoBruto, 
			[PesoLiquido] = @PesoLiquido, 
			[Quantidade] = @Quantidade, 
			[Status] = @Status
		WHERE [CodSku] = @CodSku
		SET @RET = 1; 
	END 
	ELSE IF @TYPEACTION = 'E' 
	BEGIN 
		DELETE FROM [dbo].[Produtos] 
		WHERE [CodSku] = @CodSku
		SET @RET = 2; 
	END 
 
END 
