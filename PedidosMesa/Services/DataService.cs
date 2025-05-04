using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public class DataService : IDataService
    {
        private static DataService _instance;
        public static DataService Instance => _instance ??= new DataService();

        public List<MesaResponseModel> MesaData { get; set; }

        private DataService() { }
    }
}
