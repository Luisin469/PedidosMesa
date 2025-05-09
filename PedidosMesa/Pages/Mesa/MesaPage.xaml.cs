using PedidosMesa.Services;
using PedidosMesa.ViewModels;

namespace PedidosMesa.Pages.Mesa
{
    public partial class MesaPage : ContentPage
    {
        public MesaPage()
        {
            InitializeComponent();
            BindingContext = new MesaViewModel(DataService.Instance);
        }
    }
}