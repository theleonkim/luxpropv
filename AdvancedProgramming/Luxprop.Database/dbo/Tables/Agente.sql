CREATE TABLE [dbo].[Agente] (
    [Agente_ID]     INT            IDENTITY (1, 1) NOT NULL,
    [Usuario_ID]    INT            NOT NULL,
    [Codigo_Agente] NVARCHAR (50)  NULL,
    [Sucursal]      NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Agente_ID] ASC),
    FOREIGN KEY ([Usuario_ID]) REFERENCES [dbo].[Usuario] ([Usuario_ID])
);

