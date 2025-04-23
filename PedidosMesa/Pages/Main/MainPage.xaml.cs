using PedidosMesa.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PedidosMesa.Pages.Main;

public partial class MainPage : ContentPage
{
    public ObservableCollection<PoolBlockModel> BlockPool { get; set; } = new ObservableCollection<PoolBlockModel>
        {
            new PoolBlockModel { Ubication = "Santay", Supervisor = "Sup. Marco Gutierrez", Group = "G0-5", Weight = "4 gr.", Survival = "Supervivencia 0%" },
            new PoolBlockModel { Ubication = "Santay", Supervisor = "Sup. Marco Gutierrez", Group = "A0-5", Weight = "4 gr.", Survival = "Supervivencia 0%" },
            new PoolBlockModel { Ubication = "Santay", Supervisor = "Sup. Marco Gutierrez", Group = "G1-5", Weight = "4 gr.", Survival = "Supervivencia 0%" },
            new PoolBlockModel { Ubication = "Santay", Supervisor = "Sup. Marco Gutierrez", Group = "A1-5", Weight = "4 gr.", Survival = "Supervivencia 0%" },
            new PoolBlockModel { Ubication = "Santay", Supervisor = "Sup. Marco Gutierrez", Group = "G2-5", Weight = "4 gr.", Survival = "Supervivencia 0%" },
            new PoolBlockModel { Ubication = "Santay", Supervisor = "Sup. Marco Gutierrez", Group = "A2-5", Weight = "4 gr.", Survival = "Supervivencia 0%" },
            new PoolBlockModel { Ubication = "Santay", Supervisor = "Sup. Marco Gutierrez", Group = "G3-5", Weight = "4 gr.", Survival = "Supervivencia 0%" },
            new PoolBlockModel { Ubication = "Chongón", Supervisor = "Sup. Andrea Ruiz", Group = "G6-10", Weight = "5 gr.", Survival = "Supervivencia 25%" },
            // etc...
        };

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        GetScreenInfo();

    }

    private static void GetScreenInfo()
    {
        var displayInfo = DeviceDisplay.MainDisplayInfo;

        //Resolucion en pixeles
        var widthPixels = displayInfo.Width;
        var heightPixels = displayInfo.Height;

        Debug.WriteLine($"Pixel width: {widthPixels}, Pixel height: {heightPixels}");

        //Densidad de la pantalla
        double density = displayInfo.Density;

        Debug.WriteLine($"Densidad de la pantalla: {density}");

        //DPI aproximados
        double dpi = density * 160;

        //Ancho: La escala es aproximadamente 2.88.
        //Alto: La escala es aproximadamente 2.96.

        Debug.WriteLine($"DPI aproximados: {dpi}");
    }


    private static double PixelToMaui(double pixels)
    {
        var escala = DeviceDisplay.Current.MainDisplayInfo.Density;

        if (escala == 0)
            return pixels; // Si Density no está disponible, usa el valor original

        return (pixels * escala) / DeviceDisplay.Current.MainDisplayInfo.Density;
    }
}