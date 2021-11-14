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
    public partial class OpenGoogleTable : Form
    {
        private Form1 _form1 { get; set; }
        public OpenGoogleTable(Form form)
        {
            _form1 = (Form1)form;
            InitializeComponent();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            string userText = textBox1.Text;
            Uri uriResult;
            bool result = Uri.TryCreate(userText, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (result && userText.Contains("docs.google.com/spreadsheets/d/"))
            {
                string id = userText.Substring(userText.IndexOf("d/") + 2);
                id = id.Substring(0, id.IndexOf('/'));
                Close();
                SelectGoogleSheetName chooseGoogleSheet = new SelectGoogleSheetName(_form1, id);
                chooseGoogleSheet.Show();
            }
            else
            {
                MessageBox.Show("Ваша ссылка неверна", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenGoogleTable_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            _form1.Enabled = true;
            _form1.Focus();
        }
    }
}
