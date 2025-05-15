using System.Globalization;

namespace PedidosMesa.Utils
{
    public class CantidadToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int cantidad)
            {
                return cantidad > 0 ? Colors.Green : Colors.Gray;
            }

            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
