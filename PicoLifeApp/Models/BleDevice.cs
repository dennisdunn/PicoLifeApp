using PicoLife.Helpers;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoLife.Models;

public class BleDevice(IDevice device) : ObservableBase
{
    private readonly IDevice _device = device;
    private bool _isConnected;

    public Guid Id { get => _device.Id; }
    public IDevice Device { get => _device; }
    public string Name { get => _device.Name; }

    public bool IsConnected
    {
        get => _isConnected;
        set => SetValue(ref _isConnected, value);
    }
}
