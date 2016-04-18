using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleMatrix
{
    public class Matrix
    {
        public static double[,] MatrixMul(double[,] matA, double[,] matB, int[] sizeA, int[] sizeB)
        {
            if(sizeA[1] != sizeB[0])
                return null;
            double[,] matAns = new double[sizeA[0], sizeB[1]];
            double[,] tempB = new double[sizeB[1], sizeB[0]];
            
            // transpose B
            for (int i = 0; i < sizeB[0]; i++)
            {
                for (int j = 0; j < sizeB[1]; j++)
                {
                    tempB[j, i] = matB[i, j];
                }
            }

            double temp = 0;
            for (int i = 0; i < sizeA[0]; i++)
            {
                for (int j = 0; j < sizeB[1]; j++)
                {
                    temp = 0;
                    for (int k = 0; k < sizeA[1]; k++)
                    {
                        temp += matA[i, k] * tempB[j, k];
                    }
                    matAns[i, j] = temp;
                }
            }

            return matAns;
        }

        public static double[,] MatrixMulParallel(double[,] matA, double[,] matB, int[] sizeA, int[] sizeB)
        {
            Task[] tsk = new Task[4];
            double[,] matAns = new double[sizeA[0], sizeB[1]];

            for (int _i = 0; _i < 4; _i++)
            {
                int i = _i;
                tsk[i] = Task.Factory.StartNew(() =>
                {
                    int count = sizeA[0] / 4;
                    int startRow = sizeA[0] / 4 * i;
                    double[,] piece = splitMul(matA, matB, startRow, sizeB[1], sizeB[0],count);

                    for (int row = startRow, k = 0; row < startRow + count; row++, k++)
                    {
                        for (int j = 0; j < sizeB[1]; j++)
                        {
                            matAns[row, j] = piece[k, j];
                        }
                    }
                });
            }

            Task.WaitAll(tsk);
            return matAns;
        }

        private static double[,] splitMul(double[,] matA, double[,] matB, int startRow, int colSize, int rowSize, int count)
        {
            double[,] temp = new double[count, colSize];
            for (int i = startRow, k = 0; i < startRow + count; i++, k++)
            {
                for (int j = 0; j < colSize; j++)
                {
                    temp[k, j] = matA[i, j];
                }
            }
            return MatrixMul(temp, matB, new int[] { count, colSize }, new[] { rowSize, colSize });
        }

        public static double[,] Random(int row, int col)
        {
            double[,] mat = new double[row, col];
            Random rnd = new Random();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mat[i, j] = rnd.NextDouble() * rnd.Next(10);
                }
            }
            return mat;
        }
    }
}
