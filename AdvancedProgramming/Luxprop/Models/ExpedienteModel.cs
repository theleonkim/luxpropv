using Luxprop.Data.Models;
using System;
using System.Collections.Generic;

namespace Luxprop.Models
{
    public class ExpedienteModel
    {
        public int ExpedienteId { get; set; }

        public string? TipoOcupacion { get; set; }

        public string? Estado { get; set; }

        public int? PropiedadId { get; set; }

        public int? ClienteId { get; set; }

        // 🔹 Usar DateTime para mantener compatibilidad con horas si las necesitás
        public DateTime? FechaApertura { get; set; }

        public DateTime? FechaCierre { get; set; }

        // 🔹 Relaciones
        public virtual Cliente? Cliente { get; set; }

        public virtual Propiedad? Propiedad { get; set; }

        public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

        public virtual ICollection<Citum> Citas { get; set; } = new List<Citum>();

        public virtual ICollection<TareaTramite> TareaTramites { get; set; } = new List<TareaTramite>();

        // 🔹 (Opcional) Historial, si querés incluirlo directamente en el modelo
        public virtual ICollection<HistorialExpediente> HistorialExpedientes { get; set; } = new List<HistorialExpediente>();
    }
}
