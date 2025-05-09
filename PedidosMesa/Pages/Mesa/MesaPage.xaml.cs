using PedidosMesa.ViewModels;

namespace PedidosMesa.Pages.Mesa
{
    public partial class MesaPage : ContentPage
    {
        public MesaPage(MesaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnProductoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//PedidoMesaPage");
        }
    }
}