using MSOsu.Command;
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

        private string task2_Result;
        public string Task2_Result
        {
            get { return task2_Result; }
            set
            {
                task2_Result = value;
                RaisePropetyChanged("Task2_Result");
            }
        }

        IDelegateCommand calculateJacobi;
        public IDelegateCommand CalculateJacobi
        {
            get
            {
                if (calculateJacobi == null)
                    calculateJacobi = new DelegateCommand(obj =>
                    {

                    });
                return calculateJacobi;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropetyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
