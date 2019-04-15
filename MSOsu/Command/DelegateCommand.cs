﻿using System;

namespace MSOsu.Command
{
    public class DelegateCommand : IDelegateCommand
    {
        Action<object> execute;
        Func<object, bool> canExecute;

        // Событие, необходимое для ICommand
        public event EventHandler CanExecuteChanged;

        //Два конструктора
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public DelegateCommand(Action<object> execute)
        {
            this.execute = execute;
            this.canExecute = this.AlwaysCanExecute;
        }

        // Методы, необходимые для ICommand
        public void Execute(object param)
        {
            execute(param);
        }

        public bool CanExecute(object param)
        {
            return canExecute(param);
        }

        // Метод, необходимый для IDelegateCommand
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        // Метод CanExecute по умолчанию
        private bool AlwaysCanExecute(object param)
        {
            return true;
        }
    }
}
