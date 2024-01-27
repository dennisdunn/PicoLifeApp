using PicoLife.Models;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace PicoLife.Views;

public partial class DevicePage : ContentPage
{
    public BleManager BleManager
    {
        get => BindingContext as BleManager;
        set => BindingContext = value;
    }

    public DevicePage(BleManager ble)
    {
        InitializeComponent();

        BleManager = ble;
        
        SetupToolbar(ToolbarStartScan);
    }

    private void SetupToolbar(ToolbarItem toolbar)
    {
        ToolbarItems.Clear();
        ToolbarItems.Add(toolbar);
    }

    async void OnScanClicked(object sender, EventArgs e)
    {
        SetupToolbar(ToolbarStopScan);
        IsBusy = true;
        await BleManager.ScanAsync();
        IsBusy = false;
        SetupToolbar(ToolbarStartScan);
    }


    async void OnScanStopped(object sender, EventArgs e)
    {
        await BleManager.CancelAsync();
        SetupToolbar(ToolbarStartScan);
        IsBusy = false;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection[0] is not BleDevice device)
            return;

        IsBusy = true;
        await BleManager.ConnectAsync(device);
        IsBusy = false;
    }
}