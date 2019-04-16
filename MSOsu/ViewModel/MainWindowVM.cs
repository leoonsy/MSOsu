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
        /// <summary>
        /// Таблица с данными ValuesColums
        /// </summary>
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

        /// <summary>
        /// Заголовки таблицы
        /// </summary>
        public string[] tableHeaders;
        public string[] TableHeaders
        {
            get { return tableHeaders; }
            set
            {
                tableHeaders = value;
                RaisePropetyChanged("TableHeaders");
            }
        }

        /// <summary>
        /// Таблица с исходными данными
        /// </summary>
        public double[][] tableValues;
        public double[][] TableValues
        {
            get { return tableValues; }
            set
            {
                tableValues = value;
                RaisePropetyChanged("TableValues");
            }
        }

        /// <summary>
        /// Таблица с нормированными данными
        /// </summary>
        private double[][] tableNormalizedValues;
        public double[][] TableNormalizedValues
        {
            get { return tableNormalizedValues; }
            set
            {
                tableNormalizedValues = value;
                RaisePropetyChanged("TableNormalizedValues");
            }
        }

        /// <summary>
        /// Таблица с описательной статистикой для исходной выборки
        /// </summary>
        private double[][] tableStatisticsValues;
        public double[][] TableStatisticsValues
        {
            get { return tableStatisticsValues; }
            set
            {
                tableStatisticsValues = value;
                RaisePropetyChanged("TableStatisticsValues");
            }
        }

        /// <summary>
        /// Таблица с описательной статистикой для нормализированной выборки
        /// </summary>
        private double[][] tableNormalizedStatisticsValues;
        public double[][] TableNormalizedStatisticsValues
        {
            get { return tableNormalizedStatisticsValues; }
            set
            {
                tableNormalizedStatisticsValues = value;
                RaisePropetyChanged("TableNormalizedStatisticsValues");
            }
        }

        /// <summary>
        /// Заголовки для статистик
        /// </summary>
        public string[] StatisticsHeaders = DescriptiveStatistic.Headers;

        IViewService viewService; //сервис для отображения страниц
        IDialogService dialogService; //сервис для работы с диалоговыми окнами

        public MainWindowVM(IViewService viewService, IDialogService dialogService)
        {
            this.viewService = viewService;
            this.dialogService = dialogService;
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

        /// <summary>
        /// Загрузить исходные данные
        /// </summary>
        IDelegateCommand loadDataCommand;
        public IDelegateCommand LoadDataCommand
        {
            get
            {
                if (loadDataCommand == null)
                    loadDataCommand = new DelegateCommand(obj =>
                    {
                        viewService.LoadView(ViewType.Data);
                    });
                return loadDataCommand;
            }
        }

        /// <summary>
        /// Загрузить нормализированные данные
        /// </summary>
        IDelegateCommand loadNormalizedDataCommand;
        public IDelegateCommand LoadNormalizedDataCommand
        {
            get
            {
                if (loadNormalizedDataCommand == null)
                    loadNormalizedDataCommand = new DelegateCommand(obj =>
                    {
                        viewService.LoadView(ViewType.NormalizedData);
                    });
                return loadNormalizedDataCommand;
            }
        }

        /// <summary>
        /// Загрузить описательную статистику для исходных выборок
        /// </summary>
        IDelegateCommand loadStatisticsCommand;
        public IDelegateCommand LoadStatisticsCommand
        {
            get
            {
                if (loadStatisticsCommand == null)
                    loadStatisticsCommand = new DelegateCommand(obj =>
                    {
                        viewService.LoadView(ViewType.Statistic);
                    });
                return loadStatisticsCommand;
            }
        }

        /// <summary>
        /// Загрузить описательную статистику для нормализированных выборок
        /// </summary>
        IDelegateCommand loadNormalizedStatisticsCommand;
        public IDelegateCommand LoadNormalizedStatisticsCommand
        {
            get
            {
                if (loadNormalizedStatisticsCommand == null)
                    loadNormalizedStatisticsCommand = new DelegateCommand(obj =>
                    {
                        viewService.LoadView(ViewType.NormalizedStatistic);
                    });
                return loadNormalizedStatisticsCommand;
            }
        }

        /// <summary>
        /// Загрузить таблицу
        /// </summary>
        IDelegateCommand loadTableCommand;
        public IDelegateCommand LoadTableCommand
        {
            get
            {
                if (loadTableCommand == null)
                    loadTableCommand = new DelegateCommand(obj =>
                    {
                        string filter = "Файл CSV|*.csv";
                        if (dialogService.OpenFileDialog(filter))
                        {
                            Table = TableControl.GetTable(dialogService.GetFilePath());
                            TableHeaders = TableControl.GetHeaders(Table);
                            TableValues = TableControl.GetValues(Table);
                            TableNormalizedValues = TableControl.GetNormalizedValues(Table);
                            TableStatisticsValues = DescriptiveStatistic.GetTotalStatistic(TableControl.GetTransposeTable(TableValues));
                            TableNormalizedStatisticsValues = DescriptiveStatistic.GetTotalStatistic(TableControl.GetTransposeTable(TableNormalizedValues));
                            viewService.LoadView(ViewType.Data);
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
