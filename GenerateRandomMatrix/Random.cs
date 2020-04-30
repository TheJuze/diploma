using System;
using System.IO;

namespace GenerateRandomMatrix
{
    class Random
    {
        static void Main(string[] args)
        {
            int n = Diploma.Program.n;
            int[,] A = RandGenerator(n);
            FillFile(A, n);
        }
        static int[,] RandGenerator(int n)
        {
            int[,] A = new int[n, n];
            System.Random rand = new System.Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        int symbol = rand.Next(0, 2);
                        A[i, j] = symbol;
                    }
                    else A[i, j] = 0;
                }
            }
            return A;
        }
        static void FillFile(int[,] A, int n)
        {
            string path = @"C:\Users\diman\source\repos\Diploma";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            // запись в файл
            //1. Open file
            FileStream fstream = new FileStream($"{path}/input.txt", FileMode.Create);
            //2. Запись в файл
            string msg = "";//формирование строки
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    msg += A[i, j].ToString();
                    if (j != n - 1)
                    {
                        msg += " ";
                    }

                }
                if (i != n - 1)
                {
                    msg = msg + "\r\n";//перевод строки
                }
            }
            //3. перевод строки в байты
            byte[] msgByte = System.Text.Encoding.Default.GetBytes(msg);
            //закрыть поток
            fstream.Write(msgByte, 0, msgByte.Length);
            if (fstream != null)
            {
                fstream.Close();
            }
        }
    }
}
