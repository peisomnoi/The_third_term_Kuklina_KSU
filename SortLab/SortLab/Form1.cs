using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace SortLab
{
    public partial class Form1 : Form
    {
        public double[] array;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GenerateArray(0, 1200, 50);
        }
        //Валидация ручного ввода данных в ячейки
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }
        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!Char.IsNumber(e.KeyChar)) && (e.KeyChar != ',') && (e.KeyChar != '-'))
            {
                if ((e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Delete))
                {
                    e.Handled = true;
                }
            }

        }
        public void InitChart()
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
                series.ChartArea = chart1.ChartAreas[chart1.Series.IndexOf(series)].Name;
                for (int i = 0; i < array.Length; i++)
                {
                    series.Points.AddXY(i, array[i]);
                }
            }
        }
        public void GenerateArray(int start, int end, int count)
        {
            Random rnd = new Random();
            array = new double[count];
            dataGridView1.Rows.Clear();
            for (int i = 0; i < count; i++)
            {
                array[i] = rnd.Next(start, end);
                dataGridView1.Rows.Add(array[i]);
            }
            InitChart();
            //array = new double[] { 234, 123, 1100, -100, -400 };
        }

        private void GenerateData_Click(object sender, EventArgs e)
        {
            SelectDataInterval selectDataInterval = new SelectDataInterval(this);
            selectDataInterval.Show();
        }

        private void SelectAlgorithms_Click(object sender, EventArgs e)
        {
            if (ValidateDataGrid())
            {
                SelectAlgorithmsForm selectAlgorithmsForm = new SelectAlgorithmsForm(chart1, array, richTextBox1);
                selectAlgorithmsForm.Show(this);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int count = dataGridView1.Rows.Count-1;
            array = new double[count];
            for(int i=0; i < count; i++)
            {
                array[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value);
            }
            InitChart();
        }

        private void OpenExсel_Click(object sender, EventArgs e)
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
            var ExcelArray = range.Value;
            IList<IList<object>> excelList = new List<IList<object>>();
            int RowsCount = range.Rows.Count;
            int ColumnCount = range.Columns.Count;
            for (int i = 1; i <= RowsCount; i++)
            {
                for (int j = 1; j <= ColumnCount; j++)
                {
                    excelList.Add(new object[] { ExcelArray[i, j] });
                }
            }
            //Делаем валидацию данных
            ValidateImportedData(excelList);
            //Завершаем работу с файлом Excel
            workbook.Close(false, Type.Missing, Type.Missing);
            app.Workbooks.Close();
            Marshal.ReleaseComObject(workbook);
        }

        private void GoogleTable_Click(object sender, EventArgs e)
        {
            OpenGoogleTable openGoogleTable = new OpenGoogleTable(this);
            openGoogleTable.Show();
            Enabled = false;
        }
        //Валидация введенных данных в таблицу
        public bool ValidateDataGrid()
        {
            int RowsCount = dataGridView1.Rows.Count;
            //по умолчанию уже одна строка есть
            if (RowsCount == 1)
            {
                MessageBox.Show("Данные отсутствуют", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            for (int i = 1; i < RowsCount - 1; i++)
            {
                var X = dataGridView1.Rows[i].Cells[0].Value;
                //Если хоть одна ячейка пуста, то выдаем ошибку
                if (X == null)
                {
                    MessageBox.Show("Некоторые ячейки не заполнены!", "Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
        //Валидация импортированных данных
        public bool ValidateImportedData(IList<IList<object>> data)
        {
            dataGridView1.Rows.Clear();
            List<double> tempArray = new List<double>();
            double tempElem;
            foreach(var obj in data)
            {
                foreach(var item in obj)
                {
                    if (item != null)
                    {
                        //Делаем проверку на число
                        if (!double.TryParse(item.ToString(), out tempElem))
                        {
                            MessageBox.Show("Некорректные данные", "Ошибка!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        tempArray.Add(tempElem);
                    }
                }
            }
            //Если все ок, то записываем данные в ячейки dataGridView и в массив
            array = tempArray.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                dataGridView1.Rows.Add(array[i]);
            }
            InitChart();
            return true;
        }

        private void ClearTable_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            array = new double[0];
            InitChart();
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
