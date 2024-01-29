using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PicoLife.Models;

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
