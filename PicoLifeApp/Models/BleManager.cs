using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using System.Diagnostics;
using IAdapter = Plugin.BLE.Abstractions.Contracts.IAdapter;

namespace PicoLife.Models;

public class BleManager
{
    public ObservableCollection<IDevice> Devices { get; set; } = [];

    public static IAdapter Adapter { get => CrossBluetoothLE.Current.Adapter; }

    public static BluetoothState Status { get => CrossBluetoothLE.Current.State; }

    public bool IsScanning { get; internal set; } = false;

    public bool IsConnecting { get; internal set; } = false;

    public bool IsConnected { get; internal set; } = false;

    public BleManager()
    {
        Adapter.DeviceDiscovered += (s, a) => Devices.Add(a.Device);
    }

    public async Task<bool> ScanAsync()
    {
        var success = false;

        if (await CheckAndRequstBleAccess())
        {
            IsScanning = true;

            // await MainThread.InvokeOnMainThreadAsync(async () => await Adapter.StartScanningForDevicesAsync());
            await Adapter.StartScanningForDevicesAsync();

            IsScanning = false;

            success = true;
        }
        return success;
    }

    public async Task<bool> ConnectAsync(Guid deviceId)
    {
        IsConnected = false;

        if (await CheckAndRequstBleAccess())
        {
            IsConnecting = true;

            await Adapter.ConnectToKnownDeviceAsync(deviceId);

            IsConnecting = false;

            IsConnected = true;
        }

        return IsConnected;
    }
    public async Task<bool> ConnectAsync(IDevice device)
    {
        IsConnected = false;

        if (await CheckAndRequstBleAccess())
        {
            IsConnecting = true;

            await Adapter.ConnectToDeviceAsync(device);

            IsConnecting = false;

            IsConnected = true;
        }

        return IsConnected;
    }

    public static async Task<bool> CheckAndRequstBleAccess()
    {
        var status = await CheckBluetoothStatus();
        if (!status)
        {
            return await RequestBluetoothAccess();
        }
        else
        {
            return status;
        }
    }

    #region https://gist.github.com/salarcode/da8ad2b993e67c602db88a62259d0456
    // How to use MAUI Bluetooth LE permissions

    public static async Task<bool> CheckBluetoothStatus()
    {
        try
        {
            var requestStatus = await new BlePermissions().CheckStatusAsync();
            return requestStatus == PermissionStatus.Granted;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            return false;
        }
    }

    public static async Task<bool> RequestBluetoothAccess()
    {
        try
        {
            var requestStatus = await new BlePermissions().RequestAsync();
            return requestStatus == PermissionStatus.Granted;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            return false;
        }
    }
    #endregion
}
