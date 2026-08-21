using AppJuana.Models;
using AppJuana.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppJuana.ViewModels
{
    /// <summary>
    /// ViewModel para la gestión y consulta de recaudos bancarios y estado de cartera.
    /// Estandarizado con CommunityToolkit.Mvvm.
    /// </summary>
    public partial class RecaudoViewModel : ObservableObject
    {
        private readonly IRecaudoService _recaudoService;
        private readonly ISessionService _sessionService;

        /// <summary>
        /// Indica si hay una operación en proceso (carga / consulta en red).
        /// Notifica automáticamente el cambio de estado a IsNotBusy.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        /// <summary>
        /// Propiedad calculada que indica si la interfaz está libre para interactuar.
        /// </summary>
        public bool IsNotBusy => !IsBusy;

        /// <summary>
        /// Colección observable con el desglose de recaudos obtenidos por entidad bancaria.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Recaudo> _recaudos;

        /// <summary>
        /// Fecha inicial para el filtro de consulta de recaudos.
        /// </summary>
        [ObservableProperty]
        private DateTime _fechaInicial = DateTime.Now;

        /// <summary>
        /// Fecha final para el filtro de consulta de recaudos.
        /// </summary>
        [ObservableProperty]
        private DateTime _fechaFinal = DateTime.Now;

        /// <summary>
        /// Monto total global recaudado en el periodo consultado.
        /// </summary>
        [ObservableProperty]
        private decimal _totalRecaudosGlobal;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="RecaudoViewModel"/> con sus dependencias.
        /// </summary>
        /// <param name="recaudoService">Servicio para consumo del endpoint de recaudos.</param>
        /// <param name="sessionService">Servicio de gestión de sesión segura.</param>
        public RecaudoViewModel(IRecaudoService recaudoService, ISessionService sessionService)
        {
            _recaudoService = recaudoService;
            _sessionService = sessionService;
            _recaudos = new ObservableCollection<Recaudo>();
        }

        /// <summary>
        /// Ejecuta la consulta de recaudos en la API para el rango de fechas seleccionado.
        /// Valida que las fechas sean coherentes y que exista un token de sesión activo.
        /// Genera automáticamente el comando <see cref="ConsultarCommand"/>.
        /// </summary>
        [RelayCommand]
        private async Task ConsultarAsync()
        {
            if (IsBusy)
                return;

            try
            {
                // Validación del rango de fechas
                if (FechaInicial > FechaFinal)
                {
                    await Shell.Current.DisplayAlert(
                        "Rango Inválido",
                        "La fecha inicial no puede ser superior a la fecha final.",
                        "Aceptar");
                    return;
                }

                IsBusy = true;

                // Obtención del token JWT seguro
                string? token = await _sessionService.GetTokenAsync();
                if (string.IsNullOrWhiteSpace(token))
                {
                    await Shell.Current.DisplayAlert(
                        "Sesión Expirada",
                        "No se ha encontrado una sesión válida. Por favor inicia sesión nuevamente.",
                        "Aceptar");
                    await Shell.Current.GoToAsync("//LoginPage");
                    return;
                }

                // Petición al servicio de recaudos
                var recaudosResult = await _recaudoService.GetRecaudosAsync(FechaInicial, FechaFinal, token);

                Recaudos.Clear();
                if (recaudosResult != null && recaudosResult.Any())
                {
                    foreach (var recaudo in recaudosResult)
                    {
                        Recaudos.Add(recaudo);
                    }

                    // Se asigna el consolidado global proveniente de la respuesta
                    TotalRecaudosGlobal = recaudosResult.FirstOrDefault()?.consinaGlobal ?? 0;
                }
                else
                {
                    TotalRecaudosGlobal = 0;
                    await Shell.Current.DisplayAlert(
                        "Sin Datos",
                        "No se encontraron recaudos registrados para el rango de fechas seleccionado.",
                        "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Error de Consulta",
                    $"Ocurrió un error al obtener los recaudos: {ex.Message}",
                    "Aceptar");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}