using System.ComponentModel.DataAnnotations;

namespace Shared.API.Models
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Password is required"), Length(6, 32, ErrorMessage = "Min 6 characters & Max 32 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
