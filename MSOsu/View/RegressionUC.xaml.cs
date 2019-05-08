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
    /// Логика взаимодействия для RegressionUC.xaml
    /// </summary>
    public partial class RegressionUC : UserControl
    {
        public TableUC CoeffTable { get; }
        public TableUC ErrorTable { get; }
        List<TextBox> tbParamCoeffs = new List<TextBox>();
        MainWindowVM mainVM;
        public RegressionUC(MainWindowVM vm)
        {
            InitializeComponent();
            CoeffTable = new TableUC();
            ErrorTable = new TableUC();
            cpCoeffTable.Content = CoeffTable;
            cpErrorTable.Content = ErrorTable;
            mainVM = vm;
            DataContext = mainVM;
        }

        public void SetRegressionEquation(string equation)
        {
            tblRegressionEquation.Text = equation;
        }

        public void SetApproximationError(string error)
        {
            tblApproximationError.Text = error;
        }

        public void SetSignificanceEquation(double fKrit, double significanceEquation)
        {
            tblFkrit.Text = fKrit.ToString();
            tblSignificanceEquation.Text = significanceEquation.ToString();
            if (significanceEquation > fKrit)
                tblSignificanceEquation.Foreground = Brushes.LimeGreen;
            else
                tblSignificanceEquation.Foreground = Brushes.Red;
        }

        public void SetTKrit(double tKrit)
        {
            tblTKrit.Text = tKrit.ToString();
        }

        public void SetForecast(double[] coeffs)
        {
            mainVM.ForecastingParamCoeffs = new string[coeffs.Length - 1];
            int tbWidth = 60;
            int tbHeight = 25;
            int tbMarginRight = 3;
            for (int i = 1; i < coeffs.Length; i++)
            {
                TextBlock tblParamCoeffs = new TextBlock();
                TextBox tbCoeffs = new TextBox();
                TextBlock plus = new TextBlock();
                plus.Text = " + ";
                plus.VerticalAlignment = VerticalAlignment.Center;
                tbCoeffs.Width = tbWidth;
                tbCoeffs.Height = tbHeight;
                tbCoeffs.MinHeight = tbHeight;
                tbCoeffs.Margin = new Thickness(0, 0, tbMarginRight, 0);
                tbCoeffs.VerticalAlignment = VerticalAlignment.Center;
                tbCoeffs.TextChanged += TextBox_TextChanged;
                tbCoeffs.Tag = i - 1;
                tblParamCoeffs.Text = coeffs[i] < 0 ? $"({coeffs[i].ToString()})" : $"{coeffs[i].ToString()}";
                tblParamCoeffs.Text += " · ";
                tblParamCoeffs.VerticalAlignment = VerticalAlignment.Center;
                
                wpForecast.Children.Add(tblParamCoeffs);
                wpForecast.Children.Add(tbCoeffs);
                wpForecast.Children.Add(plus);
                tbParamCoeffs.Add(tbCoeffs);
            }
            TextBlock tblLast= new TextBlock();
            tblLast.VerticalAlignment = VerticalAlignment.Center;
            tblLast.Text = coeffs[0].ToString();
            wpForecast.Children.Add(tblLast);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tbCurrent = (TextBox)sender;
            mainVM.ForecastingParamCoeffs[(int)tbCurrent.Tag] = tbCurrent.Text;
        }
    }
}
