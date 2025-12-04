using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class UsuarioRol
{
    public int UsuarioRolId { get; set; }

    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public virtual Rol Rol { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
