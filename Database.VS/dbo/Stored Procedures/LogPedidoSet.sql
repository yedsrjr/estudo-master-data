CREATE   PROCEDURE [dbo].[LogPedidoSet]
@action varchar(1), 
@Id Int = NULL, 
@IdPedido Int = NULL, 
@DataCriacao DateTime2(0) = NULL, 
@Observacao Varchar(200) = NULL, 
@Status Int = NULL, 
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
		FROM [dbo].[LogPedido] WITH (NOLOCK) 
		WHERE [Id] = @Id
		AND [IdPedido] = @IdPedido
 
		IF @NCOUNT > 0 
		BEGIN 
			SET @TYPEACTION = 'A'
		END 
	END 
 
	IF @TYPEACTION = 'I' 
	BEGIN 
		INSERT INTO [dbo].[LogPedido] (
			[IdPedido],
			[DataCriacao],
			[Observacao],
			[Status])
		OUTPUT Inserted.Id
		VALUES (
			@IdPedido,
			@DataCriacao,
			@Observacao,
			@Status)
		SET @RET = 0; 
	END 
	ELSE IF @TYPEACTION = 'A' 
	BEGIN 
		UPDATE [dbo].[LogPedido] SET 
			[DataCriacao] = @DataCriacao, 
			[Observacao] = @Observacao, 
			[Status] = @Status
		WHERE [Id] = @Id
		AND [IdPedido] = @IdPedido
		SET @RET = 1; 
	END 
	ELSE IF @TYPEACTION = 'E' 
	BEGIN 
		DELETE FROM [dbo].[LogPedido] 
		WHERE [Id] = @Id
		AND [IdPedido] = @IdPedido
		SET @RET = 2; 
	END 
 
END 
