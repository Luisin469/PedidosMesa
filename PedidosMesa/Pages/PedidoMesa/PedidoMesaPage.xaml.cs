using PedidosMesa.ViewModels;

namespace PedidosMesa.Pages.PedidoMesa
{
    public partial class PedidoMesaPage : ContentPage
    {
        public PedidoMesaPage(PedidoMesaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }


        private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (BindingContext is PedidoMesaViewModel vm)
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