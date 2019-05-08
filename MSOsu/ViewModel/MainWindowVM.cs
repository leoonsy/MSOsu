using MSOsu.Command;
using MSOsu.Common;
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
        /// Заголовки таблицы
        /// </summary>
        public string[] TableHeaders;

        /// <summary>
        /// Таблица с исходными данными
        /// </summary>
        private double[][] tableValues;
        public double[][] TableValues
        {
            get => tableValues;
            set
            {
                tableValues = value;
                RaisePropetyChanged("TableValues");
            }
        }

        /// <summary>
        /// Таблица с нормированными данными
        /// </summary>
        public double[][] TableNormalizedValues;

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
        /// Матрица парных корреляций
        /// </summary>
        public double[][] PairCorrelationsMatrix;

        /// <summary>
        /// Матрица частных корреляций
        /// </summary>
        public double[][] ParticalCorrelationsMatrix;

        /// <summary>
        /// Значимость коэффициентов парной корреляции
        /// </summary>
        public double[][] PairSignificanceCorrelationsMatrix;

        /// <summary>
        /// Значимость коэффициентов частной корреляции
        /// </summary>
        public double[][] ParticalSignificanceCorrelationsMatrix;

        /// <summary>
        /// Критические значение хи-квадрат
        /// </summary>
        public double ChiSquareKrit;

        /// <summary>
        /// Матрица для множественной корреляции (с коэффициентом детерминации и значимостью)
        /// </summary>
        public double[][] MultipleCorrelationMatrix;

        /// <summary>
        /// Заголовки для статистик
        /// </summary>
        public string[] StatisticsHeaders = DescriptiveStatistic.Headers;


        /// <summary>
        /// Критическое значение Стьюдента (для определения значимости корреляции)
        /// </summary>
        public double TStudentKritSign;

        /// <summary>
        /// Критическое значение Фишера (для определения значимости множественной корреляции)
        /// </summary>
        public double FKritSignMultiple;

        /// <summary>
        /// Коэффициенты регрессии
        /// </summary>
        public double[] RegressionCoeffs;

        /// <summary>
        /// Вычесленные значения выходного параметра
        /// </summary>
        public double[] CalculatedY;

        /// <summary>
        /// Абсолютная ошибка вычисленного выходного параметра
        /// </summary>
        public double[] AbsoluteErrorY;

        /// <summary>
        /// Ошибка аппроксимации
        /// </summary>
        public double ApproximationError;

        /// <summary>
        /// F-критическое для значимости уравнения регрессии
        /// </summary>
        public double FKritEquationSign;

        /// <summary>
        /// Значимость уравнения регрессии
        /// </summary>
        public double SignificanceEquation;

        /// <summary>
        /// Значимость коэффициентов уравнения регрессии
        /// </summary>
        public double[] SignificanceEquationCoeffs;

        /// <summary>
        /// T-критическое для значимости коэффициентов уравнения регрессии
        /// </summary>
        public double TKritEquationCoeffsSign;

        /// <summary>
        /// Интервальная оценка коэффициентов
        /// </summary>
        public double[] IntervalEstimateCoeffs;

        IViewService viewService; //сервис для отображения страниц
        IDialogService dialogService; //сервис для работы с диалоговыми окнами

        public MainWindowVM(IViewService viewService, IDialogService dialogService)
        {
            this.viewService = viewService;
            this.dialogService = dialogService;
        }

        /// <summary>
        /// Загрузить страницу
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
                    }, obj => TableValues != null ? true : false);
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
                            (TableHeaders, TableValues) = TableControl.GetTable(dialogService.GetFilePath());
                            //статистики
                            TableNormalizedValues = DescriptiveStatistic.GetNormalizedValues(TableValues);
                            TableNormalizedStatisticsValues = DescriptiveStatistic.GetTotalStatistic(TableNormalizedValues);
                            //нормальное распределение
                            TableNormalDistribution = PiersonTest.GetNormalDistributionTable(TableNormalizedValues);
                            ChiSquareKrit = PiersonTest.GetChiSquareKrit();
                            //корреляция
                            CorrelationsAnalysis correlations = new CorrelationsAnalysis(TableNormalizedValues);
                            PairCorrelationsMatrix = correlations.GetPairCorrelationsMatrix();
                            ParticalCorrelationsMatrix = correlations.GetParticalCorrelationsMatrix();
                            PairSignificanceCorrelationsMatrix = correlations.GetPairSignificanceCorrelationMatrix();
                            ParticalSignificanceCorrelationsMatrix = correlations.GetParticalSignificanceCorrelationMatrix();
                            TStudentKritSign = DataBase.GetTCrit(TableNormalizedValues[0].Length - 2);
                            MultipleCorrelationMatrix = correlations.GetMultipleCorrelationMatrix();
                            int k = TableValues.Length;
                            int n = TableValues[0].Length;
                            FKritSignMultiple = DataBase.GetFCrit(k, n - k - 1);
                            //регрессия
                            Regression regression = new Regression(TableNormalizedValues);
                            RegressionCoeffs = regression.GetRegressionCoeffs();
                            CalculatedY = regression.GetCalculatedY();
                            AbsoluteErrorY = regression.GetAbsoluteError();
                            ApproximationError = regression.GetApproximationError();
                            FKritEquationSign = regression.GetFKritEquation();
                            SignificanceEquation = regression.GetSignificanceEquation();
                            SignificanceEquationCoeffs = regression.GetSignificanceEquationCoeffs();
                            TKritEquationCoeffsSign = regression.GetTKritEquationCoeffs();
                            IntervalEstimateCoeffs = regression.GetIntervalEstimateCoeffs();
                            LoadPageCommand.Execute(ViewType.Data);
                            LoadPageCommand.RaiseCanExecuteChanged();
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
