using System.Globalization;

namespace PedidosMesa.Utils
{
    public class CantidadMayorACeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            double cantidad = 0;

            if (value is int intVal)
                cantidad = intVal;
            else if (value is double doubleVal)
                cantidad = doubleVal;

            bool resultado = cantidad > 0;

            if (parameter != null && parameter.ToString()?.ToLower() == "invert")
            {
                resultado = !resultado;
            }

            return resultado;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
