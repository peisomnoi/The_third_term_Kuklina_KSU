using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace SortLab
{
    public abstract class Sort
    {
        public double[] Array { get; set; }
        public int ArrayLength { get; set; }
        public Chart Chart { get; set; }
        public int IterationsCount { get; set; }
        public Sort(double[] myArray, Chart chart)
        {
            Array = myArray;
            ArrayLength = myArray.Length;
            Chart = chart;
            IterationsCount = 0;
        }
        //Метод перестановки элементов
        public void Swap(int i, int j, Series series)
        {
            double temp = Array[i];
            Array[i] = Array[j];
            Array[j] = temp;
            UpdateChart(series, i, j);
            IterationsCount++;
            Thread.Sleep(10);
        }
        // Абстрактный метод, наследники класса должны обязательно реализовать этот метод
        public abstract double[] MakeSort(int SortBy);
        // По возрастанию
        public double[] SortByAscending()
        {
            return MakeSort(1);
        }
        // По убыванию
        public double[] SortByDescending()
        {
            return MakeSort(-1);
        }
        // Обновляем график
        public void UpdateChart(Series series, int i, int j)
        {
            if (Chart.InvokeRequired)
            {
                Chart.Invoke(new Action(() => { 
                    UpdateChart(series, i, j); 
                }));
            }
            else
            {
                // Очищаем и строим снова по обновленному массиву
                series.Points.Clear();
                for (int k = 0; k < ArrayLength; k++)
                {
                    series.Points.AddXY(k, Array[k]);
                }
                series.Points[j].Color = Color.Green;
                series.Points[i].Color = Color.Black;
            }
        }
    }
    class BubbleSort: Sort
    {
        //конструктор класса будет такой же как у родителя
        public BubbleSort(double[] myArray, Chart chart) : base(myArray, chart) { }
        //реализация абстрактного метода 
        public override double[] MakeSort(int SortBy)
        {
            for (int i = 0; i < ArrayLength; i++)
            {
                for (int j = i + 1; j < ArrayLength; j++)
                {
                    if (SortBy*Array[i] > SortBy* Array[j])
                    {
                        Swap(i, j, Chart.Series[0]);
                    }
                }
            }
            return Array;
        }
    }
    class InsertionSort: Sort
    {
        public InsertionSort(double[] myArray, Chart chart) : base(myArray, chart) { }
        public override double[] MakeSort(int SortBy)
        {
            double x;
            int j;
            for (int i = 1; i < ArrayLength; i++)
            {
                x = Array[i];
                j = i;
                while (j > 0 && SortBy* Array[j - 1] > SortBy*x)
                {
                    Swap(j, j - 1, Chart.Series[1]);
                    j--;
                }
                Array[j] = x;
                IterationsCount++;
            }
            return Array;
        }
    }
    class ShakerSort: Sort
    {
        public ShakerSort(double[] myArray, Chart chart) : base(myArray, chart) { }
        public override double[] MakeSort(int SortBy)
        {
            int left = 0,
                 right = ArrayLength - 1;
            while (left < right)
            {
                for (int i = left; i < right; i++)
                {
                    if (SortBy* Array[i] > SortBy* Array[i + 1])
                    {
                        Swap(i, i + 1, Chart.Series[2]);
                    }
                }
                right--;
                for (int i = right; i > left; i--)
                {
                    if (SortBy* Array[i - 1] > SortBy* Array[i])
                    {
                        Swap(i - 1, i, Chart.Series[2]);
                    }
                }
                left++;
            }
            return Array;
        }
    }
    class QuickSort: Sort
    {
        public QuickSort(double[] myArray, Chart chart) : base(myArray, chart) { }
        public override double[] MakeSort(int SortBy)
        {
            return quickSort(Array, 0, ArrayLength - 1, SortBy);
        }
        //метод возвращающий индекс опорного элемента
        public int Partition(int minIndex, int maxIndex, int SortBy)
        {
            int pivot = minIndex - 1;
            for (int i = minIndex; i < maxIndex; i++)
            {
                if (SortBy* Array[i] < SortBy* Array[maxIndex])
                {
                    pivot++;
                    Swap(pivot, i, Chart.Series[3]);
                }
            }

            pivot++;
            Swap(pivot, maxIndex, Chart.Series[3]);
            return pivot;
        }
        //быстрая сортировка
        public double[] quickSort(double[] array, int minIndex, int maxIndex, int SortBy)
        {
            if (minIndex >= maxIndex)
            {
                return array;
            }
            int pivotIndex = Partition(minIndex, maxIndex, SortBy);
            quickSort(array, minIndex, pivotIndex - 1, SortBy);
            quickSort(array, pivotIndex + 1, maxIndex, SortBy);
            return array;
        }
    }
    class BogoSort: Sort
    {
        public BogoSort(double[] myArray, Chart chart) : base(myArray, chart) { }
        //метод для проверки упорядоченности массива
        public bool IsSorted(int SortBy)
        {
            for (int i = 0; i < ArrayLength - 1; i++)
            {
                if (SortBy* Array[i] > SortBy* Array[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        //перемешивание элементов массива
        public void RandomPermutation()
        {
            Random random = new Random();
            int n = ArrayLength;
            while (n > 1)
            {
                n--;
                int i = random.Next(n + 1);
                Swap(i, n, Chart.Series[4]);
            }
        }
        public override double[] MakeSort(int SortBy)
        {
            while (!IsSorted(SortBy))
            {
                RandomPermutation();
            }
            return Array;
        }
    }
}
