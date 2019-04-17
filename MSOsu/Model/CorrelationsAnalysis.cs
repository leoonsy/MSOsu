using MSOsu.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Model
{
    class CorrelationsAnalysis
    {
        public double[][] matrix;
        public CorrelationsAnalysis(double[][] matrix)
        {
            this.matrix = matrix;
        }

        /// <summary>
        /// Получить матрицу парных корреляций
        /// </summary>
        /// <returns></returns>
        public double[][] GetPairCorrelationsMatrix()
        {
            double[][] r = new double[matrix.Length][].Select(e => e = new double[matrix.Length]).ToArray();
            int n = matrix[0].Length;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    double temp = n * MatrixOperations.ScalarMultiplication(matrix[i], matrix[j]) - matrix[i].Sum() * matrix[j].Sum();
                    temp /= Math.Sqrt(Math.Abs(n * matrix[i].Sum(x => x * x) - matrix[i].Sum() * matrix[i].Sum()) *
                                         Math.Abs(n * matrix[j].Sum(y => y * y) - matrix[j].Sum() * matrix[j].Sum()));
                    r[i][j] = temp;
                }
            }

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = i; j < matrix.Length; j++)
                {
                    if (i == j)
                        r[i][j] = 1;
                    else
                        r[i][j] = r[j][i];
                }
            }
            return r;
        }
    }
}
