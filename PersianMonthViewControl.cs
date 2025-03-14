using MD.PersianDateTime;
using System.Runtime.InteropServices;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Timer = System.Windows.Forms.Timer;


namespace PersianMonthView
{

    public partial class PersianMonthViewControl : UserControl
    {
        /// <summary>
        ///       تاریخ انتخاب شده در قالب شی DateTime
        /// </summary>
        [Browsable(false)]
        public DateTime SelectedDateObject
        {
            get
            {
                return LongStringToPersianDateTime(lblSelectedDate.Text).ToDateTime();
            }
        }

        /// <summary>
        ///       تاریخ انتخاب شده در قالب شی PersianDateTime
        /// </summary>
        [Browsable(false)]
        public PersianDateTime selectedPersianDateTime
        {
            get
            {
                return LongStringToPersianDateTime(lblSelectedDate.Text);
            }
        }
        //covering tag property
        [Browsable(false)]  // Hides it from Properties Window
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]  // Prevents it from being serialized
        public new object Tag
        {
            get => base.Tag;
            set { /* Do nothing to prevent changes */ }
        }


        [DllImport("user32.dll")]
        private static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

        private const int SB_VERT = 1; // Vertical scrollbar
        private const int SB_HORZ = 0; // Horizontal scrollbar

        private void HideScrollbar(ListBox listBox)
        {
            ShowScrollBar(listBox.Handle, SB_VERT, 0); // Hide vertical scrollbar
            ShowScrollBar(listBox.Handle, SB_HORZ, 0); // Hide horizontal scrollbar
        }



        private ToolTip _toolTip = new ToolTip();

        public PersianMonthViewControl()
        {
            InitializeComponent();
            //_toolTip.
            _toolTip.SetToolTip(lnkLblPreviousYear, "سال قبل");
            _toolTip.SetToolTip(lnkLblPreviousMonth, "ماه قبل");
            _toolTip.SetToolTip(lnkLblNextMonth, "ماه بعد");
            _toolTip.SetToolTip(lnkLblNextYear, "سال بعد");
            lstYear.BeginUpdate();
            lstMonths.BeginUpdate();
            lstYear.Items.Clear();
            PersianDateTime currentDate = new PersianDateTime(DateTime.Now);
            string yearNow = currentDate.ToString("yyyy");

            for (int i = 10; i > -90; i--)
            {
                lstYear.Items.Add(currentDate.AddYears(i).ToString("yyyy"));
                if (lstYear.Items[lstYear.Items.Count - 1].ToString() == yearNow)
                {
                    lstYear.SelectedIndex = 10 - i;

                }
            }
            string monthNow = currentDate.ToString("MMMM");
            for (int i = 0; i < 12; i++)
            {
                lstMonths.Items.Add(currentDate.AddDays(-currentDate.GetDayOfYear + (i * 30) + 10).MonthName.ToString());
                if (lstMonths.Items[lstMonths.Items.Count - 1].ToString() == monthNow)
                {
                    lstMonths.SelectedIndex = i;

                }
            }

            //lstYear.TopIndex = lstYear.SelectedIndex;

            lstYear.DrawItem += lstYear_DrawItem;
            lstMonths.DrawItem += lstMonths_DrawItem;
            lstDays.DrawItem += LstDays_DrawItem;
            lstYear.EndUpdate();
            lstMonths.EndUpdate();
            PersianDatePicker.BackgroundColor = _defaultBackGroundColor;
            //_SecondBackColor= Color.Honeydew;
            // Prevent select by canceling selection
            PersianDatePicker.SelectionChanged += (s, eArgs) =>
            {
                PersianDatePicker.ClearSelection();
            };

            PersianDatePicker.DataSource = FillMonthView(DateTime.Now);

            InitializePersianDGV(PersianDatePicker);

            PersianDatePicker.CellMouseEnter += PersianDatePicker_CellMouseEnter;
            PersianDatePicker.CellMouseLeave += PersianDatePicker_CellMouseLeave;
            PersianDatePicker.ClearSelection();



            //int widthCompensate = (PersianDatePicker.Width - 3) % 7;

            //HighlightDate(PersianDatePicker, DateTime.Now);

        }



