using PedidosMesa.ViewModels;

namespace PedidosMesa.Pages.PedidoMesa
{
    public partial class PedidoMesaPage : ContentPage
    {
        public PedidoMesaPage(PedidoMesaViewModel viewModel)
        {
            InitializeComponent();
            FloatingButtonBorder.Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Color.FromArgb("#88000000")),
                Offset = new Point(2, 2),
                Radius = 4,
                Opacity = 0.8f
            };
            viewModel.SetMostrarPromptComentario(DisplayComentarioPromptAsync);
            BindingContext = viewModel;
        }

        private async Task<string> DisplayComentarioPromptAsync(Models.PedidoRequestModel producto)
        {
            return await DisplayPromptAsync(
                "Editar Comentario",
                "Modifica el comentario del producto:",
                accept: "Guardar",
                cancel: "Cancelar",
                placeholder: "Escribe un comentario aquí...",
                initialValue: producto.Comentario,
                maxLength: 500,
                keyboard: Keyboard.Default
            );
        }

        private void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (BindingContext is PedidoMesaViewModel vm)
            {
                vm.CargarMasProductosCommand.Execute(null);
            }
        }

        private void FloatingButton_Clicked(object sender, EventArgs e)
        {
            // Aquí tu lógica para cuando se pulse el botón + 
            // Por ejemplo, navegar a otra página o abrir un modal
        }


        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}