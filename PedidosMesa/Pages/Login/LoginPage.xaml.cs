using PedidosMesa.Models;
using PedidosMesa.Services;

namespace PedidosMesa.Pages.Login;

public partial class LoginPage : ContentPage
{
    private readonly IDataService _dataService;
    private readonly ILoginService _loginService;

    public LoginPage(ILoginService loginService, IDataService dataService)
    {
        InitializeComponent();
        _loginService = loginService;
        _dataService = dataService;

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
            accept: "Guardar",
            cancel:"Cancelar",
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
            if (await RealizarLoginAsync())
            {
                await Shell.Current.GoToAsync("//MesaPage");
            }
        }
    }

    public async Task<bool> RealizarLoginAsync()
    {
        LoginRequestModel login = new LoginRequestModel
        {
            Usuario = txtUsuario.Text,
            Clave = txtClave.Text
        };

        List<MesaResponseModel> mesaData = await _loginService.Login(login);

        if (mesaData != null && mesaData.Count > 0)
        {
            _dataService.SetMesas(mesaData);
            _dataService.SetLogin(login);
            return true;
        }
        else
        {
            await DisplayAlert("Advertencia", "No tiene mesas configuradas.", "Ok");
            return false;
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

}