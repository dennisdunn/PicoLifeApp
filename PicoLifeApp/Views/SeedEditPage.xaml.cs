using PicoLife.Data;
using PicoLife.Models;
using System.Collections.ObjectModel;

namespace PicoLife.Views;

[QueryProperty("Item", "Item")]
public partial class SeedEditPage : ContentPage
{
    readonly List<SeedItem> deletedSeeds = [];

    public SeedCollection Item
    {
        get => BindingContext as SeedCollection;
        set => BindingContext = value;
    }
    readonly SeedDatabase database;
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

        IsBusy=true;

        await database.SaveCollectionAsync(Item);

        foreach (var seed in Item.Seeds)
        {
            seed.CollectionId = Item.ID;
            await database.SaveItemAsync(seed);
        }

        foreach (var seed in deletedSeeds)
        {
            await database.DeleteItemAsync(seed);
        }

        IsBusy = false;

        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    void OnDeleteSeedItemClicked(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        var item = (SeedItem)btn.BindingContext;

        Item.Seeds.Remove(item);

        if (item.ID > 0)
        {
            deletedSeeds.Add(item);
        }
    }

    void OnAddSeedItemClicked(object sender, EventArgs e)
    {
        Item.Seeds.Add(new SeedItem());
    }

    void OnUploadClicked(object sender, EventArgs e)
    {
    }
}