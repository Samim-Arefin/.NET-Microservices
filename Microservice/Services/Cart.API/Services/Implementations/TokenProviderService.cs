using Cart.API.Services.Interfaces;

namespace Cart.API.Services.Implementations
{
    public class TokenProviderService : ITokenProviderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenProviderService(IHttpContextAccessor httpContextAccessor) 
            => _httpContextAccessor = httpContextAccessor;

        public async Task<string> GetTokenAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            if(context is null)
            {
                return string.Empty;
            }

            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
              return string.Empty;
            }

            var token = authorizationHeader.Split(' ')[1];

            return token;
        }
    }
}
