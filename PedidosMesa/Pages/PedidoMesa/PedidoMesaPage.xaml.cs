namespace PedidosMesa.Pages.PedidoMesa;

public partial class PedidoMesaPage : ContentPage
{
	public PedidoMesaPage()
	{
		InitializeComponent();
	}

    protected override bool OnBackPressed()
    {
        Shell.Current.GoToAsync("..");
        return true;
    }
}