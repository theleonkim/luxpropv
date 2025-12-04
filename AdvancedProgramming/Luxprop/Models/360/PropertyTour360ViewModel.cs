using System.ComponentModel.DataAnnotations;

namespace Luxprop.Models._360
{
    public class PropertyTour360ViewModel
    {
        public int PropertyId { get; set; }

        [Required]
        [Url]
        public string? TourUrl { get; set; }

        public string? Title { get; set; }
    }
}
