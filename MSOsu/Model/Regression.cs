using MSOsu.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Model
{
    class Regression
    {
        private double[][] matrix;
        private double[] regressionCoeffs;
        private double[] сalculatedY;
        private double[] absoluteErrorY;

        public Regression(double[][] matrix)
        {
            this.matrix = matrix;
        }

        /// <summary>
        /// Получить коэффициенты регрессии
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public double[] GetRegressionCoeffs()
        {
            double[] y = matrix[0];
            double[][] x = MatrixOperations.GetTransposeTable(matrix);
            for (int i = 0; i < x.Length; i++)
                x[i][0] = 1;

            double[][] xT = MatrixOperations.GetTransposeTable(x);
            double[][] xTx = MatrixOperations.MultMatrix(xT, x);
            double[][] xTx_ = MatrixOperations.GetInverseMatrixSLAU(xTx);
            double[][] xTx_xT = MatrixOperations.MultMatrix(xTx_, xT);
            double[] a = MatrixOperations.MultMatrixAndVector(xTx_xT, y);
            return regressionCoeffs = a;
        }

        /// <summary>
        /// Получить массив вычесленных значений выходного параметра
        /// </summary>
        /// <returns></returns>
        public double[] GetCalculatedY()
        {
            if (regressionCoeffs == null)
                GetRegressionCoeffs();

            double[] ys = new double[matrix[0].Length];
            for (int i = 0; i < ys.Length; i++)
            {
                ys[i] += regressionCoeffs[0];
                for (int j = 1; j < regressionCoeffs.Length; j++)
                    ys[i] += regressionCoeffs[j] * matrix[j][i];
            }

            return сalculatedY = ys;
        }

        /// <summary>
        /// Получить сумму квадратов отклонений
        /// </summary>
        /// <param name="b"></param>
        /// <param name="bs"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public double GetSLMError()
        {
            if (absoluteErrorY == null)
                GetAbsoluteError();
            return absoluteErrorY.Select(e => e * e).Sum();
        }

        /// <summary>
        /// Получить разность двух массивов
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static double[] GetDiff(double[] b1, double[] b2)
        {
            double[] result = new double[b1.Length];
            for (int i = 0; i < b1.Length; i++)
                result[i] = b1[i] - b2[i];
            return result;
        }

        /// <summary>
        /// Получить абсолютную ошибку вычисленного выходного параметра
        /// </summary>
        /// <returns></returns>
        public double[] GetAbsoluteError()
        {
            if (сalculatedY == null)
                GetCalculatedY();
            return absoluteErrorY = GetDiff(сalculatedY, matrix[0]);
        }
    }
}
