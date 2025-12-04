CREATE TABLE [dbo].[Cliente] (
    [Cliente_ID]   INT           IDENTITY (1, 1) NOT NULL,
    [Cedula]       NVARCHAR (50) NOT NULL,
    [Tipo_Cliente] NVARCHAR (50) NULL,
    [Usuario_ID]   INT           NULL,
    PRIMARY KEY CLUSTERED ([Cliente_ID] ASC),
    FOREIGN KEY ([Usuario_ID]) REFERENCES [dbo].[Usuario] ([Usuario_ID])
);

