using System;
using System.Linq;
using Microsoft.Maui.Controls;
using AppJuana.ViewModels;
using Microsoft.Maui.Controls.Shapes;

namespace AppJuana.Views
{
    public partial class HomeView : ContentPage
    {
        private readonly HomeViewModel _viewModel;
        private IDispatcherTimer _carouselTimer;
        private const int CarouselIntervalSeconds = 4;
        private const int ImageWidth = 320; // Define image width for scrolling calculation
        private int _currentImageIndex = 0;

        public HomeView(HomeViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        private void BuildImageCarousel()
        {
            // Ensure we run on the UI thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ImageContainer.Children.Clear();
                if (_viewModel.CarouselImageUrls == null) return;

                foreach (var imageSource in _viewModel.CarouselImageUrls)
                {
                    var image = new Image
                    {
                        Source = imageSource,
                        Aspect = Aspect.AspectFill,
                        HeightRequest = 180,
                        WidthRequest = ImageWidth
                    };

                    var border = new Border
                    {
                        StrokeShape = new RoundRectangle
                        {
                            CornerRadius = 10
                        },
                        Content = image,
                        // Ensure border inherits image size
                        HeightRequest = image.HeightRequest,
                        WidthRequest = image.WidthRequest,
                        Padding = 0, // No padding inside border by default
                        Margin = 0 // No margin by default
                    };
                    
                    ImageContainer.Children.Add(border);
                }

                // Start timer only if there are images
                if (ImageContainer.Children.Any())
                {
                    StartCarouselTimer();
                }
            });
        }

        private void StartCarouselTimer()
        {
            _carouselTimer?.Stop(); // Stop any existing timer
            _carouselTimer = Dispatcher.CreateTimer();
            _carouselTimer.Interval = TimeSpan.FromSeconds(CarouselIntervalSeconds);
            _carouselTimer.Tick += OnCarouselTimerTick;
            _carouselTimer.Start();
        }
        
        private async void OnCarouselTimerTick(object sender, EventArgs e)
        {
            _currentImageIndex++;
            if (_currentImageIndex >= ImageContainer.Children.Count)
            {
                _currentImageIndex = 0;
            }

            double scrollX = _currentImageIndex * (ImageWidth + ImageContainer.Spacing);
            await ImageScroller.ScrollToAsync(scrollX, 0, true);
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _carouselTimer?.Stop();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadUserDataAsync();
            BuildImageCarousel(); // Build the carousel here
        }

        async void OnGridButton1_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(RecaudoView));
        }

        async void OnGridButton2_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(OrderTracingView));
        }

        async void OnGridButton3_Click(object sender, EventArgs e)
        {
            await Shell.Current.DisplayAlert("Comercial", "Módulo de gestión comercial y seguimiento de metas.", "Aceptar");
        }

        async void OnGridButton4_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(OrderTracingView));
        }

        async void OnGridButton5_Click(object sender, EventArgs e)
        {
            await Shell.Current.DisplayAlert("Centro de Ayuda", "Para consultas sobre pedidos o recaudos, comunícate con soporte de Juana Bonita.", "Entendido");
        }

        async void OnGridButton6_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SettingsView));
        }

        async void OnNavHome_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        async void OnNavSearch_Click(object sender, EventArgs e)
        {
            // TODO: This navigation creates a page on the fly. For a more robust app,
            // this should be a registered route and view.
            await Shell.Current.Navigation.PushAsync(CreateSimplePage("Buscar", "Pantalla de bsqueda"));
        }

        async void OnNavAdd_Click(object sender, EventArgs e)
        {
            // TODO: This navigation creates a page on the fly. For a more robust app,
            // this should be a registered route and view.
            await Shell.Current.Navigation.PushAsync(CreateSimplePage("Crear", "Formulario de creacin"));
        }

        async void OnNavNotifications_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NotificationsView));
        }

        async void OnNavProfile_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProfileView));
        }

        Page CreateSimplePage(string title, string contentText)
        {
            return new ContentPage
            {
                Title = title,
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Children =
                    {
                        new Label { Text = contentText, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, FontSize = 18 }
                    }
                }
            };
        }
    }
}
