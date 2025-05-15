using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public interface IPedidoMesaService
    {
        Task<List<PedidoRequestModel>> ConsultaProductosPorMesa(string mesa);
    }
}
