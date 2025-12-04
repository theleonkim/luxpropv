CREATE TABLE [dbo].[Expediente](
    [Expediente_ID]       INT IDENTITY(1,1) NOT NULL,
    [Tipo_Ocupacion]      NVARCHAR(50) NULL,
    [Estado]              NVARCHAR(50) NULL,
    [Propiedad_ID]        INT NULL,
    [Cliente_ID]          INT NULL,
    [Fecha_Apertura]      DATE NULL,
    [Fecha_Cierre]        DATE NULL,
    [Codigo]              NVARCHAR(50) NULL,
    [Prioridad]           NVARCHAR(30) NULL,
    [Categoria]           NVARCHAR(50) NULL,
    [Descripcion]         NVARCHAR(255) NULL,
    [Observaciones]       NVARCHAR(MAX) NULL,
    [Agente_ID]           INT NULL,
    [Ultima_Actualizacion] DATETIME NULL,
    [CreadoPor_ID]        INT NULL,
    [ModificadoPor_ID]    INT NULL,
    CONSTRAINT [PK__Expedien__0AAD7FACCB26AD20] PRIMARY KEY CLUSTERED ([Expediente_ID] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

ALTER TABLE [dbo].[Expediente]  WITH CHECK 
ADD CONSTRAINT [FK__Expedient__Propi__59063A47] FOREIGN KEY([Propiedad_ID])
REFERENCES [dbo].[Propiedad] ([Propiedad_ID]);
GO

ALTER TABLE [dbo].[Expediente]  WITH CHECK 
ADD CONSTRAINT [FK__Expedient__Clien__59FA5E80] FOREIGN KEY([Cliente_ID])
REFERENCES [dbo].[Usuario] ([Usuario_ID]);
GO

ALTER TABLE [dbo].[Expediente]  WITH CHECK 
ADD CONSTRAINT [FK_Expediente_Agente] FOREIGN KEY([Agente_ID])
REFERENCES [dbo].[Usuario] ([Usuario_ID]);
GO

ALTER TABLE [dbo].[Expediente]  WITH CHECK 
ADD CONSTRAINT [FK_Expediente_CreadoPor] FOREIGN KEY([CreadoPor_ID])
REFERENCES [dbo].[Usuario] ([Usuario_ID]);
GO

ALTER TABLE [dbo].[Expediente]  WITH CHECK 
ADD CONSTRAINT [FK_Expediente_ModificadoPor] FOREIGN KEY([ModificadoPor_ID])
REFERENCES [dbo].[Usuario] ([Usuario_ID]);
GO
