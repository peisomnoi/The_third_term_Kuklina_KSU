using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace lab6
{
    public partial class SelectAlgorithmsForm : Form
    {
        List<Thread> threads;
        public Form1 Form1;
        public SelectAlgorithmsForm(Form1 form1)
        {
            Form1 = form1;
            InitializeComponent();
        }

        private void StartAlgorithms_Click(object sender, EventArgs e)
        {
            //TextBox.Invoke((MethodInvoker)delegate
            //{
            //    TextBox.Clear();
            //});
            Form1.ClearTextBox();
            threads = new List<Thread>();
            double[,] Matrix = Form1.Matrix;
            int N = Form1.N;
            double eps = 0.0000000001;
            foreach (var item in CheckedListBox.CheckedItems)
            {
                SLAE slae;
                switch (item)
                {
                    case "Метод Гаусса":
                        slae = new Gauss(N,Matrix);
                        break;
                    case "Метод квадратного корня":
                        slae = new Cholesky(N, Matrix);
                        break;
                    case "Метод прогонки":
                        slae = new RunThrough(N, Matrix);
                        break;
                    case "Метод простой итерации":
                        slae = new SimpleIteration(N, Matrix, eps);
                        break;
                    case "Метод наискорейшего спуска":
                        slae = new GradientDescent(N, Matrix, eps);
                        break;
                    case "Метод сопряженных градиентов":
                        slae = new ConjugateGradient(N, Matrix, eps);
                        break;
                    default:
                        slae = new Gauss(N, Matrix);
                        break;

                }
                Thread thread = new Thread(() => { CalculateIntegral(slae, item.ToString()); });
                threads.Add(thread);
                thread.Start();
            }
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
            if (threads != null)
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
        public void CalculateIntegral(SLAE slae, string methodName)
        {
            //создаем объект
            Stopwatch stopwatch = new Stopwatch();
            //засекаем время начала операции
            stopwatch.Start();
            double[] X = slae.FindX();
            stopwatch.Stop();
            //Console.WriteLine(methodName + ":\n" + result + "\nОптимальное число разбиений: " + integral.N);
            string result =
                "Время(мс): " + stopwatch.Elapsed.TotalMilliseconds + "\n";
            if (slae.Iterations > 0)
            {
                result += "Количество итераций: " + slae.Iterations + "\n";
            }
            Form1.OutputResults(X, result, methodName);
            //TextBox.Invoke((MethodInvoker)delegate
            //{
            //    TextBox.AppendText(methodName + "\n", Color.Green);
            //    TextBox.AppendText(result);
            //    TextBox.Update();
            //});
        }
    }
}
