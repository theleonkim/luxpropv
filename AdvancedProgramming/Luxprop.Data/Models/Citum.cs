using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class Citum
{
    public int CitaId { get; set; }

    public int ExpedienteId { get; set; }

    public string? Asunto { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string? Canal { get; set; }

    public virtual Expediente Expediente { get; set; } = null!;
}
