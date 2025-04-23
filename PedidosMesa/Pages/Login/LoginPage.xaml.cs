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
        string currentUrl = Preferences.Get("ApiUrl", "http://192.168.100.6/WebServiceApi/api/wsAPP/");

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
        if (ValidateLogin())
        {
            LoginRequestMiodel login = new LoginRequestMiodel()
            {
                Usuario = txtUsuario.Text,
                Clave = txtClave.Text
            };
            List<MesaResponseModel> mesaData = await _loginService.Login(login);
        }
    }

    private bool ValidateLogin()
    {
        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            DisplayAlert("Advertencia", "Debe ingresar el usuario.", "Ok");
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtClave.Text))
        {
            DisplayAlert("Advertencia", "Debe ingresar la clave.", "Ok");
            return false;
        }

        return true;
    }
}