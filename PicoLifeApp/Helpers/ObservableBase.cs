using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PicoLife.Helpers;

public abstract class ObservableBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public void SetValue<T>(ref T value, T newValue, [CallerMemberName] string name = "")
    {
        if (!value.Equals(newValue))
        {
            value = newValue;
            OnPropertyChanged(name);
        }
    }

    public void OnPropertyChanged([CallerMemberName] string name = "") =>
     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

}
