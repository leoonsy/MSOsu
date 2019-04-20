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
    /// Логика взаимодействия для CorrelationDiagramMainUC.xaml
    /// </summary>
    public partial class CorrelationDiagramMainUC : UserControl
    {
        public CorrelationDiagramMainUC()
        {
            InitializeComponent();
        }

        public void SetPairDiagram(UserControl diagram)
        {
            cpPairDiagram.Content = diagram;
        }

        public void SetParticalDiagram(UserControl diagram)
        {
            cpParticalDiagram.Content = diagram;
        }

        public void SetStatHeader(string str)
        {
            tblStatHeader.Text = str;
        }

        public void SetHeader(string str)
        {
            tblHeader.Text = str;
        }

        public void SetCorrelationColor(UserControl exp)
        {
            cpCorrelationColor.Content = exp;
        }

        public void SetCorrelationSign(UserControl sign)
        {
            cpCorrelationSign.Content = sign;
        }
    }
}
