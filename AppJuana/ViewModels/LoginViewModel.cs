using AppJuana.Models;
using AppJuana.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppJuana.ViewModels
{
    /// <summary>
    /// ViewModel encargado de la lógica de autenticación e inicio de sesión.
    /// Estandarizado con CommunityToolkit.Mvvm.
    /// </summary>
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ILoginService _loginService;
        private readonly ISessionService _sessionService;
        private CancellationTokenSource? _cancellationTokenSource;

        /// <summary>
        /// Nombre de usuario o cédula ingresado.
        /// </summary>
        [ObservableProperty]
        private string _username = string.Empty;

        /// <summary>
        /// Contraseña de acceso ingresada.
        /// </summary>
        [ObservableProperty]
        private string _password = string.Empty;

        /// <summary>
        /// Mensaje descriptivo de error en caso de fallo en autenticación.
        /// </summary>
        [ObservableProperty]
        private string _errorMessage = string.Empty;

        /// <summary>
        /// Determina la visibilidad del banner de error en la vista.
        /// </summary>
        [ObservableProperty]
        private bool _isErrorVisible;

        /// <summary>
        /// Indica si el proceso de autenticación está en ejecución.
        /// Notifica a IsNotBusy automáticamente.
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        /// <summary>
        /// Indica si los controles de entrada están habilitados para interacción.
        /// </summary>
        public bool IsNotBusy => !IsBusy;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="LoginViewModel"/> con sus dependencias requeridas.
        /// </summary>
        /// <param name="loginService">Servicio para consumo del endpoint de autenticación.</param>
        /// <param name="sessionService">Servicio para guardado de sesión segura.</param>
        public LoginViewModel(ILoginService loginService, ISessionService sessionService)
        {
            _loginService = loginService;
            _sessionService = sessionService;
        }

        /// <summary>
        /// Valida las credenciales ingresadas y realiza la petición asíncrona de autenticación.
        /// Al tener éxito, guarda la sesión cifrada y redirige a la pantalla principal.
        /// </summary>
        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Por favor, ingresa usuario y contraseña.";
                IsErrorVisible = true;
                return;
            }

            if (IsBusy) 
                return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;
                IsErrorVisible = false;
                _cancellationTokenSource = new CancellationTokenSource();

                var loginRequest = new LoginRequest
                {
                    usuario = this.Username,
                    clave = this.Password
                };

                LoginResponse response = await _loginService.LoginAsync(loginRequest, _cancellationTokenSource.Token);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    // Guardado seguro de credenciales
                    await _sessionService.SaveSessionAsync(response.Token, response.nombre, response.UserId);
                    
                    // Navegación al Dashboard principal
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    ErrorMessage = "Usuario o contraseña incorrectos.";
                    IsErrorVisible = true;
                }
            }
            catch (OperationCanceledException)
            {
                if (_cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested)
                {
                    ErrorMessage = "La operación de inicio de sesión fue cancelada.";
                }
                else
                {
                    ErrorMessage = "El servidor no respondió a tiempo. Verifica tu conexión a internet o intenta más tarde.";
                }
                IsErrorVisible = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                IsErrorVisible = true;
            }
            finally
            {
                IsBusy = false;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Cancela la operación de inicio de sesión si se encuentra en progreso.
        /// </summary>
        [RelayCommand]
        private void CancelLogin()
        {
            if (IsBusy && _cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
        }
    }
}
