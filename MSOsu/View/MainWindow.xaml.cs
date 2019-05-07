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
        RegressionUC regression = null;
        /// <summary>
        /// Загрузить контент
        /// </summary>
        /// <param name="type"></param>
        public void LoadView(ViewType type)
        {
            const int round = 4; //насколько округлять 
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
                        normalDataUC.Table.SetTable( MatrixOperations.Round(MatrixOperations.Transpose(mainVM.TableValues), round), mainVM.TableHeaders);
                    }
                    cpMainContent.Content = normalDataUC;
                    break;
                case ViewType.NormalizedData:
                    if (normalizedDataUC == null) 
                    {
                        normalizedDataUC = new DataTableUC();
                        normalizedDataUC.SetHeader("Нормированные данные:");
                        normalizedDataUC.Table.SetTable(MatrixOperations.Round(MatrixOperations.Transpose(mainVM.TableNormalizedValues), round), mainVM.TableHeaders);
                    }
                    cpMainContent.Content = normalizedDataUC;
                    break;
                case ViewType.NormalizedStatistic:
                    if (normilizeStatisticsUC == null) 
                    {
                        normilizeStatisticsUC = new DataTableUC();
                        normilizeStatisticsUC.SetHeader("Описательная статистика для нормированной выборки:");
                        normilizeStatisticsUC.Table.SetTable(MatrixOperations.Round(MatrixOperations.Transpose(mainVM.TableNormalizedStatisticsValues), round), mainVM.TableHeaders, mainVM.StatisticsHeaders);
                    }
                    cpMainContent.Content = normilizeStatisticsUC;
                    break;
                case ViewType.NormalDistribution:
                    if (normalDistributionUC == null)
                    {
                        normalDistributionUC = new DataTableUC();
                        normalDistributionUC.SetHeader($"Проверка гипотезы о нормальности распределения выборок (χ2-крит = {mainVM.ChiSquareKrit}):");
                        normalDistributionUC.Table.SetTable(mainVM.TableNormalDistribution, mainVM.TableHeaders, mainVM.NormalDistributionHeaders);
                    }
                    cpMainContent.Content = normalDistributionUC;
                    break;
                case ViewType.PairCorrelations:
                    if (pairCorrelationsUC == null)
                    {
                        pairCorrelationsUC = new DataTableUC();
                        pairCorrelationsUC.SetHeader($"Матрица парных корреляций:");
                        pairCorrelationsUC.Table.SetTable(MatrixOperations.Round(mainVM.PairCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.3 && Math.Abs(e) < 0.5, Brushes.LightGreen);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.5 && Math.Abs(e) < 0.7, new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0x9B, 0xFF)));
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.7 && Math.Abs(e) < 0.9, Brushes.Orange);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.9 && Math.Abs(e) <= 1, Brushes.Red);
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
                        particalCorrelationsUC.Table.SetTable(MatrixOperations.Round(mainVM.ParticalCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.3 && Math.Abs(e) < 0.5, Brushes.LightGreen);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.5 && Math.Abs(e) < 0.7, new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0x9B, 0xFF)));
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.7 && Math.Abs(e) < 0.9, Brushes.Orange);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.9 && Math.Abs(e) < 1, Brushes.Red);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) == 1, Brushes.Black);
                        particalCorrelationsUC.cpFooter.Content = new CorrelationColor();
                    }
                    cpMainContent.Content = particalCorrelationsUC;
                    break;
                case ViewType.SignificanceCorrelations:
                    if (significanceCorrelationsUC == null)
                    {
                        significanceCorrelationsUC = new SignificanceUC();
                        significanceCorrelationsUC.SetHeader1($"Значимость коэффициентов парной корреляции (t-крит = {mainVM.TStudentKritSign}):");
                        significanceCorrelationsUC.Table1.SetTable(MatrixOperations.Round(mainVM.PairSignificanceCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        significanceCorrelationsUC.Table1.Highlight(e => e >= mainVM.TStudentKritSign, Brushes.LightGreen);
                        significanceCorrelationsUC.SetHeader2($"Значимость коэффициентов частной корреляции (t-крит = {mainVM.TStudentKritSign}):");
                        significanceCorrelationsUC.Table2.SetTable(MatrixOperations.Round(mainVM.ParticalSignificanceCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        significanceCorrelationsUC.Table2.Highlight(e => e >= mainVM.TStudentKritSign, Brushes.LightGreen);
                    }
                    cpMainContent.Content = significanceCorrelationsUC;
                    break;
                case ViewType.MultipleCorrelation:
                    if (multipleCorrelationUC == null)
                    {
                        multipleCorrelationUC = new DataTableUC();
                        multipleCorrelationUC.SetHeader($"Множественная корреляция (F-крит = {mainVM.FKritSignMultiple}):");
                        double[][] table = mainVM.MultipleCorrelationMatrix;
                        multipleCorrelationUC.Table.SetTable(MatrixOperations.Round(table, round), mainVM.TableHeaders, new string[] { "R", "D", "Значимость" });
                        multipleCorrelationUC.Table.Highlight(e => e >= mainVM.FKritSignMultiple, Brushes.LightGreen, 2);
                    }
                    cpMainContent.Content = multipleCorrelationUC;
                    break;
                case ViewType.CorrelativePleiad:
                    if (correlationDiagramPageUC == null)
                    {
                        CorrelationDiagramUC pairDiagram = new CorrelationDiagramUC(MatrixOperations.Round(mainVM.PairCorrelationsMatrix, round));
                        CorrelationDiagramUC particalDiagram = new CorrelationDiagramUC(MatrixOperations.Round(mainVM.ParticalCorrelationsMatrix, round));
                        correlationDiagramPageUC = new CorrelationDiagramMainUC();
                        correlationDiagramPageUC.SetPairDiagram(pairDiagram);
                        correlationDiagramPageUC.SetParticalDiagram(particalDiagram);
                        correlationDiagramPageUC.SetHeader("Диаграммы корреляционных плеяд для коэффициентов парной (слева) и частной (справа) корреляций:");
                        correlationDiagramPageUC.SetCorrelationColor(new CorrelationColor());
                        StringBuilder statHeader = new StringBuilder();

                        //--формирование statHeader--//
                        statHeader.AppendLine($"Y: {mainVM.TableHeaders[0]}");
                        for (int i = 1; i < mainVM.TableHeaders.Length; i++)
                            statHeader.AppendLine($"X{i}: {mainVM.TableHeaders[i]}");
                        //--//

                        correlationDiagramPageUC.SetStatHeader(statHeader.ToString());
                        correlationDiagramPageUC.SetCorrelationSign(new CorrelationSignDiagram());
                    }
                    cpMainContent.Content = correlationDiagramPageUC;
                    break;
                case ViewType.Regression:
                    if (regression == null)
                    {
                        regression = new RegressionUC();
                        //формирование коэффициентов уравнения
                        string[][] regressionCoeffs = new string[3][];
                        regressionCoeffs[0] = new string[] { "-" }.Concat(mainVM.TableHeaders.Skip(1)).ToArray();
                        regressionCoeffs[1] = mainVM.RegressionCoeffs.Select(e => Math.Round(e, round).ToString()).ToArray();
                        regressionCoeffs[2] = mainVM.SignificanceEquationCoeffs.Select(e => Math.Round(e, round).ToString()).ToArray();
                        string[] rowHeader = { "Параметр", "Коэффициент регрессии", "Значимость" };
                        string[] colHeader = new string[mainVM.TableHeaders.Length].Select((e, idx) => $"b{idx}").ToArray();
                        regression.CoeffTable.SetTable(regressionCoeffs, colHeader, rowHeader);
                        regression.SetTKrit(mainVM.TKritEquationCoeffsSign);
                        regression.CoeffTable.Highlight(e => e >= mainVM.TKritEquationCoeffsSign, Brushes.LightGreen, 2);
                        //--формирование уравнения регрессии в виде строки--//
                        string equation = "y = ";
                        for (int i = 1; i < regressionCoeffs[1].Length; i++)
                            equation += double.Parse(regressionCoeffs[1][i]) < 0 ? $"({regressionCoeffs[1][i]})⋅x{i} + " : $"{regressionCoeffs[1][i]}⋅x{i} + ";
                        equation += regressionCoeffs[1][0].ToString();
                        //--//
                        regression.SetRegressionEquation(equation);

                        //значимость уравнения регрессии
                        regression.SetSignificanceEquation(mainVM.FKritEquationSign, Math.Round(mainVM.SignificanceEquation, round));

                        //Формирование матрицы оценки точности уравнения (точечная)//
                        double[][] errors = new double[3][].Select(e => e = new double[mainVM.TableNormalizedValues[0].Length]).ToArray();
                        errors[0] = mainVM.TableNormalizedValues[0];
                        errors[1] = mainVM.CalculatedY;
                        errors[2] = mainVM.AbsoluteErrorY;
                        regression.ErrorTable.SetTable(MatrixOperations.Round(errors, round), null, new string[] { "Y исходные", "Ŷ расчетные (Ŷ = X*b)", "Абсолютная ошибка (Y - Ŷ)" });
                        regression.SetApproximationError(Math.Round(mainVM.ApproximationError * 100, round).ToString());
                        //--//
                    }
                    cpMainContent.Content = regression;
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
                    case "TableValues":
                        //обнулить кэш
                        mainUC = null;
                        normalDataUC = null;
                        normalizedDataUC = null;
                        normilizeStatisticsUC = null;
                        normalDistributionUC = null;
                        particalCorrelationsUC = null;
                        significanceCorrelationsUC = null;
                        multipleCorrelationUC = null;
                        correlationDiagramPageUC = null;
                        regression = null;
                        break;
                }
            };
        }
    }
}
