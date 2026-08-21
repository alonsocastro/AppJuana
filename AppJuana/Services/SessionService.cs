using Microsoft.Maui.Storage;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AppJuana.Services
{
    public class SessionService : ISessionService
    {
        private const string TokenKey = "auth_token";
        private const string UserNameKey = "user_name";
        private const string UserIdKey = "user_id";

        public async Task SaveSessionAsync(string token, string userName, string? userId = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    await SecureStorage.SetAsync(TokenKey, token);
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    await SecureStorage.SetAsync(UserNameKey, userName);
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    await SecureStorage.SetAsync(UserIdKey, userId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SessionService] Error al guardar la sesión: {ex.Message}");
            }
        }

        public async Task<string?> GetTokenAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(TokenKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SessionService] Error al obtener el token: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> GetUserNameAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(UserNameKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SessionService] Error al obtener el nombre de usuario: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> GetUserIdAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(UserIdKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SessionService] Error al obtener el ID de usuario: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var token = await GetTokenAsync();
                return !string.IsNullOrWhiteSpace(token);
            }
            catch
            {
                return false;
            }
        }

        public async Task ClearSessionAsync()
        {
            try
            {
                SecureStorage.Remove(TokenKey);
                SecureStorage.Remove(UserNameKey);
                SecureStorage.Remove(UserIdKey);

                // Limpieza de claves heredadas de Preferences si existen
                Preferences.Remove("AuthToken");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SessionService] Error al limpiar la sesión: {ex.Message}");
            }

            await Task.CompletedTask;
        }
    }
}
