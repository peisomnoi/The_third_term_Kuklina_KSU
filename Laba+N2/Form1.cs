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
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace Laba_N1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void CalcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double result ;
            string CalcStr ;
            DialogResult msgresult;
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Add(new ChartArea("Function"));
            Series mySeriesOfPoint = new Series("Function")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Function"
            };
            double A = Convert.ToDouble(textBox1.Text.Replace(".", ","));
            double B = Convert.ToDouble(textBox2.Text.Replace(".", ","));
            double E = Convert.ToDouble(textBox3.Text.Replace(".",","));
            if (E < 0.0001) 
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                msgresult = MessageBox.Show("Задана высокая точность! для вычислений может не хватить ресурсов компьютера!. Продолжить?", "Предупреждение", buttons);
                if (msgresult == System.Windows.Forms.DialogResult.No)
                {
                    return;                
                }
            }
            if ((A > B) || (E <= 0))
                {
                    textBox5.Text="Ошибка ввода";
                    //break
                }
            string FormulaStr = textBox4.Text.ToLower();
            for (double x = A; x <= B; x += E)
                {
                    CalcStr = FormulaStr.Replace("x", x.ToString());
                try
                {
                    //Обходим деление на 0
                    result = Expr.Parse(CalcStr.Replace(",", ".")).RealNumberValue;
                    mySeriesOfPoint.Points.AddXY(x, result);
                }
                catch {
                    chart1.Series.Add(mySeriesOfPoint);
                    mySeriesOfPoint = new Series("Function"+x.ToString())
                    {
                        ChartType = SeriesChartType.Line,
                        ChartArea = "Function"
                    };
                    
                }

                    
            }
            chart1.Series.Add(mySeriesOfPoint);
            MinimumToolStripMenuItem.Enabled = true;
        }


        private void MinimumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double A = Convert.ToDouble(textBox1.Text.Replace(".", ","));
            double B = Convert.ToDouble(textBox2.Text.Replace(".", ","));
            double E = Convert.ToDouble(textBox3.Text.Replace(".", ","));
            double x = A;

            string FormulaStr = textBox4.Text.ToLower();
            string CalcStr;
            string CalcStrStart;
            string CalcStrFinish;
            double result;
            double y;
            double resultStart;
            double resultFinish;
            double startspot = A;
            double finishspot = B;
            double length = finishspot - startspot;
            Series mySeriesOfPoint = new Series("Round");

            while (length > E)
            {
                x = (startspot + finishspot) / 2;
                
                CalcStr = FormulaStr.Replace("x", x.ToString()).Replace(",", ".");
                CalcStrStart = FormulaStr.Replace("x", startspot.ToString()).Replace(",",".");
                CalcStrFinish = FormulaStr.Replace("x", finishspot.ToString()).Replace(",", ".");
                try
                {
                    //Обходим деление на 0
                    result = Expr.Parse(CalcStr).RealNumberValue;
                    resultStart = Expr.Parse(CalcStrStart).RealNumberValue;
                    resultFinish = Expr.Parse(CalcStrFinish).RealNumberValue;

                    if (result > resultStart)
                    {
                        finishspot = x;
                    }
                    else
                    {
                        startspot = x;
                    }
                }
                catch
                {
                    finishspot -= E;
                }
                // Вычисляем новую длинну.
                length = (finishspot - startspot);
            }
            textBox5.Text = x.ToString();
            //Устанавливаем найденную точку на графике
            CalcStr = FormulaStr.Replace("x", x.ToString().Replace(",", "."));
            y = Expr.Parse(CalcStr).RealNumberValue;
            //Выделяем точку
            mySeriesOfPoint.Points.AddXY(x, y);
            chart1.Series.Add(mySeriesOfPoint);
            MinimumToolStripMenuItem.Enabled = false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 44 && ch != 45 && ch != 127 && ch != 46)
            {
                textBox5.Text = "Ошибка ввода числа A";
                e.Handled = true;
            }
            else {
                textBox5.Text = "";
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 44 && ch != 45 && ch != 127 && ch != 46)
            {
                textBox5.Text = "Ошибка ввода числа B";
                e.Handled = true;
            }
            else
            {
                textBox5.Text = "";
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 44 && ch != 45 && ch != 127 && ch != 46)
            {
                textBox5.Text = "Ошибка ввода числа E";
                e.Handled = true;
            }
            else
            {
                textBox5.Text = "";
            }
        }
    }
}
