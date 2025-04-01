﻿//using System;
//using System.Windows.Input;

//namespace Memory_game.Commands
//{
//    //public class RelayCommand : ICommand
//    //{
//    //    private readonly Action _execute;
//    //    private readonly Func<bool> _canExecute;

//    //    public RelayCommand(Action execute, Func<bool> canExecute = null)
//    //    {
//    //        _execute = execute;
//    //        _canExecute = canExecute;
//    //    }

//    //    public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

//    //    public void Execute(object parameter) => _execute();

//    //    public event EventHandler CanExecuteChanged;

//    //    public void RaiseCanExecuteChanged()
//    //    {
//    //        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
//    //    }

//    //}
//    public class RelayCommand<T> : ICommand
//    {
//        private readonly Action<T> _execute;
//        private readonly Predicate<T> _canExecute;

//        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
//        {
//            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
//            _canExecute = canExecute;
//        }

//        public bool CanExecute(object parameter)
//        {
//            return _canExecute == null || _canExecute((T)parameter);
//        }

//        public void Execute(object parameter)
//        {
//            _execute((T)parameter);
//        }

//        public event EventHandler CanExecuteChanged;

//        public void RaiseCanExecuteChanged()
//        {
//            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
//        }
//    }

//}

using System;
using System.Windows.Input;

namespace Memory_game.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) =>
            _canExecute == null || _canExecute((T)parameter);

        public void Execute(object parameter) =>
            _execute((T)parameter);

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
