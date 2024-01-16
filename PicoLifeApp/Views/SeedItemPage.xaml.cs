using PicoLife.Data;
using PicoLife.Models;

namespace PicoLife.Views;

[QueryProperty("Item", "Item")]
public partial class SeedItemPage : ContentPage
{
	SeedItem item;
	public SeedItem Item
	{
		get => BindingContext as SeedItem;
		set => BindingContext = value;
	}
    SeedDatabase database;
    public SeedItemPage(SeedDatabase SeedItemDatabase)
    {
        InitializeComponent();
        database = SeedItemDatabase;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Item.Name))
        {
            await DisplayAlert("Name Required", "Please enter a name for the seed.", "OK");
            return;
        }

        await database.SaveItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Item.ID == 0)
            return;
        await database.DeleteItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}