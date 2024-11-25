using System.ComponentModel.DataAnnotations;

namespace Shared.API.Enums
{
    public enum EmailSubject
    {
        [Display(Name = "Email Confirmation")]
        EmailConfirmation,
        [Display(Name = "Reset Password Email")]
        ResetPasswordEmail
    }
}
