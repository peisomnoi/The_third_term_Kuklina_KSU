
namespace lab6
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
            this.CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.StartAlgorithms = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CheckedListBox
            // 
            this.CheckedListBox.CheckOnClick = true;
            this.CheckedListBox.FormattingEnabled = true;
            this.CheckedListBox.Items.AddRange(new object[] {
            "Метод Гаусса",
            "Метод квадратного корня",
            "Метод прогонки",
            "Метод простой итерации",
            "Метод наискорейшего спуска",
            "Метод сопряженных градиентов"});
            this.CheckedListBox.Location = new System.Drawing.Point(12, 12);
            this.CheckedListBox.Name = "CheckedListBox";
            this.CheckedListBox.Size = new System.Drawing.Size(303, 123);
            this.CheckedListBox.TabIndex = 3;
            // 
            // StartAlgorithms
            // 
            this.StartAlgorithms.Location = new System.Drawing.Point(12, 154);
            this.StartAlgorithms.Name = "StartAlgorithms";
            this.StartAlgorithms.Size = new System.Drawing.Size(303, 37);
            this.StartAlgorithms.TabIndex = 2;
            this.StartAlgorithms.Text = "Начать";
            this.StartAlgorithms.UseVisualStyleBackColor = true;
            this.StartAlgorithms.Click += new System.EventHandler(this.StartAlgorithms_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(14, 197);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(301, 37);
            this.StopButton.TabIndex = 5;
            this.StopButton.Text = "Завершить";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // SelectAlgorithmsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 240);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.CheckedListBox);
            this.Controls.Add(this.StartAlgorithms);
            this.Name = "SelectAlgorithmsForm";
            this.Text = "SelectAlgorithmsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectAlgorithmsForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox CheckedListBox;
        private System.Windows.Forms.Button StartAlgorithms;
        private System.Windows.Forms.Button StopButton;
    }
}