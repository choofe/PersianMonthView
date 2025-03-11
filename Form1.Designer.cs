namespace PersianMonthView
{
    partial class Form1
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
            this.SuspendLayout();
            // 
            // persianMonthViewControl1
            // 
            this.persianMonthViewControl1.Location = new System.Drawing.Point(370, 145);
            this.persianMonthViewControl1.MaximumSize = new System.Drawing.Size(800, 400);
            this.persianMonthViewControl1.MinimumSize = new System.Drawing.Size(405, 198);
            this.persianMonthViewControl1.Name = "persianMonthViewControl1";
            this.persianMonthViewControl1.Size = new System.Drawing.Size(405, 268);
            this.persianMonthViewControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 597);
            this.Controls.Add(this.persianMonthViewControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private PersianMonthViewControl persianMonthViewControl1;
    }
}