        //public size

        private int _fixedBlankHeightSize = 21;
        public enum NavigationSymbols
        {
            DoubleArrows,   // « < > »
            FilledArrows,   // ◄◄ ◄ ► ►►
            PersianText,    // سال قبل ماه قبل ماه بعد سال بعد
            BracketArrows,  // ⟪ ⟨ ⟩ ⟫
            SimpleArrows    // << < > >>
        }

        private NavigationSymbols _navSymbols = NavigationSymbols.DoubleArrows; // Default
        [Browsable(true)]
        [Category("PesianMonthView")]
        [Description("Sets the symbols for navigation link labels.")]
        [DefaultValue(NavigationSymbols.DoubleArrows)]
        public NavigationSymbols NavigationSymbolsSet
        {
            get
            {
                return _navSymbols;
            }
            set
            {
                if (_navSymbols != value)
                {
                    _navSymbols = value;
                    UpdateNavigationSymbols(); // Apply changes immediately

                }
            }
        }

        private Font _labelFontStyle = new Font("Tahoma", 12f); // Default
        [Browsable(true)]
        [Category("PesianMonthView")]
        [Description("Sets the font for selected date label.")]
        [DefaultValue(typeof(Font), "Tahoma, 12")]
        //[DefaultValue(typeof(Font), "Tahoma, 12")]

        public Font LabelFontStyle
        {
            get
            {
                return _labelFontStyle;
            }
            set
            {
                if (_labelFontStyle != value)
                {
                    _labelFontStyle = value;
                    UpdateFontStyle(); // Apply changes immediately

                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Debug.WriteLine($"Size changed: {Size.Width}x{Size.Height}");
        }


        private const int _gridControlWidthMargin = 10;
        private const int _gridControlHeightMargin = 61;
        private const int _labelControlWidthMargin = 10;
        private const int _labelControlMinHeight = 15;
        private const int _labelControlMaxHeight = 100;
        private const int _ColumnsNumber = 7;
        private const int _RowsNumber = 5;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //if (DesignMode) return; // Prevent unnecessary updates in design mode

            int newGridWidth = Convert.ToInt32(Math.Round((this.Width - _gridControlWidthMargin - 3) / 7f) * _ColumnsNumber);
            int thisWidth = newGridWidth + _gridControlWidthMargin + 3;
            int newGridHeight = Convert.ToInt32(Math.Round((this.Height - _gridControlHeightMargin - PersianDatePicker.ColumnHeadersHeight - 2) / 5f) * _RowsNumber);
            int thisHeight = newGridHeight + PersianDatePicker.ColumnHeadersHeight + _gridControlHeightMargin + 2;

            this.Size = new Size(thisWidth, thisHeight);
            PersianDatePicker.Width = this.Width - _gridControlWidthMargin;
            PersianDatePicker.Height = this.Height - _gridControlHeightMargin;

            ResizeGridView(); // Ensure explicit update

            lblSelectedDate.MaximumSize = new Size(this.Width - _labelControlWidthMargin, _labelControlMaxHeight);
            lblSelectedDate.MinimumSize = new Size(this.Width - _labelControlWidthMargin, _labelControlMinHeight);
            lblSelectedDate.Width = this.Width - _labelControlWidthMargin;
            NavigateLocationUpdate();
        }

        private void ResizeGridView()
        {
            if (PersianDatePicker.ColumnCount == 0 || PersianDatePicker.RowCount == 0)
                return;

            int columnWidth = PersianDatePicker.Width / _ColumnsNumber;
            int rowHeight = (PersianDatePicker.Height - PersianDatePicker.ColumnHeadersHeight) / _RowsNumber;

            foreach (DataGridViewColumn col in PersianDatePicker.Columns)
            {
                col.Width = columnWidth;
            }

            foreach (DataGridViewRow row in PersianDatePicker.Rows)
            {
                row.Height = rowHeight;
            }

            PersianDatePicker.RowTemplate.Height = rowHeight; // Ensure RowTemplate height is applied

            PersianDatePicker.Invalidate();
            PersianDatePicker.Refresh();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            ResizeGridView();  // Ensures both columns and rows update properly
        }


        private void NavigateLocationUpdate()
        {
            var yLocation = lnkLblNextYear.Location.Y;

            lnkLblPreviousYear.Location = new Point(
                this.Size.Width -
                lnkLblPreviousYear.Margin.Right -
                lnkLblPreviousYear.Size.Width -
                lnkLblPreviousYear.Margin.Left
                                                , yLocation);
            lnkLblNextYear.Location = new Point(3, yLocation);
            lnkLblNextMonth.Location = new Point(
                lnkLblNextYear.Margin.Left +
                lnkLblNextYear.Size.Width +
                lnkLblNextYear.Margin.Right +
                lnkLblNextMonth.Margin.Left
                                            , yLocation);
            lnkLblPreviousMonth.Location = new Point(
                this.Size.Width -
                lnkLblPreviousYear.Margin.Right -
                lnkLblPreviousYear.Size.Width -
                lnkLblPreviousYear.Margin.Left -
                lnkLblPreviousMonth.Margin.Right -
                lnkLblPreviousMonth.Size.Width -
                lnkLblPreviousMonth.Margin.Left
                                            , yLocation);
            lnklblToday.Location = new Point((this.Size.Width / 2) - (lnklblToday.Width / 2), yLocation);
        }
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

        }

