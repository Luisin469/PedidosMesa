using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PedidosMesa.Models;
using PedidosMesa.Services;
using System.Collections.ObjectModel;

namespace PedidosMesa.ViewModels
{
    public partial class PedidoMesaViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

        private List<ProductoResponseModel> _todosLosProductos;
        private List<ProductoResponseModel> _todosLosProductosFiltrados;

        private CancellationTokenSource _debounceCts;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private ObservableCollection<ProductoResponseModel> productosFiltrados = new();

        [ObservableProperty]
        private bool isSearchNotEmpty;

        private int _itemsPorPagina = 10;
        private int _paginaActual = 0;
        private bool _isCargandoMas = false;

        public IRelayCommand CargarMasProductosCommand => new RelayCommand(async () => await CargarMasProductosAsync());

        public PedidoMesaViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _todosLosProductos = _dataService.GetProductos();
            _ = FiltrarProductosAsync();
        }

        [RelayCommand]
        private void ClearSearch()
        {
            SearchText = string.Empty;
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

            var siguientesMesas = _todosLosProductosFiltrados
                .Skip(_paginaActual * _itemsPorPagina)
                .Take(_itemsPorPagina)
                .ToList();

            foreach (var mesa in siguientesMesas)
            {
                ProductosFiltrados.Add(mesa);
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
    }
}