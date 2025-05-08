using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public class DataService : IDataService
    {
        private static DataService _instance;

        public List<MesaResponseModel> MesaData { get; set; }

        public static DataService Instance => _instance ??= new DataService();

        private DataService() { }

        public void SetMesas(List<MesaResponseModel> mesas)
        {
            MesaData = mesas;
        }

        public List<MesaResponseModel> GetMesas()
        {
            return MesaData ?? new List<MesaResponseModel>();
        }
    }
}
