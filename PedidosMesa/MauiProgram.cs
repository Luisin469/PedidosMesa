using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using PedidosMesa.Pages.Mesa;
using PedidosMesa.Services;
using PedidosMesa.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;


namespace PedidosMesa
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Nunito-SemiBold.ttf", "NunitoSemiBold");
                    fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");
                    fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
                    fonts.AddFont("Nunito-Medium.ttf", "NunitoMedium");
                    fonts.AddFont("NunitoSans-VariableFont.ttf", "NunitoSans");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", "IconFont");
                });

            ModifyEntryHandler();

            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IDataService>(DataService.Instance);
            builder.Services.AddSingleton<ILoginService, LoginService>();
            builder.Services.AddSingleton<MesaPage>();

            builder.Services.AddTransient<MesaViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void ModifyEntryHandler()
        {
#if ANDROID
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                if (view is Entry entry)
                {
                    handler.PlatformView.Background = null;

                    // También puedes quitar el fondo completamente
                    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);

                }

            });
#endif
        }
    }
}
