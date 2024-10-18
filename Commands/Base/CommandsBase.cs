using System.Windows.Input;

namespace WpfApp2.Commands.Base
{
    public class CommandsBase : ICommand
    {
        private readonly Func<object?, Task> _executeFunc;
        private readonly Predicate<object?>? _canExecuteFunc;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CommandsBase(Action<object?> executeAction)
            : this(p => { executeAction(p); return Task.CompletedTask; }, null)
        {
        }

        public CommandsBase(Func<object?, Task> executeFunc)
            : this(executeFunc, null)
        {
        }

        public CommandsBase(Action<object?> executeAction, Predicate<object?> canExecute)
            : this(p => { executeAction(p); return Task.CompletedTask; }, canExecute)
        {
        }

        public CommandsBase(Func<object?, Task> executeFunc, Predicate<object?>? canExecute)
        {
            _executeFunc = executeFunc ?? throw new ArgumentNullException(nameof(executeFunc));
            _canExecuteFunc = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecuteFunc?.Invoke(parameter) ?? true;
        }

        public async void Execute(object? parameter)
        {
            await _executeFunc(parameter);
        }
    }
}
