using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace PedidosMesa
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
#if ANDROID
            // Cambiar el color de la barra de estado (status bar)
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window?.SetStatusBarColor(Android.Graphics.Color.ParseColor("#FFFFFF"));

                // Esto cambia los íconos de la barra de estado a oscuros (útil si tu fondo es claro)
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                }
            }
#endif
        }
    }
}
