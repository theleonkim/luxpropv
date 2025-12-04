using Luxprop.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Luxprop.Models
{
    public class HistorialExpedienteModel
    {
        [Key]
        public int HistorialId { get; set; } 

        public int ExpedienteId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
        public string? Descripcion { get; set; }
        public string? EstadoNuevo { get; set; }

        // Relaciones
        public virtual Expediente Expediente { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
