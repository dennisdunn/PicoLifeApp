using PicoLife.Helpers;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using System.Collections.ObjectModel;
using IAdapter = Plugin.BLE.Abstractions.Contracts.IAdapter;

namespace PicoLife.Models;

public class BleManager : ObservableBase, IDisposable
{
    private CancellationTokenSource _cancellationTokenSource;

    public ObservableCollection<BleDevice> Devices { get; set; } = [];

    public static IAdapter Adapter { get => CrossBluetoothLE.Current.Adapter; }

    public static BluetoothState Status { get => CrossBluetoothLE.Current.State; }

    public bool IsScanning { get => Adapter.IsScanning; }

    public BleManager()
    {
        Adapter.DeviceDiscovered += OnDeviceDiscovered;
        Adapter.DeviceAdvertised += OnDeviceDiscovered;
        Adapter.ScanTimeoutElapsed += OnScanTimeout;
        Adapter.ScanMode = ScanMode.LowLatency;

        Adapter.DeviceConnectionError += OnDeviceConnectionError;
        Adapter.DeviceConnected += OnDeviceConnected;
        Adapter.DeviceDisconnected += OnDeviceDisconnected;
    }

    private void OnDeviceConnected(object sender, DeviceEventArgs e)
    {
        var id = e.Device.Id;
        var device = Devices.FirstOrDefault(d => d.Id == id);
        if (device != null) device.IsConnected = true;
    }

    private void OnDeviceDisconnected(object sender, DeviceEventArgs e)
    {
        var id = e.Device.Id;
        var device = Devices.FirstOrDefault(d => d.Id == id);
        if (device != null) device.IsConnected = false;
    }

    private void OnDeviceConnectionError(object sender, DeviceErrorEventArgs e)
    {
    }

    private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
    {
        if (!Devices.Any(d => d.Id.Equals(e.Device.Id)) && !string.IsNullOrEmpty(e.Device.Name))
        {
            Devices.Add(new BleDevice(e.Device));
        }
    }

    public async Task ScanAsync(int timeout = 10_000)
    {

        if (await CheckAndRequstBleAccess())
        {
            await DisconnectCurrentAsync();

            Devices.Clear();

            Adapter.ScanTimeout = timeout;

            _cancellationTokenSource = new CancellationTokenSource();

            await Adapter.StartScanningForDevicesAsync(_cancellationTokenSource.Token);
        };
    }

    public async Task CancelAsync()
    {
        _cancellationTokenSource.Cancel();

        await Task.Run(() => CleanupScan());
    }

    public void OnScanTimeout(object sender, EventArgs e)
    {
        CleanupScan();
    }

    private void CleanupScan()
    {
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    public async Task ConnectAsync(BleDevice device)
    {
        if (await CheckAndRequstBleAccess())
        {
            await Adapter.ConnectToDeviceAsync(device.Device);
        }
    }

    public async Task DisconnectAsync(BleDevice device)
    {
        if (await CheckAndRequstBleAccess())
        {
            await Adapter.DisconnectDeviceAsync(device.Device);
        }
    }

    public async Task DisconnectCurrentAsync()
    {
        var currentDevice = Devices.FirstOrDefault(d => d.IsConnected);
        if (currentDevice != null)
        {
            await DisconnectAsync(currentDevice);
        }
    }

    public static async Task<bool> CheckAndRequstBleAccess()
    {
        var status = await DroidPlatformHelpers.CheckAndRequestBluetoothPermissions();
        return status == PermissionStatus.Granted;
    }

    #region IDisposable

    private bool disposedValue;
    private bool _isError;
    private string _message;
    private BleDevice _device;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Adapter.DeviceDiscovered -= OnDeviceDiscovered;
                Adapter.DeviceAdvertised -= OnDeviceDiscovered;
                Adapter.ScanTimeoutElapsed -= OnScanTimeout;

                Adapter.DeviceConnectionError -= OnDeviceConnectionError;
                Adapter.DeviceConnected -= OnDeviceConnected;
                Adapter.DeviceDisconnected -= OnDeviceDisconnected;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~BleManager()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
