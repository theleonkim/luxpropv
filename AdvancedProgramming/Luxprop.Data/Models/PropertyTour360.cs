using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Luxprop.Data.Models
{
    public class PropertyTour360
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string TourUrl { get; set; } = string.Empty;
        public string? Title { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual Propiedad Property { get; set; } = null!;
    }
}
