using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1_112gv
{
    class Program
    {
        static void Main()
        {
            // Входные данные
            double[,] A = { { 1.00, 0.42, 0.54, 0.66 }, 
                            { 0.42, 1.00, 0.32, 0.44 }, 
                            { 0.54, 0.32, 1.00, 0.22 }, 
                            { 0.66, 0.44, 0.22, 1.00 } }; ;

            double[] b = { 0.3, 0.5, 0.7, 0.9 };

            // Решение уравнения
            double[] x = SolveEquation(A, b);

            // Вывод результата
            Console.WriteLine("Решение:");
            for (int i = 0; i < x.Length; i++)
            {
                Console.WriteLine("x[{0}] = {1:f4}", i, x[i]);
            }
            Console.WriteLine(" - - - ");

            double[] res = CalculateResidual(A, x, b);
            Console.WriteLine("Вектор невязки:");
            for (int i = 0; i < x.Length; i++)
            {
                Console.WriteLine("{0}", res[i]);
            }

        }

        // Метод для решения матричного уравнения Ax = b с помощью LU-разложения
        static double[] SolveEquation(double[,] A, double[] b)
        {
            int n = A.GetLength(0);
            double[,] LU = new double[n, n];

            // Разложение матрицы A на LU
            DecomposeLU(A, LU);

            // Решение системы Ly = b
            double[] y = SolveLowerTriangular(LU, b);

            // Решение системы Ux = y
            double[] x = SolveUpperTriangular(LU, y);

            return x;
        }

        // Метод для разложения матрицы A на LU с частичным выбором ведущего элемента
        static void DecomposeLU(double[,] A, double[,] LU)
        {
            int n = A.GetLength(0);

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < i; k++)
                    {
                        sum += LU[i, k] * LU[k, j];
                    }
                    LU[i, j] = A[i, j] - sum;
                }

                for (int j = i + 1; j < n; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < i; k++)
                    {
                        sum += LU[j, k] * LU[k, i];
                    }
                    LU[j, i] = (A[j, i] - sum) / LU[i, i];
                }
            }
        }

        // Метод для решения системы с нижнетреугольной матрицей
        static double[] SolveLowerTriangular(double[,] LU, double[] b)
        {
            int n = b.Length;
            double[] y = new double[n];

            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < i; j++)
                {
                    sum += LU[i, j] * y[j];
                }
                y[i] = b[i] - sum;
            }

            return y;
        }

        // Метод для решения системы с верхнетреугольной матрицей
        static double[] SolveUpperTriangular(double[,] LU, double[] y)
        {
            int n = y.Length;
            double[] x = new double[n];

            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += LU[i, j] * x[j];
                }
                x[i] = (y[i] - sum) / LU[i, i];
            }

            return x;
        }

        // Метод для вычисления нормы вектора невязки
        static double[] CalculateResidual(double[,] A, double[] x, double[] b)
        {
            // Вычисляем вектор невязки
            int n = A.GetLength(0);
            double[] residual = new double[n];
            
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    residual[i] += A[i, j] * x[j];
                    
                }
                residual[i] -= b[i];
               
            }
            return residual;

            /* Вычисляем норму вектора невязки
            double norm = 0;
            for (int i = 0; i < n; i++)
            {
                norm += Math.Pow(residual[i], 2);
            }
                norm = Math.Sqrt(norm);

            return norm;*/
        }
    }
}
