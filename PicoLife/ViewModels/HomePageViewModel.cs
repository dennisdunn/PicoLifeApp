using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicoLife.Models;
using PicoLife.Services;
using PicoLife.Services.Bluetooth;
using PicoLife.Services.Database;
using PicoLife.Views;
using System.Collections.ObjectModel;

namespace PicoLife.ViewModels;

public partial class HomePageViewModel(DatabaseManager database, BluetoothManager bluetooth, AlertService alert) : ObservableObject
{
    private readonly DatabaseManager database = database;
    private readonly BluetoothManager bluetooth = bluetooth;
    private readonly AlertService alert = alert;

    public ObservableCollection<Seed> Seeds { get; private set; } = [];

    [ObservableProperty]
    private Seed? currentSeed;

    public async void PopulateItems()
    {
        var seeds = await database.GetSeedsAsync();
        Seeds = new ObservableCollection<Seed>(seeds);
        OnPropertyChanged(nameof(Seeds));
    }

    [RelayCommand]
    private async Task Edit(Seed seed)
    {
        var item = seed != null ? await database.GetSeedById(seed.Id) : new Seed();
        await Shell.Current.GoToAsync(nameof(EditPage), false, new Dictionary<string, object> { ["Item"] = item });
    }

    [RelayCommand]
    private async Task Upload()
    {
        if (bluetooth.IsConnected && CurrentSeed != null)
        {
            await bluetooth.Send(CurrentSeed.ToString());
        }
        else
        {
            await alert.OK("Not Connected", "Connect to a device.");
            // TODO: un-select the current item
            CurrentSeed = null;
        }
    }
}
