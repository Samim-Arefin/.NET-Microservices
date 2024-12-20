﻿using System.ComponentModel.DataAnnotations;

namespace Shared.API.Models
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Not a valid email"), MaxLength(128, ErrorMessage = "Max 128 characters.")]
        public string Email { get; set; }
    }
}
