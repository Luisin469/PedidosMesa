using PedidosMesa.ViewModels;

namespace PedidosMesa.Pages.Producto
{
    public partial class ProductoPage : ContentPage
    {
        public ProductoPage(ProductoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (BindingContext is ProductoViewModel vm)
            {
                vm.CargarMasProductosCommand.Execute(null);
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}