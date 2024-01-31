using PicoLife.ViewModels;
using System.Runtime.CompilerServices;

namespace PicoLife.Views;

public partial class HomePage: ContentPage
{
    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var vm = BindingContext as HomePageViewModel;
        vm.PopulateItems();
    }
}
