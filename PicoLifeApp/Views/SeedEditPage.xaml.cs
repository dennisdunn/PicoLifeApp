using PicoLife.Models;
using PicoLife.Services;

namespace PicoLife.Views;

[QueryProperty("Item", "Item")]
public partial class SeedEditPage : ContentPage
{
    readonly List<Models.Cell> deletedSeeds = [];

    public BleManager BleManager { get; set; }

    public Seed Item
    {
        get => BindingContext as Seed;
        set => BindingContext = value;
    }
    readonly SeedDatabase database;
    public SeedEditPage(SeedDatabase SeedItemDatabase, BleManager ble)
    {
        InitializeComponent();
        database = SeedItemDatabase;
        BleManager = ble;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Item.Name))
        {
            await DisplayAlert("Name Required", "Please enter a name for the seed.", "OK");
            return;
        }

        IsBusy = true;

        await database.SaveCollectionAsync(Item);

        foreach (var seed in Item.Cells)
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
        var item = (Models.Cell)btn.BindingContext;

        Item.Cells.Remove(item);

        if (item.ID > 0)
        {
            deletedSeeds.Add(item);
        }
    }

    void OnAddSeedItemClicked(object sender, EventArgs e)
    {
        Item.Cells.Add(new Models.Cell());
    }

    async void OnUploadClicked(object sender, EventArgs e)
    {
        await BleManager.Send(Item.ToString());
    }
}