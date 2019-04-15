using MSOsu.Model;
using MSOsu.Service;
using MSOsu.Service.DialogServices;
using MSOsu.Service.FileServices;
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
            mainVM = new MainWindowVM(this, new CSVServiceVC(), new DefaultDialogService());
            DataContext = mainVM;
            LoadView(ViewType.Main);
        }

        /// <summary>
        /// Загрузить контент
        /// </summary>
        /// <param name="type"></param>
        public void LoadView(ViewType type)
        {
            switch (type)
            {
                case ViewType.Main:
                    MainUC mainUc = new MainUC();
                    mainUc.DataContext = mainVM;
                    MainContent.Content = mainUc;
                    break;
                case ViewType.BaseData:
                    TableUC tableUc = new TableUC();
                    tableUc.NewTable(mainVM.Table);
                    tableUc.DataContext = mainVM;
                    MainContent.Content = tableUc;
                    break;
            }
        }
    }
}
