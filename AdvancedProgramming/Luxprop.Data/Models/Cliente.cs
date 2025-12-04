using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxprop.Data.Models;

public partial class Cliente
{
    [Column("Cliente_ID")]
    public int ClienteId { get; set; }

    [Column("Cedula")]
    public string Cedula { get; set; }

    [Column("Tipo_Cliente")]
    public string TipoCliente { get; set; } = string.Empty;

    [Column("Usuario_ID")]
    public int UsuarioId { get; set; }

    public virtual ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

    public virtual Usuario? Usuario { get; set; }
}
