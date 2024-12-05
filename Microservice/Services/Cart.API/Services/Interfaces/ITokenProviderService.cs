namespace Cart.API.Services.Interfaces
{
    public interface ITokenProviderService
    {
        Task<string> GetTokenAsync();
    }
}
