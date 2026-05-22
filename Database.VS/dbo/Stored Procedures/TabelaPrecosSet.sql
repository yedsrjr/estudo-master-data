CREATE   PROCEDURE [dbo].[TabelaPrecosSet]
@action varchar(1), 
@Id Int, 
@CodClient Int, 
@CodProduto Int, 
@ValorUnit Decimal(10,2), 
@DataInclusao DateTime2(0) = NULL, 
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
		FROM [dbo].[TabelaPrecos] WITH (NOLOCK) 
		WHERE [Id] = @Id
 
		IF @NCOUNT > 0 
		BEGIN 
			SET @TYPEACTION = 'A'
		END 
	END 
 
	IF @TYPEACTION = 'I' 
	BEGIN 
		INSERT INTO [dbo].[TabelaPrecos] (
			[CodClient],
			[CodProduto],
			[ValorUnit],
			[DataInclusao])
		OUTPUT Inserted.Id
		VALUES (
			@CodClient,
			@CodProduto,
			@ValorUnit,
			@DataInclusao)
		SET @RET = 0; 
	END 
	ELSE IF @TYPEACTION = 'A' 
	BEGIN 
		UPDATE [dbo].[TabelaPrecos] SET 
			[CodClient] = @CodClient, 
			[CodProduto] = @CodProduto, 
			[ValorUnit] = @ValorUnit, 
			[DataInclusao] = @DataInclusao
		WHERE [Id] = @Id
		SET @RET = 1; 
	END 
	ELSE IF @TYPEACTION = 'E' 
	BEGIN 
		DELETE FROM [dbo].[TabelaPrecos] 
		WHERE [Id] = @Id
		SET @RET = 2; 
	END 
 
END 
