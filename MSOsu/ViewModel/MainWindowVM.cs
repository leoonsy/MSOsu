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
        /// Объект для работы с регрессией
        /// </summary>
        Regression regression;

        /// <summary>
        /// Заголовки матрицы
        /// </summary>
        public string[] MatrixHeaders;

        /// <summary>
        /// Матрица с исходными данными
        /// </summary>
        private double[][] matrixValues;
        public double[][] MatrixValues
        {
            get => matrixValues;
            set
            {
                matrixValues = value;
                RaisePropetyChanged("MatrixValues");
            }
        }

        /// <summary>
        /// Матрица с нормированными данными
        /// </summary>
        public double[][] MatrixNormalizedValues;

        /// <summary>
        /// Матрица с описательной статистикой для нормализированной выборки
        /// </summary>
        public double[][] MatrixNormalizedStatisticsValues;

        /// <summary>
        /// Матрица с проверкой на нормальность распределения
        /// </summary>
        public string[][] MatrixNormalDistribution;

        /// <summary>
        /// Заголовки строк для нормального распределения
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
        public double ChiSquareCrit;

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
        public double TStudentCritSign;

        /// <summary>
        /// Критическое значение Фишера (для определения значимости множественной корреляции)
        /// </summary>
        public double FCritSignMultiple;

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
        public double FСritEquationSign;

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
        public double TCritEquationCoeffsSign;

        /// <summary>
        /// Интервальная оценка коэффициентов
        /// </summary>
        public double[] IntervalEstimateCoeffs;

        /// <summary>
        /// Интервальная оценка y~
        /// </summary>
        public double[] IntervalEstimateEquation;

        /// <summary>
        /// Интервал предсказания y
        /// </summary>
        public double[] IntervalPredicationEquation;

        /// <summary>
        /// Интервал (для каждого параметра), на который делили при нормализации
        /// </summary>
        public double[] IntervalNormallized;

        /// <summary>
        /// Коэффициенты параметров для прогнозированного значения
        /// </summary>
        public string[] PredicationParamCoeffs;

        /// <summary>
        /// Спрогнозированное значение выходного параметра в нормализованном виде
        /// </summary>
        private double predictionYNormalized;
        public double PredictionYNormalized
        {
            get => predictionYNormalized;
            set
            {
                predictionYNormalized = value;
                RaisePropetyChanged("PredictionYNormalized");
            }
        }

        /// <summary>
        /// Интервал предсказания значения выходного параметра в нормализованном виде
        /// </summary>
        private double predictionIntervalYNormalized;
        public double PredictionIntervalYNormalized
        {
            get => predictionIntervalYNormalized;
            set
            {
                predictionIntervalYNormalized = value;
                RaisePropetyChanged("PredictionIntervalYNormalized");
            }
        }

        /// <summary>
        /// Спрогнозированное значение выходного параметра
        /// </summary>
        private double predictionY;
        public double PredictionY
        {
            get => predictionY;
            set
            {
                predictionY = value;
                RaisePropetyChanged("PredictionY");
            }
        }

        /// <summary>
        /// Интервал предсказания значения выходного параметра
        /// </summary>
        private double predictionIntervalY;
        public double PredictionIntervalY
        {
            get => predictionIntervalY;
            set
            {
                predictionIntervalY = value;
                RaisePropetyChanged("PredictionIntervalY");
            }
        }

        /// <summary>
        /// Коэффициенты параметров для прогнозированного значения 2
        /// </summary>
        public string[] PredicationParamCoeffs2;

        /// <summary>
        /// Спрогнозированное значение выходного параметра в нормализованном виде 2
        /// </summary>
        private double predictionYNormalized2;
        public double PredictionYNormalized2
        {
            get => predictionYNormalized2;
            set
            {
                predictionYNormalized2 = value;
                RaisePropetyChanged("PredictionYNormalized2");
            }
        }

        /// <summary>
        /// Интервал предсказания значения выходного параметра в нормализованном виде 2
        /// </summary>
        private double predictionIntervalYNormalized2;
        public double PredictionIntervalYNormalized2
        {
            get => predictionIntervalYNormalized2;
            set
            {
                predictionIntervalYNormalized2 = value;
                RaisePropetyChanged("PredictionIntervalYNormalized2");
            }
        }

        /// <summary>
        /// Спрогнозированное значение выходного параметра 2
        /// </summary>
        private double predictionY2;
        public double PredictionY2
        {
            get => predictionY2;
            set
            {
                predictionY2 = value;
                RaisePropetyChanged("PredictionY2");
            }
        }

        /// <summary>
        /// Интервал предсказания значения выходного параметра 2
        /// </summary>
        private double predictionIntervalY2;
        public double PredictionIntervalY2
        {
            get => predictionIntervalY2;
            set
            {
                predictionIntervalY2 = value;
                RaisePropetyChanged("PredictionIntervalY2");
            }
        }

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
                    }, obj => MatrixValues != null ? true : false);
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
                            (MatrixHeaders, MatrixValues) = TableControl.GetTable(dialogService.GetFilePath());
                            //статистики
                            MatrixNormalizedValues = DescriptiveStatistic.GetNormalizedValues(MatrixValues);
                            IntervalNormallized = DescriptiveStatistic.GetNormallizedCoeffs(MatrixValues);
                            MatrixNormalizedStatisticsValues = DescriptiveStatistic.GetTotalStatistic(MatrixNormalizedValues);
                            //нормальное распределение
                            MatrixNormalDistribution = PiersonTest.GetNormalDistributionMatrix(MatrixNormalizedValues);
                            ChiSquareCrit = PiersonTest.GetChiSquareKrit();
                            //корреляция
                            CorrelationsAnalysis correlations = new CorrelationsAnalysis(MatrixNormalizedValues);
                            PairCorrelationsMatrix = correlations.GetPairCorrelationsMatrix();
                            ParticalCorrelationsMatrix = correlations.GetParticalCorrelationsMatrix();
                            PairSignificanceCorrelationsMatrix = correlations.GetPairSignificanceCorrelationMatrix();
                            ParticalSignificanceCorrelationsMatrix = correlations.GetParticalSignificanceCorrelationMatrix();
                            TStudentCritSign = DataBase.GetTCrit(MatrixNormalizedValues[0].Length - 2);
                            MultipleCorrelationMatrix = correlations.GetMultipleCorrelationMatrix();
                            int k = MatrixValues.Length;
                            int n = MatrixValues[0].Length;
                            FCritSignMultiple = DataBase.GetFCrit(k, n - k - 1);
                            //регрессия
                            EnabledParamRegression = new bool[MatrixNormalizedValues.Length].Select(e => true).ToArray();
                            CalculateRegression();

                            LoadPageCommand.Execute(ViewType.Data);
                            LoadPageCommand.RaiseCanExecuteChanged();

                            //TableControl.SaveTable(MatrixHeaders, MatrixNormalDistribution, "kek5.csv");
                        }
                    });
                return loadTableCommand;
            }
        }

        /// <summary>
        /// Предсказать выходной параметр
        /// </summary>
        IDelegateCommand checkPredicationCommand;
        public IDelegateCommand CheckPredicationCommand
        {
            get
            {
                if (checkPredicationCommand == null)
                    checkPredicationCommand = new DelegateCommand(obj =>
                    {
                        double[] paramCoeffs = null;
                        try
                        {
                            paramCoeffs = PredicationParamCoeffs.Select(e => double.Parse(e)).ToArray();
                        }
                        catch (Exception)
                        {
                            dialogService.ShowErrorMessage("Неверно введены значения параметров", "Ошибка");
                            return;
                        }
                        double result = RegressionCoeffs[0];
                        for (int i = 0; i < paramCoeffs.Length; i++)
                            result += RegressionCoeffs[i + 1] * paramCoeffs[i];
                        PredictionYNormalized = result;
                        PredictionIntervalYNormalized = regression.GetIntervalPredication(new double[] { 1 }.Concat(paramCoeffs).ToArray());
                        PredictionY = PredictionYNormalized * IntervalNormallized[0];
                        PredictionIntervalY = PredictionIntervalYNormalized * IntervalNormallized[0];
                    });
                return checkPredicationCommand;
            }
        }

        /// <summary>
        /// Предсказать выходной параметр 2 (для исходных коэффициентов)
        /// </summary>
        IDelegateCommand checkPredicationCommand2;
        public IDelegateCommand CheckPredicationCommand2
        {
            get
            {
                if (checkPredicationCommand2 == null)
                    checkPredicationCommand2 = new DelegateCommand(obj =>
                    {
                        double[] paramCoeffs2 = null;
                        try
                        {
                            paramCoeffs2 = Enumerable.Range(0, PredicationParamCoeffs2.Length).Select(idx => double.Parse(PredicationParamCoeffs2[idx]) / IntervalNormallizedRegression[idx + 1]).ToArray();
                        }
                        catch(Exception)
                        {
                            dialogService.ShowErrorMessage("Неверно введены значения параметров", "Ошибка");
                            return;
                        }
                        double result = RegressionCoeffs[0];
                        for (int i = 0; i < paramCoeffs2.Length; i++)
                            result += RegressionCoeffs[i + 1] * paramCoeffs2[i];
                        PredictionYNormalized2 = result;
                        PredictionIntervalYNormalized2 = regression.GetIntervalPredication(new double[] { 1 }.Concat(paramCoeffs2).ToArray());
                        PredictionY2 = PredictionYNormalized2 * IntervalNormallized[0];
                        PredictionIntervalY2 = PredictionIntervalYNormalized2 * IntervalNormallized[0];
                    });
                return checkPredicationCommand2;
            }
        }

        /// <summary>
        /// Заголовки таблицы (для регрессии)
        /// </summary>
        private string[] matrixHeadersRegression;
        public string[] MatrixHeadersRegression
        {
            get => matrixHeadersRegression;
            set
            {
                matrixHeadersRegression = value;
                RaisePropetyChanged("MatrixHeadersRegression");
            }
        }
        /// <summary>
        /// Матрица нормированных значениий (для регрессии)
        /// </summary>
        public double[][] MatrixNormalizedValuesRegression;
        /// <summary>
        /// Активные параметры для регрессии
        /// </summary>
        public bool[] EnabledParamRegression;
        /// <summary>
        /// Интервал (для каждого параметра), на который делили при нормализации (для регрессии)
        /// </summary>
        public double[] IntervalNormallizedRegression;

        /// <summary>
        /// Посчитать регрессию
        /// </summary>
        IDelegateCommand calculateRegressionCommand;
        public IDelegateCommand CalculateRegressionCommand
        {
            get
            {
                if (calculateRegressionCommand == null)
                    calculateRegressionCommand = new DelegateCommand(obj =>
                    {
                        CalculateRegression();
                        LoadPageCommand.Execute(ViewType.Regression);
                    });
                return calculateRegressionCommand;
            }
        }

        public void CalculateRegression()
        {
            List<double[]> matrixNormalizedValues = new List<double[]>();
            List<string> matrixHeadersRegression = new List<string>();
            List<double> intervalNormallizedRegression = new List<double>();
            for (int i = 0; i < EnabledParamRegression.Length; i++)
                if (EnabledParamRegression[i])
                {
                    matrixNormalizedValues.Add(MatrixNormalizedValues[i]);
                    matrixHeadersRegression.Add(MatrixHeaders[i]);
                    intervalNormallizedRegression.Add(IntervalNormallized[i]);
                }
            MatrixHeadersRegression = matrixHeadersRegression.ToArray();
            MatrixNormalizedValuesRegression = matrixNormalizedValues.ToArray();
            IntervalNormallizedRegression = intervalNormallizedRegression.ToArray();

            regression = new Regression(MatrixNormalizedValuesRegression);
            RegressionCoeffs = regression.GetRegressionCoeffs();
            CalculatedY = regression.GetCalculatedY();
            AbsoluteErrorY = regression.GetAbsoluteError();
            ApproximationError = regression.GetApproximationError();
            FСritEquationSign = regression.GetFСritEquation();
            SignificanceEquation = regression.GetSignificanceEquation();
            SignificanceEquationCoeffs = regression.GetSignificanceEquationCoeffs();
            TCritEquationCoeffsSign = regression.GetTKritEquationCoeffs();
            IntervalEstimateCoeffs = regression.GetIntervalEstimateCoeffs();
            IntervalEstimateEquation = regression.GetIntervalEstimateEquation();
            IntervalPredicationEquation = regression.GetIntervalPredicationAll();

            TableControl.SaveTable(MatrixHeaders, MatrixOperations.Round(MatrixOperations.Transpose(SignificanceEquationCoeffs), 3), "kek2.csv");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropetyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
