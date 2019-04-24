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
                        normalDataUC.Table.SetTable( MatrixOperations.RoundMatrix(MatrixOperations.GetTransposeTable(mainVM.TableValues), round), mainVM.TableHeaders);
                    }
                    cpMainContent.Content = normalDataUC;
                    break;
                case ViewType.NormalizedData:
                    if (normalizedDataUC == null) 
                    {
                        normalizedDataUC = new DataTableUC();
                        normalizedDataUC.SetHeader("Нормированные данные:");
                        normalizedDataUC.Table.SetTable(MatrixOperations.RoundMatrix(MatrixOperations.GetTransposeTable(mainVM.TableNormalizedValues), round), mainVM.TableHeaders);
                    }
                    cpMainContent.Content = normalizedDataUC;
                    break;
                case ViewType.NormalizedStatistic:
                    if (normilizeStatisticsUC == null) 
                    {
                        normilizeStatisticsUC = new DataTableUC();
                        normilizeStatisticsUC.SetHeader("Описательная статистика для нормированной выборки:");
                        normilizeStatisticsUC.Table.SetTable(MatrixOperations.RoundMatrix(MatrixOperations.GetTransposeTable(mainVM.TableNormalizedStatisticsValues), round), mainVM.TableHeaders, mainVM.StatisticsHeaders);
                    }
                    cpMainContent.Content = normilizeStatisticsUC;
                    break;
                case ViewType.NormalDistribution:
                    if (normalDistributionUC == null)
                    {
                        normalDistributionUC = new DataTableUC();
                        normalDistributionUC.SetHeader($"Проверка гипотезы о нормальности распределения выборок (χ2-крит = {mainVM.ChiSquareKrit}):");
                        normalDistributionUC.Table.SetTable(MatrixOperations.GetTransposeTable(mainVM.TableNormalDistribution), mainVM.TableHeaders, mainVM.NormalDistributionHeaders);
                    }
                    cpMainContent.Content = normalDistributionUC;
                    break;
                case ViewType.PairCorrelations:
                    if (pairCorrelationsUC == null)
                    {
                        pairCorrelationsUC = new DataTableUC();
                        pairCorrelationsUC.SetHeader($"Матрица парных корреляций:");
                        pairCorrelationsUC.Table.SetTable(MatrixOperations.RoundMatrix(mainVM.PairCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.3 && Math.Abs(e) < 0.5, Brushes.LightGreen);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.5 && Math.Abs(e) < 0.7, new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0x9B, 0xFF)));
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.7 && Math.Abs(e) < 0.9, Brushes.Orange);
                        pairCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.9 && Math.Abs(e) <= 1, Brushes.Red);
                        pairCorrelationsUC.cpFooter.Content = new CorrelationColor();
                    }
                    cpMainContent.Content = pairCorrelationsUC;
                    break;
                case ViewType.ParticalCorrelations:
                    if (particalCorrelationsUC == null)
                    {
                        particalCorrelationsUC = new DataTableUC();
                        particalCorrelationsUC.SetHeader($"Матрица частных корреляций:");
                        particalCorrelationsUC.Table.SetTable(MatrixOperations.RoundMatrix(mainVM.ParticalCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.3 && Math.Abs(e) < 0.5, Brushes.LightGreen);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.5 && Math.Abs(e) < 0.7, new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0x9B, 0xFF)));
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.7 && Math.Abs(e) < 0.9, Brushes.Orange);
                        particalCorrelationsUC.Table.Highlight(e => Math.Abs(e) >= 0.9 && Math.Abs(e) <= 1, Brushes.Red);
                        particalCorrelationsUC.cpFooter.Content = new CorrelationColor();
                    }
                    cpMainContent.Content = particalCorrelationsUC;
                    break;
                case ViewType.SignificanceCorrelations:
                    if (significanceCorrelationsUC == null)
                    {
                        significanceCorrelationsUC = new SignificanceUC();
                        significanceCorrelationsUC.SetHeader1($"Значимость коэффициентов парной корреляции (t-крит = {mainVM.TStudentKritSign}):");
                        significanceCorrelationsUC.Table1.SetTable(MatrixOperations.RoundMatrix(mainVM.PairSignificanceCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        significanceCorrelationsUC.Table1.Highlight(e => e >= mainVM.TStudentKritSign, Brushes.LightGreen);
                        significanceCorrelationsUC.SetHeader2($"Значимость коэффициентов частной корреляции (t-крит = {mainVM.TStudentKritSign}):");
                        significanceCorrelationsUC.Table2.SetTable(MatrixOperations.RoundMatrix(mainVM.ParticalSignificanceCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        significanceCorrelationsUC.Table2.Highlight(e => e >= mainVM.TStudentKritSign, Brushes.LightGreen);
                    }
                    cpMainContent.Content = significanceCorrelationsUC;
                    break;
                case ViewType.MultipleCorrelation:
                    if (multipleCorrelationUC == null)
                    {
                        multipleCorrelationUC = new DataTableUC();
                        multipleCorrelationUC.SetHeader("Множественная корреляция:");
                        double[][] table = new double[2][].Select(e => e = new double[mainVM.MultipleCorrelation.Length]).ToArray();
                        table[0] = mainVM.MultipleCorrelation;
                        table[1] = mainVM.MultipleCorrelation.Select(e => e * e).ToArray();
                        multipleCorrelationUC.Table.SetTable(MatrixOperations.RoundMatrix(table, round), mainVM.TableHeaders, new string[] { "R", "D" });
                    }
                    cpMainContent.Content = multipleCorrelationUC;
                    break;
                case ViewType.CorrelativePleiad:
                    if (correlationDiagramPageUC == null)
                    {
                        CorrelationDiagramUC pairDiagram = new CorrelationDiagramUC(MatrixOperations.RoundMatrix(mainVM.PairCorrelationsMatrix, round));
                        CorrelationDiagramUC particalDiagram = new CorrelationDiagramUC(MatrixOperations.RoundMatrix(mainVM.ParticalCorrelationsMatrix, round));
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
                        double[][] regressionCoeffs = new double[1][];
                        regressionCoeffs[0] = mainVM.RegressionCoeffs;
                        regressionCoeffs = MatrixOperations.RoundMatrix(regressionCoeffs, round);
                        string[] rowHeader = { "Коэффициенты регрессии:" };
                        string[] colHeader = new string[mainVM.TableHeaders.Length].Select((e, idx) => $"a{idx}").ToArray();
                        regression = new RegressionUC();
                        regression.CoeffTable.SetTable(regressionCoeffs, colHeader, rowHeader);
                        //--формирование уравнения регрессии в виде строки--//
                        string equation = "y = ";
                        for (int i = 1; i < regressionCoeffs[0].Length; i++)
                            equation += regressionCoeffs[0][i] < 0 ? $"({regressionCoeffs[0][i]})⋅x{i-1} + " : $"{regressionCoeffs[0][i]}⋅x{i-1} + ";
                        equation += regressionCoeffs[0][0].ToString();
                        //--//
                        regression.SetRegressionEquation(equation);

                        //Формирование матрицы оценки точности уравнения//
                        double[][] errors = new double[3][].Select(e => e = new double[mainVM.TableNormalizedValues[0].Length]).ToArray();
                        errors[0] = mainVM.TableNormalizedValues[0];
                        errors[1] = mainVM.CalculatedY;
                        errors[2] = mainVM.AbsoluteErrorY;
                        regression.ErrorTable.SetTable(MatrixOperations.RoundMatrix(errors, round), null, new string[] { "Y исходные", "Ỹ расчетные (Ỹ = X*b)", "Абсолютная ошибка (Y - Ỹ)" });
                        regression.SetSLMError(Math.Round(mainVM.LSMError, round).ToString());
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
