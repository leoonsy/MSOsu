using MSOsu.Command;
using MSOsu.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {

        private string a;
        public string A
        {
            get { return a; }
            set
            {
                a = value;
                RaisePropetyChanged("A");
            }
        }

        private ICodeBehind iCodeBehind;

        public MainWindowVM(ICodeBehind iCodeBehind)
        {
            this.iCodeBehind = iCodeBehind;
        }

        IDelegateCommand loadBaseDataCommand;
        public IDelegateCommand LoadBaseDataCommand
        {
            get
            {
                if (loadBaseDataCommand == null)
                    loadBaseDataCommand = new DelegateCommand(obj =>
                    {
                        iCodeBehind.LoadView(ViewType.BaseData);
                    });
                return loadBaseDataCommand;
            }
        }

        IDelegateCommand loadMainCommand;
        public IDelegateCommand LoadMainCommand
        {
            get
            {
                if (loadMainCommand == null)
                    loadMainCommand = new DelegateCommand(obj =>
                    {
                        iCodeBehind.LoadView(ViewType.Main);
                    });
                return loadMainCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropetyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
