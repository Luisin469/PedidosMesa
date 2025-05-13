using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public interface IDataService
    {
        List<MesaResponseModel> MesaData { get; set; }
        void SetLogin(LoginRequestModel login);

        LoginRequestModel GetLogin();

        void SetMesas(List<MesaResponseModel> mesas);

        List<MesaResponseModel> GetMesas();

        void SetProductos(List<ProductoResponseModel> mesas);

        List<ProductoResponseModel> GetProductos();
    }
}
