CREATE TABLE [dbo].[Clientes] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [NomeAbreviado] VARCHAR (50)  NOT NULL,
    [NomeCliente]   VARCHAR (100) NOT NULL,
    [NumCPF]        VARCHAR (14)  NOT NULL,
    [UpdatedAt]     DATETIME2 (7) NULL,
    [Status]        VARCHAR (20)  NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY NONCLUSTERED ([Id] ASC)
);

