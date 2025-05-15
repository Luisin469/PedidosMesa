using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public class DataService : IDataService
    {
        private static DataService _instance;

        public LoginRequestModel LoginData { get; set; }
        public List<MesaResponseModel> MesaData { get; set; }
        public List<PedidoRequestModel> ProductoPedidoData { get; set; }

        public static DataService Instance => _instance ??= new DataService();

        private DataService() { }

        public void SetLogin(LoginRequestModel login)
        {
            LoginData = login;
        }

        public LoginRequestModel GetLogin()
        {
            return LoginData ?? new LoginRequestModel();
        }

        public void SetMesas(List<MesaResponseModel> mesas)
        {
            MesaData = mesas;
        }

        public List<MesaResponseModel> GetMesas()
        {
            return MesaData ?? new List<MesaResponseModel>();
        }

        public void SetProductosPedido(List<PedidoRequestModel> productos)
        {
            ProductoPedidoData = productos;
        }

        public List<PedidoRequestModel> GetProductosPedido()
        {
            return ProductoPedidoData ?? new List<PedidoRequestModel>();
        }
    }
}
