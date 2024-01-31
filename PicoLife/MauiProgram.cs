using Microsoft.Extensions.Logging;
using PicoLife.Services;
using PicoLife.Services.Bluetooth;
using PicoLife.Services.Database;
using PicoLife.ViewModels;
using PicoLife.Views;

namespace PicoLife
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<BluetoothManager>();
            builder.Services.AddSingleton<DatabaseManager>();
            builder.Services.AddSingleton<AlertService>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomePageViewModel>();

            builder.Services.AddTransient<EditPage>();
            builder.Services.AddTransient<EditPageViewModel>();

            builder.Services.AddTransient<DevicesPage>();
            builder.Services.AddTransient<DevicePageViewModel>();

            return builder.Build();
        }
    }
}
