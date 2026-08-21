using AppJuana.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AppJuana.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(2); // Configurar un tiempo de espera mayor
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                loginRequest.tipo = "GESTION_INTEGRAL";

                string jsonRequest = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("Token", content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        // Treat empty successful response as an authentication failure.
                        return null;
                    }

                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        LoginResponse loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseBody, options);

                        if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                        {
                            return loginResponse;
                        }
                    }
                    catch (JsonException ex)
                    {
                        // If JSON parsing fails, it's not the expected response. Treat as auth failure.
                        Debug.WriteLine($"Error de deserialización JSON en LoginService: {ex.Message}");
                        return null;
                    }
                    
                    // Success status but invalid content (e.g., no token) is treated as an auth failure.
                    return null;
                }
                else
                {
                    // Treat client errors (e.g., bad credentials) as a recoverable auth failure.
                    // Also, handle the specific case where the server returns 500 for wrong credentials.
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                        response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                        response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        return null;
                    }
                    
                    // Other errors (server errors not related to credentials, etc.) should be thrown as exceptions.
                    throw new Exception($"Error del servidor (código: {(int)response.StatusCode}). Intente más tarde.");
                }
            }
            catch (HttpRequestException ex)
            {
                // Handles network-level errors (e.g., no connectivity)
                Debug.WriteLine($"Error de red en LoginService: {ex.Message}");
                throw new Exception("Error de conexión. Verifique su red e intente de nuevo.");
            }
            catch (TaskCanceledException ex)
            {
                // Handles timeouts
                Debug.WriteLine($"Timeout en LoginService: {ex.Message}");
                throw new Exception("La operación tardó demasiado en responder. Intente de nuevo.");
            }
            catch (Exception ex)
            {
                // Handles other unexpected errors (e.g., JSON deserialization)
                Debug.WriteLine($"Error inesperado en LoginService: {ex.Message}");
                throw new Exception("Ocurrió un error inesperado al procesar la respuesta.");
            }
        }
    }
}
