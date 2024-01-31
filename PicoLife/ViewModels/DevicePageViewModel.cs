using CommunityToolkit.Mvvm.ComponentModel;
using PicoLife.Models;
using PicoLife.Services.Bluetooth;
using PicoLife.Services.Database;
using System.Collections.ObjectModel;

namespace PicoLife.ViewModels;

public class DevicePageViewModel(BluetoothManager bluetooth): ObservableObject
{
    private readonly BluetoothManager _bluetooth = bluetooth;
}
