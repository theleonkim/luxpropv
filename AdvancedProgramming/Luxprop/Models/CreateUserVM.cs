using System.ComponentModel.DataAnnotations;

namespace Luxprop.Models
{
    public class CreateUserVM
    {
        [Required]
        public int UsuarioID { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        [Required]
        public int RolId { get; set; }

        public bool Activo { get; set; } = true;
    }
}
