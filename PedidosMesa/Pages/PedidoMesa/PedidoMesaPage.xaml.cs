using PedidosMesa.ViewModels;

namespace PedidosMesa.Pages.PedidoMesa
{
    public partial class PedidoMesaPage : ContentPage
    {
        public PedidoMesaPage(PedidoMesaViewModel viewModel)
        {
            InitializeComponent();
            FloatingButtonBorder.Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Color.FromArgb("#88000000")),
                Offset = new Point(2, 2),
                Radius = 4,
                Opacity = 0.8f
            };
            BindingContext = viewModel;
        }

        private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (BindingContext is PedidoMesaViewModel vm)
            {
                vm.CargarMasProductosCommand.Execute(null);
            }
        }

        private void FloatingButton_Clicked(object sender, EventArgs e)
        {
            // Aquí tu lógica para cuando se pulse el botón + 
            // Por ejemplo, navegar a otra página o abrir un modal
        }


        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}