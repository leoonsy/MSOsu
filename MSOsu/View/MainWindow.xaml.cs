using MSOsu.Model;
using MSOsu.Service;
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
    public partial class MainWindow : Window, ICodeBehind
    {
        MainWindowVM mainVm;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainVm = new MainWindowVM(this);
            this.DataContext = mainVm;
            LoadView(ViewType.Main);
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }


        public void LoadView(ViewType type)
        {
            switch (type)
            {
                case ViewType.Main:
                    MainUC mUc = new MainUC();
                    mUc.DataContext = mainVm;
                    MainContent.Content = mUc;
                    break;
                case ViewType.BaseData:

                    TableUC tUc = new TableUC();
                    //
                    ValuesColumn[] dataTable = TableControl.GetTable(@"../../../Data/Младенческая смертность.csv");
                    string[] headers = TableControl.GetHeaders(dataTable);
                    double[,] values = TableControl.GetValues(dataTable);
                    tUc.NewTable(values, headers);

                    //
                    tUc.DataContext = mainVm;
                    MainContent.Content = tUc;
                    break;
            }
        }
    }
}
