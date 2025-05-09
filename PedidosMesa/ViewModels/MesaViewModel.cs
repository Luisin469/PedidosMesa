using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PedidosMesa.Models;
using PedidosMesa.Services;
using System.Collections.ObjectModel;

namespace PedidosMesa.ViewModels
{
    public partial class MesaViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

        private List<MesaResponseModel> _todasLasMesas;

        private CancellationTokenSource _debounceCts;

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

        public MesaViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _todasLasMesas = _dataService.GetMesas();
            FiltrarMesas();
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

        partial void OnEstadoFiltroChanged(string value) => FiltrarMesas();

        [RelayCommand]
        private void FiltrarLibre() => EstadoFiltro = "LIBRE";

        [RelayCommand]
        private void FiltrarOcupado() => EstadoFiltro = "OCUPADO";

        [RelayCommand]
        private void FiltrarTodos() => EstadoFiltro = null;

        private void FiltrarMesas()
        {
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
                    FiltrarMesas();
                    OnPropertyChanged(nameof(IsSearchNotEmpty));
                }
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}