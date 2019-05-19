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
        /// <summary>
        /// Интервальная оценка коэффициентов
        /// </summary>
        private double[] intervalEstimateCoeffs;
        /// <summary>
        /// Cтандартная ошибка для коэффициентов (sbj)
        /// </summary>
        private double[] standartErrorOfRegressionCoeff;
        /// <summary>
        /// t-критическое
        /// </summary>
        private double tKrit = double.NaN;
        /// <summary>
        /// F-критическое
        /// </summary>
        private double fKrit = double.NaN;

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
            if (regressionCoeffs != null)
                return regressionCoeffs;

            double[] y = matrix[0];
            double[][] xT = MatrixOperations.Transpose(xMatrix);
            double[][] xTx = MatrixOperations.Mult(xT, xMatrix);
            double[][] xTx_ = MatrixOperations.InverseSLAU(xTx);
            double[][] xTx_xT = MatrixOperations.Mult(xTx_, xT);
            return regressionCoeffs = MatrixOperations.Mult(xTx_xT, y);
        }

        /// <summary>
        /// Получить массив вычисленных значений выходного параметра (Xb)
        /// </summary>
        /// <returns></returns>
        public double[] GetCalculatedY()
        {
            if (calculatedY != null)
                return calculatedY;

            double[] regressionCoeffs = GetRegressionCoeffs();
            double[] ys = new double[xMatrix.Length];

            return calculatedY = MatrixOperations.Mult(xMatrix, regressionCoeffs);
        }

        /// <summary>
        /// Получить абсолютную ошибку вычисленного выходного параметра
        /// </summary>
        /// <returns></returns>
        public double[] GetAbsoluteError()
        {
            double[] calculatedY = GetCalculatedY();
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
            if (!double.IsNaN(qOst))
                return qOst;

            double[] calculatedY = GetCalculatedY();
            double[] y_Xb = MatrixOperations.Subtraction(matrix[0], calculatedY);
            return qOst = MatrixOperations.Mult(MatrixOperations.Transpose(y_Xb), y_Xb)[0];
        }

        /// <summary>
        /// Получить значимость уравнения регрессии
        /// </summary>
        /// <returns></returns>
        public double GetSignificanceEquation()
        {
            double[] calculatedY = GetCalculatedY();
            double qOst = GetQost();
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
        public double[] GetStandartErrorOfRegressionCoeffs()
        {
            if (standartErrorOfRegressionCoeff != null)
                return standartErrorOfRegressionCoeff;

            double qOst = GetQost();
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
        /// Получить интервальную оценку коэффициентов
        /// </summary>
        /// <returns></returns>
        public double[] GetIntervalEstimateCoeffs()
        {
            if (intervalEstimateCoeffs != null)
                return intervalEstimateCoeffs;

            double[] standartErrorOfRegressionCoeff = GetStandartErrorOfRegressionCoeffs();
            double tKrit = GetTKritEquationCoeffs();
            double[] result = new double[standartErrorOfRegressionCoeff.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = Math.Abs(standartErrorOfRegressionCoeff[i] * tKrit);
            return intervalEstimateCoeffs = result;
        }

        /// <summary>
        /// Получить значимость коэффициентов уравнения
        /// </summary>
        /// <returns></returns>
        public double[] GetSignificanceEquationCoeffs()
        {
            double[] standartErrorOfRegressionCoeff = GetStandartErrorOfRegressionCoeffs();
            double[] regressionCoeffs = GetRegressionCoeffs();
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
            if (!double.IsNaN(tKrit))
                return tKrit;

            int k = matrix.Length - 1;
            int n = matrix[0].Length;
            int v = n - k - 1; //число степеней свободы
            return tKrit = DataBase.GetTCrit(v);
        }

        /// <summary>
        /// Получить критическое значение F-крит для определения значимости уравнения регрессии
        /// </summary>
        /// <returns></returns>
        public double GetFСritEquation()
        {
            if (!double.IsNaN(fKrit))
                return fKrit;

            int k = matrix.Length - 1;
            int n = matrix[0].Length;
            return fKrit = DataBase.GetFCrit(k + 1, n - k - 1);
        }

        /// <summary>
        /// Получить интервальную оценку уравнения y~
        /// </summary>
        /// <returns></returns>
        public double[] GetIntervalEstimateEquation()
        {
            double tKrit = GetTKritEquationCoeffs();
            double[] result = new double[matrix[0].Length];
            double qOst = GetQost();
            int k = matrix.Length - 1;
            int n = matrix[0].Length;
            double s = Math.Sqrt(qOst / (n - k - 1));
            for (int i = 0; i < result.Length; i++)
            {
                double[][] x0T = MatrixOperations.Transpose(xMatrix[i]);
                double[][] xTx = MatrixOperations.Mult(MatrixOperations.Transpose(xMatrix), xMatrix);
                double[][] xTx_ = MatrixOperations.InverseSLAU(xTx);
                double[][] x0TxTx_ = MatrixOperations.Mult(x0T, xTx_);
                double fin = MatrixOperations.Mult(x0TxTx_, xMatrix[i])[0];
                result[i] = tKrit * s * Math.Sqrt(fin);
            }
            return result;
        }

        /// <summary>
        /// Получить интервал предсказания
        /// </summary>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        public double GetIntervalPredication(double[] paramValues)
        {
            double tKrit = GetTKritEquationCoeffs();
            double qOst = GetQost();
            int k = matrix.Length - 1;
            int n = matrix[0].Length;
            double s = Math.Sqrt(qOst / (n - k - 1));

            double[][] x0T = MatrixOperations.Transpose(paramValues);
            double[][] xTx = MatrixOperations.Mult(MatrixOperations.Transpose(xMatrix), xMatrix);
            double[][] xTx_ = MatrixOperations.InverseSLAU(xTx);
            double[][] x0TxTx_ = MatrixOperations.Mult(x0T, xTx_);
            double fin = MatrixOperations.Mult(x0TxTx_, paramValues)[0] + 1;
            return tKrit * s * Math.Sqrt(fin);
        }

        /// <summary>
        /// Получить интервал предсказания для всех значений y
        /// </summary>
        /// <returns></returns>
        public double[] GetIntervalPredicationAll()
        {
            double[] result = new double[xMatrix.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = GetIntervalPredication(xMatrix[i]);
            return result;
        }

        /// <summary>
        /// Получить ошибку аппроксимации
        /// </summary>
        /// <returns></returns>
        public double GetApproximationError()
        {
            double[] calculatedY = GetCalculatedY();
            return Enumerable.Range(0, calculatedY.Length).Select(idx => Math.Abs((matrix[0][idx] - calculatedY[idx]) / matrix[0][idx])).Sum() / matrix[0].Length;
        }
    }
}
