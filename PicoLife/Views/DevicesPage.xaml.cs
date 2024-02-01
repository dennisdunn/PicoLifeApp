using PicoLife.ViewModels;

namespace PicoLife.Views;

public partial class DevicesPage : ContentPage
{
    public DevicesPage(DevicePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}