CREATE TABLE [dbo].[Propiedad] (
    [Propiedad_ID]       INT             IDENTITY (1, 1) NOT NULL,
    [Titulo]             NVARCHAR (150)  NULL,
    [Descripcion]        NVARCHAR (MAX)  NULL,
    [Precio]             DECIMAL (18, 2) NULL,
    [Area_Construccion]  DECIMAL (10, 2) NULL,
    [Area_Terreno]       DECIMAL (10, 2) NULL,
    [Estado_Publicacion] NVARCHAR (50)   NULL,
    [Agente_ID]          INT             NULL,
    [Ubicacion_ID]       INT             NULL,
    [Recorrido360Url]    NVARCHAR (2048) NULL,
    PRIMARY KEY CLUSTERED ([Propiedad_ID] ASC)
);

