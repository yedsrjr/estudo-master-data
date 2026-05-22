CREATE   PROCEDURE [dbo].[PedidosSet]
@action varchar(1), 
@Id Int, 
@CodClient Int, 
@Status Int = NULL, 
@Total Decimal(15,2) = NULL, 
@DataCriacao DateTime2(0) = NULL, 
@Anexo Varchar(100) = NULL, 
@ObservacaoNF Varchar(200) = NULL, 
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
		FROM [dbo].[Pedidos] WITH (NOLOCK) 
		WHERE [Id] = @Id
 
		IF @NCOUNT > 0 
		BEGIN 
			SET @TYPEACTION = 'A'
		END 
	END 
 
	IF @TYPEACTION = 'I' 
	BEGIN 
		INSERT INTO [dbo].[Pedidos] (
			[CodClient],
			[Status],
			[Total],
			[DataCriacao],
			[Anexo],
			[ObservacaoNF])
		OUTPUT Inserted.Id
		VALUES (
			@CodClient,
			@Status,
			@Total,
			@DataCriacao,
			@Anexo,
			@ObservacaoNF)
		SET @RET = 0; 
	END 
	ELSE IF @TYPEACTION = 'A' 
	BEGIN 
		UPDATE [dbo].[Pedidos] SET 
			[CodClient] = @CodClient, 
			[Status] = @Status, 
			[Total] = @Total, 
			[DataCriacao] = @DataCriacao, 
			[Anexo] = @Anexo, 
			[ObservacaoNF] = @ObservacaoNF
		WHERE [Id] = @Id
		SET @RET = 1; 
	END 
	ELSE IF @TYPEACTION = 'E' 
	BEGIN 
		DELETE FROM [dbo].[Pedidos] 
		WHERE [Id] = @Id
		SET @RET = 2; 
	END 
 
END 
