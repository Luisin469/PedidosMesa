using PedidosMesa.Services;
using PedidosMesa.ViewModels;

namespace PedidosMesa.Pages.Mesa
{
    public partial class MesaPage : ContentPage
    {
        private MesaViewModel viewModel;

        public MesaPage()
        {
            InitializeComponent();
            viewModel = new MesaViewModel(DataService.Instance);
            BindingContext = viewModel;
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.SearchText = e.NewTextValue;
        }

        private void OnDisponibleClicked(object sender, EventArgs e)
        {
            viewModel.EstablecerFiltroEstado("LIBRE");
        }

        private void OnOcupadaClicked(object sender, EventArgs e)
        {
            viewModel.EstablecerFiltroEstado("OCUPADO");
        }

        private void OnTodosClicked(object sender, EventArgs e)
        {
            viewModel.EstablecerFiltroEstado(null);
        }

        private void OnProductoClicked(object sender, EventArgs e)
        {
            // Implementar funcionalidad si aplica
        }

        private void OnFinalizarClicked(object sender, EventArgs e)
        {
            // Implementar funcionalidad si aplica
        }
    }
}