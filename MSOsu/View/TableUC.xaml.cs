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
        List<List<TextBox>> cells;
        List<string> cHeaders = null, rHeaders = null;


        public TableUC()
        {
            InitializeComponent();
        }

        public void NewTable(double[,] matrix, string[] colHeaders = null, string[] rowHeaders = null)
        {
            ClearTable();
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            cells = new List<List<TextBox>>();
            CreateGrid(n, m);
            FillColumnsAndRowsHeaders(colHeaders, rowHeaders);
            for (int i = 0; i < n; i++)
            {
                cells.Add(new List<TextBox>());
                for (int j = 0; j < m; j++)
                {
                    cells[i].Add(CreateCell(matrix[i, j], i + 1, j + 1));
                    cells[i][j].Tag = matrix[i, j];
                    gTable.Children.Add(cells[i][j]);
                }
            }
        }

        private void ClearTable()
        {
            gTable.Children.Clear();
            cells = null;
            cHeaders = null;
            rHeaders = null;
            gTable.RowDefinitions.Clear();
            gTable.ColumnDefinitions.Clear();
        }

        private void FillColumnsAndRowsHeaders(string[] columnsHeaders, string[] rowsHeaders)
        {
            if (columnsHeaders != null)
                cHeaders = columnsHeaders.ToList();
            if (rowsHeaders != null)
                rHeaders = rowsHeaders.ToList();
            bool c = columnsHeaders != null;
            bool r = rowsHeaders != null;
            if (c)
            {
                int m = columnsHeaders.Length;
                //Установка заголовков столбцов
                for (int i = 0; i < m; i++)
                    gTable.Children.Add(CreateCell(columnsHeaders[i], 0, i + 1));
            }
            if (r)
            {
                int n = rowsHeaders.Length;
                //Установка заголовков строк
                for (int i = 0; i < n; i++)
                    gTable.Children.Add(CreateCell(rowsHeaders[i], i + 1, 0));
            }
        }

        private void CreateGrid(int n, int m)
        {
            for (int i = 0; i < n + 1; i++)
                gTable.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < m + 1; i++)
                gTable.ColumnDefinitions.Add(new ColumnDefinition());
        }

        private TextBox CreateCell(double data, int i, int j)
        {
            TextBox tb = new TextBox()
            {
                Text = (double.IsNaN(data) ? "-" : (double.IsPositiveInfinity(data) ? "" : data.ToString("0.###"))),
                IsReadOnly = true,
                FontSize = 14,
                Background = i == 0 || j == 0 ? Brushes.LightSteelBlue : Brushes.White
            };
            Grid.SetRow(tb, i);
            Grid.SetColumn(tb, j);
            return tb;
        }

        private TextBox CreateCell(string content, int i, int j)
        {
            TextBox tb = new TextBox()
            {
                Text = content,
                IsReadOnly = true,
                FontSize = 14,
                Background = i == 0 || j == 0 ? Brushes.LightSteelBlue : Brushes.White
            };
            Grid.SetRow(tb, i);
            Grid.SetColumn(tb, j);
            return tb;
        }

        public void AddColumn(string[] data, string colHeader)
        {
            int n = gTable.RowDefinitions.Count - 1;
            int m = gTable.ColumnDefinitions.Count - 1;
            gTable.ColumnDefinitions.Add(new ColumnDefinition());
            gTable.Children.Add(CreateCell(colHeader, 0, m + 1));
            cHeaders.Add(colHeader);
            for (int i = 0; i < n; i++)
            {
                cells[i].Add(CreateCell(data[i].ToString(), i + 1, m + 1));
                gTable.Children.Add(cells[i][m]);

            }
        }

        public void AddColumn(double[] data, string colHeader)
        {
            int n = gTable.RowDefinitions.Count - 1;
            int m = gTable.ColumnDefinitions.Count - 1;
            gTable.ColumnDefinitions.Add(new ColumnDefinition());
            gTable.Children.Add(CreateCell(colHeader, 0, m + 1));
            cHeaders.Add(colHeader);
            for (int i = 0; i < n; i++)
            {
                cells[i].Add(CreateCell(data[i].ToString("0.#####"), i + 1, m + 1));
                gTable.Children.Add(cells[i][m]);
                cells[i][m].Tag = data[i];
            }
        }

        public void Highlight(Predicate<double> pred, Brush colour, int column = -1)
        {
            foreach (var list in cells)
            {
                foreach (var tb in list)
                {
                    if (!(tb.Tag is string) && (column == -1 || list.IndexOf(tb) == column))
                    {
                        double d = (double)tb.Tag;
                        if (!double.IsNaN(d) && !double.IsInfinity(d) && pred(d))
                            tb.Background = colour;
                    }
                }
            }
        }

        public void Highlight(bool[] correct, Brush colour, int column)
        {
            foreach (var list in cells)
            {
                foreach (var tb in list)
                {
                    if (list.IndexOf(tb) == column && !correct[cells.IndexOf(list)])
                    {
                        tb.Background = colour;
                    }
                }
            }
        }
    }
}
