using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace MSOsu.Model
{
    static class TableControl
    {
        /// <summary>
        /// Получить таблицу из файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ValuesColumn[] GetTable(string path)
        {
            using (TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                string[] headlines = parser.ReadFields();
                var fields = new List<string[]>();
                while (!parser.EndOfData)
                    fields.Add(parser.ReadFields());
                var result = Enumerable.Range(0, headlines.Length).Select(idx => new ValuesColumn(headlines[idx], null)).ToArray();
                for (int i = 0; i < result.Length; i++)
                    result[i].Values = fields.Select(e => double.Parse(e[i])).ToArray();

                return result;
            }
        }

        /// <summary>
        /// Получить массив значений
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static double[][] GetValues(ValuesColumn[] table)
        {
            int rowLength = table[0].Values.Length;
            int colLength = table.Length;
            double[][] matrix = new double[rowLength][].Select(val => val = new double[colLength]).ToArray();
            for (int i = 0; i < rowLength; i++)
                for (int j = 0; j < colLength; j++)
                    matrix[i][j] = table[j].Values[i];
            return matrix;
        }

        /// <summary>
        /// Получить массив нормированных значений
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static double[][] GetNormalizedValues(ValuesColumn[] table)
        {
            ValuesColumn[] vc = table.Select(e => new ValuesColumn(e.ColumnName, new DescriptiveStatistic(e.Values).GetNormalizedSelection().
                                Select(u => Math.Round(u, 5)).ToArray())).ToArray();
            return GetValues(vc);
        }

        /// <summary>
        /// Получить массив заголовков
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string[] GetHeaders(ValuesColumn[] table)
        {
            return table.Select(e => e.ColumnName).ToArray();
        }

        /// <summary>
        /// Транспонировать матрицу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static T[][] GetTransposeTable<T>(T[][] table)
        {
            int m = table.Length;
            int n = table[0].Length;
            T[][] transTable = new T[n][].Select(e => e = new T[m]).ToArray();
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    transTable[j][i] = table[i][j];
            return transTable;
        }

        /// <summary>
        /// Сохранить таблицу в файл
        /// </summary>
        /// <param name="values"></param>
        /// <param name="path"></param>
        public static void SaveTable(ValuesColumn[] values, string path)
        {
            StringBuilder result = new StringBuilder();
            int countOfValues = values[0].Values.Length;
            result.AppendLine( String.Join(";",values.Select(e => e.ColumnName)) );
            for (int i = 0; i < countOfValues; i++)
                result.AppendLine(String.Join(";", values.Select(e => e.Values[i])));

            File.WriteAllText(path, result.ToString(), Encoding.GetEncoding(1251));
        }
    }
}
