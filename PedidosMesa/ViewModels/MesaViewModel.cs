using PedidosMesa.Models;
using PedidosMesa.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PedidosMesa.ViewModels
{
    public class MesaViewModel : INotifyPropertyChanged
    {
        private readonly IDataService _dataService;

        private List<MesaResponseModel> _todasLasMesas;
        private string _estadoFiltro = null;

        private ObservableCollection<MesaResponseModel> _mesasFiltradas = new();
        public ObservableCollection<MesaResponseModel> MesasFiltradas
        {
            get => _mesasFiltradas;
            set
            {
                if (_mesasFiltradas != value)
                {
                    _mesasFiltradas = value;
                    OnPropertyChanged(nameof(MesasFiltradas));
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FiltrarMesas();
                }
            }
        }

        public int CountMesas => MesasFiltradas?.Count ?? 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public MesaViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _todasLasMesas = _dataService.GetMesas();

            // Llenamos una vez, sin reemplazar la colección
            foreach (var mesa in _todasLasMesas)
            {
                MesasFiltradas.Add(mesa);
            }
        }

        public void EstablecerFiltroEstado(string estado)
        {
            _estadoFiltro = estado;
            FiltrarMesas();
        }

        private void FiltrarMesas()
        {
            var filtradas = _todasLasMesas
                .Where(m =>
                    (string.IsNullOrWhiteSpace(SearchText) || m.Nombre.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(_estadoFiltro) || m.Estado == _estadoFiltro))
                .ToList();

            // Limpiamos la lista y agregamos los elementos filtrados
            MesasFiltradas.Clear();

            foreach (var mesa in filtradas)
            {
                MesasFiltradas.Add(mesa);
            }

            // Notificamos que la propiedad CountMesas ha cambiado
            OnPropertyChanged(nameof(CountMesas));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
