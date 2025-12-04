using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class Rol
{
    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<UsuarioRol> UsuarioRols { get; set; } = new List<UsuarioRol>();
}
