using Newtonsoft.Json;
using PedidosMesa.Models;
using PedidosMesa.Services;

namespace PedidosMesa.Pages.Login;

public partial class LoginPage : ContentPage
{
    private readonly ILoginService _loginService;

    public LoginPage(ILoginService loginService)
    {
        InitializeComponent();

        _loginService = loginService;

        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            IsEnabled = false,
            IsVisible = false
        });
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        string currentUrl = Preferences.Get("ApiUrl", "http://192.168.100.6/WebServiceApi/api/wsAPP");

        string result = await DisplayPromptAsync(
            "Configuración",
            "Ingrese la URL del API:",
            placeholder: "http://miapi.com/api",
            initialValue: currentUrl,
            maxLength: 200,
            keyboard: Keyboard.Url);

        if (!string.IsNullOrWhiteSpace(result))
        {
            Preferences.Set("ApiUrl", result.Trim());
            await DisplayAlert("Éxito", "URL guardada correctamente.", "OK");
        }
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (await ValidateLogin())
        {
            LoginRequestMiodel login = new LoginRequestMiodel()
            {
                Usuario = txtUsuario.Text,
                Clave = txtClave.Text
            };
            List<MesaResponseModel> mesaData = await _loginService.Login(login);

            if (mesaData != null && mesaData.Count() > 0)
            {
                DataService.Instance.MesaData = mesaData;

                await Shell.Current.GoToAsync("//MesaPage");
            }
            else
            {
                await DisplayAlert("Advertencia", "No tiene mesas configuradas.", "Ok");
            }
        }
    }

    private async Task<bool> ValidateLogin()
    {
        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            await DisplayAlert("Advertencia", "Debe ingresar el usuario.", "Ok");
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtClave.Text))
        {
            await DisplayAlert("Advertencia", "Debe ingresar la clave.", "Ok");
            return false;
        }

        return true;
    }

    private static double PixelToMaui(double pixels)
    {
        var escala = DeviceDisplay.Current.MainDisplayInfo.Density;

        if (escala == 0)
            return pixels; // Si Density no está disponible, usa el valor original

        return (pixels * escala) / DeviceDisplay.Current.MainDisplayInfo.Density;
    }
}