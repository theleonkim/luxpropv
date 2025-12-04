CREATE TABLE [dbo].[AlertasDocumento](
	[Alerta_ID] [int] IDENTITY(1,1) NOT NULL,
	[Documento_ID] [int] NOT NULL,
	[Fecha_Registro] [date] NOT NULL,
	[Tipo] [varchar](50) NOT NULL,
	[Estado] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Alerta_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AlertasDocumento]  WITH CHECK ADD  CONSTRAINT [FK_Alerta_Documento] FOREIGN KEY([Documento_ID])
REFERENCES [dbo].[Documento] ([Documento_ID])
GO

ALTER TABLE [dbo].[AlertasDocumento] CHECK CONSTRAINT [FK_Alerta_Documento]
GO


