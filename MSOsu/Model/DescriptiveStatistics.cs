﻿using MSOsu.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MSOsu.Model
{
    class DescriptiveStatistic
    {
        private double average = double.NaN;
        private double dispersion = double.NaN;
        private double standardDeviation = double.NaN;
        private double standartError = double.NaN;
        private double max = double.NaN;
        private double min = double.NaN;
        private double median = double.NaN;
        private double mode = double.NaN;
        private double interval = double.NaN;
        private double sum = double.NaN;
        private double excess = double.NaN;
        private double asymmetry = double.NaN;
        private double count = double.NaN;
        private double limitError = double.NaN;
        private double requiredValuesCount = double.NaN;
        private double recalculatedLimitError = double.NaN;

        /// <summary>
        /// Массив значений
        /// </summary>
        public double[] Values { get; set; }
        public DescriptiveStatistic(double[] values)
        {
            Values = values;
        }

        /// <summary>
        /// Среднее арифметическое
        /// </summary>
        public double Average => !double.IsNaN(average) ? average : average = Values.Sum() / Count;

        /// <summary>
        /// Дисперсия
        /// </summary>
        public double Dispersion => !double.IsNaN(dispersion) ? dispersion : dispersion = Values.Sum(e => Math.Pow(e - Average, 2)) / (Count - 1);


        /// <summary>
        /// Стандартная ошибка
        /// </summary>
        public double StandardDeviation => !double.IsNaN(standardDeviation) ? standardDeviation : standardDeviation = Math.Sqrt(Dispersion);

        /// <summary>
        /// Стандартная ошибка
        /// </summary>
        public double StandardError => !double.IsNaN(standartError) ? standartError : standartError = StandardDeviation / Math.Sqrt(Count);

        /// <summary>
        /// Максимальное значение
        /// </summary>
        public double Max => !double.IsNaN(max) ? max : max = Values.Max();

        /// <summary>
        /// Минимальное значение
        /// </summary>
        public double Min => !double.IsNaN(min) ? min : min = Values.Min();

        /// <summary>
        /// Медиана
        /// </summary>
        public double Median
        {
            get
            {
                if (!double.IsNaN(median))
                    return median;
                double[] selection = (double[])Values.Clone();
                Array.Sort(selection);
                if (selection.Length % 2 == 1)
                    return median = selection[(selection.Length + 1) / 2 - 1];
                else
                {
                    double m1 = selection[selection.Length / 2];
                    double m2 = selection[selection.Length / 2 - 1];
                    return median = (m1 + m2) / 2;
                }
            }
        }

        /// <summary>
        /// Мода
        /// </summary>
        public double Mode
        {
            get
            {
                if (!double.IsNaN(mode))
                    return median;
                Dictionary<double, int> dictionary = new Dictionary<double, int>();
                foreach (double value in Values)
                {
                    if (dictionary.ContainsKey(value))
                        ++dictionary[value];
                    else
                        dictionary[value] = 1;
                }

                int maxCount = dictionary.Max(e => e.Value);
                if (maxCount == 1)
                    return mode = double.NaN;
                else
                    return mode = dictionary.First(e => e.Value == maxCount).Key;
            }
        }

        public double Interval => !double.IsNaN(interval) ? interval : interval = (Values.Max() - Values.Min());

        /// <summary>
        /// Сумма
        /// </summary>
        public double Sum => !double.IsNaN(sum) ? sum : sum = Values.Sum();

        /// <summary>
        /// Эксцесс
        /// </summary>
        public double Excess
        {
            get
            {
                if (!double.IsNaN(excess))
                    return excess;
                double n = Count;
                double l = (n * (n + 1)) / ((n - 1) * (n - 2) * (n - 3));
                double sum = Values.Select(val => Math.Pow((val - Average) / StandardDeviation, 4)).Sum();
                double r = (3 * Math.Pow(n - 1, 2)) / ((n - 2) * (n - 3));
                return excess = l * sum - r;
            }
        }

        /// <summary>
        /// Ассиметрия
        /// </summary>
        public double Asymmetry
        {
            get
            {
                if (!double.IsNaN(asymmetry))
                    return asymmetry;
                double n = Count;
                double l = n / ((n - 1) * (n - 2));
                double sum = Values.Select(val => Math.Pow((val - Average) / StandardDeviation, 3)).Sum();
                return asymmetry = l * sum;
            }
        }

        /// <summary>
        /// Счёт
        /// </summary>
        public double Count => !double.IsNaN(count) ? count : count = Values.Length;

        /// <summary>
        /// Предельная ошибка
        /// </summary>
        public double LimitError => !double.IsNaN(limitError) ? limitError : limitError = DataBase.GetTCrit((int)Count - 1) * Math.Sqrt(Dispersion / Count);

        /// <summary>
        /// Необходимый объем выборки для предельной ошибки = 0.01
        /// </summary>
        public double RequiredValuesCount => !double.IsNaN(requiredValuesCount) ? requiredValuesCount : requiredValuesCount = (int)(Math.Pow(DataBase.GetTCrit((int)Count - 1) / 0.01, 2) * Dispersion); 

        /// <summary>
        /// Пересчитанная предельная ошибка
        /// </summary>
        public double RecalculatedLimitError => !double.IsNaN(recalculatedLimitError) ? recalculatedLimitError : recalculatedLimitError = DataBase.GetTCrit((int)Count - 1) * Math.Sqrt(Dispersion / RequiredValuesCount);
        
        /// <summary>
        /// Получить нормированные значения
        /// </summary>
        public double[] GetNormalizedValues() => Values.Select(e => e / Interval).ToArray();

        /// <summary>
        /// Получить массив нормированных значений
        /// </summary>
        /// <returns></returns>
        public static double[][] GetNormalizedValues(double[][] values)
        {
            return values.Select(e => e = new DescriptiveStatistic(e).GetNormalizedValues()).ToArray();
        }

        /// <summary>
        /// Заголовки для статистик
        /// </summary>
        public static string[] Headers = new string[] { "Среднее", "Дисперсия", "Стандартное отклонение", "Мода", "Медиана", "Минимум", "Максимум",
            "Интервал", "Ассиметричность", "Эксцесс", "Сумма", "Счет", "Стандартная ошибка", "Предельная ошибка", "Необходимый объем выборки", "Предельная ошибка при необходимом объеме выборки" };

        /// <summary>
        /// Получить таблицу со статистиками
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static double[][] GetTotalStatistic(double[][] matrix)
        {
            double[][] result = new double[Headers.Length][].Select(e => e = new double[matrix.Length]).ToArray();
            for (int i = 0; i < matrix.Length; i++)
            {
                DescriptiveStatistic stat = new DescriptiveStatistic(matrix[i]);
                int j = 0;
                foreach (double value in stat.GetNextStatistic())
                {
                    result[j][i] = value;
                    j++;
                }
            }

            return MatrixOperations.Transpose(result);
        }

        /// <summary>
        /// Получить следующую статистику
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> GetNextStatistic()
        {
            yield return Average;
            yield return Dispersion;
            yield return StandardDeviation;
            yield return Mode;
            yield return Median;
            yield return Min;
            yield return Max;
            yield return Interval;
            yield return Asymmetry;
            yield return Excess;
            yield return Sum;
            yield return Count;
            yield return StandardError;
            yield return LimitError;
            yield return RequiredValuesCount;
            yield return RecalculatedLimitError;
        }

        public static double[] GetNormallizedCoeffs(double[][] matrix)
        {
            return matrix.Select(e => new DescriptiveStatistic(e).Interval).ToArray();
        }

        /// <summary>
        /// Переопределение индексатора
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index]
        {
            get
            {
                return Values[index];
            }

            set
            {
                Values[index] = value;
            }
        }

        ///// <summary>
        ///// Получить итоговую статистику
        ///// </summary>
        ///// <returns></returns>
        //public string GetTotalStringStatistic(int? round = null)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.AppendLine($"Среднее {(round == null ? Average : Math.Round(Average, (int)round))}");
        //    sb.AppendLine($"Стандартная ошибка {(round == null ? StandardError : Math.Round(StandardError, (int)round))}");
        //    sb.AppendLine($"Медиана {(round == null ? Median : Math.Round(Median, (int)round))}");
        //    sb.AppendLine($"Мода {(round == null ? Mode : Math.Round(Mode, (int)round))}");
        //    sb.AppendLine($"Стандартное отклонение {(round == null ? StandardDeviation : Math.Round(StandardDeviation, (int)round))}");
        //    sb.AppendLine($"Дисперсия выборки {(round == null ? Dispersion : Math.Round(Dispersion, (int)round))}");
        //    sb.AppendLine($"Эксцесс {(round == null ? Excess : Math.Round(Excess, (int)round))}");
        //    sb.AppendLine($"Ассиметричность {(round == null ? Asymmetry : Math.Round(Asymmetry, (int)round))}");
        //    sb.AppendLine($"Интервал {(round == null ? Interval : Math.Round(Interval, (int)round))}");
        //    sb.AppendLine($"Минимум {(round == null ? Min : Math.Round(Min, (int)round))}");
        //    sb.AppendLine($"Максимум {(round == null ? Max : Math.Round(Max, (int)round))}");
        //    sb.AppendLine($"Сумма {(round == null ? Sum : Math.Round(Sum, (int)round))}");
        //    sb.AppendLine($"Счет {(round == null ? Count : Math.Round(Count, (int)round))}");

        //    return sb.ToString();
        //}
    }
}
