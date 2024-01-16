using Microsoft.Extensions.DependencyInjection.Extensions;
using PicoLife.Data;
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
			});

		builder.Services.AddSingleton<SeedListPage>();
		builder.Services.AddTransient<SeedItemPage>();

		builder.Services.AddSingleton<SeedDatabase>();

		return builder.Build();
	}
}
