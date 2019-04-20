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
        DataTableUC statisticsUC = null;
        DataTableUC normilizeStatisticsUC = null;
        DataTableUC normalDistribution = null;
        DataTableUC pairCorrelations = null;
        DataTableUC particalCorrelations = null;
        SignificanceUC significanceCorrelations = null;
        MultipleCorrelationUC multipleCorrelation = null;
        CorrelationDiagramMainUC correlationDiagramPage = null;

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
                        normalizedDataUC.SetHeader("Нормализованные данные:");
                        normalizedDataUC.Table.SetTable(MatrixOperations.RoundMatrix(MatrixOperations.GetTransposeTable(mainVM.TableNormalizedValues), round), mainVM.TableHeaders);
                    }
                    cpMainContent.Content = normalizedDataUC;
                    break;
                case ViewType.Statistic:
                    if (statisticsUC == null) 
                    {
                        statisticsUC = new DataTableUC();
                        statisticsUC.SetHeader("Описательная статистика для исходной выборки:");
                        statisticsUC.Table.SetTable(MatrixOperations.RoundMatrix(MatrixOperations.GetTransposeTable(mainVM.TableStatisticsValues), round), mainVM.TableHeaders, mainVM.StatisticsHeaders);
                    }
                    cpMainContent.Content = statisticsUC;
                    break;
                case ViewType.NormalizedStatistic:
                    if (normilizeStatisticsUC == null) 
                    {
                        normilizeStatisticsUC = new DataTableUC();
                        normilizeStatisticsUC.SetHeader("Описательная статистика для нормализованной выборки:");
                        normilizeStatisticsUC.Table.SetTable(MatrixOperations.RoundMatrix(MatrixOperations.GetTransposeTable(mainVM.TableNormalizedStatisticsValues), round), mainVM.TableHeaders, mainVM.StatisticsHeaders);
                    }
                    cpMainContent.Content = normilizeStatisticsUC;
                    break;
                case ViewType.NormalDistribution:
                    if (normalDistribution == null)
                    {
                        normalDistribution = new DataTableUC();
                        normalDistribution.SetHeader($"Проверка гипотезы о нормальности распределения выборок (χ2-крит = {mainVM.ChiSquareKrit}):");
                        normalDistribution.Table.SetTable(MatrixOperations.GetTransposeTable(mainVM.TableNormalDistribution), mainVM.TableHeaders, mainVM.NormalDistributionHeaders);
                    }
                    cpMainContent.Content = normalDistribution;
                    break;
                case ViewType.PairCorrelations:
                    if (pairCorrelations == null)
                    {
                        pairCorrelations = new DataTableUC();
                        pairCorrelations.SetHeader($"Матрица парных корреляций:");
                        pairCorrelations.Table.SetTable(MatrixOperations.RoundMatrix(mainVM.PairCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        pairCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.3 && Math.Abs(e) < 0.5, Brushes.LightGreen);
                        pairCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.5 && Math.Abs(e) < 0.7, new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0x9B, 0xFF)));
                        pairCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.7 && Math.Abs(e) < 0.9, Brushes.Orange);
                        pairCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.9 && Math.Abs(e) <= 1, Brushes.Red);
                        pairCorrelations.cpFooter.Content = new CorrelationColor();
                    }
                    cpMainContent.Content = pairCorrelations;
                    break;
                case ViewType.ParticalCorrelations:
                    if (particalCorrelations == null)
                    {
                        particalCorrelations = new DataTableUC();
                        particalCorrelations.SetHeader($"Матрица частных корреляций:");
                        particalCorrelations.Table.SetTable(MatrixOperations.RoundMatrix(mainVM.ParticalCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        particalCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.3 && Math.Abs(e) < 0.5, Brushes.LightGreen);
                        particalCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.5 && Math.Abs(e) < 0.7, new SolidColorBrush(Color.FromArgb(0xFF, 0xDF, 0x9B, 0xFF)));
                        particalCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.7 && Math.Abs(e) < 0.9, Brushes.Orange);
                        particalCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.9 && Math.Abs(e) <= 1, Brushes.Red);
                        particalCorrelations.cpFooter.Content = new CorrelationColor();
                    }
                    cpMainContent.Content = particalCorrelations;
                    break;
                case ViewType.SignificanceCorrelations:
                    if (significanceCorrelations == null)
                    {
                        significanceCorrelations = new SignificanceUC();
                        significanceCorrelations.SetHeader1($"Значимость коэффициентов парной корреляции (t-крит = {mainVM.TStudentKrit}):");
                        significanceCorrelations.Table1.SetTable(MatrixOperations.RoundMatrix(mainVM.PairSignificanceCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        significanceCorrelations.Table1.Highlight(e => e >= mainVM.TStudentKrit, Brushes.LightGreen);
                        significanceCorrelations.SetHeader2($"Значимость коэффициентов частной корреляции (t-крит = {mainVM.TStudentKrit}):");
                        significanceCorrelations.Table2.SetTable(MatrixOperations.RoundMatrix(mainVM.ParticalSignificanceCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        significanceCorrelations.Table2.Highlight(e => e >= mainVM.TStudentKrit, Brushes.LightGreen);
                    }
                    cpMainContent.Content = significanceCorrelations;
                    break;
                case ViewType.MultipleCorrelation:
                    if (multipleCorrelation == null)
                    {
                        multipleCorrelation = new MultipleCorrelationUC();
                        multipleCorrelation.SetHeader(mainVM.TableHeaders[0]);
                        multipleCorrelation.SetMultipleCorrelation( Math.Round(mainVM.MultipleCorrelation, round));
                        multipleCorrelation.SetDetermination(Math.Round(mainVM.MultipleCorrelation * mainVM.MultipleCorrelation, round));
                    }
                    cpMainContent.Content = multipleCorrelation;
                    break;
                case ViewType.CorrelativePleiad:
                    if (correlationDiagramPage == null)
                    {
                        CorrelationDiagramUC pairDiagram = new CorrelationDiagramUC(MatrixOperations.RoundMatrix(mainVM.PairCorrelationsMatrix, round));
                        CorrelationDiagramUC particalDiagram = new CorrelationDiagramUC(MatrixOperations.RoundMatrix(mainVM.ParticalCorrelationsMatrix, round));
                        correlationDiagramPage = new CorrelationDiagramMainUC();
                        correlationDiagramPage.SetPairDiagram(pairDiagram);
                        correlationDiagramPage.SetParticalDiagram(particalDiagram);
                        correlationDiagramPage.SetHeader("Диаграммы корреляционных плеяд для коэффициентов парной (слева) и частной (справа) корреляций");
                        correlationDiagramPage.SetCorrelationColor(new CorrelationColor());
                        StringBuilder statHeader = new StringBuilder();

                        //--формирование statHeader--//
                        statHeader.AppendLine($"Y: {mainVM.TableHeaders[0]}");
                        for (int i = 1; i < mainVM.TableHeaders.Length; i++)
                            statHeader.AppendLine($"X{i}: {mainVM.TableHeaders[i]}");
                        //--//

                        correlationDiagramPage.SetStatHeader(statHeader.ToString());
                        correlationDiagramPage.SetCorrelationSign(new CorrelationSignDiagram());
                    }
                    cpMainContent.Content = correlationDiagramPage;
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
                        statisticsUC = null;
                        normilizeStatisticsUC = null;
                        normalDistribution = null;
                        particalCorrelations = null;
                        significanceCorrelations = null;
                        multipleCorrelation = null;
                        correlationDiagramPage = null;
                        break;
                }
            };
        }
    }
}
