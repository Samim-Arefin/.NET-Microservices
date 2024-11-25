using Auth.API.Infrastructure;
using Auth.API.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Models;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService) 
            => _authenticationService = authenticationService;
        
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(RegisterRequest register) 
            => Ok(await _authenticationService.Register(register));

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginRequest login) 
            => Ok(await _authenticationService.Login(login));

        [HttpPost, Route("resendConfirmationEmail")]
        public async Task<IActionResult> resendConfirmationEmail(ConfirmationMailRequest confirmationMail) 
            => Ok(await _authenticationService.ResendConfirmationEmail(confirmationMail));
        
        [HttpPost, Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPassword) 
            => Ok(await _authenticationService.ForgotPassword(forgotPassword));

        [HttpGet, Route("confirmEmail/{id}/{token}")]
        public async Task<IActionResult> ConfirmEmail(Guid id, string token)
        {
            var response = await _authenticationService.ConfirmEmail(id, token);
            if (response.IsSuccess is true)
            {
                return Redirect($"{AppSettings.Settings.ClientUri}/authentication/login");
            }
            return BadRequest();
        }

        [HttpPost, Route("resetPassword/{id}/{token}")]
        public async Task<IActionResult> ResetPassword(Guid id, string token, ResetPasswordRequest resetPassword) 
            => Ok(await _authenticationService.ResetPassword(resetPassword, id, token));

    }
}
