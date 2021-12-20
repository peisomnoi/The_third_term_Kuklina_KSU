using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{
    public partial class GenerateData : Form
    {
        Form1 Form1 { get; set; }
        public GenerateData(Form1 form1)
        {
            Form1 = form1;
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            int N = (int)numericUpDown1.Value;
            Random rand = new Random();
            double[,] Matrix = new double[N,N +1];
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < N; j++)
                {
                    Matrix[i, j] = rand.Next(-15, 15);
                }
                Matrix[i,N] = rand.Next(-15, 15);
            }
            Form1.InitDataGrid(Matrix, N);
        }

        private void GenerateData_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.Enabled = true;
            Form1.Focus();
        }
    }
}
