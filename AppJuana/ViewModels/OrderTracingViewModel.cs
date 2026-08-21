using AppJuana.Models;
using AppJuana.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppJuana.ViewModels
{
    /// <summary>
    /// ViewModel para la consulta y seguimiento del estado logístico de pedidos (Order Tracing).
    /// Estandarizado con CommunityToolkit.Mvvm.
    /// </summary>
    public partial class OrderTracingViewModel : ObservableObject
    {
        private readonly IOrderTracingService _orderTracingService;
        private readonly ISessionService _sessionService;

        /// <summary>
        /// Indica si hay una consulta en proceso en la red.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        /// <summary>
        /// Propiedad calculada que indica si la interfaz está disponible.
        /// </summary>
        public bool IsNotBusy => !IsBusy;

        /// <summary>
        /// Colección observable con el desglose de pedidos por grupos/zonas.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<OrderTracing> _orderTracingData;

        /// <summary>
        /// Lista de tipos de consulta o seguimiento disponibles.
        /// </summary>
        public List<string> TracingTypes { get; }

        /// <summary>
        /// Tipo de seguimiento seleccionado actualmente por el usuario.
        /// </summary>
        [ObservableProperty]
        private string _selectedTracingType;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="OrderTracingViewModel"/> con sus dependencias.
        /// </summary>
        /// <param name="orderTracingService">Servicio para consulta del pipeline de pedidos.</param>
        /// <param name="sessionService">Servicio de gestión de sesión segura.</param>
        public OrderTracingViewModel(IOrderTracingService orderTracingService, ISessionService sessionService)
        {
            _orderTracingService = orderTracingService;
            _sessionService = sessionService;
            _orderTracingData = new ObservableCollection<OrderTracing>();
            TracingTypes = new List<string> { "PEDIDOS", "SIGUIENTE" };
            _selectedTracingType = TracingTypes.FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Realiza la consulta del estado de pedidos en la API según el tipo de filtro seleccionado.
        /// Genera automáticamente el comando <see cref="ConsultarCommand"/>.
        /// </summary>
        [RelayCommand]
        private async Task ConsultarAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                OrderTracingData.Clear();

                // Validación de sesión activa
                string? token = await _sessionService.GetTokenAsync();
                if (string.IsNullOrWhiteSpace(token))
                {
                    await Shell.Current.DisplayAlert(
                        "Sesión Expirada",
                        "No se ha encontrado una sesión activa. Por favor inicia sesión nuevamente.",
                        "Aceptar");
                    await Shell.Current.GoToAsync("//LoginPage");
                    return;
                }

                // Llamada al servicio de tracking
                var result = await _orderTracingService.GetOrderTracingDataAsync(SelectedTracingType, token);

                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        OrderTracingData.Add(item);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert(
                        "Sin Información",
                        "No se encontró información de pedidos para el tipo de consulta seleccionado.",
                        "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Error de Consulta",
                    $"Ocurrió un error al consultar el seguimiento de pedidos: {ex.Message}",
                    "Aceptar");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
