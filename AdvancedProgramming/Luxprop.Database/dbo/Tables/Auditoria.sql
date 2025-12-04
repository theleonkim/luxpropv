CREATE TABLE [dbo].[Auditoria] (
    [Auditoria_ID] INT            IDENTITY (1, 1) NOT NULL,
    [Usuario_ID]   INT            NOT NULL,
    [Fecha]        DATETIME       DEFAULT (getdate()) NULL,
    [Accion]       NVARCHAR (100) NULL,
    [Entidad]      NVARCHAR (100) NULL,
    [Detalle]      NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Auditoria_ID] ASC),
    FOREIGN KEY ([Usuario_ID]) REFERENCES [dbo].[Usuario] ([Usuario_ID])
);

