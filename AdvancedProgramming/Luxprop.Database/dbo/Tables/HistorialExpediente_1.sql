CREATE TABLE [dbo].[HistorialExpediente] (
    [Historial_ID]       INT           IDENTITY (1, 1) NOT NULL,
    [Expediente_ID]      INT           NOT NULL,
    [Usuario_ID]         INT           NOT NULL,
    [Fecha_Modificacion] DATETIME      NOT NULL,
    [Descripcion]        VARCHAR (500) NULL,
    [EstadoNuevo]        VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Historial_ID] ASC),
    FOREIGN KEY ([Expediente_ID]) REFERENCES [dbo].[Expediente] ([Expediente_ID]),
    FOREIGN KEY ([Usuario_ID]) REFERENCES [dbo].[Usuario] ([Usuario_ID])
);

