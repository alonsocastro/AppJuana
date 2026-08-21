using AppJuana.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace AppJuana.ViewModels
{
    /// <summary>
    /// ViewModel para la gestión y visualización de notificaciones del sistema y novedades.
    /// Estandarizado con CommunityToolkit.Mvvm.
    /// </summary>
    public partial class NotificationsViewModel : ObservableObject
    {
        /// <summary>
        /// Colección observable de notificaciones del usuario.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Notification> _notifications;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="NotificationsViewModel"/> y carga las notificaciones iniciales.
        /// </summary>
        public NotificationsViewModel()
        {
            _notifications = new ObservableCollection<Notification>();
            LoadNotifications();
        }

        /// <summary>
        /// Carga y prepara el listado de notificaciones para la vista.
        /// </summary>
        private void LoadNotifications()
        {
            Notifications = new ObservableCollection<Notification>
            {
                new Notification 
                { 
                    Icon = "pedidos.png", 
                    IconBackgroundColor = Color.FromArgb("#E9F5FF"),
                    Title = "Nuevo Pedido Recibido", 
                    Message = "Has recibido un nuevo pedido de 5 unidades.", 
                    Timestamp = "Hace 5m" 
                },
                new Notification 
                { 
                    Icon = "cartera1.png", 
                    IconBackgroundColor = Color.FromArgb("#FFF4E6"),
                    Title = "Pago Procesado", 
                    Message = "El pago de $250.000 ha sido procesado exitosamente.", 
                    Timestamp = "Hace 1h" 
                },
                new Notification 
                { 
                    Icon = "transporte.png", 
                    IconBackgroundColor = Color.FromArgb("#FEEFF2"),
                    Title = "Envío en Camino", 
                    Message = "Tu paquete ha sido despachado y está en camino.", 
                    Timestamp = "Hace 3h" 
                },
                new Notification 
                { 
                    Icon = "comercial.png", 
                    IconBackgroundColor = Color.FromArgb("#E6FFF1"),
                    Title = "Promoción Especial", 
                    Message = "Aprovecha el 20% de descuento en todos los productos.", 
                    Timestamp = "Ayer" 
                }
            };
        }
    }
}
