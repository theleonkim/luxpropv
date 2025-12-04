using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Models;

public partial class LuxpropContext : DbContext
{
    public LuxpropContext()
    {
    }

    public LuxpropContext(DbContextOptions<LuxpropContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agente> Agentes { get; set; }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<AlertasDocumento> AlertasDocumentos { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<ChatThread> ChatThreads { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<Expediente> Expedientes { get; set; }

    public DbSet<PropertyTour360> PropertyTours360 { get; set; }
    public virtual DbSet<Propiedad> Propiedads { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TareaTramite> TareaTramites { get; set; }

    public virtual DbSet<Ubicacion> Ubicacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioRol> UsuarioRols { get; set; }
    public virtual DbSet<HistorialExpediente> HistorialExpedientes { get; set; }

    public DbSet<Recordatorio> Recordatorios { get; set; } = default!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer(
        "Server=MSI\\MSSQLSERVER01;Database=Luxprop;Trusted_Connection=True;TrustServerCertificate=True;");

        


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HistorialExpediente>()
            .HasOne(h => h.Expediente)
            .WithMany(e => e.HistorialExpedientes)
            .HasForeignKey(h => h.ExpedienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Documento>()
            .HasOne(d => d.Expediente)
            .WithMany(e => e.Documentos)
            .HasForeignKey(d => d.ExpedienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TareaTramite>()
            .HasOne(t => t.Expediente)
            .WithMany(e => e.TareaTramites)
            .HasForeignKey(t => t.ExpedienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Agente>(entity =>
        {
            entity.HasKey(e => e.AgenteId).HasName("PK__Agente__29E28221EFB2DFD3");

            entity.ToTable("Agente");

            entity.Property(e => e.AgenteId).HasColumnName("Agente_ID");
            entity.Property(e => e.CodigoAgente)
                .HasMaxLength(50)
                .HasColumnName("Codigo_Agente");
            entity.Property(e => e.Sucursal).HasMaxLength(100);
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Agentes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Agente__Usuario___6C190EBB");
        });

        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.AuditoriaId).HasName("PK__Auditori__D7259D32A4BD9ADB");

            entity.Property(e => e.AuditoriaId).HasColumnName("Auditoria_ID");
            entity.Property(e => e.Accion).HasMaxLength(100);
            entity.Property(e => e.Entidad).HasMaxLength(100);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Auditoria__Usuar__5FB337D6");
        });


        modelBuilder.Entity<AlertasDocumento>(entity =>
        {
            entity.HasKey(e => e.AlertaId);

            entity.ToTable("AlertasDocumento");

            entity.Property(e => e.AlertaId).HasColumnName("Alerta_ID");
            entity.Property(e => e.DocumentoId).HasColumnName("Documento_ID");
            entity.Property(e => e.FechaRegistro).HasColumnName("Fecha_Registro");
            entity.Property(e => e.Tipo).HasMaxLength(50);
            entity.Property(e => e.Estado).HasMaxLength(30);

            entity.HasOne(d => d.Documento)
                .WithMany(p => p.AlertasDocumentos)
                .HasForeignKey(d => d.DocumentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alerta_Documento");
        });


        modelBuilder.Entity<ChatThread>(entity =>
        {
            entity.HasKey(e => e.ChatThreadId).HasName("PK__ChatThre__32405D65C01B4339");
            entity.Property(e => e.ClientEmail).HasMaxLength(200);
            entity.Property(e => e.ClientName).HasMaxLength(150);
            entity.Property(e => e.ClosedUtc).HasColumnType("datetime");
            entity.Property(e => e.CreatedUtc).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");
            entity.Property(e => e.State).HasMaxLength(20).HasDefaultValue("Open");

            entity.HasOne(d => d.UsuarioCreador)
                .WithMany(p => p.ChatThreadsCreados)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("FK_ChatThread_Usuario");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.ChatMessageId).HasName("PK__ChatMess__9AB61035A2910B89");
            entity.Property(e => e.Sender).HasMaxLength(20);
            entity.Property(e => e.SentUtc).HasDefaultValueSql("(getutcdate())").HasColumnType("datetime");

            entity.HasOne(d => d.ChatThread)
                .WithMany(p => p.ChatMessages)
                .HasForeignKey(d => d.ChatThreadId)
                .HasConstraintName("FK_ChatMessages_ChatThreads");

            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.ChatMessages)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_ChatMessages_Usuario");
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.CitaId).HasName("PK__Cita__992D0A25CA4C9BD6");

            entity.Property(e => e.CitaId).HasColumnName("Cita_ID");
            entity.Property(e => e.Asunto).HasMaxLength(150);
            entity.Property(e => e.Canal).HasMaxLength(50);
            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inicio");

            entity.HasOne(d => d.Expediente).WithMany(p => p.Cita)
                .HasForeignKey(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cita__Expediente__6383C8BA");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Cliente__EB683FB49DF209F9");

            entity.ToTable("Cliente");

            entity.Property(e => e.ClienteId).HasColumnName("Cliente_ID");
            entity.Property(e => e.Cedula).HasMaxLength(50);
            entity.Property(e => e.TipoCliente)
                .HasMaxLength(50)
                .HasColumnName("Tipo_Cliente");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Cliente__Usuario__534D60F1");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.DocumentoId).HasName("PK__Document__FBEBB4601152726E");

            entity.ToTable("Documento");

            entity.Property(e => e.DocumentoId).HasColumnName("Documento_ID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.FechaCarga).HasColumnName("Fecha_Carga");
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.TipoDocumento)
             .HasMaxLength(100)
                .HasColumnName("Tipo_Documento");
            entity.Property(e => e.FechaVencimiento).HasColumnName("Fecha_Vencimiento");

            entity.HasOne(d => d.Expediente).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.ExpedienteId)
                .HasConstraintName("FK__Documento__Exped__5CD6CB2B");
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.HasKey(e => e.ExpedienteId).HasName("PK__Expedien__0AAD7FACCB26AD20");
            entity.ToTable("Expediente");

            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.PropiedadId).HasColumnName("Propiedad_ID");
            entity.Property(e => e.ClienteId).HasColumnName("Cliente_ID");
            entity.Property(e => e.AgenteId).HasColumnName("Agente_ID");
            entity.Property(e => e.TipoOcupacion).HasMaxLength(50).HasColumnName("Tipo_Ocupacion");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.Prioridad).HasMaxLength(30);
            entity.Property(e => e.Categoria).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Observaciones).HasColumnType("nvarchar(max)");
            entity.Property(e => e.FechaApertura).HasColumnName("Fecha_Apertura");
            entity.Property(e => e.FechaCierre).HasColumnName("Fecha_Cierre");
            entity.Property(e => e.UltimaActualizacion).HasColumnName("Ultima_Actualizacion");
            entity.Property(e => e.CreadoPorId).HasColumnName("CreadoPor_ID");
            entity.Property(e => e.ModificadoPorId).HasColumnName("ModificadoPor_ID");

