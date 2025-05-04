using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public interface IDataService
    {
        List<MesaResponseModel> MesaData { get; set; }
    }
}
