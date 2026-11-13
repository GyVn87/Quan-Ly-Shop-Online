using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Quan_Ly_Shop_Online.ViewModels
{
    public class RelayCommand : ICommand
    {
        // Fields
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        // Constructors
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Events
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Methods
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
