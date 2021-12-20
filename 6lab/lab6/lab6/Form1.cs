using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace lab6
{
    public partial class Form1 : Form
    {
        public double[,] Matrix;
        public int N;
        public double[] X;
        public bool isExport = false;
        public Form1()
        {
            InitializeComponent();
        }

        public void InitDataGrid(double[,] matrix, int n)
        {
            N = n;
            Matrix = matrix;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            for (int i = 0; i < N; i++)
            {
                string columnName = "a" + (i + 1);
                dataGridView1.Columns.Add(columnName, columnName);
                dataGridView1.Rows.Add();
            }
            dataGridView1.Columns.Add("b", "b");
            dataGridView1.Columns.Add("x", "X");
            for (int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = Matrix[i, j];
                }
                dataGridView1.Rows[i].Cells[N].Value = Matrix[i, N];
            }

        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Matrix[i, j] = Convert.ToDouble(dataGridView1.Rows[i].Cells[j].Value);
                }
                Matrix[i,N] = Convert.ToDouble(dataGridView1.Rows[i].Cells[N].Value);
            }
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

        private void GenerateData_Click(object sender, EventArgs e)
        {
            GenerateData generateData = new GenerateData(this);
            generateData.Show();
            this.Enabled = false;
        }

        private void ImportExcel_Click(object sender, EventArgs e)
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
                object[] obj = new object[ColumnCount];
                for (int j = 1; j <= ColumnCount; j++)
                {
                    obj[j - 1] = ExcelArray[i, j];
                }
                excelList.Add(obj);
            }
            ////Делаем валидацию данных
            ValidateImportedData(excelList);
            //Завершаем работу с файлом Excel
            workbook.Close(false, Type.Missing, Type.Missing);
            app.Workbooks.Close();
            Marshal.ReleaseComObject(workbook);
        }
        //Валидация импортированных данных
        public bool ValidateImportedData(IList<IList<object>> data)
        {
            int n = data.Count;
            double[,] matrix = new double[n, n + 1];
            double[] b = new double[n];
            double tempElem = 0;
            for (int i = 0; i < n; i++)
            {
                if ((data[i].Count - 1) != data.Count)
                {
                    MessageBox.Show("Количество строк и столбцов в Матрице должно совпадать!", "Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                for (int j = 0; j < data[i].Count; j++)
                {
                    if (data[i][j] != null)
                    {
                        //Делаем проверку на число
                        if (!double.TryParse(data[i][j].ToString(), out tempElem))
                        {
                            MessageBox.Show("Некорректные данные", "Ошибка!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        if (j < (data[i].Count - 1))
                        {
                            matrix[i, j] = tempElem;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пустая ячейка", "Ошибка!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            InitDataGrid(matrix, n);
            return true;
        }
            //Валидация введенных данных в таблицу
        public bool ValidateDataGrid()
        {
            int RowsCount = dataGridView1.Rows.Count;
            int ColumnCount = dataGridView1.Columns.Count - 1;
            if (RowsCount == 0)
            {
                MessageBox.Show("Данные отсутствуют", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            for (int i = 0; i < RowsCount - 1; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    var X = dataGridView1.Rows[i].Cells[j].Value;
                    //Если хоть одна ячейка пуста, то выдаем ошибку
                    if (X == null)
                    {
                        MessageBox.Show("Некоторые ячейки не заполнены!", "Ошибка!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }
        private void ImportGoogleTable_Click(object sender, EventArgs e)
        {
            OpenGoogleTable openGoogleTable = new OpenGoogleTable(this);
            openGoogleTable.Show();
            Enabled = false;
        }

        private void SelectMethods_Click(object sender, EventArgs e)
        {
            if (ValidateDataGrid())
            {
                SelectAlgorithmsForm selectAlgorithmsForm = new SelectAlgorithmsForm(this);
                selectAlgorithmsForm.Show();
            }
        }
        public void OutputResults(double[] X, string results, string methodName)
        {
            this.X = X;
            string x = "";
            if (X != null)
            {
                dataGridView1.Invoke((MethodInvoker)delegate
                {
                    for (int i = 0; i < N; i++)
                    {
                        x += X[i] + ";  ";
                        dataGridView1.Rows[i].Cells[N + 1].Value = X[i];
                    }
                });
            }
            richTextBox1.Invoke((MethodInvoker)delegate
            {
                richTextBox1.AppendText("\n" + methodName + "\n", Color.Green);
                richTextBox1.AppendText(results + "\nРешения" + "\n" + x + "\n");
                richTextBox1.Update();
            });
        }
        public void ClearTextBox()
        {
            richTextBox1.Invoke((MethodInvoker)delegate
            {
                richTextBox1.Clear();
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Matrix = new double[,]
            {
                {10.8000, 0.0475, 0, 0, 12.1430},
                {0.0321, 9.9000, 0.0523, 0, 13.0897},
                {0, 0.0369, 9.0000, 0.0570, 13.6744},
                {0,0, 0.0416, 8.1000, 13.8972}
            };
            InitDataGrid(Matrix, 4);
        }

        private void ExportExcel_Click(object sender, EventArgs e)
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
            Excel.Sheets Sheets = workbook.Sheets;
            bool isExist = false;
            Excel.Worksheet sheet = null;
            foreach (Excel.Worksheet item in Sheets)
            {
                if(item.Name == "Result List")
                {
                    sheet = item;
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                sheet = (Excel.Worksheet)Sheets.Add(Sheets[1], Type.Missing, Type.Missing, Type.Missing);
                sheet.Name = "Result List";
            }
            ExportDataToExcelSheet(sheet);
            workbook.Save();
            workbook.Close(Type.Missing, Type.Missing, Type.Missing);
            app.Quit();
            Marshal.ReleaseComObject(sheet);
            Marshal.ReleaseComObject(Sheets);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(app);
            app = null;
        }
        public void ExportDataToExcelSheet(Excel.Worksheet sheet)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N + 1; j++)
                {
                    sheet.Cells[i + 1, j + 1] = Matrix[i, j];
                }
                if (X != null)
                {
                    sheet.Cells[i + 1, N + 2] = X[i];
                }
            }
        }

        private void ExportGoogleTable_Click(object sender, EventArgs e)
        {
            isExport = true;
            OpenGoogleTable openGoogleTable = new OpenGoogleTable(this);
            openGoogleTable.Show();
        }
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