using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxprop.Data.Models
{
    public partial class HistorialExpediente
    {
        public int HistorialId { get; set; }

        // Relaciones principales
        public int ExpedienteId { get; set; }
        public int? UsuarioId { get; set; }

        [ForeignKey(nameof(ExpedienteId))]
        public virtual Expediente Expediente { get; set; } = null!;

        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario? Usuario { get; set; }

        // Fechas y acciones
        public DateTime? FechaModificacion { get; set; } = DateTime.Now;

        // Detalles del cambio
        public string? EstadoAnterior { get; set; }
        public string? EstadoNuevo { get; set; }
        public string? TipoAccion { get; set; }
        public string? Descripcion { get; set; }
        public string? Observacion { get; set; }

        // Información adicional
        public string? IPRegistro { get; set; }
    }
}
