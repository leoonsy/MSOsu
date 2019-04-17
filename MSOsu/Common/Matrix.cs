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
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static T[][] GetTransposeTable<T>(T[][] matrix)
        {
            int m = matrix.Length;
            int n = matrix[0].Length;
            T[][] transTable = new T[n][].Select(e => e = new T[m]).ToArray();
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    transTable[j][i] = matrix[i][j];
            return transTable;
        }

        /// <summary>
        /// Округлить матрицу
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static double[][] RoundMatrix(double[][] matrix, int round)
        {
            return matrix.Select(e => e.Select(u => Math.Round(u, round)).ToArray()).ToArray();
        }
    }
}
