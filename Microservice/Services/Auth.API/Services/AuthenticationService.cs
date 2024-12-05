using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Shared.API.Response;
using Auth.API.Entities;
using Shared.API.Models;
using Shared.API.Enums;
using Auth.API.Infrastructure;
using System.Net;
using JWTAuthentication.JWT;

namespace Auth.API.Services
{
    public interface IAuthenticationService
    {
        Task<Unit> Register(RegisterRequest registerRequest);
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<Unit> ResendConfirmationEmail(ConfirmationMailRequest confimationMail);
        Task<Unit> ConfirmEmail(Guid id, string token);
        Task<Unit> ForgotPassword(ForgotPasswordRequest forgotPassword);
        Task<Unit> ResetPassword(ResetPasswordRequest resetPassword, Guid id, string token);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;
        public AuthenticationService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<Unit> Register(RegisterRequest registerRequest)
        {
            if (string.Equals(registerRequest.Email, AppSettings.Settings.AdminEmail, StringComparison.OrdinalIgnoreCase))
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "System error!"
                };
            }

            var hasUser = await _userManager.FindByEmailAsync(registerRequest.Email);

            if (hasUser is not null)
            {
                var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(hasUser);
                return isEmailConfirmed ? new() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "User already exits! Please Log in" }
                                        : new() { StatusCode = (int)HttpStatusCode.OK, IsSuccess = false, Message = "Existing user! please verify your email!" };
            }

            var user = new AppUser
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.Email.Split('@')[0],
                NormalizedEmail = registerRequest.Email,
                NormalizedUserName = registerRequest.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = false,
            };

            var newUser = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!newUser.Succeeded)
            {
                var errorList = newUser.Errors.ToList();
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = string.Join("/n", errorList.Select(e => e.Description))
                };
            }

            var hasRole = await _roleManager.RoleExistsAsync(UserRole.User.ToString());
            if (hasRole)
            {
                await _userManager.AddToRoleAsync(user, UserRole.User.ToString());
            }

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendEmailAsync(user.Id, user.Email, WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken)), EmailSubject.EmailConfirmation);

            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "An email sent to the user with a link for registration verification"
            };
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var hasUser = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (hasUser is null)
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "User doesn't exists!"
                };
            }

            if (!hasUser.EmailConfirmed)
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = false,
                    Message = "Please verify your email!"
                };
            }

            var login = await _signInManager.CheckPasswordSignInAsync(hasUser, loginRequest.Password, true);

            if (!login.Succeeded)
            {
                if (!login.IsLockedOut)
                {
                    return new()
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        IsSuccess = false,
                        Message = "Password is incorrect!"
                    };
                }

                return new()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = false,
                    Message = "Your account is locked out. Kindly wait for 10 minutes and try again!"
                };
            }

            var token = await CreateToken(hasUser);

            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Login successful!",
                Token = token
            };
        }

        public async Task<Unit> ResendConfirmationEmail(ConfirmationMailRequest confimationMail)
        {
            if (string.Equals(confimationMail.Email, AppSettings.Settings.AdminEmail, StringComparison.OrdinalIgnoreCase))
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "System error!"
                };
            }

            var hasUser = await _userManager.FindByEmailAsync(confimationMail.Email);

            if (hasUser is null)
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "User doesn't exists!"
                };
            }

            var IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(hasUser);
            if (IsEmailConfirmed)
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = false,
                    Message = "Email already verified!"
                };
            }

            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(hasUser);

            await _emailService.SendEmailAsync(hasUser.Id, hasUser.Email, WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken)), EmailSubject.EmailConfirmation);

            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "An email sent to the user with a link for registration verification"
            };
        }

        public async Task<Unit> ConfirmEmail(Guid id, string token)
        {
            var hasUser = await _userManager.FindByIdAsync(id.ToString());
            if (hasUser is null)
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "User doesn't exists!"
                };
            }

            if (string.IsNullOrEmpty(token))
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Token is not valid!"
                };
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            var response = await _userManager.ConfirmEmailAsync(hasUser, Encoding.UTF8.GetString(decodedToken));

            if (!response.Succeeded)
            {
                var errorList = response.Errors.ToList();
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = string.Join("/n", errorList.Select(e => e.Description))
                };
            }

            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Email verified successfully!"
            };
        }

        public async Task<Unit> ForgotPassword(ForgotPasswordRequest forgotPassword)
        {
            if (string.Equals(forgotPassword.Email, AppSettings.Settings.AdminEmail, StringComparison.OrdinalIgnoreCase))
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "System error!"
                };
            }

            var hasUser = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (hasUser is null)
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "User doesn't exists!"
                };
            }

            var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            await _emailService.SendEmailAsync(hasUser.Id, hasUser.Email, WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetPasswordToken)), EmailSubject.ResetPasswordEmail);
            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "An email sent to the user with a link for password reset"
            };
        }

        public async Task<Unit> ResetPassword(ResetPasswordRequest resetPassword, Guid id, string token)
        {
            var hasUser = await _userManager.FindByIdAsync(id.ToString());
            if (hasUser is null)
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "User doesn't exists!"
                };
            }

            if (string.IsNullOrEmpty(token))
            {
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Token is not valid!"
                };
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            var response = await _userManager.ResetPasswordAsync(hasUser, Encoding.UTF8.GetString(decodedToken), resetPassword.Password);

            if (!response.Succeeded)
            {
                var errorList = response.Errors.ToList();
                return new()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = string.Join("/n", errorList.Select(e => e.Description))
                };
            }

            if (await _userManager.IsLockedOutAsync(hasUser))
            {
                await _userManager.SetLockoutEndDateAsync(hasUser, DateTimeOffset.UtcNow);
            }

            return new()
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Password changed successfully!"
            };
        }

        private async Task<string> CreateToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTCredentials.TokenKey));
            var token = new JwtSecurityToken(
              issuer: JWTCredentials.ApiUri,
              expires: DateTime.Now.AddDays(3),
              claims: authClaims,
              signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
              );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
