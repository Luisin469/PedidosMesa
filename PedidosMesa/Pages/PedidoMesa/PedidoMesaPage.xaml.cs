using PedidosMesa.Models;
using PedidosMesa.Services;
using PedidosMesa.ViewModels;

namespace PedidosMesa.Pages.PedidoMesa
{
    public partial class PedidoMesaPage : ContentPage
    {
        private readonly IDataService _dataService;
        private readonly IPedidoMesaService _pedidoMesaService;

        public PedidoMesaPage(PedidoMesaViewModel viewModel, IPedidoMesaService pedidoMesaService, IDataService dataService)
        {
            _pedidoMesaService = pedidoMesaService;
            _dataService = dataService;
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

        private async void OnFinalizarPedidoClicked(object sender, EventArgs e)
        {
            var vm = BindingContext as PedidoMesaViewModel;
            if (vm == null) return;

            try
            {
                string observacioon = string.Empty;

                string result = await DisplayPromptAsync(
                "Finzalizar pedido",
                "Observación:",
                accept: "Finalizar",
                cancel: "Cancelar",
                placeholder: "Ingrese una observación",
                initialValue: "",
                maxLength: 200,
                keyboard: Keyboard.Default);

                if (result == null)
                {
                    return;
                }

                vm.IsLoading = true;
                string observacion = result.Trim();
                string numeroMesa = (BindingContext as PedidoMesaViewModel)?.NombreMesa ?? "0";

                var cabecera = new PedidoMesaCabeceraRequest
                {
                    NUMMESA = numeroMesa,
                    OBSERVACION = observacioon,
                    USUARIO = _dataService.GetLogin().Usuario
                };

                var detalle = vm?.ProductosFiltrados
                    .Where(p => p.EsModificado)
                    .Select(p => new PedidoMesaDetalleRequest
                    {
                        cantidad = p.Cantidad.ToString(),
                        codigo = p.Codigo.ToString(),
                        comentario = p.Comentario ?? ""
                    }).ToList() ?? new List<PedidoMesaDetalleRequest>();

                if (detalle.Count == 0)
                {
                    await DisplayAlert("Aviso", "No hay productos modificados para enviar.", "OK");
                    return;
                }

                bool resultado = await _pedidoMesaService.ConfirmarPedidoAsync(cabecera, detalle);

                vm.IsLoading = false;

                if (resultado)
                {
                    await DisplayAlert("Éxito", "Pedido confirmado.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo confirmar el pedido.", "OK");
                }
            }
            catch (Exception ex)
            {
                vm.IsLoading = false;
                await DisplayAlert("Error", "Ocurrió un error inesperado.", "OK");
            }
            finally
            {
                vm.IsLoading = false;
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}