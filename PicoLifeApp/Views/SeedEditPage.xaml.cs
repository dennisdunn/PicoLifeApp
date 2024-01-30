using AndroidX.Activity;
using PicoLife.Models;
using PicoLife.Services;
using System.Windows.Input;
using Cell = PicoLife.Models.Cell;

namespace PicoLife.Views;

[QueryProperty("Item", "Item")]
public partial class SeedEditPage : ContentPage
{
    readonly List<Cell> removedCells = [];
    readonly SeedDatabase database;

    public BleManager BleManager { get; set; }

    public Seed Item
    {
        get => BindingContext as Seed;
        set => BindingContext = value;
    }
    public SeedEditPage(SeedDatabase SeedItemDatabase, BleManager ble)
    {
        InitializeComponent();
        database = SeedItemDatabase;
        BleManager = ble;

        backbutton.BindingContext = this;
    }

    public ICommand SaveSeedCommand => new Command(SaveSeed);

    //protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    //{
    //    if (string.IsNullOrWhiteSpace(Item.Name))
    //    {
    //        DisplayAlert("Name Required", "Please enter a name for the seed.", "OK").Wait();
    //        return;
    //    }

    //    IsBusy = true;

    //    database.SaveSeedAsync(Item).Wait();

    //    foreach (var cell in deletedCells)
    //    {
    //        database.DeleteCellAsync(cell).Wait();
    //    }

    //    IsBusy = false;

    //    //Shell.Current.GoToAsync("..").Wait();
    //    base.OnNavigatedFrom(args);
    //}

    async void SaveSeed()
    {
        if (string.IsNullOrWhiteSpace(Item.Name))
        {
            await DisplayAlert("Name Required", "Please enter a name for the seed.", "OK");
        }
        else
        {
            await database.DeleteCellAsync(removedCells);
            await database.SaveSeedAsync(Item);
            await Shell.Current.GoToAsync("..");
        }
    }

    async void DeleteSeedClicked(object sender, EventArgs e)
    {
        await database.DeleteSeedAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    void DeleteCellClicked(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        var item = (Cell)btn.BindingContext;

        Item.Cells.Remove(item);

        if (item.ID > 0)
        {
            removedCells.Add(item);
        }
    }

    void AddCellClicked(object sender, EventArgs e)
    {
        Item.Cells.Add(new Cell());
    }
}