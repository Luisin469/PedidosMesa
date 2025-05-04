using PedidosMesa.Pages.Login;
using PedidosMesa.Pages.Mesa;
using PedidosMesa.Pages.Welcome;

namespace PedidosMesa
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MesaPage), typeof(MesaPage));

        }
    }
}
