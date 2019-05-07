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

        public void SetQost(string error)
        {
            tblQost.Text = error;
        }
    }
}
