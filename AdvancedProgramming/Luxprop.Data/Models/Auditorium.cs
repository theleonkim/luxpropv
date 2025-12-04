using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class Auditorium
{
    public int AuditoriaId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Accion { get; set; }

    public string? Entidad { get; set; }

    public string? Detalle { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
