using AppJuana.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppJuana.ViewModels
{
    /// <summary>
    /// ViewModel para la pantalla principal (Dashboard / Home).
    /// Administra el saludo dinámico del usuario y las imágenes del carrusel de novedades.
    /// </summary>
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;

        /// <summary>
        /// Mensaje de bienvenida personalizado con el nombre del usuario autenticado.
        /// </summary>
        [ObservableProperty]
        private string _welcomeMessage = "¡Bienvenido!";

        /// <summary>
        /// Colección de fuentes de imagen remotas para el carrusel de campañas y premios.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UriImageSource> _carouselImageUrls;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="HomeViewModel"/> con la sesión y carga los banners iniciales.
        /// </summary>
        /// <param name="sessionService">Servicio de gestión de sesión del usuario.</param>
        public HomeViewModel(ISessionService sessionService)
        {
            _sessionService = sessionService;
            _carouselImageUrls = new ObservableCollection<UriImageSource>
            {
                new UriImageSource { Uri = new Uri("https://juanabonitavirtual.com.co/img/JB/Premio_1.webp") },
                new UriImageSource { Uri = new Uri("https://juanabonitavirtual.com.co/img/JB/Premio2.webp") },
                new UriImageSource { Uri = new Uri("https://juanabonitavirtual.com.co/img/JB/premio3.webp") },
                new UriImageSource { Uri = new Uri("https://juanabonitavirtual.com.co/img/JB/premio4.webp") }
            };
        }

        /// <summary>
        /// Carga los datos del usuario activo desde el almacenamiento seguro y formatea el saludo.
        /// </summary>
        public async Task LoadUserDataAsync()
        {
            var userName = await _sessionService.GetUserNameAsync();
            if (string.IsNullOrWhiteSpace(userName))
            {
                WelcomeMessage = "¡Hola!";
            }
            else
            {
                WelcomeMessage = $"¡Hola, {userName}!";
            }
        }
    }
}
