using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PedidosMesa.Models;
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

        private int _itemsPorPagina = 10;
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
           }
       });

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


    }
}
