using System.Globalization;

namespace PedidosMesa.Utils
{
    public class PorcentajeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dimension && parameter is string porcentajeStr && double.TryParse(porcentajeStr, out double porcentaje))
            {
                return dimension * porcentaje;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
