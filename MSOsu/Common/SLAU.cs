using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSOsu.Common
{
    public class SLAU
    {
        /// <summary>
        /// Тип решения СЛАУ методом Гаусса
        /// </summary>
        public enum GaussMethod
        {
            Org,
            Cols,
            Rows,
            All
        }

        /// <summary>
        /// Обменять значения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Обменять колонки
        /// </summary>
        /// <param name="a"></param>
        /// <param name="indx1"></param>
        /// <param name="indx2"></param>
        private static void SwapCols(ref double[][] a, int indx1, int indx2)
        {
            double[] temp = a.Select(e => e[indx1]).ToArray();
            for (int i = 0; i < temp.Length; i++)
            {
                a[i][indx1] = a[i][indx2];
                a[i][indx2] = temp[i];
            }
        }

        /// <summary>
        /// Установить решение СЛАУ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static double[] GetSLAUResolve(double[][] a, double[] b, GaussMethod type)
        {
            a = MatrixOperations.Copy(a);
            b = MatrixOperations.Copy(b);
            int varCount = a.Length;

            const double epsilon = 0.00000000000001; //точность нуля

            int[] perm = null;
            if (type == GaussMethod.Rows || type == GaussMethod.All)
                perm = Enumerable.Range(0, b.Length).ToArray(); //перестановка


            for (int k = 0; k < varCount; k++)
            {
                switch (type)
                {
                    //выбор ведущего (максимального) элемента по столбцу
                    case GaussMethod.Cols:
                        int index = k;
                        double max = Math.Abs(a[k][k]);
                        for (int i = k + 1; i < varCount; i++)
                        {
                            if (Math.Abs(a[i][k]) > max)
                            {
                                max = Math.Abs(a[i][k]);
                                index = i;
                            }
                        }

                        if (max < epsilon)
                            throw new Exception("Нет ненулевых диагональных элементов");

                        Swap(ref a[k], ref a[index]);
                        Swap(ref b[k], ref b[index]);
                        break;
                    //выбор ведущего (максимального) элемента по строке
                    case GaussMethod.Rows:
                        index = k;
                        max = Math.Abs(a[k][k]);
                        for (int i = k + 1; i < varCount; i++)
                        {
                            if (Math.Abs(a[k][i]) > max)
                            {
                                max = Math.Abs(a[k][i]);
                                index = i;
                            }
                        }

                        if (max < epsilon)
                            throw new Exception("Нет ненулевых диагональных элементов");

                        SwapCols(ref a, k, index);
                        Swap(ref perm[k], ref perm[index]);
                        break;
                    //выбор ведущего (максимального) элемента по всей матрице
                    case GaussMethod.All:
                        int nRow = k, nCol = k;
                        max = Math.Abs(a[k][k]);
                        for (int i = k; i < varCount; i++)
                        {
                            for (int j = k; j < varCount; j++)
                            {
                                if (Math.Abs(a[i][j]) > max)
                                {
                                    max = Math.Abs(a[i][j]);
                                    nRow = i;
                                    nCol = j;
                                }
                            }
                        }

                        if (max < epsilon)
                            throw new Exception("Нет ненулевых диагональных элементов");

                        Swap(ref a[k], ref a[nRow]);
                        Swap(ref b[k], ref b[nRow]);
                        SwapCols(ref a, k, nCol);
                        Swap(ref perm[k], ref perm[nCol]);
                        break;
                }


                //Приведение к треугольному виду
                double temp = a[k][k];
                for (int j = k; j < varCount; j++)
                    a[k][j] = a[k][j] / temp;
                b[k] = b[k] / temp;

                for (int i = k + 1; i < varCount; i++)
                {
                    temp = a[i][k];
                    if (Math.Abs(a[i][k]) < epsilon) continue; // для нулевого коэффициента пропустить

                    for (int j = k; j < varCount; j++)
                        a[i][j] = a[k][j] * temp - a[i][j];
                    b[i] = b[k] * temp - b[i];
                }
            }

            //нахождение x

            double[] x = new double[varCount];
            for (int k = varCount - 1; k >= 0; k--)
            {
                x[k] = b[k];
                for (int i = 0; i < k; i++) //пробег по столбцам до k-1
                    b[i] = b[i] - a[i][k] * x[k];
            }


            //Учитываем перестановку вектора x
            if (type == GaussMethod.Rows || type == GaussMethod.All)
            {
                double[] temp = new double[x.Length];
                for (int i = 0; i < perm.Length; i++)
                    temp[perm[i]] = x[i];
                x = temp;
            }

            return x;
        }
    }
}
