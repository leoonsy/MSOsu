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
            return a;
        }

        public Regression(double[][] matrix)
        {
            this.matrix = matrix;
        }
    }
}
