using System;
using System.Linq;
using System.Collections.Generic;

namespace Diploma
{
    public class Program
    {
        public static int n { get; } = 13;//размерность матрицы
        public static List<WeightCoefficient> weights { get; set; } = new List<WeightCoefficient>();//весовые коэф. индекс/значение
        private static Matrix a = new Matrix(n, n);//исходная структурная матрица
        private static int[,] b = new int[n, n];//преобразованная структурная матрица

        public static int[] transition = new int[n];  //перестановка

        private static int taskType = 2;//тип задачи


        private static List<List<int>> omega = new List<List<int>>();//множество Омега
        private static List<List<int>> secondOmega = new List<List<int>>();//множество Омега

        private static OrderedMultitude firstRecord;//рекордное B1
        private static OrderedMultitude secondRecord;//рекордное B2

        private static OrderedMultitude firstOrderedMultitude;//B1
        private static OrderedMultitude secondOrderedMultitude;//B2

        private static List<List<CentralElement>> supportingMultitudeF = new List<List<CentralElement>>();//первое вспомогательное множество
        private static List<List<PossibleExtension>> supportingMultitudeQ = new List<List<PossibleExtension>>();//второе вспомогательное множество
        private static List<List<PossibleExtension>> helpfullMultitudeQ = new List<List<PossibleExtension>>();//вспомогательное множество для второго дерева
        private static List<List<CentralElement>> centralSecondElement = new List<List<CentralElement>>();//центральные элементы для 2 дерева
        //уровни дерева перебора
        private static int indexOfFirstPlunk;
        private static int indexOfSecondPlunk;

        private static CentralElement centralElement;//центральный элемент для 1 мн-ва
        private static PossibleExtension knotElement;//узловой элемент

