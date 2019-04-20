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
    /// Логика взаимодействия для SignificanceUC.xaml
    /// </summary>
    public partial class SignificanceUC : UserControl
    {
        public SignificanceUC()
        {
            InitializeComponent();
            Table1 = new TableUC();
            cpTable1.Content = Table1;
            Table2 = new TableUC();
            cpTable2.Content = Table2;
        }

        public TableUC Table1 { get; }
        public TableUC Table2 { get; }

        public void SetHeader1(string text)
        {
            tblHeader1.Text = text;
        }

        public void SetHeader2(string text)
        {
            tblHeader2.Text = text;
        }
    }
}
