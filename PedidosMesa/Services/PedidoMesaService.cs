using Newtonsoft.Json;
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

        public async Task<bool> ConfirmarPedidoAsync(PedidoMesaCabeceraRequest cabecera, List<PedidoMesaDetalleRequest> detalle)
        {
            try
            {
                string baseUrl = Preferences.Get("ApiUrl", "http://192.168.100.6/WebServiceApi/api/wsAPP");

                string cabeceraJson = JsonConvert.SerializeObject(cabecera);
                string detalleJson = JsonConvert.SerializeObject(detalle);

                string cabeceraEncoded = Uri.EscapeDataString(cabeceraJson);
                string detalleEncoded = Uri.EscapeDataString(detalleJson);

                string url = $"{baseUrl}/setGrabarPedido?cabecera={cabeceraEncoded}&detalle={detalleEncoded}";

                var response = await _httpClient.PostAsync(url, null);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                content = content.Trim('"');

                return content.Equals("OK", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al confirmar pedido: {ex.Message}");
                return false;
            }
        }
    }
}
