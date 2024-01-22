using Microsoft.Extensions.DependencyInjection.Extensions;
using PicoLife.Data;
using PicoLife.Models;
using PicoLife.Views;

namespace PicoLife;

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
                fonts.AddFont("MaterialIcons-Regulard.ttf", "MaterialIcons");
            });

		builder.Services.AddSingleton<SeedListPage>();
        builder.Services.AddTransient<SeedEditPage>();
        builder.Services.AddTransient<DevicePage>();

        builder.Services.AddSingleton<BleManager>();
        builder.Services.AddSingleton<SeedDatabase>();

        return builder.Build();
	}
}
