CREATE TABLE [dbo].[Documento](
	[Documento_ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](150) NULL,
	[Tipo_Documento] [nvarchar](100) NULL,
	[Estado] [nvarchar](50) NULL,
	[Fecha_Carga] [date] NULL,
	[Expediente_ID] [int] NULL,
	[UrlArchivo] [nvarchar](500) NULL,
	[Etiquetas] [nvarchar](255) NULL,
	[Fecha_Vencimiento] [date] NULL,
 CONSTRAINT [PK__Document__FBEBB4601152726E] PRIMARY KEY CLUSTERED 
(
	[Documento_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Documento]  WITH CHECK ADD  CONSTRAINT [FK__Documento__Exped__5CD6CB2B] FOREIGN KEY([Expediente_ID])
REFERENCES [dbo].[Expediente] ([Expediente_ID])
GO

ALTER TABLE [dbo].[Documento] CHECK CONSTRAINT [FK__Documento__Exped__5CD6CB2B]
GO


