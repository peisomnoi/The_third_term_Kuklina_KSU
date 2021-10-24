using laba3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;

namespace laba2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double[] x;
        double[] y;
        int Model = 1;
        //Валидация ручного ввода данных в ячейки
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }
        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!Char.IsNumber(e.KeyChar)) && (e.KeyChar != ','))
            {
                if ((e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Delete))
                { 
                    e.Handled = true; 
                }
            }

        }
        private void GenerateData_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            x = new double[200];
            y = new double[200];
            Random rand = new Random();
            //Генерируем случайнык данные
            for (int i = 0; i < 200; i++)
            {
                dataGridView1.Rows.Add();
                if (i < 20)
                {
                    y[i] = rand.Next(0, 4);
                }
                if (i >= 20 && i < 40)
                {
                    y[i] = rand.Next(2, 63);
                }
                if (i >= 40 && i < 60)
                {
                    y[i] = rand.Next(42, 103);
                }
                if (i >= 60 && i < 80)
                {
                    y[i] = rand.Next(60, 121);
                }
                if (i >= 80 && i < 100)
                {
                    y[i] = rand.Next(78, 139);
                }
                if (i >= 100 && i < 120)
                {
                    y[i] = rand.Next(96, 157);
                }
                if (i >= 120 && i < 140)
                {
                    y[i] = rand.Next(114, 175);
                }
                if (i >= 140 && i < 160)
                {
                    y[i] = rand.Next(132, 193);
                }
                if (i >= 160 && i < 180)
                {
                    y[i] = rand.Next(150, 211);
                }
                if (i >= 180 && i < 200)
                {
                    y[i] = rand.Next(168, 229);
                }
                dataGridView1.Rows[i].Cells[1].Value = y[i];
                x[i] = i + 1;
                dataGridView1.Rows[i].Cells[0].Value = x[i];
            }
        }
        // Чистим dataGridView и Chart
        private void Clear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            ClearChart();
        }
        //Валидация введенных данных в таблицу
        public bool ValidateDataGrid()
        {
            int RowsCount = dataGridView1.Rows.Count;
            //Если массивы пусты, то инициализируем их
            if (x == null || y == null)
            {
                x = new double[RowsCount];
                y = new double[RowsCount];
            }
            //по умолчанию уже одна строка есть
            if (RowsCount == 1)
            {
                MessageBox.Show("Данные отсутствуют", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            for(int i=1; i < RowsCount-1; i++)
            {
                var X = dataGridView1.Rows[i].Cells[0].Value;
                var Y = dataGridView1.Rows[i].Cells[1].Value;
                //Если хоть одна ячейка пуста, то выдаем ошибку
                if (X == null || Y == null)
                {
                    MessageBox.Show("Некоторые ячейки не заполнены!", "Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    //Заполняем массивы, если все ок
                    x[i - 1] = Convert.ToDouble(X);
                    y[i - 1] = Convert.ToDouble(Y);
                }
            }
            return true;
        }
        //Валидация импортированных данных
        public bool ValidateImportedData(IList<IList<object>> xy)
        {
            dataGridView1.Rows.Clear();
            if (xy == null || xy[0].Count() != 2)
            {
                MessageBox.Show("Некорректные данные", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            int count = xy.Count();
            x = new double[count];
            y = new double[count];
            for (int i = 0; i < count; i++)
            {
                if (xy[i][0] != null && xy[i][1] != null)
                {
                    //Делаем проверку на число
                    if (!double.TryParse(xy[i][0].ToString(), out x[i]) || !double.TryParse(xy[i][1].ToString(), out y[i]))
                    {
                        MessageBox.Show("Некорректные данные", "Ошибка!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            //Если все ок, то записываем данные в ячейки dataGridView
            for (int i = 0; i < count; i++)
            {
                dataGridView1.Rows.Add();
                for (int j = 0; j < 2; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = Convert.ToDouble(xy[i][j]);
                }
            }
            return true;
        }
        //Реализация рассчетов с помощью метода наименьших квадратов
        private void Calculate_Click(object sender, EventArgs e)
        {
            if (ValidateDataGrid())
            {
                double[] coef = MNK(x, y, Model);              
                //Рисуем графики
                CreateChart(coef);
                //Показываем MessageBox с результатами
                if(Model == 1)
                {
                    MessageBox.Show("k = " + coef[0].ToString() + " и b = " + coef[1].ToString(), "Результат",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if(Model == 2)
                {
                    MessageBox.Show("a = " + coef[0].ToString() + "; b = "+ coef[1].ToString()+ "; c = " + coef[2].ToString(), "Результат",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        public double[] MNK(double[] X, double[] Y, int model)
        {
            double SumX = 0;
            double SumY = 0;
            double SumX2 = 0;
            double SumXY = 0;
            double SumX2Y = 0;
            double SumX3 = 0;
            double SumX4 = 0;
            int N = Y.Length;
            double[] coef = new double[N];
            for (int i = 0; i < N; i++)
            {
                SumX += X[i];
                SumY += Y[i];
                SumX2 += X[i] * X[i];
                SumXY += X[i] * Y[i];
                SumX2Y += X[i] * X[i] * Y[i];
                SumX3 += Math.Pow(X[i], 3);
                SumX4 += Math.Pow(X[i], 4);
            }
            //Линейная модель
            if (model == 1)
            {
                double D = 1 / (N * SumX2 - SumX * SumX);
                double k = D * (N * SumXY - SumX * SumY);
                double b = D * (-SumX * SumXY + SumX2 * SumY);
                coef[0] = Math.Round(k, 2);
                coef[1] = Math.Round(b, 2);
            }
            //Параболическая модель
            if (model == 2)
            {
                double D = 1 / (SumX2 * (SumX4 * N + 2 * SumX3 * SumX - Math.Pow(SumX2, 2)) - Math.Pow(SumX3, 2) * N - SumX4 * Math.Pow(SumX, 2));
                double a = D * (SumX2Y * (N * SumX2 - Math.Pow(SumX, 2)) + SumXY * (-SumX3 * N + SumX * SumX2) + SumY * (SumX3 * SumX - Math.Pow(SumX2, 2)));
                double b = D * (SumX2Y * (-N * SumX3 + SumX2 * SumX) + SumXY * (N * SumX4 - Math.Pow(SumX2, 2)) + SumY * (-SumX4 * SumX + SumX3 * SumX2));
                double c = D * (SumX2Y * (SumX3 * SumX - Math.Pow(SumX2, 2)) + SumXY * (-SumX4 * SumX + SumX3 * SumX2) + SumY * (SumX4 * SumX2 - Math.Pow(SumX3, 2)));
                coef[0] = Math.Round(a, 9);
                coef[1] = Math.Round(b, 9);
                coef[2] = Math.Round(c, 9);
            }
            return coef;
        }
        private void OpenExel_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            //Пишем необходимые форматы
            opf.Filter = "XML Files (*.xml; *.xls; *.xlsx; *.xlsm; *.xlsb) |*.xml; *.xls; *.xlsx; *.xlsm; *.xlsb";
            //Открываем окно выбора файла
            if (opf.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = opf.FileName;
            //получаем доступ к нашему Exel файлу с помощью библиотеки Microsoft.Office.Interop.Excel
            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Open(filename);
            Excel.Worksheet worksheet = workbook.ActiveSheet;
            Excel.Range range = worksheet.UsedRange;
            // массив со всеми данными
            var ExcelArray = (object[,])range.Value;
            IList<IList<object>> excelList = new List<IList<object>>();
            int RowsCount = range.Rows.Count - 1;
            int ColumnCount = range.Columns.Count;
            for (int i = 1; i <= RowsCount; i++)
            {
                excelList.Add(new object[] { ExcelArray[i, 1], ExcelArray[i, 2] });
            }
            //Делаем валидацию данных
            ValidateImportedData(excelList);
            //Завершаем работу с файлом Excel
            workbook.Close(false, Type.Missing, Type.Missing);
            app.Workbooks.Close();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        }
        public void CreateChart(double[] coef)
        {
            double Yrasch = 0;
            //очищаем график
            ClearChart();
            //Получаем два графика, созданных вручную через конструктор
            Series sourceChart = chart1.Series[0];
            Series resultChart = chart1.Series[1];
            for (int i = 0; i < x.Length; i++)
            {
                //Добавляем точки на график с исходными данными x и y
                sourceChart.Points.AddXY(x[i], y[i]);
                //Находим y рассчетное
                if(Model == 1)
                {
                    Yrasch = coef[0] * x[i] + coef[1];
                }
                else
                {
                    Yrasch = coef[0] * x[i]*x[i] + coef[1]*x[i] + coef[2];
                }
                //Строим второй график y=kx+b с найденными коэффициентами
                resultChart.Points.AddXY(x[i], Yrasch);
                //Записываем y рассчетное в таблицу dataGridView
                dataGridView1.Rows[i].Cells[2].Value = Yrasch;
            }
        }
        public void ClearChart()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
        }

        private void GoogleTable_Click(object sender, EventArgs e)
        {
            OpenGoogleTable openGoogleTable = new OpenGoogleTable(this);
            openGoogleTable.Show();
            Enabled = false;
        }

        private void LineModel_Click(object sender, EventArgs e)
        {
            Model = 1;
            Series resultChart = chart1.Series[1];
            resultChart.LegendText = "y=kx + b";
        }

        private void ParabModel_Click(object sender, EventArgs e)
        {
            Model = 2;
            Series resultChart = chart1.Series[1];
            resultChart.LegendText = "y = ax^2 + bx + c";
        }
    }
}
