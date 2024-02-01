using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using System.Collections.ObjectModel;
using System.Text;
using IAdapter = Plugin.BLE.Abstractions.Contracts.IAdapter;

namespace PicoLife.Services;

public partial class BluetoothService : ObservableObject, IDisposable
{
    private CancellationTokenSource? _cancellationTokenSource;

    public static IAdapter Adapter { get => CrossBluetoothLE.Current.Adapter; }
    public static BluetoothState Status { get => CrossBluetoothLE.Current.State; }
    public ObservableCollection<IDevice> Devices { get; } = [];
    public static bool IsOn { get => Status == BluetoothState.On; }
    public bool IsConnected { get => ConnectedDevice != null; }

    [ObservableProperty]
    private bool isScanning;

    [ObservableProperty]
    private IDevice? connectedDevice;

    public BluetoothService()
    {
        Adapter.DeviceDiscovered += OnDeviceDiscovered;
        Adapter.DeviceAdvertised += OnDeviceDiscovered;
        Adapter.ScanTimeoutElapsed += OnScanTimeout;
        Adapter.ScanMode = ScanMode.LowLatency;

        Adapter.DeviceConnected += OnDeviceConnected;
        Adapter.DeviceDisconnected += OnDeviceDisconnected;
    }

    private void OnDeviceConnected(object? sender, DeviceEventArgs e)
    {
        ConnectedDevice = e.Device;
        OnPropertyChanged(nameof(IsConnected));
    }

    private void OnDeviceDisconnected(object? sender, DeviceEventArgs e)
    {
        ConnectedDevice = null;
        OnPropertyChanged(nameof(IsConnected));
    }

    private void OnDeviceDiscovered(object? sender, DeviceEventArgs e)
    {
        // Ignore no-names and duplicates.
        if (!string.IsNullOrWhiteSpace(e.Device.Name) && !Devices.Any(d => d.Id == e.Device.Id))
        {
            Devices.Add(e.Device);
        }
    }

    public async Task ScanAsync(int timeout = 10_000)
    {

        if (await CheckAndRequstBleAccess())
        {
            IsScanning = true;
            await DisconnectCurrentAsync();

            Devices.Clear();

            Adapter.ScanTimeout = timeout;

            _cancellationTokenSource = new CancellationTokenSource();

            await Adapter.StartScanningForDevicesAsync(_cancellationTokenSource.Token);
            IsScanning = false;
        };
    }

    public async Task CancelAsync()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            await Task.Run(() => CleanupScan());
            IsScanning = false;
        }
    }

    public void OnScanTimeout(object? sender, EventArgs e)
    {
        CleanupScan();
    }

    private void CleanupScan()
    {
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }

    public async Task ConnectAsync(IDevice device)
    {
        if (await CheckAndRequstBleAccess())
        {
            await Adapter.ConnectToDeviceAsync(device);
            ConnectedDevice = device;
        }
    }

    public async Task DisconnectAsync(IDevice device)
    {
        if (await CheckAndRequstBleAccess())
        {
            await Adapter.DisconnectDeviceAsync(device);
            ConnectedDevice = null;
        }
    }

    public async Task DisconnectCurrentAsync()
    {
        if (ConnectedDevice != null)
        {
            await DisconnectAsync(ConnectedDevice);
        }
    }

    public static async Task<bool> CheckAndRequstBleAccess()
    {
        var status = await AndroidHelpers.CheckAndRequestBluetoothPermissions();
        return status == PermissionStatus.Granted;
    }

    public static async Task<IService> GetUartService(IDevice device)
    {
        return await device.GetServiceAsync(Constants.UART_SERVICE);
    }

    public static async Task<ICharacteristic> GetUartCharacteristic(IService service, Guid id)
    {
        return await service.GetCharacteristicAsync(id);
    }

    public async Task Send(string data)
    {
        if (ConnectedDevice != null)
        {
            var service = await GetUartService(ConnectedDevice);
            var rx = await GetUartCharacteristic(service, Constants.UART_RX_CHARACTERISTIC);
            rx.WriteType = Plugin.BLE.Abstractions.CharacteristicWriteType.WithResponse;
            await rx.WriteAsync(Encoding.ASCII.GetBytes(data));
        }
    }

    #region IDisposable

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Adapter.DeviceDiscovered -= OnDeviceDiscovered;
                Adapter.DeviceAdvertised -= OnDeviceDiscovered;
                Adapter.ScanTimeoutElapsed -= OnScanTimeout;

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

public static partial class Constants
{
    public static readonly Guid UART_SERVICE = Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");
    public static readonly Guid UART_RX_CHARACTERISTIC = Guid.Parse("6E400002-B5A3-F393-E0A9-E50E24DCCA9E");
    public static readonly Guid UART_TX_CHARACTERISTIC = Guid.Parse("6E400003-B5A3-F393-E0A9-E50E24DCCA9E");
}