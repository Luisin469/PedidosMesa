using PedidosMesa.Models;
using System.Net.Http.Json;

namespace PedidosMesa.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<MesaResponseModel>> Login(LoginRequestMiodel loginData)
        {
            try
            {
                string baseUrl = Preferences.Get("ApiUrl", "http://192.168.100.6/WebServiceApi/api/wsAPP");
                var url = $"{baseUrl}/login?usuario={loginData.Usuario}&clave={loginData.Clave}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<MesaResponseModel>>() ?? new List<MesaResponseModel>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de conexión: {ex.Message}");
                return new List<MesaResponseModel>();
            }
        }
    }
}
