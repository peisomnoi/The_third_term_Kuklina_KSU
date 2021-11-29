using IntegralLab;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IntegralLab
{
    public partial class SelectMethodsForm : Form
    {
        List<Thread> threads;
        public Form1 Form1;
        public Chart Chart1;
        public Chart Chart2;
        public Chart Chart3;
        private RichTextBox TextBox;
        int rectType = 0;
        public SelectMethodsForm(Form1 form1, Chart chart1, Chart chart2, Chart chart3, RichTextBox textBox)
        {
            Form1 = form1;
            Chart1 = chart1;
            Chart2 = chart2;
            TextBox = textBox;
            Chart3 = chart3;
            InitializeComponent();
        }

        private void StartAlgorithms_Click(object sender, EventArgs e)
        {
            TextBox.Invoke((MethodInvoker)delegate
            {
                TextBox.Clear();
            });
            threads = new List<Thread>();
            string f = Form1.function;
            double a = Form1.a;
            double b = Form1.b;
            double eps = Form1.eps;
            if (rectType != 0)
            {
                Integral RectIntegral = new RectangleIntegral(f, a, b, eps, Chart1, rectType);
                Thread thread = new Thread(() => { CalculateIntegral(RectIntegral, "Метод прямоугольников"); });
                threads.Add(thread);
                thread.Start();
            }
            foreach (var item in checkedListBox1.CheckedItems)
            {
                Integral integral;
                switch (item)
                {
                    case "Метод трапеций":
                        integral = new TrapezeIntegral(f, a, b, eps, Chart2);
                        break;
                    case "Метод Симпсона":
                        integral = new SympsonIntegral(f, a, b, eps, Chart3);
                        break;
                    default:
                        integral = new SympsonIntegral(f, a, b, eps, Chart3);
                        break;

                }
                Thread thread = new Thread(() => { CalculateIntegral(integral, item.ToString()); });
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
        public void CalculateIntegral(Integral integral, string methodName)
        {
            double result = integral.Calculate();
            Console.WriteLine(methodName + ":\n" + result + "\nОптимальное число разбиений: " + integral.n);
            TextBox.Invoke((MethodInvoker)delegate
            {
                TextBox.AppendText(methodName + "\n", Color.Green);
                TextBox.AppendText(result.ToString() + "\nОптимальное число разбиений: " + integral.n + "\n");
                TextBox.Update();
            });
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            switch (radioButton.Text)
            {
                case "Левых":
                    rectType = 1;
                    break;
                case "Правых":
                    rectType = 2;
                    break;
                case "Центральных":
                    rectType = 3;
                    break;
            }
        }
    }
}
