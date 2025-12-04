CREATE TABLE [dbo].[Usuario] (
    [Usuario_ID] INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]     NVARCHAR (100) NOT NULL,
    [Apellido]   NVARCHAR (100) NOT NULL,
    [Email]      NVARCHAR (150) NOT NULL,
    [Telefono]   NVARCHAR (20)  NULL,
    [Activo]     BIT            DEFAULT ((1)) NULL,
    [Password]   NVARCHAR (200) DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([Usuario_ID] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

