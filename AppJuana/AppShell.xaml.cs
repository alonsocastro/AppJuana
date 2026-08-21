using AppJuana.Views;

namespace AppJuana
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute(nameof(NotificationsView), typeof(NotificationsView));
            Routing.RegisterRoute(nameof(RecaudoView), typeof(RecaudoView));
            Routing.RegisterRoute(nameof(OrderTracingView), typeof(OrderTracingView));
            Routing.RegisterRoute(nameof(ProfileView), typeof(ProfileView));
            Routing.RegisterRoute(nameof(SplashPage), typeof(SplashPage));
            Routing.RegisterRoute(nameof(SettingsView), typeof(SettingsView));
        }
    }
}
