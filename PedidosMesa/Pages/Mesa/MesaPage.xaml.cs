using PedidosMesa.Models;

namespace PedidosMesa.Pages.Mesa
{
    public partial class MesaPage : ContentPage
    {
        private readonly MesaViewModel _viewModel;

        public MesaPage(MesaViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        private async void OnDisponibleClicked(object sender, EventArgs e)
        {
            await _viewModel.AplicarFiltroAsync("Libre");
        }

        private async void OnOcupadaClicked(object sender, EventArgs e)
        {
            await _viewModel.AplicarFiltroAsync("Ocupado");
        }

        private async void OnTodosClicked(object sender, EventArgs e)
        {
            await _viewModel.AplicarFiltroAsync(null);
        }
    }
}