        private void UpdateFontStyle()
        {
            lblSelectedDate.Font = _labelFontStyle;
            float ratio = 21f / 12f;
            lblSelectedDate.Height = Convert.ToInt32(lblSelectedDate.Font.Height * ratio);
            this.Height =
                _fixedBlankHeightSize +
                lnklblToday.Height +
                lblSelectedDate.Height +
                PersianDatePicker.Height;

            PersianDatePicker.Top =
                lblSelectedDate.Top +
                lblSelectedDate.Height +
                lblSelectedDate.Margin.Bottom +
                PersianDatePicker.Margin.Top;
        }

        private Color _defaultBackGroundColor = Color.White; // Default
        [Browsable(true)]
        [Category("PesianMonthView")]
        [Description("Sets the background color of the calendar.")]
        [DefaultValue(typeof(Color), "White")]
        public Color CalendarBackColor
        {
            get
            {
                return PersianDatePicker.BackgroundColor;
            }

            set
            {
                PersianDatePicker.BackgroundColor = value;
            }
        }

        private Color _secondBackColor = Color.Honeydew; // Default
        [Browsable(true)]
        [Category("PesianMonthView")]
        [Description("Sets the distinguishing rows color.")]
        [DefaultValue(typeof(Color), "Honeydew")]
        public Color SecondBackColor
        {
            get
            {
                return _secondBackColor;
            }

            set
            {
                _secondBackColor = value;
            }
        }
        private Color _dateHighLightColor = Color.Gold; // Default
        [Browsable(true)]
        [Category("PesianMonthView")]
        [Description("Sets the selected cell back color.")]
        [DefaultValue(typeof(Color), "Gold")]
        public Color DateHighLightColor
        {
            get
            {
                return _dateHighLightColor;
            }

            set
            {
                _dateHighLightColor = value;
            }
        }
        private Color _headerBackColor = Color.MintCream; // Default
        [Browsable(true)]
        [Category("PesianMonthView")]
        [Description("Sets the headers back color.")]
        [DefaultValue(typeof(Color), "MintCream")]
        public Color HeaderBackColor
        {
            get
            {
                return _headerBackColor;
            }

            set
            {
                _headerBackColor = value;
            }
        }
        private Color _dateLabelBackColor = Color.White; // Default
        [Browsable(true)]
        [Category("PesianMonthView")]
        [Description("Sets the date text label back color.")]
        [DefaultValue(typeof(Color), "White")]
        public Color DateLabelBackColor
        {
            get
            {
                return _dateLabelBackColor;
            }

            set
            {
                _dateLabelBackColor = value;
                base.Invalidate();
            }
        }
        private Color _hoverColor = Color.LightBlue; // Default
        [Browsable(true)]
        [Category("PesianMonthView")]
        [Description("Sets the mouse hovering over cells color.")]
        [DefaultValue(typeof(Color), "LightBlue")]
        public Color HoverColor
        {
            get
            {
                return _hoverColor;
            }

            set
            {
                _hoverColor = value;
            }
        }


