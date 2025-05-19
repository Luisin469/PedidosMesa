using PedidosMesa.Models;
using System.Net.Http.Json;

namespace PedidosMesa.Services
{
    public class ClienteService : IClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ClienteResponseModel>> ConsultaCliente(string identificacion)
        {
            try
            {
                string baseUrl = Preferences.Get("ApiUrl", "http://192.168.100.6/WebServiceApi/api/wsAPP");
                var url = $"{baseUrl}/getClienteCedula?filtar={identificacion}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ClienteResponseModel>>() ?? new List<ClienteResponseModel>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return new List<ClienteResponseModel>();
            }
        }
    }
}
