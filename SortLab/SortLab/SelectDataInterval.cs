using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SortLab
{
    public partial class SelectDataInterval : Form
    {
        public Form1 Form1;
        public SelectDataInterval(Form1 form1)
        {
            Form1 = form1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
                int count = (int)numericUpDown3.Value;
                Form1.GenerateArray(start, end, count);
            }
        }
    }
}
