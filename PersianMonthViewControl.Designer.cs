namespace PersianMonthView
{
    partial class PersianMonthViewControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lnkLblPreviousYear = new System.Windows.Forms.LinkLabel();
            this.lnkLblNextYear = new System.Windows.Forms.LinkLabel();
            this.PersianDatePicker = new System.Windows.Forms.DataGridView();
            this.lnkLblNextMonth = new System.Windows.Forms.LinkLabel();
            this.lnklblToday = new System.Windows.Forms.LinkLabel();
            this.lnkLblPreviousMonth = new System.Windows.Forms.LinkLabel();
            this.lblSelectedDate = new System.Windows.Forms.Label();
            this.lstYear = new System.Windows.Forms.ListBox();
            this.lstMonths = new System.Windows.Forms.ListBox();
            this.lstDays = new System.Windows.Forms.ListBox();
            this.pnlDatePick = new System.Windows.Forms.Panel();
            this.lblGregMonth = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PersianDatePicker)).BeginInit();
            this.pnlDatePick.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkLblPreviousYear
            // 
            this.lnkLblPreviousYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkLblPreviousYear.AutoSize = true;
            this.lnkLblPreviousYear.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lnkLblPreviousYear.Location = new System.Drawing.Point(364, 3);
            this.lnkLblPreviousYear.Name = "lnkLblPreviousYear";
            this.lnkLblPreviousYear.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lnkLblPreviousYear.Size = new System.Drawing.Size(35, 19);
            this.lnkLblPreviousYear.TabIndex = 123;
            this.lnkLblPreviousYear.TabStop = true;
            this.lnkLblPreviousYear.Text = ">>";
            this.lnkLblPreviousYear.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblPreviousYear_LinkClicked);
            // 
            // lnkLblNextYear
            // 
            this.lnkLblNextYear.AutoSize = true;
            this.lnkLblNextYear.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lnkLblNextYear.Location = new System.Drawing.Point(4, 3);
            this.lnkLblNextYear.Name = "lnkLblNextYear";
            this.lnkLblNextYear.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lnkLblNextYear.Size = new System.Drawing.Size(35, 19);
            this.lnkLblNextYear.TabIndex = 124;
            this.lnkLblNextYear.TabStop = true;
            this.lnkLblNextYear.Text = "<<";
            this.lnkLblNextYear.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblNextYear_LinkClicked);
            // 
            // PersianDatePicker
            // 
            this.PersianDatePicker.AllowUserToAddRows = false;
            this.PersianDatePicker.AllowUserToDeleteRows = false;
            this.PersianDatePicker.AllowUserToResizeColumns = false;
            this.PersianDatePicker.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.PersianDatePicker.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.PersianDatePicker.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PersianDatePicker.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.PersianDatePicker.ColumnHeadersHeight = 25;
            this.PersianDatePicker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.PersianDatePicker.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.PersianDatePicker.EnableHeadersVisualStyles = false;
            this.PersianDatePicker.GridColor = System.Drawing.Color.WhiteSmoke;
            this.PersianDatePicker.Location = new System.Drawing.Point(4, 55);
            this.PersianDatePicker.MultiSelect = false;
            this.PersianDatePicker.Name = "PersianDatePicker";
            this.PersianDatePicker.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PersianDatePicker.RowHeadersVisible = false;
            this.PersianDatePicker.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.PersianDatePicker.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.PersianDatePicker.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.PersianDatePicker.Size = new System.Drawing.Size(395, 137);
            this.PersianDatePicker.TabIndex = 128;
            this.PersianDatePicker.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PersianDatePicker_MouseDown);
            // 
            // lnkLblNextMonth
            // 
            this.lnkLblNextMonth.AutoSize = true;
            this.lnkLblNextMonth.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lnkLblNextMonth.Location = new System.Drawing.Point(45, 3);
            this.lnkLblNextMonth.Name = "lnkLblNextMonth";
            this.lnkLblNextMonth.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lnkLblNextMonth.Size = new System.Drawing.Size(22, 19);
            this.lnkLblNextMonth.TabIndex = 125;
            this.lnkLblNextMonth.TabStop = true;
            this.lnkLblNextMonth.Text = "<";
            this.lnkLblNextMonth.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblNextMonth_LinkClicked);
            // 
            // lnklblToday
            // 
            this.lnklblToday.ActiveLinkColor = System.Drawing.Color.Blue;
            this.lnklblToday.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lnklblToday.AutoSize = true;
            this.lnklblToday.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lnklblToday.Location = new System.Drawing.Point(178, 3);
            this.lnklblToday.Name = "lnklblToday";
            this.lnklblToday.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lnklblToday.Size = new System.Drawing.Size(46, 19);
            this.lnklblToday.TabIndex = 126;
            this.lnklblToday.TabStop = true;
            this.lnklblToday.Text = "امروز";
            this.lnklblToday.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblToday_LinkClicked);
            // 
            // lnkLblPreviousMonth
            // 
            this.lnkLblPreviousMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkLblPreviousMonth.AutoSize = true;
            this.lnkLblPreviousMonth.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lnkLblPreviousMonth.Location = new System.Drawing.Point(336, 3);
            this.lnkLblPreviousMonth.Name = "lnkLblPreviousMonth";
            this.lnkLblPreviousMonth.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lnkLblPreviousMonth.Size = new System.Drawing.Size(22, 19);
            this.lnkLblPreviousMonth.TabIndex = 127;
            this.lnkLblPreviousMonth.TabStop = true;
            this.lnkLblPreviousMonth.Text = ">";
            this.lnkLblPreviousMonth.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblPreviousMonth_LinkClicked);
            // 
            // lblSelectedDate
            // 
            this.lblSelectedDate.AutoSize = true;
            this.lblSelectedDate.BackColor = System.Drawing.Color.White;
            this.lblSelectedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSelectedDate.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedDate.Location = new System.Drawing.Point(4, 28);
            this.lblSelectedDate.MaximumSize = new System.Drawing.Size(395, 100);
            this.lblSelectedDate.MinimumSize = new System.Drawing.Size(395, 15);
            this.lblSelectedDate.Name = "lblSelectedDate";
            this.lblSelectedDate.Size = new System.Drawing.Size(395, 21);
            this.lblSelectedDate.TabIndex = 129;
            this.lblSelectedDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblSelectedDate.Click += new System.EventHandler(this.lblSelectedDate_Click);
            // 
            // lstYear
            // 
            this.lstYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lstYear.IntegralHeight = false;
            this.lstYear.ItemHeight = 25;
            this.lstYear.Location = new System.Drawing.Point(0, 1);
            this.lstYear.Margin = new System.Windows.Forms.Padding(0);
            this.lstYear.Name = "lstYear";
            this.lstYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lstYear.Size = new System.Drawing.Size(132, 32);
            this.lstYear.TabIndex = 130;
            this.lstYear.Visible = false;
            // 
            // lstMonths
            // 
            this.lstMonths.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstMonths.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstMonths.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lstMonths.IntegralHeight = false;
            this.lstMonths.ItemHeight = 25;
            this.lstMonths.Location = new System.Drawing.Point(132, 1);
            this.lstMonths.Margin = new System.Windows.Forms.Padding(0);
            this.lstMonths.Name = "lstMonths";
            this.lstMonths.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lstMonths.Size = new System.Drawing.Size(132, 32);
            this.lstMonths.TabIndex = 130;
            this.lstMonths.Visible = false;
            this.lstMonths.SelectedIndexChanged += new System.EventHandler(this.lstMonths_SelectedIndexChanged);
            // 
            // lstDays
            // 
            this.lstDays.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lstDays.IntegralHeight = false;
            this.lstDays.ItemHeight = 25;
            this.lstDays.Location = new System.Drawing.Point(262, 1);
            this.lstDays.Margin = new System.Windows.Forms.Padding(0);
            this.lstDays.Name = "lstDays";
            this.lstDays.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lstDays.Size = new System.Drawing.Size(132, 32);
            this.lstDays.TabIndex = 130;
            this.lstDays.Visible = false;
            this.lstDays.Click += new System.EventHandler(this.lstDays_Click);
            // 
            // pnlDatePick
            // 
            this.pnlDatePick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDatePick.Controls.Add(this.lstDays);
            this.pnlDatePick.Controls.Add(this.lstYear);
            this.pnlDatePick.Controls.Add(this.lstMonths);
            this.pnlDatePick.Location = new System.Drawing.Point(3, 28);
            this.pnlDatePick.Name = "pnlDatePick";
            this.pnlDatePick.Size = new System.Drawing.Size(396, 164);
            this.pnlDatePick.TabIndex = 131;
            this.pnlDatePick.Visible = false;
            // 
            // lblGregMonth
            // 
            this.lblGregMonth.AutoSize = true;
            this.lblGregMonth.BackColor = System.Drawing.Color.Transparent;
            this.lblGregMonth.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGregMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblGregMonth.Location = new System.Drawing.Point(69, 8);
            this.lblGregMonth.MaximumSize = new System.Drawing.Size(0, 100);
            this.lblGregMonth.MinimumSize = new System.Drawing.Size(0, 15);
            this.lblGregMonth.Name = "lblGregMonth";
            this.lblGregMonth.Size = new System.Drawing.Size(46, 15);
            this.lblGregMonth.TabIndex = 132;
            this.lblGregMonth.Text = "M-M-Y";
            this.lblGregMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PersianMonthViewControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lnkLblPreviousYear);
            this.Controls.Add(this.lblGregMonth);
            this.Controls.Add(this.lnkLblNextYear);
            this.Controls.Add(this.lnkLblNextMonth);
            this.Controls.Add(this.lnklblToday);
            this.Controls.Add(this.lnkLblPreviousMonth);
            this.Controls.Add(this.pnlDatePick);
            this.Controls.Add(this.lblSelectedDate);
            this.Controls.Add(this.PersianDatePicker);
            this.MaximumSize = new System.Drawing.Size(800, 400);
            this.MinimumSize = new System.Drawing.Size(405, 198);
            this.Name = "PersianMonthViewControl";
            this.Size = new System.Drawing.Size(405, 198);
            this.Tag = "Loading";
            ((System.ComponentModel.ISupportInitialize)(this.PersianDatePicker)).EndInit();
            this.pnlDatePick.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkLblPreviousYear;
        private System.Windows.Forms.LinkLabel lnkLblNextYear;
        private System.Windows.Forms.DataGridView PersianDatePicker;
        private System.Windows.Forms.LinkLabel lnkLblNextMonth;
        private System.Windows.Forms.LinkLabel lnklblToday;
        private System.Windows.Forms.LinkLabel lnkLblPreviousMonth;
        private System.Windows.Forms.Label lblSelectedDate;
        private System.Windows.Forms.ListBox lstYear;
        private System.Windows.Forms.ListBox lstMonths;
        private System.Windows.Forms.ListBox lstDays;
        private System.Windows.Forms.Panel pnlDatePick;
        private System.Windows.Forms.Label lblGregMonth;
    }
}
