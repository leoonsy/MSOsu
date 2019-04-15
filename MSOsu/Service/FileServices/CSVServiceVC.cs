using MSOsu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Service.FileServices
{
    class CSVServiceVC : IFileService<ValuesColumn[]>
    {
        public string GetOpenFilter()
        {
            return "Файл CSV|*.csv";
        }

        public string GetSaveFilter()
        {
            return "Файл CSV|*.csv";
        }

        public ValuesColumn[] Read(string path)
        {
            return TableControl.GetTable(path);
        }

        public void Save(string path, ValuesColumn[] table)
        {
            TableControl.SaveTable(table, path);
        }
    }
}
