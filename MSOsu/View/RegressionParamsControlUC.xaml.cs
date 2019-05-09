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
    /// Логика взаимодействия для RegressionParamsControlUC.xaml
    /// </summary>
    public partial class RegressionParamsControlUC : UserControl
    {
        MainWindowVM mainVM;
        public RegressionParamsControlUC(MainWindowVM vm)
        {
            InitializeComponent();
            mainVM = vm;
            DataContext = mainVM;
        }

        public void SetChoiceParamsTable(string[] headers)
        {
            for (int i = 1; i < headers.Length; i++)
            {
                CheckBox cb = new CheckBox();
                cb.IsChecked = true;
                cb.Tag = i;
                cb.Checked += CheckBox_Checked;
                cb.Unchecked += CheckBox_Unchecked;
                cb.Content = $"X{i}: {headers[i]}";
                cb.Margin = new Thickness(0, 0, 0, 3);
                spMain.Children.Add(cb);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            mainVM.EnabledParamRegression[(int)((CheckBox)sender).Tag] = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            mainVM.EnabledParamRegression[(int)((CheckBox)sender).Tag] = false;
        }
    }
}
