using PicoLife.Models;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoLife.Helpers;

public class BleConvertor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return new BleDevice
        {
            Id = ((IDevice)value).Id,
            Name = ((IDevice)value).Name
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
