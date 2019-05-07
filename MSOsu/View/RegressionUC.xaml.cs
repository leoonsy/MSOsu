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
        public RegressionUC()
        {
            InitializeComponent();
            CoeffTable = new TableUC();
            ErrorTable = new TableUC();
            cpCoeffTable.Content = CoeffTable;
            cpErrorTable.Content = ErrorTable;
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
    }
}
