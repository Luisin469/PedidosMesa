using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PedidosMesa.Models;
using PedidosMesa.Services;
using static PedidosMesa.Services.DataService;

namespace PedidosMesa.ViewModels
{
    [QueryProperty(nameof(NombreMesa), "nombreMesa")]
    public partial class CerrarMesaViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly IClienteService _clienteService;
        private readonly ICerrarPedidoService _cerrarMesaService;

        public CerrarMesaViewModel(IDataService dataService, IClienteService clienteService, ICerrarPedidoService cerrarMesaService)
        {
            _dataService = dataService;
            _clienteService = clienteService;
            _cerrarMesaService = cerrarMesaService;
        }

        [ObservableProperty]
        private double codigo;
        [ObservableProperty]
        private string identificacion;
        [ObservableProperty]
        private string nombres;
        [ObservableProperty]
        private string direccion;
        [ObservableProperty]
        private string correo;
        [ObservableProperty]
        private string telefono;
        [ObservableProperty]
        private bool camposHabilitados = true;


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

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public string Titulo => $"Cerrar mesa {NombreMesa}";

        private float totalPedido;
        public float TotalPedido
        {
            get => totalPedido;
            set => SetProperty(ref totalPedido, value);
        }

        [RelayCommand]
        public async Task BuscarClienteAsync()
        {
            if (string.IsNullOrWhiteSpace(Identificacion))
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Ingrese una identificación.", "OK");
                return;
            }

            try
            {
                var clientes = await _clienteService.ConsultaCliente(Identificacion);
                var cliente = clientes?.FirstOrDefault();

                if (cliente != null)
                {
                    Codigo = cliente.Codigo;
                    Nombres = cliente.Descripcion;
                    CamposHabilitados = false;
                }
                else
                {
                    await MostrarClienteNoEncontrado();
                    CamposHabilitados = true;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo obtener el cliente. {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task CerrarMesaAsync()
        {
            try
            {
                if (await ValidarCerrarPedido())
                {
                    var clienteRequest = new ClienteRequestModel
                    {
                        CEDULA = Identificacion,
                        CORREO = Correo,
                        DIRECCION = Direccion,
                        IDCLIENTE = Codigo.ToString(),
                        NOMBRE = Nombres,
                        NUMMESA = NombreMesa,
                        TELEFONO = Telefono,
                        USUARIO = _dataService.GetLogin().Usuario
                    };

                    bool resultado = await _cerrarMesaService.CerrarMesa(clienteRequest);

                    if (resultado)
                    {
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Pedido cerrado.", "OK");
                        AppState.DebeActualizarMesas = true;
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cerrar el pedido.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo obtener el cliente. {ex.Message}", "OK");
            }
        }

        public async Task OnNavigatedToAsync()
        {
            await CargarTotalAsync();
        }


        public async Task CargarTotalAsync()
        {
            var productos = _dataService.GetProductosPedido();
            TotalPedido = productos.Sum(x => x.Total);
        }

        private async Task MostrarClienteNoEncontrado()
        {
            Nombres = Direccion = Correo = Telefono = string.Empty;
            await Application.Current.MainPage.DisplayAlert("No encontrado", "El cliente no encontrado, por favor, ingresarlo.", "OK");
        }

        private async Task<bool> ValidarCerrarPedido()
        {
            if (string.IsNullOrWhiteSpace(nombres))
            {
                await Application.Current.MainPage.DisplayAlert("Validación", "Debe ingresar el nombre del cliente.", "OK");
                return false;
            }
            return true;
        }
    }
}
