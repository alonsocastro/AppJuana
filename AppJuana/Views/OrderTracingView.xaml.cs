using AppJuana.ViewModels;

namespace AppJuana.Views;

public partial class OrderTracingView : ContentPage
{
	public OrderTracingView(OrderTracingViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
