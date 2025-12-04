using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models
{
    public partial class AlertasDocumento
    {
        public int AlertaId { get; set; }   

        public int DocumentoId { get; set; }   

        public DateOnly FechaRegistro { get; set; }   

        public string Tipo { get; set; } = null!;  

        public string Estado { get; set; } = null!; 

        public virtual Documento Documento { get; set; } = null!;
    }
}
