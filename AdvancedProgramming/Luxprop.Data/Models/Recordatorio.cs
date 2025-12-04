using System.ComponentModel.DataAnnotations.Schema;
using Luxprop.Data.Models;

public class Recordatorio
{
    public int RecordatorioId { get; set; }
    public string Titulo { get; set; } = default!;
    public string? Descripcion { get; set; }

    public string Tipo { get; set; } = "Cita";          // Cita | Vencimiento | Tarea
    public string Estado { get; set; } = "Pendiente";    // Pendiente | Completado | Incumplido
    public string Prioridad { get; set; } = "Media";     // Baja | Media | Alta

    public DateTime Inicio { get; set; }
    public DateTime? Fin { get; set; }
    public bool TodoElDia { get; set; }

    public int? PropiedadId { get; set; }
    public int? ExpedienteId { get; set; }
    public int? UsuarioId { get; set; }

    public bool NotificarCorreo { get; set; } = true;
    public int MinutosAntes { get; set; } = 60;
    public string? ReglaRecurrencia { get; set; }
    public DateTime? UltimoAviso { get; set; }

    public DateTime CreadoEn { get; set; }
    public DateTime ActualizadoEn { get; set; }

    public virtual Propiedad? Propiedad { get; set; }
    public virtual Expediente? Expediente { get; set; }

    [ForeignKey("UsuarioId")]
    public virtual Usuario? Usuario { get; set; }
}
