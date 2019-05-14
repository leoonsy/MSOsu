using MSOsu.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Model
{
    static class PearsonTest
    {
        /// <summary>
        /// Вычислить значение хи-квадрат
        /// </summary>
        /// <param name="values"></param>
        /// <param name="intervalNumber"></param>
        /// <returns></returns>
        public static double GetChiSquared(double[] values, int intervalNumber)
        {
            DescriptiveStatistic stat = new DescriptiveStatistic(values);
            int[] m = new int[intervalNumber]; //эмпирические частоты
            double[] mt = new double[intervalNumber]; //теоретические частоты
            const int round = 13; //насколько округлять расчеты, делается, чтобы все значения попали в интервал
            //формирование массива m
            double step = (stat.Interval) / intervalNumber;
            foreach (double val in values)
            {
                for (int i = 0; i < intervalNumber; i++)
                    if (Math.Round(val, round) <= Math.Round(stat.Min + step * (i + 1), round))
                    {
                        ++m[i];
                        break;
                    }
            }

            if (m.Sum() != values.Length)
                throw new Exception();

            //формирование массива mt
            for (int i = 0; i < mt.Length; i++)
            {
                double x = stat.Min + step * i + step / 2;
                double u = (x - stat.Average) / stat.StandardDeviation;
                double f = Math.Pow(Math.E, -u * u / 2) / Math.Sqrt(2 * Math.PI);
                double p = (step / stat.StandardDeviation) * f;
                mt[i] = stat.Count * p;
            }

            Console.WriteLine(string.Join(", ", m) + "\n" + string.Join(", ", mt));

            //вычисление хи-квадрат
            double chiSquare = 0;
            for (int i = 0; i < intervalNumber; i++)
                chiSquare += Math.Pow(m[i] - mt[i], 2) / mt[i];

            return chiSquare;
        }

        /// <summary>
        /// Проверить на нормальность распределения
        /// </summary>
        /// <param name="values"></param>
        /// <param name="chiSquare"></param>
        /// <returns></returns>
        public static (bool, double chiSquare) CheckNormalDistribution(double[] values)
        {
            const int intervalNumber = 5; //делим на 5 интервалов
            double chiSquare = GetChiSquared(values, intervalNumber);
            double krit = GetChiSquareKrit(); 
            return chiSquare < krit ? (true, chiSquare) : (false, chiSquare);
        }

        /// <summary>
        /// Получить значение хи-квадрат
        /// </summary>
        /// <returns></returns>
        public static double GetChiSquareKrit() => 5.99146; //критическое значение для intervalNumber = 5, при другом k = intervalNumber - r - 1 (r = 2 для нормального распределения) стоит вычислить krit снова

        /// <summary>
        /// Получить таблицу нормального распределения
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string[][] GetNormalDistributionMatrix(double[][] values)
        {
            string[][] result = new string[2][].Select(e => e = new string[values.Length]).ToArray();
            for (int i = 0; i < values.Length; i++)
            {
                (bool isNormal, double chiSquare) = CheckNormalDistribution(values[i]);
                result[0][i] = Math.Round(chiSquare, 6).ToString();
                result[1][i] = isNormal ? "+" : "-";
            }
            return result;
        }
    }
}
