CREATE TABLE [dbo].[Recordatorio] (
    [RecordatorioId]   INT            IDENTITY (1, 1) NOT NULL,
    [Titulo]           NVARCHAR (150) NOT NULL,
    [Descripcion]      NVARCHAR (500) NULL,
    [Tipo]             NVARCHAR (20)  DEFAULT (N'Cita') NOT NULL,
    [Estado]           NVARCHAR (20)  DEFAULT (N'Pendiente') NOT NULL,
    [Prioridad]        NVARCHAR (10)  DEFAULT (N'Media') NOT NULL,
    [Inicio]           DATETIME2 (0)  NOT NULL,
    [Fin]              DATETIME2 (0)  NULL,
    [TodoElDia]        BIT            DEFAULT ((0)) NOT NULL,
    [PropiedadId]      INT            NULL,
    [ExpedienteId]     INT            NULL,
    [CreadoPorId]      INT            NOT NULL,
    [NotificarCorreo]  BIT            DEFAULT ((1)) NOT NULL,
    [MinutosAntes]     INT            DEFAULT ((60)) NOT NULL,
    [ReglaRecurrencia] NVARCHAR (200) NULL,
    [UltimoAviso]      DATETIME2 (0)  NULL,
    [CreadoEn]         DATETIME2 (0)  DEFAULT (sysutcdatetime()) NOT NULL,
    [ActualizadoEn]    DATETIME2 (0)  DEFAULT (sysutcdatetime()) NOT NULL,
    [UsuarioId]        INT            NULL,
    PRIMARY KEY CLUSTERED ([RecordatorioId] ASC),
    CONSTRAINT [FK_Recordatorio_CreadoPor] FOREIGN KEY ([CreadoPorId]) REFERENCES [dbo].[Usuario] ([Usuario_ID]),
    CONSTRAINT [FK_Recordatorio_Expediente] FOREIGN KEY ([ExpedienteId]) REFERENCES [dbo].[Expediente] ([Expediente_ID]),
    CONSTRAINT [FK_Recordatorio_Propiedad] FOREIGN KEY ([PropiedadId]) REFERENCES [dbo].[Propiedad] ([Propiedad_ID]),
    CONSTRAINT [FK_Recordatorio_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([Usuario_ID])
);








GO
CREATE NONCLUSTERED INDEX [IX_Recordatorio_Prop_Exp]
    ON [dbo].[Recordatorio]([PropiedadId] ASC, [ExpedienteId] ASC);


GO



GO
CREATE NONCLUSTERED INDEX [IX_Recordatorio_Inicio]
    ON [dbo].[Recordatorio]([Inicio] ASC);

