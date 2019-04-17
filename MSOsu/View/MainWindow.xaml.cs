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
using MSOsu.Common;

namespace MSOsu.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewService
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
                        normalDataUC.Table.SetTable( Matrix.GetTransposeTable(mainVM.TableValues), mainVM.TableHeaders);
                        normalDataUC.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = normalDataUC;
                    break;
                case ViewType.NormalizedData:
                    if (normalizedDataUC == null) 
                    {
                        normalizedDataUC = new DataTableUC();
                        normalizedDataUC.SetHeader("Нормализованные данные:");
                        normalizedDataUC.Table.SetTable(Matrix.GetTransposeTable(mainVM.TableNormalizedValues), mainVM.TableHeaders);
                        normalizedDataUC.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = normalizedDataUC;
                    break;
                case ViewType.Statistic:
                    if (statisticsUC == null) 
                    {
                        statisticsUC = new DataTableUC();
                        statisticsUC.SetHeader("Описательная статистика для исходной выборки:");
                        statisticsUC.Table.SetTable(Matrix.GetTransposeTable(mainVM.TableStatisticsValues), mainVM.TableHeaders, mainVM.StatisticsHeaders);
                        statisticsUC.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = statisticsUC;
                    break;
                case ViewType.NormalizedStatistic:
                    if (normilizeStatisticsUC == null) 
                    {
                        normilizeStatisticsUC = new DataTableUC();
                        normilizeStatisticsUC.SetHeader("Описательная статистика для нормализованной выборки:");
                        normilizeStatisticsUC.Table.SetTable(Matrix.GetTransposeTable(mainVM.TableNormalizedStatisticsValues), mainVM.TableHeaders, mainVM.StatisticsHeaders);
                        normilizeStatisticsUC.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = normilizeStatisticsUC;
                    break;
                case ViewType.NormalDistribution:
                    if (normalDistribution == null)
                    {
                        normalDistribution = new DataTableUC();
                        normalDistribution.SetHeader($"Проверка гипотезы о нормальности распределения выборок (χ-крит = {mainVM.ChiSquareKrit})");
                        normalDistribution.Table.SetTable(Matrix.GetTransposeTable(mainVM.TableNormalDistribution), mainVM.TableHeaders, mainVM.NormalDistributionHeaders);
                        normalDistribution.Table.DataContext = mainVM;
                    }
                    cpMainContent.Content = normalDistribution;
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
                    case "Table":
                        //обнулить кэш
                        mainUC = null;
                        normalDataUC = null;
                        normalizedDataUC = null;
                        statisticsUC = null;
                        normilizeStatisticsUC = null;
                        normalDistribution = null;
                        break;
                }
            };
        }
    }
}
