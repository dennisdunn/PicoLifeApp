using Microsoft.Extensions.Logging;
using PicoLife.Services;
using PicoLife.ViewModels;
using PicoLife.Views;
using CommunityToolkit.Maui;

namespace PicoLife
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<BluetoothService>();
            builder.Services.AddSingleton<DataService>();
            builder.Services.AddSingleton<AlertService>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddSingleton<HomePageViewModel>();

            builder.Services.AddTransient<EditPage>();
            builder.Services.AddTransient<EditPageViewModel>();

            builder.Services.AddTransient<DevicesPage>();
            builder.Services.AddSingleton<DevicePageViewModel>();

            return builder.Build();
        }
    }
}
