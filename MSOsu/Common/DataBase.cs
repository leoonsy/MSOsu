﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Common
{
    class DataBase
    {
        /// <summary>
        /// Таблица распределения Стьюдента
        /// </summary>
        private static double[] tStudentKrit = new double[] { 12.706,  4.303,  3.182,  2.776,  2.571,  2.447,  2.365,  2.306,  2.262,  2.228,  2.201,  2.179,  2.16,  2.145,  2.131,  2.12,  2.11,  2.101,  2.093,  2.086,  2.08,  2.074,  2.069,  2.064,  2.06,  2.056,  2.052,  2.048,  2.045,  2.042,  2.021,  2.009,  2,  1.99,  1.984,  1.98,  1.972 };

        /// <summary>
        /// Число степеней свободы
        /// </summary>
        private static double[] kStudent = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 40, 50, 60, 80, 100, 120, 200, };

        /// <summary>
        /// Получить значение t-критерия Стьюдента
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double GetTCrit(int k) //альфа = 0.05
        {
            if (k > kStudent.Last())
                return tStudentKrit.Last();
            int tIndex = kStudent.ToList().IndexOf(k);
            if (tIndex != -1)
                return tStudentKrit[tIndex];
            int y = 0;
            while (kStudent[y] < k)
                y++;
            return tStudentKrit[y - 1] + (tStudentKrit[y] - tStudentKrit[y - 1]) * (k - kStudent[y - 1]) / (kStudent[y] - kStudent[y - 1]);
        }

        /// <summary>
        /// Таблица распределения Фишера
        /// </summary>
        public static double[][] FFisherKrit = new double[][]
            {
                new double[] { 161.4476,  18.5128,  10.128,  7.7086,  6.6079,  5.9874,  5.5914,  5.3177,  5.1174,  4.9646,  4.8443,  4.7472,  4.6672,  4.6001,  4.5431,  4.494,  4.4513,  4.4139,  4.3807,  4.3512,  4.3248,  4.3009,  4.2793,  4.2597,  4.2417,  4.2252,  4.21,  4.196,  4.183,  4.1709,  4.0847,  4.0012,  3.9201,  3.8415},
                new double[] { 199.5,  19,  9.5521,  6.9443,  5.7861,  5.1433,  4.7374,  4.459,  4.2565,  4.1028,  3.9823,  3.8853,  3.8056,  3.7389,  3.6823,  3.6337,  3.5915,  3.5546,  3.5219,  3.4928,  3.4668,  3.4434,  3.4221,  3.4028,  3.3852,  3.369,  3.3541,  3.3404,  3.3277,  3.3158,  3.2317,  3.1504,  3.0718,  2.9957},
                new double[] { 215.7073,  19.1643,  9.2766,  6.5914,  5.4095,  4.7571,  4.3468,  4.0662,  3.8625,  3.7083,  3.5874,  3.4903,  3.4105,  3.3439,  3.2874,  3.2389,  3.1968,  3.1599,  3.1274,  3.0984,  3.0725,  3.0491,  3.028,  3.0088,  2.9912,  2.9752,  2.9604,  2.9467,  2.934,  2.9223,  2.8387,  2.7581,  2.6802,  2.6049},
                new double[] { 224.5832,  19.2468,  9.1172,  6.3882,  5.1922,  4.5337,  4.1203,  3.8379,  3.6331,  3.478,  3.3567,  3.2592,  3.1791,  3.1122,  3.0556,  3.0069,  2.9647,  2.9277,  2.8951,  2.8661,  2.8401,  2.8167,  2.7955,  2.7763,  2.7587,  2.7426,  2.7278,  2.7141,  2.7014,  2.6896,  2.606,  2.5252,  2.4472,  2.3719},
                new double[] { 230.1619,  19.2964,  9.0135,  6.2561,  5.0503,  4.3874,  3.9715,  3.6875,  3.4817,  3.3258,  3.2039,  3.1059,  3.0254,  2.9582,  2.9013,  2.8524,  2.81,  2.7729,  2.7401,  2.7109,  2.6848,  2.6613,  2.64,  2.6207,  2.603,  2.5868,  2.5719,  2.5581,  2.5454,  2.5336,  2.4495,  2.3683,  2.2899,  2.2141},
                new double[] { 233.986,  19.3295,  8.9406,  6.1631,  4.9503,  4.2839,  3.866,  3.5806,  3.3738,  3.2172,  3.0946,  2.9961,  2.9153,  2.8477,  2.7905,  2.7413,  2.6987,  2.6613,  2.6283,  2.599,  2.5727,  2.5491,  2.5277,  2.5082,  2.4904,  2.4741,  2.4591,  2.4453,  2.4324,  2.4205,  2.3359,  2.2541,  2.175,  2.0986},
                new double[] { 236.7684,  19.3532,  8.8867,  6.0942,  4.8759,  4.2067,  3.787,  3.5005,  3.2927,  3.1355,  3.0123,  2.9134,  2.8321,  2.7642,  2.7066,  2.6572,  2.6143,  2.5767,  2.5435,  2.514,  2.4876,  2.4638,  2.4422,  2.4226,  2.4047,  2.3883,  2.3732,  2.3593,  2.3463,  2.3343,  2.249,  2.1665,  2.0868,  2.0096},
                new double[] { 238.8827,  19.371,  8.8452,  6.041,  4.8183,  4.1468,  3.7257,  3.4381,  3.2296,  3.0717,  2.948,  2.8486,  2.7669,  2.6987,  2.6408,  2.5911,  2.548,  2.5102,  2.4768,  2.4471,  2.4205,  2.3965,  2.3748,  2.3551,  2.3371,  2.3205,  2.3053,  2.2913,  2.2783,  2.2662,  2.1802,  2.097,  2.0164,  1.9384},
                new double[] { 240.5433,  19.3848,  8.8123,  5.9988,  4.7725,  4.099,  3.6767,  3.3881,  3.1789,  3.0204,  2.8962,  2.7964,  2.7144,  2.6458,  2.5876,  2.5377,  2.4943,  2.4563,  2.4227,  2.3928,  2.366,  2.3419,  2.3201,  2.3002,  2.2821,  2.2655,  2.2501,  2.236,  2.2229,  2.2107,  2.124,  2.0401,  1.9588,  1.8799},
                new double[] { 241.8817,  19.3959,  8.7855,  5.9644,  4.7351,  4.06,  3.6365,  3.3472,  3.1373,  2.9782,  2.8536,  2.7534,  2.671,  2.6022,  2.5437,  2.4935,  2.4499,  2.4117,  2.3779,  2.3479,  2.321,  2.2967,  2.2747,  2.2547,  2.2365,  2.2197,  2.2043,  2.19,  2.1768,  2.1646,  2.0772,  1.9926,  1.9105,  1.8307},
                new double[] { 243.906,  19.4125,  8.7446,  5.9117,  4.6777,  3.9999,  3.5747,  3.2839,  3.0729,  2.913,  2.7876,  2.6866,  2.6037,  2.5342,  2.4753,  2.4247,  2.3807,  2.3421,  2.308,  2.2776,  2.2504,  2.2258,  2.2036,  2.1834,  2.1649,  2.1479,  2.1323,  2.1179,  2.1045,  2.0921,  2.0035,  1.9174,  1.8337,  1.7522},
                new double[] { 245.9499,  19.4291,  8.7029,  5.8578,  4.6188,  3.9381,  3.5107,  3.2184,  3.0061,  2.845,  2.7186,  2.6169,  2.5331,  2.463,  2.4034,  2.3522,  2.3077,  2.2686,  2.2341,  2.2033,  2.1757,  2.1508,  2.1282,  2.1077,  2.0889,  2.0716,  2.0558,  2.0411,  2.0275,  2.0148,  1.9245,  1.8364,  1.7505,  1.6664},
                new double[] { 248.0131,  19.4458,  8.6602,  5.8025,  4.5581,  3.8742,  3.4445,  3.1503,  2.9365,  2.774,  2.6464,  2.5436,  2.4589,  2.3879,  2.3275,  2.2756,  2.2304,  2.1906,  2.1555,  2.1242,  2.096,  2.0707,  2.0476,  2.0267,  2.0075,  1.9898,  1.9736,  1.9586,  1.9446,  1.9317,  1.8389,  1.748,  1.6587,  1.5705},
                new double[] { 249.0518,  19.4541,  8.6385,  5.7744,  4.5272,  3.8415,  3.4105,  3.1152,  2.9005,  2.7372,  2.609,  2.5055,  2.4202,  2.3487,  2.2878,  2.2354,  2.1898,  2.1497,  2.1141,  2.0825,  2.054,  2.0283,  2.005,  1.9838,  1.9643,  1.9464,  1.9299,  1.9147,  1.9005,  1.8874,  1.7929,  1.7001,  1.6084,  1.5173},
                new double[] { 250.0951,  19.4624,  8.6166,  5.7459,  4.4957,  3.8082,  3.3758,  3.0794,  2.8637,  2.6996,  2.5705,  2.4663,  2.3803,  2.3082,  2.2468,  2.1938,  2.1477,  2.1071,  2.0712,  2.0391,  2.0102,  1.9842,  1.9605,  1.939,  1.9192,  1.901,  1.8842,  1.8687,  1.8543,  1.8409,  1.7444,  1.6491,  1.5543,  1.4591},
                new double[] { 251.1432,  19.4707,  8.5944,  5.717,  4.4638,  3.7743,  3.3404,  3.0428,  2.8259,  2.6609,  2.5309,  2.4259,  2.3392,  2.2664,  2.2043,  2.1507,  2.104,  2.0629,  2.0264,  1.9938,  1.9645,  1.938,  1.9139,  1.892,  1.8718,  1.8533,  1.8361,  1.8203,  1.8055,  1.7918,  1.6928,  1.5943,  1.4952,  1.394},
                new double[] { 252.1957,  19.4791,  8.572,  5.6877,  4.4314,  3.7398,  3.3043,  3.0053,  2.7872,  2.6211,  2.4901,  2.3842,  2.2966,  2.2229,  2.1601,  2.1058,  2.0584,  2.0166,  1.9795,  1.9464,  1.9165,  1.8894,  1.8648,  1.8424,  1.8217,  1.8027,  1.7851,  1.7689,  1.7537,  1.7396,  1.6373,  1.5343,  1.429,  1.318},
                new double[] { 253.2529,  19.4874,  8.5494,  5.6581,  4.3985,  3.7047,  3.2674,  2.9669,  2.7475,  2.5801,  2.448,  2.341,  2.2524,  2.1778,  2.1141,  2.0589,  2.0107,  1.9681,  1.9302,  1.8963,  1.8657,  1.838,  1.8128,  1.7896,  1.7684,  1.7488,  1.7306,  1.7138,  1.6981,  1.6835,  1.5766,  1.4673,  1.3519,  1.2214},
                new double[] { 254.3144,  19.4957,  8.5264,  5.6281,  4.365,  3.6689,  3.2298,  2.9276,  2.7067,  2.5379,  2.4045,  2.2962,  2.2064,  2.1307,  2.0658,  2.0096,  1.9604,  1.9168,  1.878,  1.8432,  1.8117,  1.7831,  1.757,  1.733,  1.711,  1.6906,  1.6717,  1.6541,  1.6376,  1.6223,  1.5089,  1.3893,  1.2539,  1}
            };

        public static int[] Fv1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 15, 20, 24, 30, 40, 60, 120, };
        public static int[] Fv2 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 40, 60, 120 };

        /// <summary>
        /// Получить значение F-критерия Фишера
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double GetFCrit(int v1, int v2) //альфа = 0.05
        {
            if (v1 > Fv1.Last())
                v1 = Fv1.Length;
            if (v2 > Fv2.Last())
                v2 = Fv2.Length;
            int vv1 = Fv1.ToList().FindIndex(x => x == v1);
            int vv2 = Fv2.ToList().FindIndex(x => x == v2);
            if (vv1 != -1 && vv2 != -1)
                return FFisherKrit[vv1][vv2];

            int y, w;
            if (vv1 != -1)
            {
                double[] fLine = FFisherKrit[vv1];
                y = 0;
                while (Fv2[y] < v2)
                    y++;
                return fLine[y - 1] + (fLine[y] - fLine[y - 1]) * (v2 - Fv2[y - 1]) / (Fv2[y] - Fv2[y - 1]);
            }
            if (vv2 != -1)
            {
                double[] fline = MatrixOperations.Transpose(FFisherKrit)[vv2];
                y = 0;
                while (Fv1[y] < v1)
                    y++;
                return fline[y - 1] + (fline[y] - fline[y - 1]) * (v1 - Fv1[y - 1]) / (Fv1[y] - Fv1[y - 1]);
            }
            y = 0; w = 0;
            while (Fv1[y] < v1)
                y++;
            while (Fv2[w] < v2)
                w++;
            double t1 = FFisherKrit[y - 1][w - 1] + (FFisherKrit[y - 1][w] - FFisherKrit[y - 1][w - 1]) * (v2 - Fv2[w - 1]) / (Fv2[w] - Fv2[w - 1]);
            double t2 = FFisherKrit[y][w - 1] + (FFisherKrit[y][w] - FFisherKrit[y][w - 1]) * (v2 - Fv2[w - 1]) / (Fv2[w] - Fv2[w - 1]);
            return t1 + (t2 - t1) * (v1 - Fv1[y - 1]) / (Fv1[y] - Fv1[y - 1]);
        }
    }
}