        public DataTable FillMonthView(DateTime _date)
        {

            var persianToday = new PersianDateTime(_date);
            //Current date to label
            var dateWeekDayName = persianToday.GetLongDayOfWeekName;
            var dateMonthName = persianToday.GetLongMonthName;
            var dateLongFormat =
                        CenterAlign(dateWeekDayName + "\u00A0", 10) +
                        CenterAlign(persianToday.Day.ToString() + "\u00A0", 4) +
                        CenterAlign(dateMonthName + "\u00A0", 10) +
                        CenterAlign(persianToday.Year.ToString(), 6);
            lblSelectedDate.Text = dateLongFormat;
            var monthDays = persianToday.GetMonthDays;
            var monthFirstDay = persianToday.GetPersianDateOfLastDayOfMonth().AddDays(1 - monthDays);
            var firstDayofweekIndex = Convert.ToInt32(monthFirstDay.PersianDayOfWeek);

            DataColumn[] weekdays = new DataColumn[7];
            for (int i = 0; i < 7; i++)
            {
                var week1stDay = persianToday.GetFirstDayOfWeek().ToDateTime();
                var weekDayName = new PersianDateTime(week1stDay.AddDays(i));

                // Create a DataColumn with the Persian weekday name as the ColumnName
                weekdays[i] = new DataColumn
                {
                    DataType = typeof(int),
                    ColumnName = weekDayName.DayOfWeek.ToString(),
                    Caption = weekDayName.GetLongDayOfWeekName
                    //DataType = typeof(int),
                    //ColumnName = weekDayName.GetLongDayOfWeekName // Use the Persian weekday name
                };

            }
            DataTable _month = new DataTable();
            foreach (var item in weekdays)
            {
                _month.Columns.Add(item);
            }
            for (int i = 0; i < 5; i++)
            {
                _month.Rows.Add(); // Add a new row
            }
            // placing day Number in right places
            for (int i = 0; i < monthDays; i++)
            {
                if (i + firstDayofweekIndex < 35)
                {
                    var row = (i + firstDayofweekIndex) / 7;
                    var column = (i + firstDayofweekIndex) % 7;
                    _month.Rows[row][column] = i + 1;
                }
                else
                {
                    var row = (i + firstDayofweekIndex - 35) / 7;
                    var column = (i + firstDayofweekIndex - 35) % 7;
                    _month.Rows[row][column] = i + 1;
                }
            }


            return _month;
        }

        private void InitializePersianDGV(DataGridView DGV)
        {
            DGV.EnableHeadersVisualStyles = false;
            //DGV.RowsDefaultCellStyle.BackColor = Color.White;

            for (int i = 0; i < DGV.Columns.Count; i++)
            {
                DGV.Columns[i].HeaderText = FillMonthView(DateTime.Now).Columns[DGV.Columns[i].Name].Caption;
                DGV.Columns[i].HeaderCell.Style.Font = new Font("Tahoma", 7f);
                //DGV.Columns[i].HeaderCell.Style.BackColor = _headerBackColor;
                //DGV.Columns[i].Width = 56;
                DGV.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            DGV.ClearSelection();
        }
        private void lnkLblPreviousYear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var currentDate = LongStringToPersianDateTime(lblSelectedDate.Text);
            PersianDatePicker.DataSource = FillMonthView(currentDate.AddYears(-1));
            HighlightDate(PersianDatePicker, currentDate.ToDateTime());

        }

