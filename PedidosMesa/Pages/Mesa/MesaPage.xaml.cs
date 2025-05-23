using PedidosMesa.Models;
using PedidosMesa.Services;
using PedidosMesa.ViewModels;
using static PedidosMesa.Services.DataService;

namespace PedidosMesa.Pages.Mesa
{
    public partial class MesaPage : ContentPage
    {
        private readonly IDataService _dataService;
        private readonly IPedidoMesaService _pedidoMesaService;

        public MesaPage(MesaViewModel viewModel, IPedidoMesaService pedidoMesaService, IDataService dataService)
        {
            InitializeComponent();
            _pedidoMesaService = pedidoMesaService;
            _dataService = dataService;
            BindingContext = viewModel;
        }

        private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (BindingContext is MesaViewModel vm)
            {
                vm.CargarMasMesasCommand.Execute(null);
            }
        }

        private async void OnProductoClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is MesaResponseModel mesa)
            {
                string nombreMesa = mesa.Nombre.Replace("Mesa ", "", StringComparison.OrdinalIgnoreCase);

                if (await ConsultaProductoPorMesaAsync(nombreMesa))
                {
                    await Shell.Current.GoToAsync($"PedidoMesaPage?nombreMesa={nombreMesa}");
                }
            }
        }

        private async void OnConfirmarClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is MesaResponseModel mesa)
            {
                string nombreMesa = mesa.Nombre.Replace("Mesa ", "", StringComparison.OrdinalIgnoreCase);

                if (await ConsultaProductoPorMesaConfirmacionAsync(nombreMesa))
                {
                    await Shell.Current.GoToAsync($"CerrarMesaPage?nombreMesa={nombreMesa}");
                }
            }
        }

        public async Task<bool> ConsultaProductoPorMesaAsync(string mesa)
        {

            List<PedidoRequestModel> productoPedidoData = await _pedidoMesaService.ConsultaProductosPorMesa(mesa);

            if (productoPedidoData != null && productoPedidoData.Count > 0)
            {
                var productosConCantidad = productoPedidoData.OrderByDescending(p => p.Cantidad).ToList();

                if (productosConCantidad.Count > 0)
                {
                    _dataService.SetProductosPedido(productosConCantidad);
                    return true;
                }
                return false;
            }
            else
            {
                await DisplayAlert("Advertencia", "No tiene producto.", "Ok");
                return false;
            }
        }

        public async Task<bool> ConsultaProductoPorMesaConfirmacionAsync(string mesa)
        {

            List<PedidoRequestModel> productoPedidoData = await _pedidoMesaService.ConsultaProductosPorMesa(mesa);

            if (productoPedidoData != null && productoPedidoData.Count > 0)
            {
                var productosConCantidad = productoPedidoData.Where(x => x.Cantidad > 0).OrderByDescending(p => p.Cantidad).ToList();

                if (productosConCantidad.Count > 0)
                {
                    _dataService.SetProductosPedido(productosConCantidad);
                    return true;
                }
                return false;
            }
            else
            {
                await DisplayAlert("Advertencia", "No tiene producto.", "Ok");
                return false;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (AppState.DebeActualizarMesas && BindingContext is MesaViewModel vm)
            {
                await vm.FiltrarTodosCommand.ExecuteAsync(null);
                AppState.DebeActualizarMesas = false;
            }
        }

    }
}