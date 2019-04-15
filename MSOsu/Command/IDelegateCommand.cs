using System;
using System.Windows.Input;

namespace MSOsu.Command
{
    public interface IDelegateCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
