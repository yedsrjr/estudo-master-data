CREATE   PROCEDURE [dbo].[AuxStatusPedidoSet]
@action varchar(1), 
@Id Int, 
@Descricao Varchar(20) = NULL, 
@Icone Varchar(10) = NULL, 
@Cor Varchar(10) = NULL, 
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
		FROM [dbo].[AuxStatusPedido] WITH (NOLOCK) 
		WHERE [Id] = @Id
 
		IF @NCOUNT > 0 
		BEGIN 
			SET @TYPEACTION = 'A'
		END 
	END 
 
	IF @TYPEACTION = 'I' 
	BEGIN 
		INSERT INTO [dbo].[AuxStatusPedido] (
			[Descricao],
			[Icone],
			[Cor])
		OUTPUT Inserted.Id
		VALUES (
			@Descricao,
			@Icone,
			@Cor)
		SET @RET = 0; 
	END 
	ELSE IF @TYPEACTION = 'A' 
	BEGIN 
		UPDATE [dbo].[AuxStatusPedido] SET 
			[Descricao] = @Descricao, 
			[Icone] = @Icone, 
			[Cor] = @Cor
		WHERE [Id] = @Id
		SET @RET = 1; 
	END 
	ELSE IF @TYPEACTION = 'E' 
	BEGIN 
		DELETE FROM [dbo].[AuxStatusPedido] 
		WHERE [Id] = @Id
		SET @RET = 2; 
	END 
 
END 
