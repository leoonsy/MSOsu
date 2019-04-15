using MSOsu.Command;
using MSOsu.Model;
using MSOsu.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private ValuesColumn[] table;
        public ValuesColumn[] Table
        {
            get { return table; }
            set
            {
                table = value;
                RaisePropetyChanged("Table");
            }
        }

        IViewService viewService;
        IFileService<ValuesColumn[]> fileService;
        IDialogService dialogService;

        public MainWindowVM(IViewService viewService, IFileService<ValuesColumn[]> fileService, IDialogService dialogService)
        {
            this.viewService = viewService;
            this.fileService = fileService;
            this.dialogService = dialogService;
        }

        /// <summary>
        /// Загрузить исходные данные
        /// </summary>
        IDelegateCommand loadBaseDataCommand;
        public IDelegateCommand LoadBaseDataCommand
        {
            get
            {
                if (loadBaseDataCommand == null)
                    loadBaseDataCommand = new DelegateCommand(obj =>
                    {
                        viewService.LoadView(ViewType.BaseData);
                    });
                return loadBaseDataCommand;
            }
        }

        /// <summary>
        /// Загрузить главную страницу
        /// </summary>
        IDelegateCommand loadMainCommand;
        public IDelegateCommand LoadMainCommand
        {
            get
            {
                if (loadMainCommand == null)
                    loadMainCommand = new DelegateCommand(obj =>
                    {
                        viewService.LoadView(ViewType.Main);
                    });
                return loadMainCommand;
            }
        }

        IDelegateCommand loadTableCommand;
        public IDelegateCommand LoadTableCommand
        {
            get
            {
                if (loadTableCommand == null)
                    loadTableCommand = new DelegateCommand(obj =>
                    {
                        if (dialogService.OpenFileDialog(fileService.GetOpenFilter()))
                        {
                            Table = fileService.Read(dialogService.GetFilePath());
                            viewService.LoadView(ViewType.BaseData);
                        }
                    });
                return loadTableCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropetyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
