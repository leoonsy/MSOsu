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
    /// Логика взаимодействия для MultipleCorrelationUC.xaml
    /// </summary>
    public partial class MultipleCorrelationUC : UserControl
    {
        public MultipleCorrelationUC()
        {
            InitializeComponent();
        }

        public void SetHeader(string header)
        {
            tblHeaderName.Text = header;
        }

        public void SetMultipleCorrelation(double value)
        {
            tblMultipleCorrelation.Text = value.ToString();
        }

        public void SetDetermination(double value)
        {
            tblDetermination.Text = value.ToString();
        }
    }
}
