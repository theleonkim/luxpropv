CREATE TABLE [dbo].[AlertaVencimiento] (
    [Alerta_ID]        INT           IDENTITY (1, 1) NOT NULL,
    [Documento_ID]     INT           NOT NULL,
    [Fecha_Programada] DATE          NULL,
    [Tipo]             NVARCHAR (50) NULL,
    [Estado]           NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Alerta_ID] ASC),
    FOREIGN KEY ([Documento_ID]) REFERENCES [dbo].[Documento] ([Documento_ID])
);

