CREATE TABLE [dbo].[Usuario_Rol] (
    [UsuarioRol_ID] INT IDENTITY (1, 1) NOT NULL,
    [Usuario_ID]    INT NOT NULL,
    [Rol_ID]        INT NOT NULL,
    PRIMARY KEY CLUSTERED ([UsuarioRol_ID] ASC),
    FOREIGN KEY ([Rol_ID]) REFERENCES [dbo].[Rol] ([Rol_ID]),
    FOREIGN KEY ([Usuario_ID]) REFERENCES [dbo].[Usuario] ([Usuario_ID])
);

