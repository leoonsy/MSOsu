using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace MSOsu.View
{
    /// <summary>
    /// Логика взаимодействия для InformationMessage.xaml
    /// </summary>
    public partial class MetroMessage : MetroWindow
    {
        public MetroMessage()
        {
            InitializeComponent();
        }

        public void SetMessage(string message, string caption, bool isError)
        {
            tbInformation.Text = message;
            if (isError)
            {
                this.Title = caption;
                this.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xC0, 0xC0));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
