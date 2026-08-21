using AppJuana.ViewModels;

namespace AppJuana.Views;

public partial class RecaudoView : ContentPage
{
	public RecaudoView(RecaudoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}