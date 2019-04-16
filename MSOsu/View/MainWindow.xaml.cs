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
        TableUC tableUC = null;
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
                case ViewType.BaseData:
                    if (tableUC == null) //если требуется перерисовка
                    {
                        tableUC = new TableUC();
                        tableUC.NewTable(mainVM.Table);
                        tableUC.DataContext = mainVM;
                    }
                    MainContent.Content = tableUC;
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
                        tableUC = null; 
                        break;
                }
            };
        }
    }
}
