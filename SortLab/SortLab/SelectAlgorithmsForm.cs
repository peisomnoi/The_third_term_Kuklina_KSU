using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SortLab
{
    public partial class SelectAlgorithmsForm : Form
    {
        public int SortBy;
        private Chart Chart;
        public double[] Array;
        private RichTextBox TextBox;
        List<Thread> threads;
        public SelectAlgorithmsForm(Chart chart, double[] array, RichTextBox textBox)
        {
            Chart = chart;
            Array = array;
            TextBox = textBox;
            InitializeComponent();
        }

        private void StartAlgorithms_Click(object sender, EventArgs e)
        {
            TextBox.Invoke((MethodInvoker)delegate
            {
                TextBox.Clear();
            });
            threads = new List<Thread>();
            SortBy = ((KeyValuePair<string,int>)comboBox1.SelectedItem).Value;
            foreach (var item in SortCheckedListBox.CheckedItems)
            {
                Sort sort;
                double[] tempArray = (double[])Array.Clone();
                switch (item)
                {
                    case "Пузырьковая":
                        sort = new BubbleSort(tempArray, Chart);
                        break;
                    case "Вставками":
                        sort = new InsertionSort(tempArray, Chart);
                        break;
                    case "Шейкерная":
                        sort = new ShakerSort(tempArray, Chart);
                        break;
                    case "Быстрая":
                        sort = new QuickSort(tempArray, Chart);
                        break;
                    case "BOGO":
                        sort = new BogoSort(tempArray, Chart);
                        break;
                    default:
                        sort = new QuickSort(tempArray, Chart);
                        break;
                }
                Thread thread = new Thread(() => { SortArray(sort,item.ToString()); });
                threads.Add(thread);
                thread.Start();
            }
        }
        public void SortArray(Sort sort, string SortName)
        {
            //создаем объект
            Stopwatch stopwatch = new Stopwatch();
            //засекаем время начала операции
            stopwatch.Start();
            double[] sortedArray;
            if (SortBy == 1)
            {
                sortedArray = sort.SortByAscending();
            }
            else
            {
                sortedArray = sort.SortByDescending();
            }
            stopwatch.Stop();
            //смотрим сколько миллисекунд было затрачено на выполнение
            string result =
                "Время(мс): " + stopwatch.Elapsed.TotalMilliseconds + "\n" +
                "Количество итераций: " + sort.IterationsCount + "\n\n";
            Console.WriteLine(SortName + "\n" + result);
            TextBox.Invoke((MethodInvoker)delegate
            {
                TextBox.AppendText(SortName + "\n", Color.Green);
                TextBox.AppendText(result);
                TextBox.Update();
            });
        }

        private void SelectAlgorithmsForm_Load(object sender, EventArgs e)
        {
            Dictionary<string, int> comboBoxSource = new Dictionary<string, int>(2);
            comboBoxSource.Add("По возрастанию", 1);
            comboBoxSource.Add("По убыванию" ,- 1);
            comboBox1.DataSource = new BindingSource(comboBoxSource, null);
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
        }
        private void StopButton_Click(object sender, EventArgs e)
        {
            AbortThreads();
        }
        private void SelectAlgorithmsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AbortThreads();
        }
        //Остановка потоков
        public void AbortThreads()
        {
            foreach (var thread in threads)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }
        }
    }
}
