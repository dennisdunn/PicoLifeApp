using PicoLife.Helpers;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System.Collections.ObjectModel;
using IAdapter = Plugin.BLE.Abstractions.Contracts.IAdapter;

namespace PicoLife.Models;

public class BleManager : ObservableBase
{
    private bool _isScanning, _isConnecting, _isConnected;

    private CancellationTokenSource _cancellationTokenSource;

    public ObservableCollection<IDevice> Devices { get; set; } = [];

    public static IAdapter Adapter { get => CrossBluetoothLE.Current.Adapter; }

    public static BluetoothState Status { get => CrossBluetoothLE.Current.State; }

    public bool IsScanning
    {
        get => _isScanning;
        set => SetValue(ref _isScanning, value);
    }

    public bool IsConnecting
    {
        get => _isConnecting;
        set => SetValue(ref _isConnecting, value);
    }

    public bool IsConnected
    {
        get => _isConnected;
        set => SetValue(ref _isConnected, value);
    }

    public BleManager() : this(10_000) { }

    public BleManager(int timeout)
    {
        Adapter.DeviceDiscovered += OnDeviceDiscovered;
        Adapter.DeviceAdvertised += OnDeviceDiscovered;
        Adapter.ScanTimeoutElapsed += OnScanTimeout;
        Adapter.ScanTimeout = timeout;
    }

    private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
    {
        Devices.Add(e.Device);
    }

    public async Task<bool> ScanAsync()
    {
        var success = false;

        _cancellationTokenSource = new CancellationTokenSource();

        if (await CheckAndRequstBleAccess())
        {
            IsScanning = true;

            await Adapter.StartScanningForDevicesAsync();

            IsScanning = false;

            success = true;
        }
        return success;
    }

    private void OnScanTimeout(object sender, EventArgs e)
    {
      //  throw new NotImplementedException();
      Console.WriteLine("scan timeout");
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
        var status = await DroidPlatformHelpers.CheckAndRequestBluetoothPermissions();
        return status == PermissionStatus.Granted;
    }
}
