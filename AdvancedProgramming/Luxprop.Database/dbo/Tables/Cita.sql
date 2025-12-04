CREATE TABLE [dbo].[Cita] (
    [Cita_ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Expediente_ID] INT            NOT NULL,
    [Asunto]        NVARCHAR (150) NULL,
    [Fecha_Inicio]  DATETIME       NULL,
    [Fecha_Fin]     DATETIME       NULL,
    [Canal]         NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([Cita_ID] ASC),
    FOREIGN KEY ([Expediente_ID]) REFERENCES [dbo].[Expediente] ([Expediente_ID])
);

