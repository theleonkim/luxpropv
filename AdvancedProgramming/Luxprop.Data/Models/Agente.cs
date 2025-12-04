using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class Agente
{
    public int AgenteId { get; set; }

    public string CodigoAgente { get; set; } = null!;

    public string? Sucursal { get; set; }

    public int UsuarioId { get; set; }

    public virtual Usuario? Usuario { get; set; }   // ✅ RELACIÓN CORRECTA
}

