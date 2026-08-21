using AppJuana.Services;
using System;

namespace AppJuana.Views;

public partial class SettingsView : ContentPage
{
    private readonly ISessionService _sessionService;

	public SettingsView(ISessionService sessionService)
	{
		InitializeComponent();
        _sessionService = sessionService;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            var userName = await _sessionService.GetUserNameAsync();
            if (!string.IsNullOrEmpty(userName))
            {
                LblUserName.Text = userName;
            }
        }
        catch
        {
            // Ignorar en caso de error
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Cerrar Sesión", "¿Estás seguro de que deseas salir?", "Sí, salir", "Cancelar");
        if (confirm)
        {
            await _sessionService.ClearSessionAsync();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}