        private void lnkLblPreviousMonth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var currentDate = LongStringToPersianDateTime(lblSelectedDate.Text);
            PersianDatePicker.DataSource = FillMonthView(currentDate.AddMonths(-1));
            HighlightDate(PersianDatePicker, currentDate.ToDateTime());

        }

        private void lnkLblNextMonth_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var currentDate = LongStringToPersianDateTime(lblSelectedDate.Text);
            PersianDatePicker.DataSource = FillMonthView(currentDate.AddMonths(1));
            HighlightDate(PersianDatePicker, currentDate.ToDateTime());

        }

        private void lnkLblNextYear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var currentDate = LongStringToPersianDateTime(lblSelectedDate.Text);
            PersianDatePicker.DataSource = FillMonthView(currentDate.AddYears(1));
            HighlightDate(PersianDatePicker, currentDate.ToDateTime());

        }

        private void lnklblToday_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PersianDatePicker.DataSource = FillMonthView(new PersianDateTime(DateTime.Now));
            HighlightDate(PersianDatePicker, DateTime.Now);
        }
        private void HighlightDate(DataGridView DGV, DateTime selectedDate)
        {
            lblSelectedDate.BackColor = _dateLabelBackColor;
            var persianDate = new PersianDateTime(selectedDate);
            int selectedDay = persianDate.Day;

            // Reset all cells to default style
            foreach (DataGridViewRow row in DGV.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    /*if (cell.Value != null && int.TryParse(cell.Value.ToString(), out _))
                    {*/
                    cell.Style.BackColor = (row.Index % 2 == 0) ? PersianDatePicker.BackgroundColor : _secondBackColor;
                    //if (row.Index % 2 == 0) cell.Style.BackColor = Color.Honeydew;
                    cell.Style.ForeColor = Color.Black;
                    cell.Style.Font = new Font("Tahoma", 14f);
                    //}

                }
            }

            // Highlight the selected date
            foreach (DataGridViewRow row in DGV.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && int.TryParse(cell.Value.ToString(), out int day) && day == selectedDay)
                    {
                        cell.Style.BackColor = _dateHighLightColor;
                        cell.Style.ForeColor = Color.Black;
                        cell.Style.Font = new Font("Tahoma", 14f, FontStyle.Bold);
                        return; // Exit loop after finding the date
                    }
                }
            }
        }

        private Color previousColor; // Store the original color

        private void PersianDatePicker_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = PersianDatePicker.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Store the original color before changing it
                previousColor = cell.Style.BackColor;

                // Change to hover color
                cell.Style.BackColor = Color.LightBlue;
            }
        }

        private void PersianDatePicker_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = PersianDatePicker.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Restore the stored original color
                cell.Style.BackColor = previousColor;
            }
        }

        private void PersianDatePicker_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = PersianDatePicker.HitTest(e.X, e.Y);

            if (hit.RowIndex >= 0 && hit.ColumnIndex >= 0) // Ensure it's a valid cell
            {
                DataGridViewCell clickedCell = PersianDatePicker.Rows[hit.RowIndex].Cells[hit.ColumnIndex];

                //MessageBox.Show($"Clicked Cell: Row {hit.RowIndex}, Column {hit.ColumnIndex}, Value: {clickedCell.Value}");
            }
            Int16 cellValue = 0;
            if (hit.RowIndex >= 0)
            {
                // value of selected row which is the day of current month
                if (PersianDatePicker.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Value != DBNull.Value)
                { cellValue = Int16.Parse(PersianDatePicker.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Value.ToString()); }
            }

            if (cellValue != 0) // Ensure a valid row is clicked
            {
                //MessageBox.Show(cellValue);
                var currentDisplayedDate = LongStringToPersianDateTime(lblSelectedDate.Text);
                var selectedDate = new PersianDateTime(currentDisplayedDate.Year, currentDisplayedDate.Month, cellValue);
                var dateWeekDayName = selectedDate.GetLongDayOfWeekName;
                var dateMonthName = selectedDate.GetLongMonthName;
                // label text "\u00A0" is NO-BREAK SPACE a hidden character to make the label parse-able
                var dateLongFormat =
                        CenterAlign(dateWeekDayName + "\u00A0", 10) +
                        CenterAlign(selectedDate.Day.ToString() + "\u00A0", 4) +
                        CenterAlign(dateMonthName + "\u00A0", 10) +
                        CenterAlign(selectedDate.Year.ToString(), 6);
                lblSelectedDate.Text = dateLongFormat;

                HighlightDate(PersianDatePicker, selectedDate.ToDateTime());
                previousColor = PersianDatePicker.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Style.BackColor;

            }
        }
        string CenterAlign(string text, int width)
        {
            if (text.Length >= width) return text.Substring(0, width); // Truncate if too long

            int padding = (width - text.Length) / 2;
            return text.PadLeft(text.Length + padding).PadRight(width);
        }
        private PersianDateTime LongStringToPersianDateTime(string _dateString)
        {
            string[] dateParts = _dateString.Split('\u00A0');
            var dayname = dateParts[0].Trim();
            var dayNo = Int32.Parse(dateParts[1].Trim());
            var monthNO = Array.IndexOf(new string[]
            {
                "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
                "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
            }
                , dateParts[2].Trim()) + 1;

            var year = Int32.Parse(dateParts[3].Trim());
            return new PersianDateTime(year, monthNO, dayNo);
        }
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            // Only apply highlight when the control is fully visible
            if (this.Visible && PersianDatePicker.DataSource != null)
            {
                foreach (DataGridViewColumn column in PersianDatePicker.Columns)
                {
                    column.HeaderCell.Style.BackColor = _headerBackColor;
                }
                int newGridWidth = Convert.ToInt32(Math.Round((this.Width - _gridControlWidthMargin - 3) / 7f) * 7);
                this.Width = newGridWidth + _gridControlWidthMargin + 3;
                int columnWidth = (this.Width - _gridControlWidthMargin - 3) / 7;
                foreach (DataGridViewColumn column in PersianDatePicker.Columns)
                {
                    column.Width = columnWidth;
                    //column.Width = (widthCompensate > 0) ? columnWidth + 1 : columnWidth;
                    //widthCompensate--;
                }
                HighlightDate(PersianDatePicker, DateTime.Now);
                this.Invalidate();
                this.Refresh();
                this.Update();
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // 🔥 Ensure styles & layout are applied immediately
            this.Tag = "Loaded";
            ResizeGridView();
            HighlightDate(PersianDatePicker, DateTime.Now);
            this.Invalidate();
            this.Refresh();
        }


        private void UpdateNavigationSymbols()
        {
            switch (_navSymbols)
            {
                case NavigationSymbols.DoubleArrows:
                    lnkLblPreviousYear.Text = "»";
                    lnkLblPreviousMonth.Text = ">";
                    lnkLblNextMonth.Text = "<";
                    lnkLblNextYear.Text = "«";
                    break;

                case NavigationSymbols.FilledArrows:
                    lnkLblPreviousYear.Text = "►►";
                    lnkLblPreviousMonth.Text = "►";
                    lnkLblNextMonth.Text = "◄";
                    lnkLblNextYear.Text = "◄◄";
                    break;

                case NavigationSymbols.PersianText:
                    lnkLblPreviousYear.Text = "سال قبل";
                    lnkLblPreviousMonth.Text = "ماه قبل";
                    lnkLblNextMonth.Text = "ماه بعد";
                    lnkLblNextYear.Text = "سال بعد";
                    break;

                case NavigationSymbols.BracketArrows:
                    lnkLblPreviousYear.Text = "⟫";
                    lnkLblPreviousMonth.Text = "⟩";
                    lnkLblNextMonth.Text = "⟨";
                    lnkLblNextYear.Text = "⟪";
                    break;

                case NavigationSymbols.SimpleArrows:
                    lnkLblPreviousYear.Text = ">>";
                    lnkLblPreviousMonth.Text = ">";
                    lnkLblNextMonth.Text = "<";
                    lnkLblNextYear.Text = "<<";
                    break;
            }
            var yLocation = lnkLblNextYear.Location.Y;

            lnkLblPreviousYear.Location = new Point(
                this.Size.Width -
                lnkLblPreviousYear.Margin.Right -
                lnkLblPreviousYear.Size.Width -
                lnkLblPreviousYear.Margin.Left
                                                , yLocation);
            lnkLblNextYear.Location = new Point(3, yLocation);
            lnkLblNextMonth.Location = new Point(
                lnkLblNextYear.Margin.Left +
                lnkLblNextYear.Size.Width +
                lnkLblNextYear.Margin.Right +
                lnkLblNextMonth.Margin.Left
                                            , yLocation);
            lnkLblPreviousMonth.Location = new Point(
                this.Size.Width -
                lnkLblPreviousYear.Margin.Right -
                lnkLblPreviousYear.Size.Width -
                lnkLblPreviousYear.Margin.Left -
                lnkLblPreviousMonth.Margin.Right -
                lnkLblPreviousMonth.Size.Width -
                lnkLblPreviousMonth.Margin.Left
                                            , yLocation);
            lnklblToday.Location = new Point(this.Size.Width / 2 -
                lnklblToday.Width / 2, yLocation);


        }
        private void LstDays_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,  // Center text horizontally
                    LineAlignment = StringAlignment.Center // Center text vertically
                };
                // Convert English numbers to Persian numbers before drawing
                string persianText = ConvertToPersianNumbers(lstDays.Items[e.Index].ToString());

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(persianText, e.Font, textBrush, e.Bounds, format);
                }

            }

            e.DrawFocusRectangle();

        }



        private void lstMonths_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,  // Center text horizontally
                    LineAlignment = StringAlignment.Center // Center text vertically
                };

                string persianText = lstMonths.Items[e.Index].ToString();

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(persianText, e.Font, textBrush, e.Bounds, format);
                }
            }

            e.DrawFocusRectangle();
        }


        private void lstYear_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {

                // 🔥 Set up centered text alignment
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,  // Center text horizontally
                    LineAlignment = StringAlignment.Center // Center text vertically
                };
                // Convert English numbers to Persian numbers before drawing
                string persianText = ConvertToPersianNumbers(lstYear.Items[e.Index].ToString());

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(persianText, e.Font, textBrush, e.Bounds, format);
                }
            }

            e.DrawFocusRectangle();
        }
        private string ConvertToPersianNumbers(string input)
        {
            char[] persianDigits = { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
            for (int i = 0; i < 10; i++)
            {
                input = input.Replace(i.ToString(), persianDigits[i].ToString());
            }
            return input;
        }

        private void lblSelectedDate_Click(object sender, EventArgs e)
        {
            ShowPopup();

        }
        private void ShowPopup()
        {
            this.SuspendLayout();



            pnlDatePick.Width = this.Width - 5;
            pnlDatePick.Height = this.Height - lblSelectedDate.Top - 3;
            int listboxWidth = pnlDatePick.Width / 3;
            int widthModulos = pnlDatePick.Width % 3;
            lstYear.BeginUpdate();
            lstMonths.BeginUpdate();
            lstYear.Left = widthModulos;
            lstYear.Width = listboxWidth;
            lstYear.Height = pnlDatePick.Height - 4;
            lstYear.Visible = true;
            // 🔥 Ensure the selected item is in the middle of the ListBox
            int visibleItems = lstYear.Height / lstYear.ItemHeight; // How many items fit
            int middleIndex = Math.Max(0, lstYear.SelectedIndex - (visibleItems / 2) + 2);
            lstYear.TopIndex = middleIndex; // 🔥 Scroll to center the selected item
            lstMonths.Left = listboxWidth + ((widthModulos > 1) ? widthModulos - 1 : widthModulos); //placing months names in 1/3rd of the Panel width
            lstMonths.Width = listboxWidth; //set the width to 1/3rd of the panel width
            lstMonths.Height = pnlDatePick.Height - 4;
            lstMonths.Visible = true;

            visibleItems = lstMonths.Height / lstYear.ItemHeight; // How many items fit
            middleIndex = Math.Max(0, lstMonths.SelectedIndex - (visibleItems / 2) + 1);
            lstMonths.TopIndex = middleIndex; // 🔥 Scroll to center the selected item

            // Add the global mouse click detector
            Application.AddMessageFilter(new GlobalClickDetector(new Control[] { lstYear, lstMonths, lstDays, pnlDatePick }, HidePopups));
            HideScrollbar(lstYear);
            HideScrollbar(lstMonths);

            removeScroll(lstYear);
            removeScroll(lstMonths);

            this.ResumeLayout();
            lstYear.EndUpdate();
            lstMonths.EndUpdate();
            SlideDownPanel();
        }


        private void SlideDownPanel()
        {

            int maxHeight = pnlDatePick.Height;
            pnlDatePick.Height = 0;
            this.pnlDatePick.Visible = true;
            for (int i = 3; i < maxHeight; i += 4)
            {
                pnlDatePick.Height = i;
                //pnlDatePick.Invalidate();
                Application.DoEvents();
            }
            pnlDatePick.Height = maxHeight;

        }
        private void removeScroll(ListBox listBox)
        {
            listBox.MouseWheel += (s, e) => { HideScrollbar(listBox); ScrollWithoutScrollbar(listBox, e); };
            listBox.Resize += (s, e) => HideScrollbar(listBox);
            listBox.SelectedIndexChanged += (s, e) => HideScrollbar(listBox);
        }
        private void ScrollWithoutScrollbar(ListBox listBox, MouseEventArgs e)
        {
            int moveBy = e.Delta > 0 ? -1 : 1; // Scroll up or down
            int newIndex = Math.Max(0, Math.Min(listBox.TopIndex + moveBy, listBox.Items.Count - 1));
            listBox.TopIndex = newIndex; // Manually scroll the ListBox
        }


        private void HidePopups()
        {
            this.SuspendLayout();
            //lstYear.Visible = false;
            //lstMonths.Visible = false;
            lstDays.Visible = false;
            this.ResumeLayout();

            SlideUpPanel();
        }
        private void SlideUpPanel()
        {
            lstYear.BeginUpdate();
            lstMonths.BeginUpdate();
            lstDays.BeginUpdate();
            for (int i = pnlDatePick.Height; i > 0; i -= 12)
            {
                pnlDatePick.Height = i;
                //pnlDatePick.Invalidate();
                Application.DoEvents();
            }
            pnlDatePick.Visible = false;
            lstYear.EndUpdate();
            lstMonths.EndUpdate();
            lstDays.EndUpdate();
        }
        private void lstMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            int listboxWidth = pnlDatePick.Width / 3;
            int widthModulos = pnlDatePick.Width % 3;
            this.SuspendLayout();
            if (this.Tag.ToString() == "Loading") { return; }
            lstDays.Left = lstMonths.Left + listboxWidth + ((widthModulos > 1) ? widthModulos - 1 : 0); //placing days in 2/3rd of the Panel width from left
            lstDays.Width = listboxWidth - 3; //set the width to 1/3rd of the panel width
            lstDays.Height = pnlDatePick.Height - 4;
            lstDays.Visible = true;
            lstDays.Items.Clear();
            //find days in month
            int daysCount = new PersianDateTime(
                Convert.ToInt32(lstYear.SelectedItem.ToString()),
                monthIndex(lstMonths.SelectedItem.ToString()), 1).GetMonthDays;

            for (int i = 1; i <= daysCount; i++)
            {
                lstDays.Items.Add(i.ToString());
                if (Convert.ToInt32(lstDays.Items[lstDays.Items.Count - 1].ToString()) == new PersianDateTime(DateTime.Now).Day)
                {
                    lstDays.SelectedIndex = i - 1;

                }
            }

            // 🔥 Ensure the selected item is in the middle of the ListBox
            int visibleItems = lstDays.Height / lstDays.ItemHeight; // How many items fit
            int middleIndex = Math.Max(0, lstDays.SelectedIndex - (visibleItems / 2) + 2);
            lstDays.TopIndex = middleIndex; // 🔥 Scroll to center the selected item
            HideScrollbar(lstDays);
            removeScroll(lstDays);
            this.ResumeLayout();
        }
        int monthIndex(string monthName)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (monthName == PersianDateTime.GetPersianMonthName(i))
                    return i;
            }
            return -1;
        }

        private void lstDays_Click(object sender, EventArgs e)
        {
            HidePopups();
            DateTime selectedDate = new PersianDateTime(Convert.ToInt32(lstYear.SelectedItem.ToString()),
                                    monthIndex(lstMonths.SelectedItem.ToString()),
                                    Convert.ToInt32(lstDays.SelectedItem.ToString()));
            PersianDatePicker.DataSource = FillMonthView(selectedDate.Date);
            HighlightDate(PersianDatePicker, selectedDate.Date);
        }
    }
}