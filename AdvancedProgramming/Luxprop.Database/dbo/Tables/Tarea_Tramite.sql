CREATE TABLE [dbo].[Tarea_Tramite] (
    [Tarea_ID]         INT            IDENTITY (1, 1) NOT NULL,
    [Expediente_ID]    INT            NOT NULL,
    [Titulo]           NVARCHAR (150) NULL,
    [Descripcion]      NVARCHAR (MAX) NULL,
    [Estado]           NVARCHAR (50)  NULL,
    [Prioridad]        NVARCHAR (50)  NULL,
    [Fecha_Inicio]     DATE           NULL,
    [Fecha_Cierre]     DATE           NULL,
    [Fecha_Compromiso] DATE           NULL,
    PRIMARY KEY CLUSTERED ([Tarea_ID] ASC),
    FOREIGN KEY ([Expediente_ID]) REFERENCES [dbo].[Expediente] ([Expediente_ID])
);

