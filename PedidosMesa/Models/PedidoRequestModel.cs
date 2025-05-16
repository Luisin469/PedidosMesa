using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PedidosMesa.Models
{
    public class PedidoRequestModel : INotifyPropertyChanged
    {
        private double _codigo;
        private string _descripcion;
        private float _precio;
        private int _cantidad;
        private string _comentario;
        private bool _esModificado;

        public double Codigo
        {
            get => _codigo;
            set => SetProperty(ref _codigo, value);
        }

        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        public float Precio
        {
            get => _precio;
            set
            {
                if (SetProperty(ref _precio, value))
                    OnPropertyChanged(nameof(Total));
            }
        }

        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (SetProperty(ref _cantidad, value))
                    OnPropertyChanged(nameof(Total));
            }
        }

        public float Total => Precio * Cantidad;

        public string Comentario
        {
            get => _comentario;
            set => SetProperty(ref _comentario, value);
        }

        public bool EsModificado
        {
            get => _esModificado;
            set => SetProperty(ref _esModificado, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
