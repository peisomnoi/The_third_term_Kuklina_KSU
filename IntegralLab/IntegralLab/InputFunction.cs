using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IntegralLab
{
    public partial class InputFunction : Form
    {
        public Form1 Form1;
        public InputFunction(Form1 form1)
        {
            Form1 = form1;
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("Поле ввода функции пусто", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int start = (int)numericUpDown1.Value;
                int end = (int)numericUpDown2.Value;
                if (start > end)
                {
                    MessageBox.Show("Стартовое значение не может быть больше конечного!", "Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string function = "f(x)=" + textBox1.Text;
                    double a = (double)numericUpDown1.Value;
                    double b = (double)numericUpDown2.Value;
                    double eps = (double)numericUpDown3.Value;
                    Function f = new Function(function);
                    if (double.IsNaN(f.calculate(a)))
                    {
                        MessageBox.Show("Неверный формат функции", "Ошибка!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Form1.function = function;
                        await Form1.WolframAsync(textBox1.Text);
                        Form1.a = a;
                        Form1.b = b;
                        Form1.eps = eps;
                        Form1.ClearChart();
                        Form1.InitChart();
                        Close();
                    }
                }
            }
        }

        private void InputFunction_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.Enabled = true;
        }
    }
}
