using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace PicoLife.Views;

public partial class DevicePage : ContentPage
{
    IBluetoothLE BLE = CrossBluetoothLE.Current;
    IAdapter BLEAdapter = CrossBluetoothLE.Current.Adapter;

    public DevicePage()
    {
        InitializeComponent();
    }

    async void OnScanClicked(object sender, EventArgs e)
    {
        var state = BLE.State;

        var deviceList = new List<IDevice>();
        BLEAdapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
        await BLEAdapter.StartScanningForDevicesAsync();
    }
}