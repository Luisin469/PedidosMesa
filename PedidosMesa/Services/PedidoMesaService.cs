using PedidosMesa.Models;
using System.Net.Http.Json;

namespace PedidosMesa.Services
{
    public class PedidoMesaService : IPedidoMesaService
    {
        private readonly HttpClient _httpClient;

        public PedidoMesaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PedidoRequestModel>> ConsultaProductosPorMesa(string mesa)
        {
            try
            {
                string baseUrl = Preferences.Get("ApiUrl", "http://192.168.100.6/WebServiceApi/api/wsAPP");
                var url = $"{baseUrl}/getItemMesa?numMesa={mesa}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<PedidoRequestModel>>() ?? new List<PedidoRequestModel>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return new List<PedidoRequestModel>();
            }
        }

        public async Task<List<ProductoResponseModel>> ConsultaProductos()
        {
            try
            {
                string baseUrl = Preferences.Get("ApiUrl", "http://192.168.100.6/WebServiceApi/api/wsAPP");
                var url = $"{baseUrl}/getItem";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ProductoResponseModel>>() ?? new List<ProductoResponseModel>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return new List<ProductoResponseModel>();
            }
        }
    }
}
