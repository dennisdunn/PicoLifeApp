using PicoLife.Views;

namespace PicoLife;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(SeedListPage), typeof(SeedListPage));
        Routing.RegisterRoute(nameof(SeedEditPage), typeof(SeedEditPage));
        Routing.RegisterRoute(nameof(DevicePage), typeof(DevicePage));
    }
}
