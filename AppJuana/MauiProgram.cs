using AppJuana.Services;
using AppJuana.ViewModels;
using AppJuana.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace AppJuana
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // =======================================================
            // AÑADIR ESTAS LÍNEAS PARA INYECCIÓN DE DEPENDENCIAS
            // =======================================================

#if DEBUG
            // Handler para ignorar la validación de certificados SSL en modo DEBUG.
            // ADVERTENCIA: ¡NO USAR EN PRODUCCIÓN!
            var insecureHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
#endif

            // Configuración de HttpClient para LoginService
            var loginClientBuilder = builder.Services.AddHttpClient<ILoginService, LoginService>(client =>
            {
                client.BaseAddress = new Uri("https://juanabapl.juanabonita.com.co:446/JuanaBonitaAPI/api/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(30); // Añadir tiempo de espera
                client.DefaultRequestHeaders.ExpectContinue = false; // Desactivar 'Expect: 100-continue'
            });

            // Configuración de HttpClient para RecaudoService
            var recaudoClientBuilder = builder.Services.AddHttpClient<IRecaudoService, RecaudoService>(client =>
            {
                client.BaseAddress = new Uri("https://juanabapl.juanabonita.com.co:446/JuanaBonitaAPI/api/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(30); // Añadir tiempo de espera
                client.DefaultRequestHeaders.ExpectContinue = false; // Desactivar 'Expect: 100-continue'
            });

            // Configuración de HttpClient para OrderTracingService
            var orderTracingClientBuilder = builder.Services.AddHttpClient<IOrderTracingService, OrderTracingService>(client =>
            {
                client.BaseAddress = new Uri("https://juanabapl.juanabonita.com.co:446/JuanaBonitaAPI/api/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(30); // Añadir tiempo de espera
                client.DefaultRequestHeaders.ExpectContinue = false; // Desactivar 'Expect: 100-continue'
            });

#if DEBUG
            // Aplicar el handler inseguro a todos los clientes en modo DEBUG
            loginClientBuilder.ConfigurePrimaryHttpMessageHandler(() => insecureHandler);
            recaudoClientBuilder.ConfigurePrimaryHttpMessageHandler(() => insecureHandler);
            orderTracingClientBuilder.ConfigurePrimaryHttpMessageHandler(() => insecureHandler);
#endif

            // Servicios de Sesión y Autenticación
            builder.Services.AddSingleton<ISessionService, SessionService>();

            // Vistas (Transient: Se crea una nueva cada vez que se pide)
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<HomeView>();
            builder.Services.AddTransient<NotificationsView>();
            builder.Services.AddTransient<RecaudoView>();
            builder.Services.AddTransient<OrderTracingView>();
            builder.Services.AddTransient<ProfileView>();
            builder.Services.AddTransient<SplashPage>();
            builder.Services.AddTransient<SettingsView>();

            // ViewModels (Transient)
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<NotificationsViewModel>();
            builder.Services.AddTransient<RecaudoViewModel>();
            builder.Services.AddTransient<OrderTracingViewModel>();


            // =iat
            // ... (resto de registros)
            // =======================================================

            return builder.Build();
        }
    }
}
