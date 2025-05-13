using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public interface IPedidoMesaService
    {
        Task<List<ProductoResponseModel>> ConsultaProductosPorMesa(string mesa);
    }
}
