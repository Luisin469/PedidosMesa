using PedidosMesa.Pages.Login;
using PedidosMesa.Pages.Mesa;
using PedidosMesa.Pages.PedidoMesa;
using PedidosMesa.Pages.CerrarMesa;

namespace PedidosMesa
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MesaPage), typeof(MesaPage));
            Routing.RegisterRoute(nameof(PedidoMesaPage), typeof(PedidoMesaPage));
            Routing.RegisterRoute(nameof(CerrarMesaPage), typeof(CerrarMesaPage));
        }
    }
}
