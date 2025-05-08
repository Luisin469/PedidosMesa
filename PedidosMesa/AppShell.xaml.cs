using PedidosMesa.Pages.Login;
using PedidosMesa.Pages.Mesa;

namespace PedidosMesa
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MesaPage), typeof(MesaPage));
        }
    }
}
