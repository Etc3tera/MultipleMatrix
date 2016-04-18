using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] testsize = new int[]{ 100, 500, 1000 };

            for (int round = 0; round < testsize.Length; round++)
            {
                int MATSIZE = testsize[round];

                Console.WriteLine("Multiple matrix size {0}x{0} with {0}x{0}", MATSIZE);

                double[,] A = Matrix.Random(MATSIZE, MATSIZE);
                double[,] B = Matrix.Random(MATSIZE, MATSIZE);

                Stopwatch sw = Stopwatch.StartNew();
                double[,] R = Matrix.MatrixMul(A, B, new int[] { MATSIZE, MATSIZE }, new int[] { MATSIZE, MATSIZE });
                sw.Stop();
                Console.WriteLine("Single Thread: {0} ms.", sw.ElapsedMilliseconds);

                sw = Stopwatch.StartNew();
                double[,] R2 = Matrix.MatrixMulParallel(A, B, new int[] { MATSIZE, MATSIZE }, new int[] { MATSIZE, MATSIZE });
                sw.Stop();
                Console.WriteLine("10 Tasks     : {0} ms.", sw.ElapsedMilliseconds);

                for (int i = 0; i < MATSIZE; i++)
                {
                    for (int j = 0; j < MATSIZE; j++)
                    {
                        if (R[i, j] != R2[i, j])
                        {
                            Console.WriteLine("Assert Fail {0}, {1}", R[i, j], R2[i, j]);
                            return;
                        }
                    }
                }
                Console.WriteLine("Assert Pass\n");
            }
        }
    }
   
}
