using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public interface IClienteService
    {
        Task<List<ClienteResponseModel>> ConsultaCliente(string identificacion);
    }
}
