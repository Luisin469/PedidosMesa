using PedidosMesa.ViewModels;

namespace PedidosMesa.Pages.CerrarMesa;

public partial class CerrarMesaPage : ContentPage
{
    public CerrarMesaPage(CerrarMesaViewModel viewModel)
    {
        InitializeComponent();

        FloatingButtonBorder.Shadow = new Shadow
        {
            Brush = new SolidColorBrush(Color.FromArgb("#88000000")),
            Offset = new Point(2, 2),
            Radius = 4,
            Opacity = 0.8f
        };

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CerrarMesaViewModel vm)
        {
            await vm.OnNavigatedToAsync();
        }
    }

    private async void OnBuscarClienteClicked(object sender, EventArgs e)
    {
        if (BindingContext is CerrarMesaViewModel vm)
        {
            vm.IsLoading = true;
            try
            {
                await vm.BuscarClienteAsync();
            }
            finally
            {
                vm.IsLoading = false;
            }
        }
    }


    private async void OnCerrarMesaClicked(object sender, EventArgs e)
    {
        if (BindingContext is CerrarMesaViewModel vm)
        {
            vm.IsLoading = true;
            try
            {
                await vm.CerrarMesaAsync();
            }
            finally
            {
                vm.IsLoading = false;
            }
        }
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}