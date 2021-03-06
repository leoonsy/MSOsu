﻿using System;
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
    /// Логика взаимодействия для NormalDataUC.xaml
    /// </summary>
    public partial class DataTableUC : UserControl
    {
        public TableUC Table { get; }
        public DataTableUC()
        {
            InitializeComponent();
            Table = new TableUC();
            cpTable.Content = Table;
        }
        public void SetHeader(string text)
        {
            tblHeader.Text = text;
        }
    }
}
