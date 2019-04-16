﻿using MSOsu.Model;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                    MainContent.Content = mainUC;
                    break;
                case ViewType.Data:
                    if (normalDataUC == null) //если требуется перерисовка
                    {
                        normalDataUC = new DataTableUC();
                        normalDataUC.SetHeader("Исходные данные:");
                        normalDataUC.Table.SetTable(mainVM.TableValues, mainVM.TableHeaders);
                        normalDataUC.Table.DataContext = mainVM;
                    }
                    MainContent.Content = normalDataUC;
                    break;
                case ViewType.NormalizedData:
                    if (normalizedDataUC == null) //если требуется перерисовка
                    {
                        normalizedDataUC = new DataTableUC();
                        normalizedDataUC.SetHeader("Нормализованные данные:");
                        normalizedDataUC.Table.SetTable(mainVM.TableNormalizedValues, mainVM.TableHeaders);
                        normalizedDataUC.Table.DataContext = mainVM;
                    }
                    MainContent.Content = normalizedDataUC;
                    break;
                case ViewType.Statistic:
                    if (statisticsUC == null) //если требуется перерисовка
                    {
                        statisticsUC = new DataTableUC();
                        statisticsUC.SetHeader("Описательная статистика для исходной выборки:");
                        statisticsUC.Table.SetTable(mainVM.TableStatisticsValues, mainVM.TableHeaders, mainVM.StatisticsHeaders);
                        statisticsUC.Table.DataContext = mainVM;
                    }
                    MainContent.Content = statisticsUC;
                    break;
                case ViewType.NormalizedStatistic:
                    if (normilizeStatisticsUC == null) //если требуется перерисовка
                    {
                        normilizeStatisticsUC = new DataTableUC();
                        normilizeStatisticsUC.SetHeader("Описательная статистика для нормализованной выборки:");
                        normilizeStatisticsUC.Table.SetTable(mainVM.TableNormalizedStatisticsValues, mainVM.TableHeaders, mainVM.StatisticsHeaders);
                        normilizeStatisticsUC.Table.DataContext = mainVM;
                    }
                    MainContent.Content = normilizeStatisticsUC;
                    break;

            }
        }

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
                        break;
                }
            };
        }
    }
}
