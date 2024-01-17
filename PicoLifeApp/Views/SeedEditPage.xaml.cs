using PicoLife.Data;
using PicoLife.Models;

namespace PicoLife.Views;

[QueryProperty("Item", "Item")]
public partial class SeedEditPage : ContentPage
{
	SeedCollection item;
	public SeedCollection Item
	{
		get => BindingContext as SeedCollection;
		set => BindingContext = value;
	}
    SeedDatabase database;
    public SeedEditPage(SeedDatabase SeedItemDatabase)
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

        await database.SaveCollectionAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Item.ID == 0)
            return;
        await database.DeleteCollectionAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteSeedItemClicked(object sender, EventArgs e)
    {
    }

    async void OnAddSeedItemClicked(object sender, EventArgs e)
    {
    }
}