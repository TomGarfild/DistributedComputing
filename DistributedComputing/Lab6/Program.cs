using MPI;

namespace Lab6
{
    internal class Program
    {
        public static void Gen(double[,] a)
        {
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    a[i, j] = Random.Shared.Next(10);
                }
            }
        }

        public static void Print(double[,] c)
        {
            int N = c.GetLength(0);
            Console.WriteLine("******************************************************\n");
            Console.WriteLine("Result Matrix:\n");
            for (int i = 0; i < N; i++)
            {
                Console.WriteLine("\n");
                for (int j = 0; j < N; j++)
                    Console.Write($"{c[i, j]} ");
            }
            Console.WriteLine("\n******************************************************\n");

        }

        static void Main(string[] args)
        {
            var m = new Multiplicator();
            var a = new double[5,5];
            Gen(a);
            var b = new double[5, 5];
            Gen(b);
            Print(a);
            Print(b);
            m.SetMatrixes(a, b, 5);
            m.Solve(Intracommunicator.Adopt(0));
        }
    }
    public class Multiplicator
    {
        private const int Root = 0;

        const int FROM_MASTER = 2;

        const int FROM_WORKER = 2;

        public int N { get; private set; }

        public double[,] A { get; private set; }
        public double[,] B { get; private set; }

        public bool IsMaster { get; private set; }

        private double[,] c;

        public void SetMatrixes(double[,] a, double[,] b, int n)
        {
            N = n;
            A = a;
            B = b;
        }

        public void Solve(MPI.Intracommunicator mpi)
        {
            IsMaster = mpi.Rank == 0; // proccess with rank 0            

            int numtasks,              /* number of tasks in partition */
                taskid,                /* a task identifier */
                numworkers,            /* number of worker tasks */
                source,                /* task id of message source */
                dest,                  /* task id of message destination */
                mtype,                 /* message type */
                rows,                  /* rows of matrix A sent to each worker */
                averow, extra, offset, /* used to determine rows sent to each worker */
                i, j, k, rc;           /* misc */

            numworkers = mpi.Size - 1;

            if (IsMaster)
            {
                /* Send matrix data to the worker tasks */
                averow = N / numworkers;
                extra = N % numworkers;
                offset = 0;
                mtype = FROM_MASTER;
                for (dest = 1; dest <= numworkers; dest++)
                {
                    rows = (dest <= extra) ? averow + 1 : averow;
                    Console.WriteLine("Sending {0} rows to task {1} offset={2}\n", rows, dest, offset);
                    mpi.Send(offset, dest, mtype);
                    mpi.Send(rows, dest, mtype);
                    mpi.Send(A[offset, 0], dest, mtype);
                    mpi.Send(B, dest, mtype);
                    offset += rows;
                }

                /* Receive results from worker tasks */
                mtype = FROM_WORKER;
                for (i = 1; i <= numworkers; i++)
                {
                    source = i;
                    mpi.Receive(source, mtype, out offset);
                    mpi.Receive(source, mtype, out rows);
                    mpi.Receive(source, mtype, out c[offset, 0]);
                    Console.WriteLine("Received results from task \n", source);
                }

                /* Print results */
                Console.WriteLine("******************************************************\n");
                Console.WriteLine("Result Matrix:\n");
                for (i = 0; i < N; i++)
                {
                    Console.WriteLine("\n");
                    for (j = 0; j < N; j++)
                        Console.Write($"{c[i, j]} ");
                }
                Console.WriteLine("\n******************************************************\n");

            }
            if (!IsMaster)
            {
                mtype = FROM_MASTER;
                mpi.Receive<int>(0, mtype);
                mpi.Receive(0, mtype, out rows);

                mpi.Receive(0, mtype, out double[,] a);
                mpi.Receive(0, mtype, out double[,] b);

                A = a;
                B = b;

                for (k = 0; k < N; k++)
                {
                    for (i = 0; i < rows; i++)
                    {
                        c[i, k] = 0.0;
                        for (j = 0; j < N; j++)
                            c[i, k] += A[i, j] * B[j, k];
                    }
                }
                mtype = FROM_WORKER;
                offset = 0;
                mpi.Send(offset, 0, mtype);
                mpi.Send(rows, 0, mtype);
                mpi.Send(c, 0, mtype);
            }

        }
    }

    public class Multiplier
    {
        public void Naive()
        {
            int rank, numberOfThreads;
            double[,] A, B, C;

        }
    }
}