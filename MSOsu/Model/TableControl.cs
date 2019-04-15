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

        /// <summary>
        /// Получить таблицу в строковом виде (пока не реализовал и не буду, сделал пока просто вывод вниз)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string GetStringTable(ValuesColumn[] values)
        {
            string result = String.Empty;
            foreach (ValuesColumn value in values)
                result += value.ColumnName + "\r\n" + String.Join("\r\n", value.Values) + "\r\n\r\n";
            return result;
        }
    }
}
