using MSOsu.Model;
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
    /// Логика взаимодействия для Table.xaml
    /// </summary>
    public partial class TableUC : UserControl
    {
        public TableUC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Создать новую таблицу
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="colHeaders"></param>
        /// <param name="rowHeaders"></param>
        public void SetTable<T>(T[][] matrix, string[] colHeaders = null, string[] rowHeaders = null)
        {
            ClearTable();
            int m = matrix.Length;
            int n = matrix[0].Length;
            CreateGrid(m, n);
            FillColumnsAndRowsHeaders(colHeaders, rowHeaders);
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    gTable.Children.Add(CreateCell(matrix[i][j], i + 1, j + 1));
        }

        ///// <summary>
        ///// Создать новую таблицу
        ///// </summary>
        ///// <param name="table"></param>
        ///// <param name="rowHeaders"></param>
        //public void SetTable(ValuesColumn[] table, string[] rowHeaders = null)
        //{
        //    string[] colHeaders = TableControl.GetHeaders(table);
        //    double[,] matrix = TableControl.GetValues(table);
        //    SetTable(matrix, colHeaders, rowHeaders);
        //}

        /// <summary>
        /// Очистить таблицу
        /// </summary>
        private void ClearTable()
        {
            gTable.Children.Clear();
            gTable.RowDefinitions.Clear();
            gTable.ColumnDefinitions.Clear();
        }

        /// <summary>
        /// Заполнить заголовки столбцов и строк
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        private void FillColumnsAndRowsHeaders(string[] columns, string[] rows)
        {
            if (columns != null)
            {
                //Установка заголовков столбцов
                for (int i = 0; i < columns.Length; i++)
                    gTable.Children.Add(CreateCell(columns[i], 0, i + 1));
            }
            if (rows != null)
            {
                //Установка заголовков строк
                for (int i = 0; i < rows.Length; i++)
                    gTable.Children.Add(CreateCell(rows[i], i + 1, 0));
            }
        }

        /// <summary>
        /// Создать таблицу
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        private void CreateGrid(int m, int n)
        {
            for (int i = 0; i < m + 1; i++) //+1 для headers
                gTable.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < n + 1; i++) //+1 для headers
                gTable.ColumnDefinitions.Add(new ColumnDefinition());
        }

        /// <summary>
        /// Заполнить ячейку в таблице
        /// </summary>
        /// <param name="data"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private TextBox CreateCell(object data, int i, int j)
        {
            TextBox tb = new TextBox()
            {
                Text = data is double ?
                    (double.IsNaN((double)data) ? "-" : data.ToString()) :
                    data.ToString(),
                IsReadOnly = true,
                FontSize = 14,
                Background = i == 0 || j == 0 ? new SolidColorBrush(Color.FromArgb(0xFF,0xD5,0xEE,0xFF)) : Brushes.White
            };
            Grid.SetRow(tb, i);
            Grid.SetColumn(tb, j);
            return tb;
        }
    }
}
