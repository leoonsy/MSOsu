using System;
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
        public static T[][] Transpose<T>(T[][] matrix)
        {
            int m = matrix.Length;
            int n = matrix[0].Length;
            T[][] transMatrix = new T[n][].Select(e => e = new T[m]).ToArray();
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    transMatrix[j][i] = matrix[i][j];
            return transMatrix;
        }

        /// <summary>
        /// Транспонировать вектор
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static T[][] Transpose<T>(T[] vector)
        {
            T[][] result = new T[1][];
            result[0] = new T[vector.Length];
            result[0] = (T[])vector.Clone();
            return result;
        }

        /// <summary>
        /// Округлить матрицу
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static double[][] Round(double[][] matrix, int round)
        {
            return matrix.Select(e => e.Select(u => Math.Round(u, round)).ToArray()).ToArray();
        }

        /// <summary>
        /// Округлить матрицу
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static string[][] Round(string[][] matrix, int round)
        {
            matrix = Copy(matrix);
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    double val;
                    if (double.TryParse(matrix[i][j], out val))
                    {
                        matrix[i][j] = Math.Round(val, round).ToString();
                    }
                }
            return matrix;
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
        public static (double[][] l, double[][] u) LUDecomp(double[][] matrix)
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
        public static double DeterminantLU(double[][] matrix)
        {
            matrix = Copy(matrix);
            int count = matrix.Length;

            (double[][] l, double[][] u) = LUDecomp(matrix);

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
        public static double ExtraMinor(double[][] matrix, int iDel, int jDel)
        {
            double[][] newMatrix = Copy(matrix, iDel, jDel);
            return DeterminantLU(newMatrix);
        }

        /// <summary>
        /// Получить копию матрицы без строки и столбца
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static double[][] Copy(double[][] matrix, int iDel, int jDel)
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
        public static T[][] Copy<T>(T[][] matrix)
        {
            T[][] result = new T[matrix.Length][].Select(e => e = new T[matrix[0].Length]).ToArray();
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
        public static T[] Copy<T>(T[] array)
        {
            return (T[])array.Clone();
        }

        /// <summary>
        /// Найти прозведение матрицы и вектора
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static double[] Mult(double[][] matrix, double[] vector)
        {
            double[] result = new double[matrix.Length];

            for (int i = 0; i < matrix.Length; i++)
            {
                result[i] = 0;
                for (int j = 0; j < matrix[i].Length; j++)
                    result[i] += matrix[i][j] * vector[j];
            }
            return result;
        }

        /// <summary>
        /// Найти прозведение двух матриц
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static double[][] Mult(double[][] m1, double[][] m2)
        {
            var result = new double[m1.Length][].Select(e => e = new double[m2[0].Length]).ToArray();

            for (var i = 0; i < m1.Length; ++i)
                for (var j = 0; j < m2[0].Length; ++j)
                    for (var k = 0; k < m2.Length; ++k)
                        result[i][j] += m1[i][k] * m2[k][j];
            return result;
        }

        /// <summary>
        /// Получить обратную матрицу через решения СЛАУ
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static double[][] InverseSLAU(double[][] matrix)
        {
            int count = matrix.Length;

            double[][] result = new double[count][].Select(e => e = new double[count]).ToArray();

            for (int i = 0; i < count; i++)
            {
                double[] b = new double[count];
                b[i] = 1;
                result[i] = SLAU.GetSLAUResolve(matrix, b, SLAU.GaussMethod.All);
            }

            return Transpose(result);
        }

        /// <summary>
        /// Получить разность двух векторов
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static double[] Subtraction(double[] b1, double[] b2)
        {
            double[] result = new double[b1.Length];
            for (int i = 0; i < b1.Length; i++)
                result[i] = b1[i] - b2[i];
            return result;
        }

        /// <summary>
        /// Получить разность двух массивов
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static double[][] Subtraction(double[][] b1, double[][] b2)
        {
            double[][] result = new double[b1.Length][].Select(e => e = new double[b1[0].Length]).ToArray();
            for (int i = 0; i < b1.Length; i++)
                for (int j = 0; j < b1[0].Length; j++)
                    result[i][j] = b1[i][j] - b2[i][j];
            return result;
        }

        /// <summary>
        /// Получить сумму двух векторов
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static double[] Sum(double[] b1, double[] b2)
        {
            double[] result = new double[b1.Length];
            for (int i = 0; i < b1.Length; i++)
                result[i] = b1[i] + b2[i];
            return result;
        }

        /// <summary>
        /// Получить сумму двух массивов
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static double[][] Sum(double[][] b1, double[][] b2)
        {
            double[][] result = new double[b1.Length][].Select(e => e = new double[b1[0].Length]).ToArray();
            for (int i = 0; i < b1.Length; i++)
                for (int j = 0; j < b1[0].Length; j++)
                    result[i][j] = b1[i][j] + b2[i][j];
            return result;
        }
    }
}
