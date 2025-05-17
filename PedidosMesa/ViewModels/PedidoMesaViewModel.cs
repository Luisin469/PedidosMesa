using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PedidosMesa.Models;
using PedidosMesa.Pages.Popups;
using PedidosMesa.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PedidosMesa.ViewModels
{
    [QueryProperty(nameof(NombreMesa), "nombreMesa")]
    public partial class PedidoMesaViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

        private List<PedidoRequestModel> _todosLosProductos;
        private List<PedidoRequestModel> _todosLosProductosFiltrados;
        private Func<PedidoRequestModel, Task<string>> _mostrarPromptComentario;

        private CancellationTokenSource _debounceCts;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private ObservableCollection<PedidoRequestModel> productosFiltrados = new();

        [ObservableProperty]
        private bool isSearchNotEmpty;

        [ObservableProperty]
        private float totalPedido;

        private int _itemsPorPagina = 12;
        private int _paginaActual = 0;
        private bool _isCargandoMas = false;

        private string nombreMesa;
        public string NombreMesa
        {
            get => nombreMesa;
            set
            {
                if (SetProperty(ref nombreMesa, value))
                {
                    OnPropertyChanged(nameof(Titulo));
                }
            }
        }

        public string Titulo => $"Pedido mesa {NombreMesa}";

        public IRelayCommand CargarMasProductosCommand => new RelayCommand(async () => await CargarMasProductosAsync());

        public PedidoMesaViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _todosLosProductos = _dataService.GetProductosPedido();
            _ = FiltrarProductosAsync();
            RecalcularTotalPedido();
        }

        public void SetMostrarPromptComentario(Func<PedidoRequestModel, Task<string>> mostrarPrompt)
        {
            _mostrarPromptComentario = mostrarPrompt;
        }

        public ICommand VerComentarioCommand => new RelayCommand<PedidoRequestModel>(async (producto) =>
        {
            if (_mostrarPromptComentario == null || producto == null)
                return;

            var nuevoComentario = await _mostrarPromptComentario(producto);

            if (!string.IsNullOrWhiteSpace(nuevoComentario))
            {
                producto.Comentario = nuevoComentario.Trim();
                producto.EsModificado = true;
            }
        });

        [RelayCommand]
        private void ClearSearch()
        {
            SearchText = string.Empty;
        }

        public ICommand AgregarProductoCommand => new RelayCommand<PedidoRequestModel>((producto) =>
        {
            if (producto == null) return;

            int cantidadAnterior = producto.Cantidad;

            producto.Cantidad++;
            producto.EsModificado = true;

            ReubicarProducto(producto, cantidadAnterior);
            RecalcularTotalPedido();
        });

        [RelayCommand]
        private async Task EditarProductoAsync(PedidoRequestModel producto)
        {
            if (producto == null) return;

            int cantidadAnterior = producto.Cantidad;

            var popup = new EditarProductoPopup(producto, false);
            await Application.Current.MainPage.ShowPopupAsync(popup);

            ReubicarProducto(producto, cantidadAnterior);
            RecalcularTotalPedido();
        }


        partial void OnSearchTextChanged(string value)
        {
            DebounceFiltrarProductos();
        }

        private async Task FiltrarProductosAsync()
        {
            IsLoading = true;

            _todosLosProductosFiltrados = _todosLosProductos
                .Where(m =>
                    string.IsNullOrWhiteSpace(SearchText) || m.Descripcion.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            ProductosFiltrados.Clear();
            _paginaActual = 0;

            await CargarMasProductosAsync();

            IsSearchNotEmpty = !string.IsNullOrWhiteSpace(SearchText);

            IsLoading = false;
        }

        private async Task CargarMasProductosAsync()
        {
            if (_isCargandoMas) return;
            if (_todosLosProductosFiltrados == null || !_todosLosProductosFiltrados.Any()) return;

            _isCargandoMas = true;
            IsLoading = true;

            await Task.Delay(100);

            var siguientesProductos = _todosLosProductosFiltrados
                .Skip(_paginaActual * _itemsPorPagina)
                .Take(_itemsPorPagina)
                .ToList();

            foreach (var producto in siguientesProductos)
            {
                ProductosFiltrados.Add(producto);
            }

            _paginaActual++;
            _isCargandoMas = false;
            IsLoading = false;
        }

        private async void DebounceFiltrarProductos()
        {
            _debounceCts?.Cancel();
            _debounceCts = new CancellationTokenSource();
            var token = _debounceCts.Token;

            try
            {
                await Task.Delay(2000, token);

                if (!token.IsCancellationRequested)
                {
                    await Task.Run(() => FiltrarProductosAsync());
                }
            }
            catch (TaskCanceledException)
            {
            }
        }

        private void ReubicarProducto(PedidoRequestModel producto, int cantidadAnterior)
        {
            if (producto == null) return;

            if (cantidadAnterior > 0 && producto.Cantidad > 0)
                return;

            if (producto.Cantidad == 0)
            {
                MoverProductoDebajoDeConCantidadMayorCero(producto);
            }
            else if (cantidadAnterior == 0 && producto.Cantidad > 0)
            {
                MoverProductoDebajoDeConCantidadMayorCero(producto);
            }
        }

        private void MoverProductoDebajoDeConCantidadMayorCero(PedidoRequestModel producto)
        {
            if (ProductosFiltrados.Contains(producto))
            {
                ProductosFiltrados.Remove(producto);

                int index = ProductosFiltrados
                    .TakeWhile(p => p.Cantidad > 0)
                    .Count();

                ProductosFiltrados.Insert(index, producto);
            }

            if (_todosLosProductos.Contains(producto))
            {
                _todosLosProductos.Remove(producto);

                int indexTodos = _todosLosProductos
                    .TakeWhile(p => p.Cantidad > 0)
                    .Count();

                _todosLosProductos.Insert(indexTodos, producto);
            }
        }

        private void RecalcularTotalPedido()
        {
            TotalPedido = _todosLosProductos.Sum(static p => p.Total);
        }


        [RelayCommand]
        private void ConfirmarPedido()
        {
            // Aquí agregas la lógica para guardar o confirmar pedido
        }
    }
}
