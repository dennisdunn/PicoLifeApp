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
    private bool _isScanning, _isConnecting, _isConnected, disposedValue;

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

    public BleManager()
    {
        Adapter.DeviceDiscovered += OnDeviceDiscovered;
        Adapter.DeviceAdvertised += OnDeviceDiscovered;
        Adapter.ScanTimeoutElapsed += OnScanTimeout;
        Adapter.ScanMode = ScanMode.LowLatency;
    }

    private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() => Devices.Add(e.Device));
        //Devices.Add(e.Device);
    }

    public async Task ScanAsync(int timeout = 10_000)
    {
        if (await CheckAndRequstBleAccess())
        {
            IsScanning = true;

            Adapter.ScanTimeout = timeout;

            _cancellationTokenSource = new CancellationTokenSource();
            try
            {

                await Adapter.StartScanningForDevicesAsync(_cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                _cancellationTokenSource.Cancel();
            }
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
        IsScanning = false;
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

    #region IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Adapter.DeviceDiscovered -= OnDeviceDiscovered;
                Adapter.DeviceAdvertised -= OnDeviceDiscovered;
                Adapter.ScanTimeoutElapsed -= OnScanTimeout;
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
