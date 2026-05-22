CREATE PROCEDURE [dbo].[ClientesSet]
    @action        VARCHAR(1),
    @Id            INT          = NULL,
    @NomeAbreviado VARCHAR(50),
    @NomeCliente   VARCHAR(100),
    @NumCPF        VARCHAR(14),
    @Status        VARCHAR(20)  = NULL,   -- VARCHAR para bater com a coluna
    @UpdatedAt     DATETIME2(0) = NULL,
    @RET           INT OUTPUT
AS
BEGIN

    DECLARE @TYPEACTION VARCHAR(1) = @action

    IF @TYPEACTION = ' '
    BEGIN
        SET @TYPEACTION = 'I'

        IF EXISTS (SELECT 1 FROM [dbo].[Clientes] WITH (NOLOCK) WHERE [Id] = @Id)
            SET @TYPEACTION = 'A'
    END

    -- INSERT: Status padrão '1' (Ativo) quando não informado
    IF @TYPEACTION = 'I'
    BEGIN
        INSERT INTO [dbo].[Clientes]
            ([NomeAbreviado], [NomeCliente], [NumCPF], [Status], [UpdatedAt])
        OUTPUT Inserted.Id
        VALUES
            (@NomeAbreviado, @NomeCliente, @NumCPF,
             @Status,
             ISNULL(@UpdatedAt, GETDATE()))

        SET @RET = 0
    END

    -- UPDATE: se @Status vier NULL, mantém o valor que já está gravado
    ELSE IF @TYPEACTION = 'A'
    BEGIN
        UPDATE [dbo].[Clientes]
        SET
            [NomeAbreviado] = @NomeAbreviado,
            [NomeCliente]   = @NomeCliente,
            [NumCPF]        = @NumCPF,
            [Status] = @Status,
            [UpdatedAt]     = ISNULL(@UpdatedAt, GETDATE())
        WHERE [Id] = @Id

        SET @RET = 1
    END

    -- DELETE
    ELSE IF @TYPEACTION = 'E'
    BEGIN
        DELETE FROM [dbo].[Clientes]
        WHERE [Id] = @Id

        SET @RET = 2
    END

END