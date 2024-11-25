using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Auth.API.Infrastructure
{
    public class ResetPasswordEmailTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public ResetPasswordEmailTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<ResetPasswordEmailTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
    public class ResetPasswordEmailTokenProviderOptions : DataProtectionTokenProviderOptions
    {
    }
}