            entity.HasOne(d => d.Propiedad)
                .WithMany(p => p.Expedientes)
                .HasForeignKey(d => d.PropiedadId)
                .HasConstraintName("FK__Expedient__Propi__59063A47");

            entity.HasOne(d => d.Cliente)
                .WithMany(p => p.Expedientes)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Expedient__Clien__59FA5E80");

            entity.HasOne(d => d.Agente)
                .WithMany()
                .HasForeignKey(d => d.AgenteId)
                .HasConstraintName("FK_Expediente_Agente");

            entity.HasOne(d => d.CreadoPor)
                .WithMany()
                .HasForeignKey(d => d.CreadoPorId)
                .HasConstraintName("FK_Expediente_CreadoPor");

            entity.HasOne(d => d.ModificadoPor)
                .WithMany()
                .HasForeignKey(d => d.ModificadoPorId)
                .HasConstraintName("FK_Expediente_ModificadoPor");
        });

        modelBuilder.Entity<HistorialExpediente>(entity =>
        {
            entity.HasKey(e => e.HistorialId).HasName("PK__HistorialExpediente");
            entity.ToTable("HistorialExpediente");

            entity.Property(e => e.HistorialId).HasColumnName("Historial_ID");
            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.Property(e => e.FechaModificacion)
                .HasColumnName("Fecha_Modificacion")
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.EstadoNuevo).HasMaxLength(100);
            entity.Property(e => e.EstadoAnterior).HasMaxLength(50);
            entity.Property(e => e.TipoAccion).HasMaxLength(100);
            entity.Property(e => e.Observacion).HasColumnType("nvarchar(max)");
            entity.Property(e => e.IPRegistro).HasMaxLength(50);

            entity.HasOne(d => d.Expediente)
                .WithMany(p => p.HistorialExpedientes)
                .HasForeignKey(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialExpediente_Expediente");

            entity.HasOne(d => d.Usuario)
                .WithMany()
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_HistorialExpediente_Usuario");
        });

        modelBuilder.Entity<PropertyTour360>(entity =>
        {
            entity.ToTable("PropertyTour360");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Property)
                  .WithOne(p => p.Tour360)      // si solo permitís 1 recorrido por propiedad
                  .HasForeignKey<PropertyTour360>(e => e.PropertyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Propiedad>(entity =>
        {
            entity.HasKey(e => e.PropiedadId).HasName("PK__Propieda__D578EA759D6EDEF3");

            entity.ToTable("Propiedad");

            entity.Property(e => e.PropiedadId).HasColumnName("Propiedad_ID");
            entity.Property(e => e.AgenteId).HasColumnName("Agente_ID");
            entity.Property(e => e.AreaConstruccion)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Area_Construccion");
            entity.Property(e => e.AreaTerreno)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Area_Terreno");
            entity.Property(e => e.EstadoPublicacion)
                .HasMaxLength(50)
                .HasColumnName("Estado_Publicacion");
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Titulo).HasMaxLength(150);
            entity.Property(e => e.UbicacionId).HasColumnName("Ubicacion_ID");

            entity.HasOne(d => d.Agente).WithMany(p => p.Propiedads)
                .HasForeignKey(d => d.AgenteId)
                .HasConstraintName("FK__Propiedad__Agent__5629CD9C");

            entity.HasOne(d => d.Ubicacion).WithMany(p => p.Propiedads)
                .HasForeignKey(d => d.UbicacionId)
                .HasConstraintName("FK__Propiedad__Ubica__693CA210");
        });


        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recordatorio>(entity =>
        {
            entity.ToTable("Recordatorio");

            entity.HasKey(e => e.RecordatorioId);
            entity.Property(e => e.Titulo).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Tipo).HasMaxLength(20);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.Prioridad).HasMaxLength(10);
            entity.Property(e => e.ReglaRecurrencia).HasMaxLength(200);

            // ✅ Solo queda esta relación con Usuario
            entity.HasOne(d => d.Usuario)
                .WithMany()
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recordatorio_Usuario");

            entity.HasOne(d => d.Propiedad)
                .WithMany()
                .HasForeignKey(d => d.PropiedadId)
                .HasConstraintName("FK_Recordatorio_Propiedad");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Rol__795EBD69D5D309A9");

            entity.ToTable("Rol");

            entity.Property(e => e.RolId).HasColumnName("Rol_ID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TareaTramite>(entity =>
        {
            entity.HasKey(e => e.TareaId).HasName("PK__Tarea_Tr__327AB98ABCB31B89");

            entity.ToTable("Tarea_Tramite");

            entity.Property(e => e.TareaId).HasColumnName("Tarea_ID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.FechaCierre).HasColumnName("Fecha_Cierre");
            entity.Property(e => e.FechaCompromiso).HasColumnName("Fecha_Compromiso");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_Inicio");
            entity.Property(e => e.Prioridad).HasMaxLength(50);
            entity.Property(e => e.Titulo).HasMaxLength(150);

            entity.HasOne(d => d.Expediente).WithMany(p => p.TareaTramites)
                .HasForeignKey(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tarea_Tra__Exped__66603565");
        });

        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.HasKey(e => e.UbicacionId).HasName("PK__Ubicacio__AE143812222F5524");

            entity.ToTable("Ubicacion");

            entity.Property(e => e.UbicacionId).HasColumnName("Ubicacion_ID");
            entity.Property(e => e.Canton).HasMaxLength(50);
            entity.Property(e => e.DetalleDireccion).HasColumnName("Detalle_Direccion");
            entity.Property(e => e.Distrito).HasMaxLength(50);
            entity.Property(e => e.Provincia).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuario__7711133563933B9C");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534AD4CCCAB").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasDefaultValue("");
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.ResetPasswordToken).HasMaxLength(200).HasColumnName("ResetPasswordToken");
            entity.Property(e => e.ResetPasswordExpiration)
                  .HasColumnType("datetime")
                  .HasColumnName("ResetPasswordExpiration");
        });

        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => e.UsuarioRolId).HasName("PK__Usuario___72F88187E8620268");

            entity.ToTable("Usuario_Rol");

            entity.Property(e => e.UsuarioRolId).HasColumnName("UsuarioRol_ID");
            entity.Property(e => e.RolId).HasColumnName("Rol_ID");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario_R__Rol_I__5070F446");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario_R__Usuar__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
