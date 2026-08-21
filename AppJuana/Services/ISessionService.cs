using System.Threading.Tasks;

namespace AppJuana.Services
{
    public interface ISessionService
    {
        Task SaveSessionAsync(string token, string userName, string? userId = null);
        Task<string?> GetTokenAsync();
        Task<string?> GetUserNameAsync();
        Task<string?> GetUserIdAsync();
        Task<bool> IsAuthenticatedAsync();
        Task ClearSessionAsync();
    }
}
