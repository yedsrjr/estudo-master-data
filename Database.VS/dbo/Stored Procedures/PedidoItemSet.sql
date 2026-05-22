CREATE   PROCEDURE [dbo].[PedidoItemSet]
@action varchar(1), 
@Id Int = NULL, 
@PedidoId Int, 
@ItemId Int, 
@PesoBruto Decimal(10,3), 
@PesoLiquido Decimal(10,3), 
@Quantidade Int, 
@ValorUnit Decimal(10,2), 
@ValorTotal Decimal(10,2) = NULL, 
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
		FROM [dbo].[PedidoItem] WITH (NOLOCK) 
		WHERE [Id] = @Id
 
		IF @NCOUNT > 0 
		BEGIN 
			SET @TYPEACTION = 'A'
		END 
	END 
 
	IF @TYPEACTION = 'I' 
	BEGIN 
		INSERT INTO [dbo].[PedidoItem] (
			[PedidoId],
			[ItemId],
			[PesoBruto],
			[PesoLiquido],
			[Quantidade],
			[ValorUnit],
			[ValorTotal],
			[Status])
		OUTPUT Inserted.Id
		VALUES (
			@PedidoId,
			@ItemId,
			@PesoBruto,
			@PesoLiquido,
			@Quantidade,
			@ValorUnit,
			@ValorTotal,
			@Status)
		SET @RET = 0; 
	END 
	ELSE IF @TYPEACTION = 'A' 
	BEGIN 
		UPDATE [dbo].[PedidoItem] SET 
			[PedidoId] = @PedidoId, 
			[ItemId] = @ItemId, 
			[PesoBruto] = @PesoBruto, 
			[PesoLiquido] = @PesoLiquido, 
			[Quantidade] = @Quantidade, 
			[ValorUnit] = @ValorUnit, 
			[ValorTotal] = @ValorTotal, 
			[Status] = @Status
		WHERE [Id] = @Id
		SET @RET = 1; 
	END 
	ELSE IF @TYPEACTION = 'E' 
	BEGIN 
		DELETE FROM [dbo].[PedidoItem] 
		WHERE [Id] = @Id
		SET @RET = 2; 
	END 
 
END 
