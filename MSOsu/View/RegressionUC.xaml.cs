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
            InitializeMyBindings();
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
            tblFCrit.Text = fKrit.ToString();
            tblSignificanceEquation.Text = significanceEquation.ToString();
            if (significanceEquation > fKrit)
                tblSignificanceEquation.Foreground = Brushes.LimeGreen;
            else
                tblSignificanceEquation.Foreground = Brushes.Red;
        }

        public void SetTKrit(double tKrit)
        {
            tblTCrit.Text = tKrit.ToString();
        }

        public void SetPredication(double[] coeffs, double[] intervals)
        {
            double[] coeffsCopy = (double[])coeffs.Clone();
            //для нормированных коэффициентов
            coeffs = coeffs.Select(e => Math.Round(e, RoundConverter.Round)).ToArray();
            mainVM.PredicationParamCoeffs = new string[coeffs.Length - 1];
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

                wpPredicationCoeffs.Children.Add(tblParamCoeffs);
                wpPredicationCoeffs.Children.Add(tbCoeffs);
                wpPredicationCoeffs.Children.Add(plus);
                tbParamCoeffs.Add(tbCoeffs);
            }
            TextBlock tblLast= new TextBlock();
            tblLast.VerticalAlignment = VerticalAlignment.Center;
            tblLast.Text = coeffs[0].ToString();
            wpPredicationCoeffs.Children.Add(tblLast);

            //для исходных коэффициентов
            coeffs = Enumerable.Range(0, coeffs.Length).Select(idx => coeffsCopy[idx] / intervals[idx]).ToArray();
            mainVM.PredicationParamCoeffs2 = new string[coeffs.Length - 1];
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
                tbCoeffs.TextChanged += TextBox_TextChanged2;
                tbCoeffs.Tag = i - 1;
                tblParamCoeffs.Text = coeffs[i] < 0 ? $"({coeffs[i].ToString("0.####e0")})" : $"{coeffs[i].ToString("0.####e0")}";
                tblParamCoeffs.Text += " · ";
                tblParamCoeffs.VerticalAlignment = VerticalAlignment.Center;

                wpPredicationCoeffs2.Children.Add(tblParamCoeffs);
                wpPredicationCoeffs2.Children.Add(tbCoeffs);
                wpPredicationCoeffs2.Children.Add(plus);
                tbParamCoeffs.Add(tbCoeffs);
            }
            tblLast = new TextBlock();
            tblLast.VerticalAlignment = VerticalAlignment.Center;
            tblLast.Text = coeffs[0].ToString("0.####e0");
            wpPredicationCoeffs2.Children.Add(tblLast);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tbCurrent = (TextBox)sender;
            mainVM.PredicationParamCoeffs[(int)tbCurrent.Tag] = tbCurrent.Text;
        }

        private void TextBox_TextChanged2(object sender, TextChangedEventArgs e)
        {
            TextBox tbCurrent = (TextBox)sender;
            mainVM.PredicationParamCoeffs2[(int)tbCurrent.Tag] = tbCurrent.Text;
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
                    case "PredictionYNormalized":
                        if (mainVM.PredictionYNormalized == 0)
                            wpPredicationResult.Visibility = Visibility.Collapsed;
                        else
                            wpPredicationResult.Visibility = Visibility.Visible;
                        break;
                    case "PredictionYNormalized2":
                        if (mainVM.PredictionYNormalized2 == 0)
                            wpPredicationResult2.Visibility = Visibility.Collapsed;
                        else
                            wpPredicationResult2.Visibility = Visibility.Visible;
                        break;
                }
            };
        }
    }
}
