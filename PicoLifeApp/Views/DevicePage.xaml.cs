using PicoLife.Models;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PicoLife.Views;

public partial class DevicePage : ContentPage
{
    public ObservableCollection<BleDevice> BleDevices { get; set; } = [];

    public DevicePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    async void OnScanClicked(object sender, EventArgs e)
    {
        if (!await CheckBluetoothStatus())
        {
            await RequestBluetoothAccess();
        }

        if (await CheckBluetoothStatus())
        {
            // var ble = CrossBluetoothLE.Current;
            var adapter = CrossBluetoothLE.Current.Adapter;

            // adapter.ScanMode = ScanMode.LowLatency;
            // adapter.ScanMatchMode = ScanMatchMode.AGRESSIVE;
            // adapter.ScanTimeout = 60_000;

            adapter.DeviceDiscovered += (s, a) => BleDevices.Add(Convert(a.Device));

            IsBusy = true;
            await adapter.StartScanningForDevicesAsync();
            IsBusy = false;
        }
    }
    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        throw new NotImplementedException();
        //if (e.CurrentSelection.FirstOrDefault() is not BleDevice item)
        //    return;

    }

    internal BleDevice Convert(IDevice device)
    {
        return new BleDevice
        {
            Id = device.Id,
            Name = device.Name
        };
    }

    #region https://gist.github.com/salarcode/da8ad2b993e67c602db88a62259d0456
    // How to use MAUI Bluetooth LE permissions

    private async Task<bool> CheckBluetoothStatus()
    {
        try
        {
            var requestStatus = await new BluetoothPermissions().CheckStatusAsync();
            return requestStatus == PermissionStatus.Granted;
        }
        catch (Exception ex)
        {
            // logger.LogError(ex);
            return false;
        }
    }

    public async Task<bool> RequestBluetoothAccess()
    {
        try
        {
            var requestStatus = await new BluetoothPermissions().RequestAsync();
            return requestStatus == PermissionStatus.Granted;
        }
        catch (Exception ex)
        {
            // logger.LogError(ex);
            return false;
        }
    }
    #endregion
}