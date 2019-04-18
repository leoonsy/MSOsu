﻿using MSOsu.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Model
{
    class CorrelationsAnalysis
    {
        public double[][] matrix; //исходная матрица
        private double[][] pairMatrix; //матрица парных корреляций
        private double[][] particalMatrix; //матрица частных корреляций
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
            return pairMatrix = r;
        }


        /// <summary>
        /// Получить матрицу значимости коэффициентов
        /// </summary>
        /// <param name="R"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private double[][] GetSignificanceCorrelation(double[][] r, int n)
        {
            int count = r.Length;
            double[][] s = new double[count][].Select(e => e = new double[count]).ToArray();
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < i; j++)
                    s[i][j] = Math.Abs(r[i][j]) * Math.Sqrt((n - 2) / (1 - r[i][j] * r[i][j]));
            }
            for (int i = 0; i < count; i++)
            {
                s[i][i] = double.NaN;
                for (int j = i; j < count; j++)
                    s[i][j] = s[j][i];
            }
            return s;
        }

        /// <summary>
        /// Получить матрицу значимости коэффициентов парной корреляции
        /// </summary>
        /// <returns></returns>
        public double[][] GetPairSignificanceCorrelation()
        {
            if (pairMatrix == null)
                pairMatrix = GetPairCorrelationsMatrix();
            return GetSignificanceCorrelation(pairMatrix, matrix[0].Length);
        }

        /// <summary>
        /// Получить матрицу значимости коэффициентов частной корреляции
        /// </summary>
        /// <returns></returns>
        public double[][] GetParticalSignificanceCorrelation()
        {
            if (particalMatrix == null)
                particalMatrix = GetPairCorrelationsMatrix();
            return GetSignificanceCorrelation(particalMatrix, matrix[0].Length);
        }

        /// <summary>
        /// Получить матрицу частных корреляций
        /// </summary>
        /// <returns></returns>
        public double[][] GetParticalCorrelationsMatrix()
        {
            if (pairMatrix == null)
                pairMatrix = GetPairCorrelationsMatrix();
            double[][] r = new double[pairMatrix.Length][].Select(e => e = new double[pairMatrix.Length]).ToArray();
            for (int i = 0; i < pairMatrix.Length; i++)
            {
                for (int j = 0; j < pairMatrix.Length; j++)
                {
                    if (i == j)
                        r[i][j] = 1;
                    else
                        r[i][j] = -Math.Pow(-1, i + j) * MatrixOperations.GetExtraMinor(pairMatrix, i, j) / 
                            Math.Sqrt(MatrixOperations.GetExtraMinor(pairMatrix, i, i) * MatrixOperations.GetExtraMinor(pairMatrix, j, j));
                }
            }
            return particalMatrix = r;
        }
    }
}