        private static List<List<PossibleExtension>> quantityOfPossibleExtension = new List<List<PossibleExtension>>();//Множество возможных продолжений для первого множества
        private static List<List<PossibleExtension>> quantityOfSecondPossibleExtension = new List<List<PossibleExtension>>();//Множество возможных продолжений для второго множества
        static void Main(string[] args)
        {
            a.Fill();//заполнить структурную матрицу из файла
            Console.WriteLine("Исходная структурная матрица:");
            a.Print();//Показать заполненную матрицу
            //TODO: брать веса из файла
            for (int i = 0; i < n; i++)
            {
                weights.Add(new WeightCoefficient(i, 1));
            }

            omega = a.FindOmegaZero();//ищем омега (1,0)

            if (omega[0].Count == 0)
            {
                Console.WriteLine("Никакое изменение порядка следования уравнений исходной системы не может обеспечить выделения групп уравнений, имеющих структурные особенности");
            }
            else
            {
                if (taskType == 1)
                {
                    firstOrderedMultitude = new OrderedMultitude();
                    firstRecord = new OrderedMultitude();
                    secondRecord = new OrderedMultitude();
                    ThirdStep();
                }
                else if (taskType == 2)
                {
                    FirstStep();
                }
            }
        }
        private static void FirstStep()
        {
            indexOfFirstPlunk = 0;
            firstRecord = new OrderedMultitude();//B1
            secondRecord = new OrderedMultitude();//B2
            supportingMultitudeF.Add(new List<CentralElement>());
            supportingMultitudeQ.Add(new List<PossibleExtension>());
            quantityOfPossibleExtension.Add(new List<PossibleExtension>());
            SecondStep();
        }
        private static void SecondStep()
        {
            if (omega[indexOfFirstPlunk].Count != 0)
            {
                SecondStepA();
            }
            else if (omega[indexOfFirstPlunk].Count == 0 && indexOfFirstPlunk > 0)
            {
                SecondStepB();
            }
        }
        private static void SecondStepA()
        {
            quantityOfPossibleExtension[indexOfFirstPlunk] = PossibleExtension.FindPossibleExtensions(omega[indexOfFirstPlunk]);
            SecondStepA1();
        }
        private static void SecondStepA1()
        {
            //формируем список F
            #region
            List<int> newMultitudeF = new List<int>();
            foreach (CentralElement element in supportingMultitudeF[indexOfFirstPlunk])
            {
                newMultitudeF.Add(element.index);
            }
            #endregion
            if (omega[indexOfFirstPlunk].Except(newMultitudeF).Count() == 0)
            {
                SecondStepA1b();
            }
            else
            {
                //формируем список F
                #region
                newMultitudeF = new List<int>();
                foreach (CentralElement element in supportingMultitudeF[indexOfFirstPlunk])
                {
                    newMultitudeF.Add(element.index);
                }
                #endregion
                centralElement = CentralElement.FindCentral(new List<int>(omega[indexOfFirstPlunk].Except(newMultitudeF)));
                if (IsEqual(quantityOfPossibleExtension[indexOfFirstPlunk], supportingMultitudeQ[indexOfFirstPlunk]))
                {
                    SecondStepA1c();
                }
                else if (new List<PossibleExtension>(quantityOfPossibleExtension[indexOfFirstPlunk].Except(supportingMultitudeQ[indexOfFirstPlunk])).Count != 0)
                {
                    knotElement = PossibleExtension.FindKnot(new List<PossibleExtension>(quantityOfPossibleExtension[indexOfFirstPlunk].Except(supportingMultitudeQ[indexOfFirstPlunk])));
                    //сравниваем веса претендентов на роль узлового и центрального эл-тов
                    if (knotElement.weight < centralElement.weight)//неравенство 2.3.1
                    {
                        SecondStepA1c();
                    }
                    else//неравенство 2.3.2
                    {
                        //левая часть 
                        #region
                        int newWeight = 0;
                        foreach (List<PossibleExtension> element in supportingMultitudeQ)
                        {
                            if (element.Any())
                            {
                                foreach (WeightCoefficient weight in weights)
                                {
                                    if (element.Last().i == weight.index || element.Last().j == weight.index)
                                    {
                                        newWeight += weight.weight;//первое слагаемое
                                    }
                                }
                            }

                        }

                        newWeight += knotElement.weight;//сумма слагаемых
                        #endregion
                        if (newWeight > (firstRecord.weight + secondRecord.weight) / 2)
                        {
                            supportingMultitudeQ[indexOfFirstPlunk].Add(knotElement);
                            //формируем новое множество омега
                            List<int> usedIndexes = new List<int>();
                            usedIndexes.Add(knotElement.i);
                            usedIndexes.Add(knotElement.j);

                            omega.Add(new List<int>());
                            supportingMultitudeF.Add(new List<CentralElement>());
                            supportingMultitudeQ.Add(new List<PossibleExtension>());
                            quantityOfPossibleExtension.Add(new List<PossibleExtension>());
                            indexOfFirstPlunk++;

                            omega[indexOfFirstPlunk] = new List<int>(knotElement.elements.Except(usedIndexes));
                            SecondStep();
                        }
                        else
                        {
                            SecondStepA1b();
                        }
                    }
                }

            }
        }
        private static void SecondStepA1b()
        {
            if (indexOfFirstPlunk == 0)
            {
                FifthStep();
            }
            else
            {
                omega.RemoveAt(indexOfFirstPlunk);
                supportingMultitudeF.RemoveAt(indexOfFirstPlunk);
                supportingMultitudeQ.RemoveAt(indexOfFirstPlunk);
                quantityOfPossibleExtension.RemoveAt(indexOfFirstPlunk);
                indexOfFirstPlunk--;
                SecondStepA1();
            }
        }
        private static void SecondStepA1c()
        {
            //неравенство 2.3.3
            #region
            int newWeight = 0;

            foreach (List<PossibleExtension> element in supportingMultitudeQ)
            {
                if (element.Any())
                {
                    foreach (WeightCoefficient weight in weights)
                    {
                        if (element.Last().i == weight.index || element.Last().j == weight.index)
                        {
                            newWeight += weight.weight;//первое слагаемое
                        }
                    }
                }

            }
            newWeight += centralElement.weight;//сумма слагаемых
            #endregion
            if (newWeight > (firstRecord.weight + secondRecord.weight) / 2)
            {
                supportingMultitudeF[indexOfFirstPlunk].Add(centralElement);
                //cтроим упорядоченное множество B1 (firstOrderedMultitude)
                OrderFirstMultitude();
                ThirdStep();
            }
            else
            {
                SecondStepA1b();
            }
        }
        private static void SecondStepB()
        {/*
            supportingMultitudeF.Add(new List<ICentralElement>());
            supportingMultitudeQ.Add(new List<IPossibleExtension>());*/
            if (omega[indexOfFirstPlunk].Count == 0 && indexOfFirstPlunk > 0)
            {
                OrderFirstMultitude();
                ThirdStep();
            }
            else if (omega[indexOfFirstPlunk].Count == 0 && indexOfFirstPlunk == 0)
            {
                Console.WriteLine("В рамках рассматриваемых преобразований не существует перестановки, приводящей систему к нужному виду");
            }
        }

