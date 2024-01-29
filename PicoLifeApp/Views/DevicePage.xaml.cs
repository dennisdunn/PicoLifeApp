using PicoLife.Models;
using PicoLife.Services;

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

    private async void OnConnectToDevice(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (BleManager.IsScanning) return;
            if (e.CurrentSelection.Count == 0) return;
            if (e.CurrentSelection[0] is not BleDevice device) return;
            if (device.IsConnected) return;

            if(BleManager.Devices.Any(d=>d.IsConnected)) await BleManager.DisconnectCurrentAsync();

            IsBusy = true;
            await BleManager.ConnectAsync(device);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Connect Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            DevicesView.SelectedItem = null;
        }
    }
    private async void OnDisconnectClicked(object sender, EventArgs e)
    {
        var device = BleManager.Devices.FirstOrDefault(d => d.IsConnected);

        try
        {
            IsBusy = true;
            await BleManager.DisconnectAsync(device);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Disconnect Error", ex.Message, "OK");
        }
        finally
        {
            if (DevicesView.SelectedItem != null) DevicesView.SelectedItem = null;
            IsBusy = false;
        }
    }
}