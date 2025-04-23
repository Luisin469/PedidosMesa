using PedidosMesa.Pages.Login;
using PedidosMesa.Pages.Main;
using PedidosMesa.Pages.Welcome;

namespace PedidosMesa
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Welcome", typeof(WelcomePage));
            Routing.RegisterRoute("HomePage", typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

        }
    }
}
