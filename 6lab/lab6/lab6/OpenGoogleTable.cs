using System;
using System.Windows.Forms;

namespace lab6
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
            if (CheckUrl(userText))
            {
                string id = GetDocumentId(userText);
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
        public string GetDocumentId(string userText)
        {
            string id = userText.Substring(userText.IndexOf("d/") + 2);
            id = id.Substring(0, id.IndexOf('/'));
            return id;
        }
        public bool CheckUrl(string userText)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(userText, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (result && userText.Contains("docs.google.com/spreadsheets/d/"))
            {
                return true;
            }
            return false;
        }
        private void OpenGoogleTable_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            _form1.Enabled = true;
            _form1.Focus();
        }

        private void Export_Click(object sender, EventArgs e)
        {
            _form1.isExport = false;
            string userText = textBox1.Text;
            if (CheckUrl(userText))
            {
                string id = GetDocumentId(userText);
                GoogleTable Table = new GoogleTable(id);
                bool isExist = false;
                string Name = "Result List";
                foreach (var title in Table.GetSheetTitles())
                {
                    if(title == Name)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    Table.AddNewSheet(Name);
                }
                double[,] export = new double[_form1.N,_form1.N+2];
                for (int i = 0; i < export.GetLength(0); i++)
                {
                    for (int j = 0; j < _form1.N + 1; j++)
                    {
                        export[i, j] = _form1.Matrix[i, j];
                    }
                    if (_form1.X != null)
                    {
                        export[i, _form1.N + 1] = _form1.X[i];
                    }
                }
                Table.ExportToSheet(Name, export);
                Close();
            }
        }

        private void OpenGoogleTable_Load(object sender, EventArgs e)
        {
            Export.Enabled = _form1.isExport;
        }
    }
}
