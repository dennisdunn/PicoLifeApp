using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicoLife.Services;
using Plugin.BLE.Abstractions.Contracts;

namespace PicoLife.ViewModels;

public partial class DevicePageViewModel(BluetoothService bluetooth, AlertService alerts) : ObservableObject
{
    private readonly BluetoothService bluetooth = bluetooth;
    private readonly AlertService alerts = alerts;

    public BluetoothService BT { get { return bluetooth; } }

    [ObservableProperty]
    private IDevice selectedDevice;

    [RelayCommand]
    private async Task ScanAsync() => await BT.ScanAsync();

    [RelayCommand]
    private async Task CancelScanAsync() => await BT.CancelAsync();

    [RelayCommand]
    private async Task ConnectAsync(IDevice device)
    {
        try
        {
            if (BT.IsConnected) await BT.DisconnectCurrentAsync();

            await BT.ConnectAsync(device);
        }
        catch (Exception ex)
        {
            await alerts.OK("Connection Error", ex.Message);
            SelectedDevice = null;
        }
    }

    [RelayCommand]
    private async Task DisconnectAsync()
    {
        try
        {
            await BT.DisconnectCurrentAsync();
        }
        catch (Exception ex)
        {
            await alerts.OK("Connection Error", ex.Message);
        }
        finally
        {
            SelectedDevice = null;
        }
    }
}
