using MSOsu.Model;
using MSOsu.Service;
using MSOsu.Service.DialogServices;
using MSOsu.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;
using MSOsu.Common;
using MahApps.Metro.Controls;

namespace MSOsu.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IViewService
    {
        MainWindowVM mainVM;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainVM = new MainWindowVM(this, new DefaultDialogService());
            DataContext = mainVM;
            InitializeMyBindings();
            LoadView(ViewType.Main);
        }

        MainUC mainUC = null;
        DataTableUC normalDataUC = null;
        DataTableUC normalizedDataUC = null;
        DataTableUC normilizeStatisticsUC = null;
        DataTableUC normalDistributionUC = null;
        DataTableUC pairCorrelationsUC = null;
        DataTableUC particalCorrelationsUC = null;
        SignificanceUC significanceCorrelationsUC = null;
        DataTableUC multipleCorrelationUC = null;
        CorrelationDiagramMainUC correlationDiagramPageUC = null;
        RegressionUC regressionUC = null;
        RegressionParamsControlUC regressionParamsControlUC = null;
        /// <summary>
        /// Загрузить контент
        /// </summary>
        /// <param name="type"></param>
        public void LoadView(ViewType type)
        {
            switch (type)
            {
                case ViewType.Main:
                    if (mainUC == null) //если требуется перерисовка
                    {
                        mainUC = new MainUC();
                        mainUC.DataContext = mainVM;
                    }
                    cpMainContent.Content = mainUC;
                    break;
                case ViewType.Data:
                    if (normalDataUC == null) 
                    {
                        normalDataUC = new DataTableUC();
                        normalDataUC.SetHeader("Исходные данные:");
                        normalDataUC.Table.SetTable( MatrixOperations.Round(MatrixOperations.Transpose(mainVM.MatrixValues), RoundConverter.Round), mainVM.MatrixHeaders);
                    }
                    cpMainContent.Content = normalDataUC;
                    break;
                case ViewType.NormalizedData:
                    if (normalizedDataUC == null) 
                    {
                        normalizedDataUC = new DataTableUC();
                        normalizedDataUC.SetHeader("Нормированные данные:");
                        normalizedDataUC.Table.SetTable(MatrixOperations.Round(MatrixOperations.Transpose(mainVM.MatrixNormalizedValues), RoundConverter.Round), mainVM.MatrixHeaders);
                    }
                    cpMainContent.Content = normalizedDataUC;
                    break;
                case ViewType.NormalizedStatistic:
                    if (normilizeStatisticsUC == null) 
                    {
                        normilizeStatisticsUC = new DataTableUC();
                        normilizeStatisticsUC.SetHeader("Описательная статистика для нормированной выборки:");
                        normilizeStatisticsUC.Table.SetTable(MatrixOperations.Round(MatrixOperations.Transpose(mainVM.MatrixNormalizedStatisticsValues), RoundConverter.Round), mainVM.MatrixHeaders, mainVM.StatisticsHeaders);
                    }
                    cpMainContent.Content = normilizeStatisticsUC;
                    break;
                case ViewType.NormalDistribution:
                    if (normalDistributionUC == null)
                    {
                        normalDistributionUC = new DataTableUC();
                        normalDistributionUC.SetHeader($"Проверка гипотезы о нормальности распределения выборок (χ2-крит = {mainVM.ChiSquareCrit}):");
                        normalDistributionUC.Table.SetTable(mainVM.MatrixNormalDistribution, mainVM.MatrixHeaders, mainVM.NormalDistributionHeaders);
                    }
                    cpMainContent.Content = normalDistributionUC;
                    break;
                case ViewType.PairCorrelations:
                    if (pairCorrelationsUC == null)
                    {
                        pairCorrelationsUC = new DataTableUC();
                        pairCorrelationsUC.SetHeader($"Матрица парных корреляций:");
                        pairCorrelationsUC.Table.SetTable(MatrixOperations.Round(mainVM.PairCorrelationsMatrix, RoundConverter.Round), mainVM.MatrixHeaders, mainVM.MatrixHeaders);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.2 && Math.Abs(e) < 0.4, Brushes.LightGreen);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.4 && Math.Abs(e) < 0.6, new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0x9B, 0xFF)));
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.6 && Math.Abs(e) < 0.8, Brushes.Orange);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.8 && Math.Abs(e) <= 1, Brushes.Red);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) == 1, Brushes.Black);
                        pairCorrelationsUC.cpFooter.Content = new CorrelationColor();
                    }
                    cpMainContent.Content = pairCorrelationsUC;
                    break;
                case ViewType.ParticalCorrelations:
                    if (particalCorrelationsUC == null)
                    {
                        particalCorrelationsUC = new DataTableUC();
                        particalCorrelationsUC.SetHeader($"Матрица частных корреляций:");
                        particalCorrelationsUC.Table.SetTable(MatrixOperations.Round(mainVM.ParticalCorrelationsMatrix, RoundConverter.Round), mainVM.MatrixHeaders, mainVM.MatrixHeaders);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.2 && Math.Abs(e) < 0.4, Brushes.LightGreen);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.4 && Math.Abs(e) < 0.6, new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0x9B, 0xFF)));
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.6 && Math.Abs(e) < 0.8, Brushes.Orange);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.8 && Math.Abs(e) < 1, Brushes.Red);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) == 1, Brushes.Black);
                        particalCorrelationsUC.cpFooter.Content = new CorrelationColor();
                    }
                    cpMainContent.Content = particalCorrelationsUC;
                    break;
                case ViewType.SignificanceCorrelations:
                    if (significanceCorrelationsUC == null)
                    {
                        significanceCorrelationsUC = new SignificanceUC();
                        significanceCorrelationsUC.SetHeader1($"Значимость коэффициентов парной корреляции (t-крит = {mainVM.TStudentCritSign}):");
                        significanceCorrelationsUC.Table1.SetTable(MatrixOperations.Round(mainVM.PairSignificanceCorrelationsMatrix, RoundConverter.Round), mainVM.MatrixHeaders, mainVM.MatrixHeaders);
                        significanceCorrelationsUC.Table1.Highlight(e => e >= mainVM.TStudentCritSign, Brushes.LightGreen);
                        significanceCorrelationsUC.SetHeader2($"Значимость коэффициентов частной корреляции (t-крит = {mainVM.TStudentCritSign}):");
                        significanceCorrelationsUC.Table2.SetTable(MatrixOperations.Round(mainVM.ParticalSignificanceCorrelationsMatrix, RoundConverter.Round), mainVM.MatrixHeaders, mainVM.MatrixHeaders);
                        significanceCorrelationsUC.Table2.Highlight(e => e >= mainVM.TStudentCritSign, Brushes.LightGreen);
                    }
                    cpMainContent.Content = significanceCorrelationsUC;
                    break;
                case ViewType.MultipleCorrelation:
                    if (multipleCorrelationUC == null)
                    {
                        multipleCorrelationUC = new DataTableUC();
                        multipleCorrelationUC.SetHeader($"Множественная корреляция (F-крит = {mainVM.FCritSignMultiple}):");
                        double[][] table = mainVM.MultipleCorrelationMatrix;
                        multipleCorrelationUC.Table.SetTable(MatrixOperations.Round(table, RoundConverter.Round), mainVM.MatrixHeaders, new string[] { "R", "D", "Значимость" });
                        multipleCorrelationUC.Table.Highlight(e => e >= mainVM.FCritSignMultiple, Brushes.LightGreen, 2);
                    }
                    cpMainContent.Content = multipleCorrelationUC;
                    break;
                case ViewType.CorrelativePleiad:
                    if (correlationDiagramPageUC == null)
                    {
                        CorrelationDiagramUC pairDiagram = new CorrelationDiagramUC(MatrixOperations.Round(mainVM.PairCorrelationsMatrix, RoundConverter.Round));
                        CorrelationDiagramUC particalDiagram = new CorrelationDiagramUC(MatrixOperations.Round(mainVM.ParticalCorrelationsMatrix, RoundConverter.Round));
                        correlationDiagramPageUC = new CorrelationDiagramMainUC();
                        correlationDiagramPageUC.SetPairDiagram(pairDiagram);
                        correlationDiagramPageUC.SetParticalDiagram(particalDiagram);
                        correlationDiagramPageUC.SetHeader("Диаграммы корреляционных плеяд для коэффициентов парной (слева) и частной (справа) корреляций:");
                        correlationDiagramPageUC.SetCorrelationColor(new CorrelationColor());
                        StringBuilder statHeader = new StringBuilder();

                        //--формирование statHeader--//
                        statHeader.AppendLine($"Y: {mainVM.MatrixHeaders[0]}");
                        for (int i = 1; i < mainVM.MatrixHeaders.Length; i++)
                            statHeader.AppendLine($"X{i}: {mainVM.MatrixHeaders[i]}");
                        //--//

                        correlationDiagramPageUC.SetStatHeader(statHeader.ToString());
                        correlationDiagramPageUC.SetCorrelationSign(new CorrelationSignDiagramUC());
                    }
                    cpMainContent.Content = correlationDiagramPageUC;
                    break;
                case ViewType.Regression:
                    if (regressionUC == null)
                    {
                        regressionUC = new RegressionUC(mainVM);
                        //формирование коэффициентов уравнения
                        string[][] regressionCoeffs = new string[4][];
                        regressionCoeffs[0] = new string[] { "-" }.Concat(mainVM.MatrixHeadersRegression.Skip(1)).ToArray();
                        regressionCoeffs[1] = mainVM.RegressionCoeffs.Select(e => e.ToString()).ToArray();
                        regressionCoeffs[2] = Enumerable.Range(0, mainVM.RegressionCoeffs.Length).Select(idx => $"{Math.Round(mainVM.RegressionCoeffs[idx], RoundConverter.Round)} ± {Math.Round(mainVM.IntervalEstimateCoeffs[idx], RoundConverter.Round)}").ToArray();
                        regressionCoeffs[3] = mainVM.SignificanceEquationCoeffs.Select(e => e.ToString()).ToArray();
                        string[] rowHeader = { "Название параметра", "Коэффициент регрессии b", "Интервальная оценка β", "Значимость" };
                        string[] colHeader = new string[mainVM.MatrixHeadersRegression.Length].Select((e, idx) => $"{idx}").ToArray();
                        regressionUC.CoeffTable.SetTable(MatrixOperations.Round(regressionCoeffs, RoundConverter.Round), colHeader, rowHeader);
                        regressionUC.SetTKrit(mainVM.TCritEquationCoeffsSign);
                        regressionUC.CoeffTable.Highlight(e => e >= mainVM.TCritEquationCoeffsSign, Brushes.LightGreen, 3);
                        //--формирование уравнения регрессии в виде строки--//
                        string equation = "y = ";
                        double[] regressionCoeffsVMCopy = mainVM.RegressionCoeffs.Select(e => Math.Round(e, RoundConverter.Round)).ToArray();
                        for (int i = 1; i < regressionCoeffsVMCopy.Length; i++)
                            equation += regressionCoeffsVMCopy[i] < 0 ? $"({regressionCoeffsVMCopy[i]})⋅x{i} + " : $"{regressionCoeffsVMCopy[i]}⋅x{i} + ";
                        equation += regressionCoeffsVMCopy[0];
                        //--//
                        regressionUC.SetRegressionEquation(equation);

                        //значимость уравнения регрессии
                        regressionUC.SetSignificanceEquation(mainVM.FСritEquationSign, Math.Round(mainVM.SignificanceEquation, RoundConverter.Round));

                        //Формирование матрицы оценки точности уравнения (точечная)//
                        string[][] errors = new string[4][].Select(e => e = new string[mainVM.MatrixNormalizedValuesRegression[0].Length]).ToArray();
                        errors[0] = mainVM.MatrixNormalizedValuesRegression[0].Select(e => e.ToString()).ToArray();
                        errors[1] = mainVM.CalculatedY.Select(e => e.ToString()).ToArray();
                        errors[2] = mainVM.AbsoluteErrorY.Select(e => e.ToString()).ToArray();
                        errors[3] = Enumerable.Range(0, mainVM.CalculatedY.Length).Select(idx => $"{Math.Round(mainVM.CalculatedY[idx], RoundConverter.Round)} ± {Math.Round(mainVM.IntervalEstimateEquation[idx], RoundConverter.Round)}").ToArray();
                        //errors[4] = Enumerable.Range(0, mainVM.CalculatedY.Length).Select(idx => $"{Math.Round(mainVM.CalculatedY[idx], RoundConverter.Round)} ± {Math.Round(mainVM.IntervalPredicationEquation[idx], RoundConverter.Round)}").ToArray();
                        regressionUC.ErrorTable.SetTable(MatrixOperations.Round(errors, RoundConverter.Round), null, new string[] { "Y исходные", "Ŷ расчетные (Ŷ = X*b)", "Абсолютная ошибка (Y - Ŷ)", "Интервальная оценка Ỹ" });
                        regressionUC.SetApproximationError(Math.Round(mainVM.ApproximationError * 100, RoundConverter.Round).ToString());
                        //--//

                        //Формирование прогнозирования//
                        regressionUC.SetPredication(mainVM.RegressionCoeffs, mainVM.IntervalNormallized);
                        Label kek = new Label();

                    }
                    cpMainContent.Content = regressionUC;
                    break;
                case ViewType.RegressionParamsControl:
                    if (regressionParamsControlUC == null)
                    {
                        regressionParamsControlUC = new RegressionParamsControlUC(mainVM);
                        regressionParamsControlUC.SetChoiceParamsTable(mainVM.MatrixHeaders);
                    }
                    cpMainContent.Content = regressionParamsControlUC;
                    break;
            }
        }

        /// <summary>
        /// Мои привязки
        /// </summary>
        void InitializeMyBindings()
        {
            mainVM.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case "MatrixValues":
                        //обнулить кэш
                        mainUC = null;
                        normalDataUC = null;
                        normalizedDataUC = null;
                        normilizeStatisticsUC = null;
                        normalDistributionUC = null;
                        pairCorrelationsUC = null;
                        particalCorrelationsUC = null;
                        significanceCorrelationsUC = null;
                        multipleCorrelationUC = null;
                        correlationDiagramPageUC = null;
                        regressionUC = null;
                        regressionParamsControlUC = null;
                        break;
                    case "MatrixHeadersRegression":
                        //обнулить кэш
                        regressionUC = null;
                        break;
                }
            };
        }
    }
}
