using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxprop.Data.Models
{
    public partial class Expediente
    {
        public int ExpedienteId { get; set; }

        // Identificadores y relaciones
        public int? PropiedadId { get; set; }
        public int? ClienteId { get; set; }
        public int? AgenteId { get; set; }

        [ForeignKey(nameof(PropiedadId))]
        public virtual Propiedad? Propiedad { get; set; }

        [ForeignKey(nameof(ClienteId))]
        public virtual Cliente? Cliente { get; set; }

        [ForeignKey(nameof(AgenteId))]
        public virtual Usuario? Agente { get; set; }

        // Datos del expediente
        public string? Codigo { get; set; }
        public string? TipoOcupacion { get; set; }
        public string? Estado { get; set; } = "En proceso";
        public string? Prioridad { get; set; }
        public string? Categoria { get; set; }
        public string? Descripcion { get; set; }
        public string? Observaciones { get; set; }

        // Fechas
        public DateOnly? FechaApertura { get; set; }

        public DateOnly? FechaCierre { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        // Auditoría
        public int? CreadoPorId { get; set; }
        public int? ModificadoPorId { get; set; }

        [ForeignKey(nameof(CreadoPorId))]
        public virtual Usuario? CreadoPor { get; set; }

        [ForeignKey(nameof(ModificadoPorId))]
        public virtual Usuario? ModificadoPor { get; set; }

        // Relaciones con otras entidades
        public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
        public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();
        public virtual ICollection<TareaTramite> TareaTramites { get; set; } = new List<TareaTramite>();
        public virtual ICollection<HistorialExpediente>? HistorialExpedientes { get; set; }
    }
}
