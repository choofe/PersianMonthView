namespace PersianMonthViewControl
{
    partial class test
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
            this.persianMonthViewControl1 = new PersianMonthView.PersianMonthViewControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // persianMonthViewControl1
            // 
            this.persianMonthViewControl1.Location = new System.Drawing.Point(46, 119);
            this.persianMonthViewControl1.MaximumSize = new System.Drawing.Size(800, 400);
            this.persianMonthViewControl1.MinimumSize = new System.Drawing.Size(405, 198);
            this.persianMonthViewControl1.Name = "persianMonthViewControl1";
            this.persianMonthViewControl1.Size = new System.Drawing.Size(405, 198);
            this.persianMonthViewControl1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.persianMonthViewControl1);
            this.groupBox1.Location = new System.Drawing.Point(557, 79);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(511, 481);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 602);
            this.Controls.Add(this.groupBox1);
            this.Name = "test";
            this.Text = "test";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PersianMonthView.PersianMonthViewControl persianMonthViewControl1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}