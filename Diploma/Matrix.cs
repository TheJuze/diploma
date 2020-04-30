using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma
{
    class Matrix
    {
        public static int rows;
        public static int cols;
        public static int[,] A;
        public Matrix(int row, int column)// конструктор
        {
            rows = row;
            cols = column;
            A = new int[rows, cols];

        }
        public int this[int row, int column]//индексатор
        {
            get { return A[row, column]; }
            set { A[row, column] = value; }
        }
        public void Fill()//Заполнить исходную матрицу из файла
        {
            int[] arr = File.ReadAllText(@"C:\Users\diman\source\repos\Diploma\input.txt").Split(' ', '\n').Select(n => int.Parse(n)).ToArray();
            int k = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    A[i, j] = arr[k];
                    k++;
                }
            }
        }
        public int[,] Displacement(int[] pi) //Возвращает матрицу после перестановки, pi- вектор перестановки
        {
            int[,] B = new int[rows,cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    B[i, j] = A[pi[i], pi[j]];
                }
            }
            return B;
        }

        private static List<List<int>> FindE()
        {

            List<List<int>> myList = new List<List<int>>();
            for (int i = 0; i < rows; i++)
            {
                List<int> subList = new List<int>();
                for (int j = 0; j < cols; j++)
                {
                    if (A[i, j] == 0)
                    {
                        subList.Add(j);
                    }
                }
                myList.Add(subList);

            }
            return myList;
        }

        private static List<List<int>> FindH()
        {

            List<List<int>> myList = new List<List<int>>();
            for (int i = 0; i < rows; i++)
            {
                List<int> subList = new List<int>();
                for (int j = 0; j < cols; j++)
                {
                    if (A[j, i] == 0)
                    {
                        subList.Add(j);
                    }
                }
                myList.Add(subList);

            }
            return myList;
        }

        public List<List<int>> FindOmegaZero()//Омега (1,0)
        {
            List<List<int>> Omega = new List<List<int>>();
            List<int> first = new List<int>();
            for (int i = 0; i < rows; i++)
            {
                if (A[i, i] == 0)
                {
                    first.Add(i);
                }
            }
            Omega.Add(first);
            return Omega;
        }
        
        public static D FindD(List<int> omega) //на вход омега, исходя из него строятся множества
        {
            List<List<int>> E = new List<List<int>>();
            E = FindE();
            List<List<int>> H = new List<List<int>>();
            H = FindH();
            D DArray = new D();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i == j || !E[i].Contains(j) || !H[j].Contains(i) || !E[i].Contains(i) || !H[j].Contains(j)) continue;
                    List<int> EH = new List<int>(E[i].Intersect(H[j]).Intersect(omega));//Пересечение E[i],H[j] и омега
                    DArray.Index.Add(EH);
                    DArray.i.Add(i);
                    DArray.j.Add(j);
                }
            }
            return DArray;
        }
       
        public void Print()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write("{0} ", A[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
