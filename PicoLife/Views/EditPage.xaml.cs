using PicoLife.ViewModels;

namespace PicoLife.Views;

public partial class EditPage : ContentPage
{
    public EditPage(EditPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}