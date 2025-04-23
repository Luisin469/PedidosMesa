namespace PedidosMesa.Utils
{
    public class PixelToMauiExtension : IMarkupExtension
    {
        public string Pixels { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var escala = DeviceDisplay.Current.MainDisplayInfo.Density;

            if (DeviceDisplay.Current.MainDisplayInfo.Density == 0)
                return Pixels; // Si Density no está disponible, usa el valor original

            //return (Pixels * escala) / DeviceDisplay.Current.MainDisplayInfo.Density;

            // Procesar cada valor (split por comas)
            string[] valores = Pixels.Split(',');
            string[] valoresConvertidos = new string[valores.Length];

            for (int i = 0; i < valores.Length; i++)
            {
                if (double.TryParse(valores[i], out double pixels))
                {
                    double mauiUnits = (pixels * escala) / DeviceDisplay.Current.MainDisplayInfo.Density;
                    valoresConvertidos[i] = mauiUnits.ToString("0.##"); // Formato opcional
                }
                else
                {
                    valoresConvertidos[i] = "0"; // Fallback si no es un número
                }
            }

            // Reconstruye el string con los valores convertidos
            return string.Join(",", valoresConvertidos);
        }
    }
}
