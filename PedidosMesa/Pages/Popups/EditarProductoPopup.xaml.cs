using CommunityToolkit.Maui.Views;
using PedidosMesa.Models;

namespace PedidosMesa.Pages.Popups;

public partial class EditarProductoPopup : Popup
{
    private readonly PedidoRequestModel _producto;
    private readonly bool _esAgregar;

    public EditarProductoPopup(PedidoRequestModel producto, bool esAgregar)
    {
        InitializeComponent();
        AjustarTamanioPopup();
        _producto = producto;
        _esAgregar = esAgregar;

        TituloLabel.Text = esAgregar ? "Agregar Producto" : producto.Descripcion.Length > 20 ? $"{producto.Descripcion.Substring(0, 20)}..." : producto.Descripcion;
        CantidadEntry.Text = esAgregar ? "1" : producto.Cantidad.ToString();
        ComentarioEditor.Text = esAgregar ? "" : producto.Comentario;

        CantidadEntry.TextChanged += CantidadEntry_TextChanged;

        ActualizarTotal();
    }

    private void AjustarTamanioPopup()
    {
        var displayInfo = DeviceDisplay.MainDisplayInfo;

        double ancho = displayInfo.Width / displayInfo.Density;
        double alto = displayInfo.Height / displayInfo.Density;

        PopupContent.WidthRequest = ancho * 0.82;
        PopupContent.HeightRequest = alto * 0.4;
    }

    private void OnIncrementClicked(object sender, EventArgs e)
    {
        if (int.TryParse(CantidadEntry.Text, out int cantidad))
        {
            cantidad++;
            CantidadEntry.Text = cantidad.ToString();
        }
        else
        {
            CantidadEntry.Text = "1";
        }
    }

    private void OnDecrementClicked(object sender, EventArgs e)
    {
        if (int.TryParse(CantidadEntry.Text, out int cantidad))
        {
            cantidad = Math.Max(0, cantidad - 1);
            CantidadEntry.Text = cantidad.ToString();
        }
        else
        {
            CantidadEntry.Text = "1";
        }
    }

    private void CantidadEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        ActualizarTotal();
    }

    private void ActualizarTotal()
    {
        if (int.TryParse(CantidadEntry.Text, out int cantidad) && _producto != null)
        {
            var total = cantidad * _producto.Precio;
            TotalLabel.Text = $"${total:F2}";
        }
        else
        {
            TotalLabel.Text = "$0.00";
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void OnGuardarClicked(object sender, EventArgs e)
    {
        if (int.TryParse(CantidadEntry.Text, out int cantidad))
        {
            _producto.Cantidad = cantidad;
            _producto.Comentario = cantidad == 0 ? "" : ComentarioEditor.Text?.Trim();
            _producto.EsModificado = true;
            Close(_producto);
        }
        else
        {
            // Mostrar un error aquí si se valida alfo
        }
    }
}