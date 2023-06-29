using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MvvmUtils;

/// <summary>
/// INotifyPropertyChanged 実装用基底クラス
/// </summary>
public abstract class BindableBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetProperty<T>(ref T store, T value, [CallerMemberName] string propertyName = null!)
    {
        if (Equals(store, value)) return false;
        VerifyPropertyName(propertyName);
        store = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName));
    }

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public void VerifyPropertyName(string propertyName)
    {
        if (TypeDescriptor.GetProperties(this)[propertyName] == null)
        {
            throw new ArgumentException($"Invalid property name:{propertyName}");
        }
    }
}