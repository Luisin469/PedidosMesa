using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PedidosMesa.Models;
using PedidosMesa.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;

namespace PedidosMesa.ViewModels
{
    public partial class MesaViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly ILoginService _loginService;

        private List<MesaResponseModel> _todasLasMesas;

        private CancellationTokenSource _debounceCts;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string estadoFiltro;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private ObservableCollection<MesaResponseModel> mesasFiltradas = new();

        [ObservableProperty]
        private int countMesas;

        [ObservableProperty]
        private bool isSearchNotEmpty;

        public MesaViewModel(IDataService dataService, ILoginService loginService)
        {
            _dataService = dataService;
            _loginService = loginService;
            _todasLasMesas = _dataService.GetMesas();
            _ = FiltrarMesasAsync();
        }

        [RelayCommand]
        private void ClearSearch()
        {
            SearchText = string.Empty;
        }

        partial void OnSearchTextChanged(string value)
        {
            DebounceFiltrarMesas();
        }

        partial void OnEstadoFiltroChanged(string value)
        {
            _ = OnEstadoFiltroChangedAsync(value);
        }

        private async Task OnEstadoFiltroChangedAsync(string value)
        {
            await Task.Run(() => FiltrarMesasAsync());
        }

        [RelayCommand]
        private void FiltrarLibre() => EstadoFiltro = "LIBRE";

        [RelayCommand]
        private void FiltrarOcupado() => EstadoFiltro = "OCUPADO";

        [RelayCommand]
        private async Task FiltrarTodos()
        {
            var login = _dataService.GetLogin();
            var mesasActualizadas = await _loginService.Login(login);

            if (mesasActualizadas != null)
            {
                _todasLasMesas = mesasActualizadas;
                _dataService.SetMesas(mesasActualizadas);
            }

            EstadoFiltro = null;
            await Task.Run(() => FiltrarMesasAsync());
        }

        private async Task FiltrarMesasAsync()
        {
            IsLoading = true;

            var filtradas = _todasLasMesas
                .Where(m =>
                    (string.IsNullOrWhiteSpace(SearchText) || m.Nombre.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(EstadoFiltro) || m.Estado == EstadoFiltro))
                .ToList();

            if (!filtradas.SequenceEqual(MesasFiltradas))
            {
                MesasFiltradas.Clear();
                filtradas.ForEach(m => MesasFiltradas.Add(m));
                CountMesas = MesasFiltradas.Count;
                IsSearchNotEmpty = !string.IsNullOrWhiteSpace(SearchText);
            }
            IsLoading = false;
        }

        private async void DebounceFiltrarMesas()
        {
            _debounceCts?.Cancel();
            _debounceCts = new CancellationTokenSource();
            var token = _debounceCts.Token;

            try
            {
                await Task.Delay(2000, token);

                if (!token.IsCancellationRequested)
                {
                    await Task.Run(() => FiltrarMesasAsync());
                }
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}