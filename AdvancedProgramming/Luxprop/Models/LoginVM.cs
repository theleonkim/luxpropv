// Luxprop/Models/LoginVM.cs
using System.ComponentModel.DataAnnotations;

namespace Luxprop.Models
{
    public class LoginVM
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
