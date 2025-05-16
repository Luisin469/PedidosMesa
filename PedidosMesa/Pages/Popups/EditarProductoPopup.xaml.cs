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
        _producto = producto;
        _esAgregar = esAgregar;

        TituloLabel.Text = esAgregar ? "Agregar Producto" : "Editar Producto";
        CantidadEntry.Text = esAgregar ? "1" : producto.Cantidad.ToString();
        ComentarioEditor.Text = esAgregar ? "" : producto.Comentario;
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
            // Puedes mostrar un error aquí si deseas validar
        }
    }
}