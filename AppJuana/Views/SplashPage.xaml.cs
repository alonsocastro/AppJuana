using System.Threading.Tasks;
using Microsoft.Maui.Dispatching;

namespace AppJuana.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();

        _ = Task.Run(async () =>
        {
            await Task.Delay(2000);
            await Dispatcher.DispatchAsync(async () =>
            {
                await Shell.Current.GoToAsync("//LoginPage");
            });
        });
    }
}