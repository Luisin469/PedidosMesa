using Newtonsoft.Json;
using PedidosMesa.Models;

namespace PedidosMesa.Services
{
    public class CerrarPedidoService : ICerrarPedidoService
    {
        private readonly HttpClient _httpClient;

        public CerrarPedidoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CerrarMesa(ClienteRequestModel cliente)
        {
            try
            {
                string baseUrl = Preferences.Get("ApiUrl", "http://192.168.100.6/WebServiceApi/api/wsAPP");

                string json = JsonConvert.SerializeObject(cliente);

                string cabeceraEncoded = Uri.EscapeDataString(json);

                var url = $"{baseUrl}/setGrabarPedidoFinal?cabecera={json}";

                var response = await _httpClient.PostAsync(url, null);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                content = content.Trim('"');

                return content.Equals("OK", StringComparison.OrdinalIgnoreCase);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }
    }
}
