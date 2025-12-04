CREATE TABLE [dbo].[Ubicacion] (
    [Ubicacion_ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Provincia]         NVARCHAR (50)  NULL,
    [Canton]            NVARCHAR (50)  NULL,
    [Distrito]          NVARCHAR (50)  NULL,
    [Detalle_Direccion] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Ubicacion_ID] ASC)
);

