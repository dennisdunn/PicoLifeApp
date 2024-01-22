using PicoLife.Models;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace PicoLife.Views;

public partial class DevicePage : ContentPage
{
    public BleManager BleManager { get; set; }

    public DevicePage(BleManager ble)
    {
        InitializeComponent();
        BindingContext = ble;

        BleManager = ble;
    }

    async void OnScanClicked(object sender, EventArgs e)
    {
        IsBusy = true;
        await BleManager.ScanAsync();
        IsBusy = false;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection[0] is not IDevice device)
            return;

        IsBusy = true;
        await BleManager.ConnectAsync(device);
        IsBusy = false;
    }
}