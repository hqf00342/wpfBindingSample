using System;
using System.Windows.Input;

namespace MvvmUtils;

public class DelegateCommand : ICommand
{
    private readonly Action<object?> _execFunc;
    private readonly Func<object?, bool> _canExecFunc;

    public event EventHandler? CanExecuteChanged = null!;

    public DelegateCommand(Action<object?> execFunc, Func<object?, bool> canExecFunc = null!)
    {
        _execFunc = execFunc ?? throw new ArgumentNullException(nameof(execFunc));
        _canExecFunc = canExecFunc;
    }

    bool ICommand.CanExecute(object? parameter) => _canExecFunc?.Invoke(parameter) ?? true;

    void ICommand.Execute(object? parameter) => _execFunc?.Invoke(parameter);

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}