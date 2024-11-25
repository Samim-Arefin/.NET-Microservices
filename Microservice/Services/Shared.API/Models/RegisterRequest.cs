using System.ComponentModel.DataAnnotations;

namespace Shared.API.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required"), Length(6, 32, ErrorMessage = "Min 6 characters & Max 32 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
