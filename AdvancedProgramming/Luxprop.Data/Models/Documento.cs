using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxprop.Data.Models;

public partial class Documento
{
    public int DocumentoId { get; set; }

    public string? Nombre { get; set; }

    public string? TipoDocumento { get; set; }

    public string? Estado { get; set; }

    public DateOnly? FechaCarga { get; set; }

    public int? ExpedienteId { get; set; }
    public string? UrlArchivo { get; set; }

    public string? Etiquetas { get; set; }
    public int? UsuarioId { get; set; }

    [ForeignKey(nameof(UsuarioId))]
    public virtual Usuario? Usuario { get; set; }


    public DateTime? FechaVencimiento { get; set; }

    public virtual Expediente? Expediente { get; set; }

    public virtual ICollection<AlertasDocumento> AlertasDocumentos { get; set; } = new List<AlertasDocumento>();

}
