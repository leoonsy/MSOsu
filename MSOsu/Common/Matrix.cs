using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Common
{
    class Matrix
    {
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
    }
}
