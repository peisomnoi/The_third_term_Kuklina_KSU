using org.mariuszgromada.math.mxparser;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wolfram.Alpha;
using Wolfram.Alpha.Models;

namespace IntegralLab
{
    public partial class Form1 : Form
    {
        public string function;
        public double a;
        public double b;
        public double eps;
        public Form1()
        {
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            function = "f(x)=sin(x)";
            a = 1;
            b = 10;
            eps = 0.001;
            InitChart();
            await WolframAsync("sin(x)");
        }
        public async Task WolframAsync(string function)
        {
            WolframAlphaService service = new WolframAlphaService("4Y5H7V-4R24UYPQEH");
            WolframAlphaRequest request = new WolframAlphaRequest("Integral" + function);
            WolframAlphaResult result = await service.Compute(request);
            string integral = result.QueryResult.Pods[0].SubPods[0].Plaintext;
            richTextBox1.AppendText(integral + "\n");
        }
        public void InitChart()
        {
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            Function f = new Function(function);
            for (double i = a; i <= b; i += 0.1)
            {
                for(int k = 0; k < chart1.Series.Count; k += 2)
                {
                    chart1.Series[0].Points.AddXY(i, f.calculate(i));
                    chart2.Series[0].Points.AddXY(i, f.calculate(i));
                    chart3.Series[0].Points.AddXY(i, f.calculate(i));
                }
            }
        }
        public void ClearChart()
        {
            foreach(var series in chart1.Series)
            {
                series.Points.Clear();
            }
        }

        private void SelectMethods_Click(object sender, EventArgs e)
        {
            SelectMethodsForm selectAlgorithmsForm = new SelectMethodsForm(this, chart1, chart2, chart3, richTextBox1);
            selectAlgorithmsForm.Show();
        }

        private void FunctionButton_Click(object sender, EventArgs e)
        {
            InputFunction inputFunction = new InputFunction(this);
            Enabled = false;
            inputFunction.Show();
        }
    }
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
