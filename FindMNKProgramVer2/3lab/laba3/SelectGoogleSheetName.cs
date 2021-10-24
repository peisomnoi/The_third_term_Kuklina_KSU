using laba2;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace laba3
{
    public partial class SelectGoogleSheetName : Form
    {
        private Form1 _form1 { get; set; }
        private GoogleTable Table { get; set; }
        public SelectGoogleSheetName(Form form, string id)
        {
            _form1 = (Form1)form;
            Table = new GoogleTable(id);
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            string SelectedName = listBox1.SelectedItem.ToString();
            IList<IList<object>> list = (IList<IList<object>>)Table.ReadEntries(SelectedName);
            _form1.ValidateImportedData(list);
            _form1.Enabled = true;
            _form1.Focus();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
        private void ChooseGoogleSheet_Load(object sender, EventArgs e)
        {
            foreach (var title in Table.GetSheetTitles())
            {
                listBox1.Items.Add(title);
            }
        }
        private void SelectGoogleSheetName_FormClosed(object sender, FormClosedEventArgs e)
        {
            _form1.Enabled = true;
            _form1.Focus();
        }
    }
}
