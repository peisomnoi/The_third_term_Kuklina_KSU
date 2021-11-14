
namespace SortLab
{
    partial class SelectAlgorithmsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SortCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.StartAlgorithms = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SortCheckedListBox
            // 
            this.SortCheckedListBox.CheckOnClick = true;
            this.SortCheckedListBox.FormattingEnabled = true;
            this.SortCheckedListBox.Items.AddRange(new object[] {
            "Пузырьковая",
            "Вставками",
            "Шейкерная",
            "Быстрая",
            "BOGO"});
            this.SortCheckedListBox.Location = new System.Drawing.Point(12, 12);
            this.SortCheckedListBox.Name = "SortCheckedListBox";
            this.SortCheckedListBox.Size = new System.Drawing.Size(169, 106);
            this.SortCheckedListBox.TabIndex = 3;
            // 
            // StartAlgorithms
            // 
            this.StartAlgorithms.Location = new System.Drawing.Point(12, 154);
            this.StartAlgorithms.Name = "StartAlgorithms";
            this.StartAlgorithms.Size = new System.Drawing.Size(169, 37);
            this.StartAlgorithms.TabIndex = 2;
            this.StartAlgorithms.Text = "Начать";
            this.StartAlgorithms.UseVisualStyleBackColor = true;
            this.StartAlgorithms.Click += new System.EventHandler(this.StartAlgorithms_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DisplayMember = "0";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "По возрастанию",
            "По убыванию"});
            this.comboBox1.Location = new System.Drawing.Point(12, 124);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(169, 24);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.Text = "По возрастанию";
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(14, 197);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(169, 37);
            this.StopButton.TabIndex = 5;
            this.StopButton.Text = "Завершить";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // SelectAlgorithmsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 240);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.SortCheckedListBox);
            this.Controls.Add(this.StartAlgorithms);
            this.Name = "SelectAlgorithmsForm";
            this.Text = "SelectAlgorithmsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectAlgorithmsForm_FormClosing);
            this.Load += new System.EventHandler(this.SelectAlgorithmsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox SortCheckedListBox;
        private System.Windows.Forms.Button StartAlgorithms;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button StopButton;
    }
}