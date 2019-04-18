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
        DataTableUC significanceCorrelations = null;

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
                        normalDataUC.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = normalDataUC;
                    break;
                case ViewType.NormalizedData:
                    if (normalizedDataUC == null) 
                    {
                        normalizedDataUC = new DataTableUC();
                        normalizedDataUC.SetHeader("Нормализованные данные:");
                        normalizedDataUC.Table.SetTable(MatrixOperations.RoundMatrix(MatrixOperations.GetTransposeTable(mainVM.TableNormalizedValues), round), mainVM.TableHeaders);
                        normalizedDataUC.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = normalizedDataUC;
                    break;
                case ViewType.Statistic:
                    if (statisticsUC == null) 
                    {
                        statisticsUC = new DataTableUC();
                        statisticsUC.SetHeader("Описательная статистика для исходной выборки:");
                        statisticsUC.Table.SetTable(MatrixOperations.RoundMatrix(MatrixOperations.GetTransposeTable(mainVM.TableStatisticsValues), round), mainVM.TableHeaders, mainVM.StatisticsHeaders);
                        statisticsUC.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = statisticsUC;
                    break;
                case ViewType.NormalizedStatistic:
                    if (normilizeStatisticsUC == null) 
                    {
                        normilizeStatisticsUC = new DataTableUC();
                        normilizeStatisticsUC.SetHeader("Описательная статистика для нормализованной выборки:");
                        normilizeStatisticsUC.Table.SetTable(MatrixOperations.RoundMatrix(MatrixOperations.GetTransposeTable(mainVM.TableNormalizedStatisticsValues), round), mainVM.TableHeaders, mainVM.StatisticsHeaders);
                        normilizeStatisticsUC.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = normilizeStatisticsUC;
                    break;
                case ViewType.NormalDistribution:
                    if (normalDistribution == null)
                    {
                        normalDistribution = new DataTableUC();
                        normalDistribution.SetHeader($"Проверка гипотезы о нормальности распределения выборок (χ-крит = {mainVM.ChiSquareKrit}):");
                        normalDistribution.Table.SetTable(MatrixOperations.GetTransposeTable(mainVM.TableNormalDistribution), mainVM.TableHeaders, mainVM.NormalDistributionHeaders);
                        normalDistribution.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = normalDistribution;
                    break;
                case ViewType.PairCorrelations:
                    if (pairCorrelations == null)
                    {
                        pairCorrelations = new DataTableUC();
                        pairCorrelations.SetHeader($"Матрица парных корреляций:");
                        pairCorrelations.Table.SetTable(MatrixOperations.RoundMatrix(mainVM.PairCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        pairCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.3 && Math.Abs(e) < 0.5, new SolidColorBrush(Color.FromArgb(0xFF,0xDA,0xE0,0xB5)));
                        pairCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.5 && Math.Abs(e) < 0.7, new SolidColorBrush(Color.FromArgb(0xFF,0xF3,0xFF,0x00)));
                        pairCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.7 && Math.Abs(e) < 0.9, new SolidColorBrush(Color.FromArgb(0xFF,0xFF,0x86,0x40)));
                        pairCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.9 && Math.Abs(e) <= 1, new SolidColorBrush(Color.FromArgb(0xFF,0xFF,0x3A,0x3A)));
                        pairCorrelations.cpFooter.Content = new CorrelationColor();
                        pairCorrelations.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = pairCorrelations;
                    break;
                case ViewType.ParticalCorrelations:
                    if (particalCorrelations == null)
                    {
                        particalCorrelations = new DataTableUC();
                        particalCorrelations.SetHeader($"Матрица частных корреляций:");
                        particalCorrelations.Table.SetTable(MatrixOperations.RoundMatrix(mainVM.ParticalCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        particalCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.3 && Math.Abs(e) < 0.5, new SolidColorBrush(Color.FromArgb(0xFF, 0xDA, 0xE0, 0xB5)));
                        particalCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.5 && Math.Abs(e) < 0.7, new SolidColorBrush(Color.FromArgb(0xFF, 0xF3, 0xFF, 0x00)));
                        particalCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.7 && Math.Abs(e) < 0.9, new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x86, 0x40)));
                        particalCorrelations.Table.Highlight(e => Math.Abs(e) >= 0.9 && Math.Abs(e) <= 1, new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x3A, 0x3A)));
                        particalCorrelations.cpFooter.Content = new CorrelationColor();
                        particalCorrelations.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = particalCorrelations;
                    break;
                case ViewType.SignificanceCorrelations:
                    if (significanceCorrelations == null)
                    {
                        significanceCorrelations = new DataTableUC();
                        significanceCorrelations.SetHeader($"Значимость коэффициентов парной корреляции (t-крит = {mainVM.TStudentKrit}):");
                        significanceCorrelations.Table.SetTable(MatrixOperations.RoundMatrix(mainVM.PairSignificanceCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        significanceCorrelations.Table.Highlight(e => e >= mainVM.TStudentKrit, new SolidColorBrush(Color.FromArgb(0xFF,0x75,0xFF,0x5F)));
                        DataTableUC particalCorrelation = new DataTableUC();
                        particalCorrelation.SetHeader($"Значимость коэффициентов частной корреляции (t-крит = {mainVM.TStudentKrit}):");
                        particalCorrelation.Table.SetTable(MatrixOperations.RoundMatrix(mainVM.ParticalSignificanceCorrelationsMatrix, round), mainVM.TableHeaders, mainVM.TableHeaders);
                        particalCorrelation.Table.Highlight(e => e >= mainVM.TStudentKrit, new SolidColorBrush(Color.FromArgb(0xFF, 0x75, 0xFF, 0x5F)));
                        significanceCorrelations.cpFooter.Content = particalCorrelation;
                        significanceCorrelations.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = significanceCorrelations;
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
                        break;
                }
            };
        }
    }
}
