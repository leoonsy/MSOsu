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
        /// <summary>
        /// Исходная матрица
        /// </summary>
        private double[][] matrix;
        /// <summary>
        /// Матрица X с первым столбцом-единицей
        /// </summary>
        private double[][] xMatrix;
        /// <summary>
        /// Вычисленные коеффициенты регрессии
        /// </summary>
        private double[] regressionCoeffs;
        /// <summary>
        /// Вычисленные значения Ỹ по данному уравнению регрессии
        /// </summary>
        private double[] calculatedY;
        /// <summary>
        /// Сумма квадратов отклонений
        /// </summary>
        private double qOst = double.NaN;

        private double[] standartErrorOfRegressionCoeff;

        public Regression(double[][] matrix)
        {
            this.matrix = matrix;
            xMatrix = MatrixOperations.Transpose(matrix);
            for (int i = 0; i < xMatrix.Length; i++)
                xMatrix[i][0] = 1;
        }

        /// <summary>
        /// Получить коэффициенты регрессии
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public double[] GetRegressionCoeffs()
        {
            double[] y = matrix[0];
            double[][] xT = MatrixOperations.Transpose(xMatrix);
            double[][] xTx = MatrixOperations.Mult(xT, xMatrix);
            double[][] xTx_ = MatrixOperations.InverseSLAU(xTx);
            double[][] xTx_xT = MatrixOperations.Mult(xTx_, xT);
            return regressionCoeffs = MatrixOperations.Mult(xTx_xT, y);
        }

        /// <summary>
        /// Получить массив вычисленных значений выходного параметра Ỹ (Xb)
        /// </summary>
        /// <returns></returns>
        public double[] GetCalculatedY()
        {
            if (regressionCoeffs == null)
                GetRegressionCoeffs();

            double[] ys = new double[xMatrix.Length];

            return calculatedY = MatrixOperations.Mult(xMatrix, regressionCoeffs);
        }

        /// <summary>
        /// Получить абсолютную ошибку вычисленного выходного параметра
        /// </summary>
        /// <returns></returns>
        public double[] GetAbsoluteError()
        {
            if (calculatedY == null)
                GetCalculatedY();
            return MatrixOperations.Subtraction(matrix[0], calculatedY);
        }

        /// <summary>
        /// Получить сумму квадратов отклонений
        /// </summary>
        /// <param name="b"></param>
        /// <param name="bs"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public double GetQost()
        {
            if (calculatedY == null)
                GetCalculatedY();
            double[] y_Xb = MatrixOperations.Subtraction(matrix[0], calculatedY);
            return qOst = MatrixOperations.Mult(MatrixOperations.Transpose(y_Xb), y_Xb)[0];
        }

        /// <summary>
        /// Получить значимость уравнения регрессии
        /// </summary>
        /// <returns></returns>
        public double GetSignificanceEquation()
        {
            if (double.IsNaN(qOst))
                GetQost();

            double qR = MatrixOperations.Mult(MatrixOperations.Transpose(calculatedY), calculatedY)[0];
            int k = matrix.Length - 1;
            int n = matrix[0].Length;
            double result = qR / qOst * (n - k - 1) / (k + 1);
            return result;
        }

        /// <summary>
        /// Получить стандартную ошибку для коэффициентов (sbj)
        /// </summary>
        /// <returns></returns>
        private double[] GetStandartErrorOfRegressionCoeff()
        {
            int k = matrix.Length - 1;
            int n = matrix[0].Length;
            double s2 = qOst / (n - k - 1);
            double[] result = new double[k + 1];
            double[][] xT = MatrixOperations.Transpose(xMatrix);
            double[][] xTx = MatrixOperations.Mult(xT, xMatrix);
            double[][] xTx_ = MatrixOperations.InverseSLAU(xTx);
            for (int i = 0; i < k + 1; i++)
                result[i] = Math.Sqrt(s2 * xTx_[i][i]);
            return standartErrorOfRegressionCoeff = result;
        }

        /// <summary>
        /// Получить значимость коэффициентов уравнения
        /// </summary>
        /// <returns></returns>
        public double[] GetSignificanceEquationCoeffs()
        {
            if (standartErrorOfRegressionCoeff == null)
                GetStandartErrorOfRegressionCoeff();

            if (regressionCoeffs == null)
                GetRegressionCoeffs();

            double[] result = new double[matrix.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = Math.Abs(regressionCoeffs[i] / standartErrorOfRegressionCoeff[i]);

            return result;
        }

        /// <summary>
        /// Получить t-критическое
        /// </summary>
        /// <returns></returns>
        public double GetTKritEquationCoeffs()
        {
            int k = matrix.Length - 1;
            int n = matrix[0].Length;
            int v = n - k - 1; //число степеней свободы
            return DataBase.GetTCrit(v);
        }

        /// <summary>
        /// Получить критическое значение F-крит для определения значимости уравнения регрессии
        /// </summary>
        /// <returns></returns>
        public double GetFKritEquation()
        {
            int k = matrix.Length - 1;
            int n = matrix[0].Length;
            return DataBase.GetFCrit(k + 1, n - k - 1);
        }

        /// <summary>
        /// Получить ошибку аппроксимации
        /// </summary>
        /// <returns></returns>
        public double GetApproximationError()
        {
            if (calculatedY == null)
                GetCalculatedY();

            return Enumerable.Range(0, calculatedY.Length).Select(idx => Math.Abs((matrix[0][idx] - calculatedY[idx]) / matrix[0][idx])).Sum() / matrix[0].Length;
        }
    }
}
