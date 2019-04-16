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
        public string[] TableHeaders;

        /// <summary>
        /// Таблица с исходными данными
        /// </summary>
        public double[][] TableValues;

        /// <summary>
        /// Таблица с нормированными данными
        /// </summary>
        public double[][] TableNormalizedValues;

        /// <summary>
        /// Таблица с описательной статистикой для исходной выборки
        /// </summary>
        public double[][] TableStatisticsValues;

        /// <summary>
        /// Таблица с описательной статистикой для нормализированной выборки
        /// </summary>
        public double[][] TableNormalizedStatisticsValues;

        /// <summary>
        /// Таблица с проверкой на нормальность распределения
        /// </summary>
        public string[][] TableNormalDistribution;

        /// <summary>
        /// Таблица с проверкой на нормальность распределения
        /// </summary>
        public string[] NormalDistributionHeaders = new string[] { "Значение χ2", "Нормальность распределения" };

        /// <summary>
        /// Критические значение хи-квадрат
        /// </summary>
        public double ChiSquareKrit;

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
        /// Загрузить исходные данные
        /// </summary>
        IDelegateCommand loadPageCommand;
        public IDelegateCommand LoadPageCommand
        {
            get
            {
                if (loadPageCommand == null)
                    loadPageCommand = new DelegateCommand(obj =>
                    {
                        viewService.LoadView((ViewType)obj);
                    });
                return loadPageCommand;
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
                            TableNormalDistribution = GetNormalDistributionTable(TableControl.GetTransposeTable(TableNormalizedValues));
                            ChiSquareKrit = PiersonTest.GetChiSquareKrit();
                            LoadPageCommand.Execute(ViewType.Data);
                        }
                    });
                return loadTableCommand;
            }
        }

        public string[][] GetNormalDistributionTable(double[][] values)
        {
            string[][] result = new string[2][].Select(e => e = new string[values.Length]).ToArray();
            for (int i = 0; i < values.Length; i++)
            {
                (bool isNormal, double chiSquare) = PiersonTest.CheckNormalDistribution(values[i]);
                result[0][i] = chiSquare.ToString();
                result[1][i] = isNormal ? "+" : "-";
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropetyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
