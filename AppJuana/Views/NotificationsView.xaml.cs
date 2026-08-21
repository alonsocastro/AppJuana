using AppJuana.ViewModels;

namespace AppJuana.Views;

public partial class NotificationsView : ContentPage
{
	public NotificationsView(NotificationsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
