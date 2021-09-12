using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxResult.Text = Convert.ToString(ResultXk(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text)));
            }
            catch
            {
                textBoxResult.Text = "Ошибка ввода!";
            }
        }

        public double ResultXk(double A, double B, double Y1, double Y2)
        {
            double X1;
            double X2;
            if (A != 0)
            {
                X1 = (Y1 - B) / A;
                X2 = (Y2 - B) / A;
            }
            else
            {
                X1 = 0;
                X2 = 0;
            }
            return (X1+X2)/2;
        }

    }
}
