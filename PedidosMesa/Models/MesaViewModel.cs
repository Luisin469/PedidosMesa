using PedidosMesa.Services;
using System.Collections.ObjectModel;

namespace PedidosMesa.Models
{
    public class MesaViewModel : BindableObject
    {
        private readonly IDataService _dataService;
        private List<MesaResponseModel> MesaDataOriginal { get; set; } = new List<MesaResponseModel>();
        public ObservableCollection<MesaResponseModel> FilteredMesaData { get; set; } = new ObservableCollection<MesaResponseModel>();
        public int CountMesas => FilteredMesaData?.Count ?? 0;

        public MesaViewModel(IDataService dataService)
        {
            _dataService = dataService;
            LoadData();
        }

        private async void LoadData()
        {
            MesaDataOriginal = _dataService.MesaData ?? new List<MesaResponseModel>();
            await AplicarFiltroAsync(null);
        }

        public async Task AplicarFiltroAsync(string? estado)
        {
            var resultado = string.IsNullOrEmpty(estado)
                ? MesaDataOriginal
                : MesaDataOriginal.Where(mesa => mesa.Estado.Contains(estado, StringComparison.OrdinalIgnoreCase)).ToList();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                FilteredMesaData.Clear();

                foreach (var mesa in resultado)
                    FilteredMesaData.Add(mesa);

                OnPropertyChanged(nameof(CountMesas));
            });
        }

    }
}
