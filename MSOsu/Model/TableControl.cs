using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using MSOsu.Common;

namespace MSOsu.Model
{
    static class TableControl
    {
        /// <summary>
        /// Получить таблицу из файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static (string[] headers, double[][] values) GetTable(string path)
        {
            using (TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding(1251)))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                string[] headers = parser.ReadFields();
                List<string[]> fields = new List<string[]>();
                while (!parser.EndOfData)
                    fields.Add(parser.ReadFields());
                var strValues = MatrixOperations.GetTransposeTable(fields.ToArray());
                double[][] values = strValues.Select(e => e.Select(u => double.Parse(u)).ToArray()).ToArray();

                return (headers, values);
            }
        }

        /// <summary>
        /// Сохранить таблицу в файл
        /// </summary>
        /// <param name="values"></param>
        /// <param name="path"></param>
        public static void SaveTable(string[] headers, double[][] values, string path)
        {
            StringBuilder result = new StringBuilder();
            int countOfValues = headers.Length;
            result.AppendLine(String.Join(";", headers));
            for (int i = 0; i < countOfValues; i++)
                result.AppendLine(String.Join(";", values[i]));

            File.WriteAllText(path, result.ToString(), Encoding.GetEncoding(1251));
        }
    }
}
