using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public interface ILoginService
    {
        Task<List<MesaResponseModel>> Login(LoginRequestMiodel login);
    }
}
