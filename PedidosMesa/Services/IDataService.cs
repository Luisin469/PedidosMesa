using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public interface IDataService
    {
        List<MesaResponseModel> MesaData { get; set; }
        void SetLogin(LoginRequestMiodel login);

        LoginRequestMiodel GetLogin();

        void SetMesas(List<MesaResponseModel> mesas);

        List<MesaResponseModel> GetMesas();
    }
}
