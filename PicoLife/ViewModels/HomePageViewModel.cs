using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicoLife.Models;
using PicoLife.Services;
using PicoLife.Views;
using System.Collections.ObjectModel;

namespace PicoLife.ViewModels;

public partial class HomePageViewModel(DataService database, BluetoothService bluetooth, AlertService alert) : ObservableObject
{
    private readonly DataService database = database;
    private readonly BluetoothService bluetooth = bluetooth;
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
            try
            {
                var data = await database.GetSeedById(CurrentSeed.Id);
                await bluetooth.Send(data.ToString());
            }
            catch (Exception ex)
            {
                await alert.OK("Upload Error", ex.Message);
                CurrentSeed = null;
            }
        }
        else
        {
            await alert.OK("Not Connected", "Connect to a device.");
            CurrentSeed = null;
        }
    }
}
