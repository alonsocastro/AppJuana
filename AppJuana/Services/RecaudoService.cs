using AppJuana.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppJuana.Services
{
    public class RecaudoService : IRecaudoService
    {
        private readonly HttpClient _httpClient;

        public RecaudoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Recaudo>> GetRecaudosAsync(DateTime fechaInicial, DateTime fechaFinal, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token), "El token de autenticación no puede ser nulo o vacío.");
            }

            // Configurar el encabezado de autorización para esta petición específica
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Construir la URL con los parámetros de fecha en formato ISO 8601
            string url = $"transacciones/GetRecaudo/{fechaInicial:yyyy-MM-dd}/{fechaFinal:yyyy-MM-dd}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var recaudos = JsonSerializer.Deserialize<List<Recaudo>>(content, options);

                return recaudos ?? new List<Recaudo>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener recaudos: {ex.Message}");
                return new List<Recaudo>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error general: {ex.Message}");
                return new List<Recaudo>();
            }
        }
    }
}
