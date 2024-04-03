using PicoLife.Models;
using System.Collections.ObjectModel;
using PicoLife.Services;
using Cell = PicoLife.Models.Cell;

namespace PicoLife.Views;

public partial class SeedListPage : ContentPage
{
    readonly SeedDatabase database;
    private readonly BleManager bluetooth;

    public ObservableCollection<Seed> Items { get; set; } = [];

    public SeedListPage(SeedDatabase SeedCollectionDatabase, BleManager bluetooth)
    {
        InitializeComponent();
        database = SeedCollectionDatabase;
        this.bluetooth = bluetooth;
        BindingContext = this;
    }


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

       await BleManager.CheckAndRequstBleAccess();

        var items = await database.GetSeedsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);

        });
    }
    async void AddSeedClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SeedEditPage), true, new Dictionary<string, object>
        {
            ["Item"] = new Seed()
        });
    }

    async void EditSeedClicked(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        var seed = (Seed)btn.BindingContext;
        await PopulateCells(seed);
        await Shell.Current.GoToAsync(nameof(SeedEditPage), true, new Dictionary<string, object>
        {
            ["Item"] = seed
        });
    }

    private async void SelectSeedClicked(object sender, SelectionChangedEventArgs e)
    {
        if (seedList.SelectedItem == null) return;
        if (!bluetooth.IsConnected)
        {
            await DisplayAlert("Not Connected", "Connect to a device.", "OK");
            seedList.SelectedItem = null;
            return;
        }
        var seed = (Seed)e.CurrentSelection[0];
        await PopulateCells(seed);
        await bluetooth.Send(seed.ToString());
    }

    private async Task<Seed> PopulateCells(Seed seed)
    {
        var cells = await database.GetCellsBySeedIdAsync(seed.ID);
        seed.Cells = new ObservableCollection<Cell>(cells);
        return seed;
    }
}

