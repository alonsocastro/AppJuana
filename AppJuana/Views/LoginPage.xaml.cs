using AppJuana.ViewModels;

namespace AppJuana.Views;

public partial class LoginPage : ContentPage
{
    // El constructor ahora pide el ViewModel
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();

        // Asigna el ViewModel (que ya incluye el ILoginService)
        // al BindingContext de la página.
        BindingContext = viewModel;
    }
}