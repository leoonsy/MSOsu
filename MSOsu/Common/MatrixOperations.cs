﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Common
{
    class MatrixOperations
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

        /// <summary>
        /// Скалярное произведение векторов
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double ScalarMultiplication(double[] x, double[] y)
        {
            double r = 0;
            for (int i = 0; i < x.Length; i++)
                r += x[i] * y[i];
            return r;
        }

        /// <summary>
        /// Получить LU-разложение
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="l"></param>
        /// <param name="u"></param>
        public static (double[][] l, double[][] u) GetLUDecomp(double[][] matrix)
        {
            int n = matrix.Length;
            var l = new double[n][].Select(e => e = new double[n]).ToArray();
            var u = new double[n][].Select(e => e = new double[n]).ToArray();

            //Нахождение матриц L, U
            for (int i = 0; i < n; i++)
            {
                u[0][i] = matrix[0][i];
                l[i][0] = matrix[i][0] / u[0][0];
                for (int j = 0; j < n; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < i; k++)
                        sum += l[i][k] * u[k][j];
                    u[i][j] = matrix[i][j] - sum;
                    if (i > j)
                        l[j][i] = 0;
                    else
                    {
                        sum = 0;
                        for (int k = 0; k < i; k++)
                            sum += l[j][k] * u[k][i];
                        l[j][i] = (matrix[j][i] - sum) / u[i][i];
                    }
                }
            }

            return (l, u);
        }

        /// <summary>
        /// Получить определитель матрицы с помощью LU
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double GetDeterminantLU(double[][] matrix)
        {
            matrix = GetCopyMatrix(matrix);
            int count = matrix.Length;

            (double[][] l, double[][] u) = GetLUDecomp(matrix);

            double result = 1;
            for (int i = 0; i < count; i++)
                result *= u[i][i];

            return result;
        }

        /// <summary>
        /// Получить дополнительный минор
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="iDel"></param>
        /// <param name="jDel"></param>
        /// <returns></returns>
        public static double GetExtraMinor(double[][] matrix, int iDel, int jDel)
        {
            double[][] newMatrix = GetCopyMatrix(matrix, iDel, jDel);
            return GetDeterminantLU(newMatrix);
        }

        /// <summary>
        /// Получить копию матрицы без строки и столбца
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[][] GetCopyMatrix(double[][] matrix, int iDel, int jDel)
        {
            int m = matrix.Length;
            int n = matrix[0].Length;
            double[][] newMatrix = new double[m - 1][].Select(e => e = new double[n - 1]).ToArray();
            for (int i = 0; i < m; i++)
            {
                if (i == iDel) continue;
                for (int j = 0; j < n; j++)
                {
                    if (j == jDel) continue;
                    newMatrix[i > iDel ? i - 1 : i][j > jDel ? j - 1 : j] = matrix[i][j];
                }
            }
            return newMatrix;
        }

        /// <summary>
        /// Получить копию матрицы
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[][] GetCopyMatrix(double[][] matrix)
        {
            double[][] result = new double[matrix.Length][].Select(e => e = new double[matrix[0].Length]).ToArray();
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    result[i][j] = matrix[i][j];

            return result;
        }

        /// <summary>
        /// Получить копию матрицы
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double[] GetCopyMatrix(double[] array)
        {
            return (double[])array.Clone();
        }
    }
}