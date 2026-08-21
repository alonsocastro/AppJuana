using AppJuana.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppJuana.Services
{


    public class OrderTracingService : IOrderTracingService
    {
    
        private readonly HttpClient _httpClient;

        public OrderTracingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<OrderTracing>> GetOrderTracingDataAsync(string filter, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token), "El token de autenticación no puede ser nulo o vacío.");
            }

            // Configurar el encabezado de autorización para esta petición específica
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Construir la URL con los parámetros de fecha en formato ISO 8601
            string ctrl = "undefined";
            string url = $"facturacion/GetCargarOrderTracing/{filter}/{ctrl}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var orderTracing = JsonSerializer.Deserialize<List<OrderTracing>>(content, options);

                return orderTracing ?? new List<OrderTracing>();
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener Pedidos: {ex.Message}");
                return new List<OrderTracing>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error general: {ex.Message}");
                return new List<OrderTracing>();
            }
        }
    }
}