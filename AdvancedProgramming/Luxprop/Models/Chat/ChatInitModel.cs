using System.ComponentModel.DataAnnotations;

namespace Luxprop.Models.Chat
{
    public class ChatInitModel
    {
        [Required(ErrorMessage = "Please enter your name.")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string ClientEmail { get; set; } = string.Empty;
    }
}
