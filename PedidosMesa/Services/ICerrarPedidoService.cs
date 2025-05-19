using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public interface ICerrarPedidoService
    {
        Task<bool> CerrarMesa(ClienteRequestModel cliente);
    }
}
