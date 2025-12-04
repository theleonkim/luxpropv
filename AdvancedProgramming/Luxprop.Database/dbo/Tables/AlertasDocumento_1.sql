CREATE TABLE [dbo].[AlertasDocumento] (
    [Alerta_ID]      INT          IDENTITY (1, 1) NOT NULL,
    [Documento_ID]   INT          NOT NULL,
    [Fecha_Registro] DATE         NOT NULL,
    [Tipo]           VARCHAR (50) NOT NULL,
    [Estado]         VARCHAR (30) NOT NULL,
    PRIMARY KEY CLUSTERED ([Alerta_ID] ASC),
    CONSTRAINT [FK_Alerta_Documento] FOREIGN KEY ([Documento_ID]) REFERENCES [dbo].[Documento] ([Documento_ID])
);

