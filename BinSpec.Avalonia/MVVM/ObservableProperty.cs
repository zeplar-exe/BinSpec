using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BinSpec.Avalonia.MVVM;

public class ObservableProperty<T> : INotifyPropertyChanged
{
    private T? b_value;

    public T? Value
    {
        get => b_value;
        set
        {
            if (b_value?.Equals(value) ?? value == null)
                return;

            b_value = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);

        return true;
    }
}