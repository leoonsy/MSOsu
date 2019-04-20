using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Common
{
    class DataBase
    {
        private static double[] tStudentKrit = new double[] { 12.706,  4.303,  3.182,  2.776,  2.571,  2.447,  2.365,  2.306,  2.262,  2.228,  2.201,  2.179,  2.16,  2.145,  2.131,  2.12,  2.11,  2.101,  2.093,  2.086,  2.08,  2.074,  2.069,  2.064,  2.06,  2.056,  2.052,  2.048,  2.045,  2.042,  2.021,  2.009,  2,  1.99,  1.984,  1.98,  1.972 };
        private static double[] kStudent = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 40, 50, 60, 80, 100, 120, 200, };

        public static double GetTCrit(int k) //альфа = 0.05
        {
            if (k > kStudent.Last())
                throw new Exception("Ошибка при поиске критического значения по таблице t-критерия Стьюдента");
            int tIndex = kStudent.ToList().IndexOf(k);
            if (tIndex != -1)
                return tStudentKrit[tIndex];
            int y = 0;
            while (kStudent[y] < k)
                y++;
            return tStudentKrit[y - 1] + (tStudentKrit[y] - tStudentKrit[y - 1]) * (k - kStudent[y - 1]) / (kStudent[y] - kStudent[y - 1]);
        }
    }
}