        private static void ThirdStep()//перебор для задачи 1 с этого пункта
        {
            //инициализация омега 2,0
            secondOmega = new List<List<int>>
            {
                new List<int>(omega[0].Except(firstOrderedMultitude.elements))
            };
            indexOfSecondPlunk = 0;//Номер уровня на 2 дереве перебора
            secondOrderedMultitude = new OrderedMultitude(0, new List<int>());//очищаем
            //вводим второе вспомогательные множества
            centralSecondElement = new List<List<CentralElement>>();
            helpfullMultitudeQ = new List<List<PossibleExtension>>();
            quantityOfSecondPossibleExtension = new List<List<PossibleExtension>>();
            centralSecondElement.Add(new List<CentralElement>());
            helpfullMultitudeQ.Add(new List<PossibleExtension>());
            quantityOfSecondPossibleExtension.Add(new List<PossibleExtension>());
            FourthStep();
        }

        private static void FourthStep()
        {
            if (secondOmega[indexOfSecondPlunk].Count != 0)
            {
                FourthStepA();
            }
            else
            {
                FourthStepB();
            }
        }
        private static void FourthStepA()
        {
            quantityOfSecondPossibleExtension[indexOfSecondPlunk] = PossibleExtension.FindPossibleExtensions(secondOmega[indexOfSecondPlunk]);
            centralSecondElement[indexOfSecondPlunk].Add(CentralElement.FindCentral(secondOmega[indexOfSecondPlunk]));
            FourthStepA0();
        }
        private static void FourthStepA0()
        {
            if ((new List<PossibleExtension>(quantityOfSecondPossibleExtension[indexOfSecondPlunk].Except(helpfullMultitudeQ[indexOfSecondPlunk])).Count == 0))
            {
                FourthStepA1c();
            }
            else
            {
                FourthStepA1a();
            }
        }
        private static void FourthStepA1a()//FourthStepA1a here
        {

            PossibleExtension possibleKnot = PossibleExtension.FindKnot(new List<PossibleExtension>(quantityOfSecondPossibleExtension[indexOfSecondPlunk].Except(helpfullMultitudeQ[indexOfSecondPlunk])));
            if (centralSecondElement[indexOfSecondPlunk].Last().weight > possibleKnot.weight)//неравенство 2.3.4
            {
                FourthStepA1c();
            }
            else
            {//неравенство 2.3.5
             //левая часть 
                #region
                int newWeight = 0;
                newWeight += firstOrderedMultitude.weight;//первое слагаемое
                foreach (PossibleExtension element in helpfullMultitudeQ[indexOfSecondPlunk])
                {
                    foreach (WeightCoefficient weight in Program.weights)
                    {
                        if (weight.index == element.i || weight.index == element.j)
                        {
                            secondOrderedMultitude.weight += weight.weight;
                        }
                    }
                }
                newWeight += possibleKnot.weight;//сумма слагаемых
                #endregion
                if (newWeight > firstRecord.weight + secondRecord.weight)
                {
                    helpfullMultitudeQ[indexOfSecondPlunk].Add(possibleKnot);
                    //формируем новое множество омега
                    List<int> usedIndexes = new List<int>();
                    usedIndexes.Add(possibleKnot.i);
                    usedIndexes.Add(possibleKnot.j);

                    secondOmega.Add(new List<int>());
                    centralSecondElement.Add(new List<CentralElement>());
                    helpfullMultitudeQ.Add(new List<PossibleExtension>());
                    quantityOfSecondPossibleExtension.Add(new List<PossibleExtension>());
                    indexOfSecondPlunk++;

                    secondOmega[indexOfSecondPlunk] = new List<int>(possibleKnot.elements.Except(usedIndexes));
                    FourthStep();
                }
                else
                {
                    FourthStepA1b();
                }
            }

        }
        private static void FourthStepA1b()
        {
            if (indexOfSecondPlunk > 0)
            {
                secondOmega.RemoveAt(indexOfSecondPlunk);
                centralSecondElement.RemoveAt(indexOfSecondPlunk);
                helpfullMultitudeQ.RemoveAt(indexOfSecondPlunk);
                quantityOfSecondPossibleExtension.RemoveAt(indexOfSecondPlunk);
                indexOfSecondPlunk--;
                FourthStepA0();
            }
            else if (indexOfSecondPlunk == 0)
            {
                Console.WriteLine("Работа алгоритма по поиску наилучшего упорядоченного множества B2 при фиксированном B1 завершена");//конец
                Console.Write("B1: ");
                foreach (int element in firstOrderedMultitude.elements)
                {
                    Console.Write("{0}", element);
                }
                Console.Write("    B2: ");
                foreach (int element in secondOrderedMultitude.elements)
                {
                    Console.Write("{0}", element);
                }
                Console.WriteLine();
                if (taskType == 1)
                {
                    Console.WriteLine("Найденное рекордное множество B2 и есть искомое");
                    Console.WriteLine();
                    FifthStep();
                }
                else if (taskType == 2)
                {
                    Console.WriteLine("Необходимо построить новое упорядоченное множество B1");
                    Console.WriteLine();
                    SecondStepA1();
                }
            }
        }
        private static void FourthStepA1c()
        {//неравенство 2.3.6
            //левая часть 
            #region
            int newWeight = 0;
            newWeight += firstOrderedMultitude.weight;//первое слагаемое
            newWeight += centralSecondElement[indexOfSecondPlunk].Last().weight;//второе слагаемое

            foreach (PossibleExtension element in helpfullMultitudeQ[indexOfSecondPlunk])//3 слагаемое
            {
                foreach (WeightCoefficient weight in Program.weights)
                {
                    if (weight.index == element.i || weight.index == element.j)
                    {
                        secondOrderedMultitude.weight += weight.weight;
                    }
                }
            }
            #endregion
            if (newWeight > firstRecord.weight + secondRecord.weight)
            {
                OrderSecondMultitude();
                FourthStepB1a();
            }
            else
            {
                FourthStepA1b();
            }
        }
        private static void FourthStepB()
        {
            if (indexOfSecondPlunk != 0)
            {
                FourthStepB1();
            }
            else
            {
                FourthStepB2();
            }
        }
        private static void FourthStepB1()
        {
            Console.WriteLine("Проход по этой ветви закончен");
            //helpfullMultitudeQ[indexOfSecondPlunk]=helpfullMultitudeQ[indexOfSecondPlunk - 1];
            OrderSecondMultitude();//конец
            FourthStepB1a();
        }
        private static void FourthStepB1a()
        {
            firstRecord = firstOrderedMultitude;
            secondRecord = secondOrderedMultitude;
            if (firstRecord.weight + secondRecord.weight == omega[0].Count)//неравенство 2.3.7
            {
                Console.WriteLine("Перебор окончен, сумма весов рекордных множеств имеет максимально возможный вес");//конец
                Console.Write("B1: ");
                foreach (int element in firstRecord.elements)
                {
                    Console.Write("{0}", element);
                }
                Console.Write("    B2: ");
                foreach (int element in secondRecord.elements)
                {
                    Console.Write("{0}", element);
                }
                Console.WriteLine();
                FifthStep();
            }
            else
            {
                FourthStepA1b();
            }
        }
        private static void FourthStepB2()
        {
            Console.WriteLine("На множестве Im/B^1 не существует упорядоченного непустого множества B^2 с заданными св-вами");
            if (firstOrderedMultitude.elements.Count != 0)
            {
                firstRecord = firstOrderedMultitude;
                secondOrderedMultitude = new OrderedMultitude();
                FifthStep();
            }
        }
        private static void FifthStep()//построение перестановки
        {
            List<int> diagonal = new List<int>();
            for (int j = 0; j < n; j++)
            {
                diagonal.Add(j);
            }
            if (firstRecord.elements == null)
            {
                firstRecord.elements = new List<int>(n + 1);
            }
            if (secondRecord.elements == null)
            {
                secondRecord.elements = new List<int>(n + 1);
            }
            List<int> nonRecord = new List<int>(diagonal.Except(firstRecord.elements).Except(secondRecord.elements));
            int i = 0;
            foreach (int element in nonRecord)
            {
                transition[i] = element;
                i++;
            }
            foreach (int element in firstRecord.elements)
            {
                transition[i] = element;
                i++;
            }
            foreach (int element in secondRecord.elements)
            {
                transition[i] = element;
                i++;
            }
            SixthStep();//FIXME: transition without 1 in diagonal
        }
        private static void SixthStep()
        {

            b = a.Displacement(transition);
            Console.Write("Первое рекордное множество:");
            foreach (int element in firstRecord.elements)
            {
                Console.Write("{0}", element);
            }
            Console.WriteLine();
            Console.Write("Первое рекордное множество:");
            foreach (int element in secondRecord.elements)
            {
                Console.Write("{0}", element);
            }
            Console.WriteLine();
            Console.WriteLine("Вид исходной перестановки: ");
            foreach (int element in transition)
            {
                Console.Write("{0}", element);
            }
            Console.WriteLine();
            Console.WriteLine("Матрица после перестановки:");
            PrintB();
            // Stack.Clear(); Надо очистить стэк
        }
        private static void PrintB()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write("{0} ", b[i, j]);
                }
                Console.WriteLine();
            }
        }
        private static void OrderFirstMultitude()
        {
            if (supportingMultitudeF[indexOfFirstPlunk].Count != 0)
            {
                //инициализация
                #region
                firstOrderedMultitude = new OrderedMultitude(0, new List<int>());
                int k = indexOfFirstPlunk * 2 + 1;
                for (int i = 0; i < k; i++)
                {
                    firstOrderedMultitude.elements.Add(new int());
                }
                #endregion
                //заполнение
                #region
                for (int eps = 0; eps < indexOfFirstPlunk; eps++)
                {
                    firstOrderedMultitude.elements[eps] = supportingMultitudeQ[eps].Last().i;
                    firstOrderedMultitude.elements[k - eps - 1] = supportingMultitudeQ[eps].Last().j;
                }
                firstOrderedMultitude.elements[indexOfFirstPlunk] = supportingMultitudeF[indexOfFirstPlunk].Last().index;
                #endregion
                //подсчет веса
                foreach (int element in firstOrderedMultitude.elements)
                {
                    foreach (WeightCoefficient weight in Program.weights)
                    {
                        if (weight.index == element)
                        {
                            firstOrderedMultitude.weight += weight.weight;
                        }
                    }
                }
            }
            else
            {
                //инициализация
                #region
                firstOrderedMultitude = new OrderedMultitude(0, new List<int>());
                int k = indexOfFirstPlunk * 2;
                for (int i = 0; i < k; i++)
                {
                    firstOrderedMultitude.elements.Add(new int());
                }
                #endregion
                //заполнение
                #region
                for (int eps = 0; eps < indexOfFirstPlunk; eps++)
                {
                    firstOrderedMultitude.elements[eps] = supportingMultitudeQ[eps].Last().i;
                    firstOrderedMultitude.elements[k - eps - 1] = supportingMultitudeQ[eps].Last().j;
                }
                #endregion
                //подсчет веса
                foreach (int element in firstOrderedMultitude.elements)
                {
                    foreach (WeightCoefficient weight in Program.weights)
                    {
                        if (weight.index == element)
                        {
                            firstOrderedMultitude.weight += weight.weight;
                        }
                    }
                }
            }
        }
        private static void OrderSecondMultitude()
        {
            if (centralSecondElement[indexOfSecondPlunk].Count != 0)
            {
                //инициализация
                #region
                secondOrderedMultitude = new OrderedMultitude(0, new List<int>());
                int k = indexOfSecondPlunk * 2 + 1;
                for (int i = 0; i < k; i++)
                {
                    secondOrderedMultitude.elements.Add(new int());
                }
                #endregion
                //заполнение
                #region
                for (int eps = 0; eps < indexOfSecondPlunk; eps++)
                {
                    secondOrderedMultitude.elements[eps] = helpfullMultitudeQ[eps].Last().i;
                    secondOrderedMultitude.elements[k - eps - 1] = helpfullMultitudeQ[eps].Last().j;
                }
                secondOrderedMultitude.elements[indexOfSecondPlunk] = centralSecondElement[indexOfSecondPlunk].Last().index;
                #endregion
                //подсчет веса
                foreach (int element in secondOrderedMultitude.elements)
                {
                    foreach (WeightCoefficient weight in Program.weights)
                    {
                        if (weight.index == element)
                        {
                            secondOrderedMultitude.weight += weight.weight;
                        }
                    }
                }
            }
            else
            {
                //инициализация
                #region
                secondOrderedMultitude = new OrderedMultitude(0, new List<int>());
                int k = indexOfSecondPlunk * 2;
                for (int i = 0; i < k; i++)
                {
                    secondOrderedMultitude.elements.Add(new int());
                }
                #endregion
                //заполнение
                #region
                for (int eps = 0; eps < indexOfSecondPlunk; eps++)
                {
                    secondOrderedMultitude.elements[eps] = helpfullMultitudeQ[eps].Last().i;
                    secondOrderedMultitude.elements[k - eps - 1] = helpfullMultitudeQ[eps].Last().j;
                }
                #endregion
                //подсчет веса
                foreach (int element in secondOrderedMultitude.elements)
                {
                    foreach (WeightCoefficient weight in Program.weights)
                    {
                        if (weight.index == element)
                        {
                            secondOrderedMultitude.weight += weight.weight;
                        }
                    }
                }
            }
        }
        private static bool IsEqual(List<PossibleExtension> first, List<PossibleExtension> second)//FIXME: написать норм сравнение
        {
            int ind = 0;
            if (second.Count == first.Count)
            {
                foreach (PossibleExtension possibleExtension in first)
                {
                    if (!possibleExtension.Equals(second[ind]))
                    {
                        return false;
                    }
                    ind++;
                }
                return true;
            }
            else return false;
        }

    }
}
