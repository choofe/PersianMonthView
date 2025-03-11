using MD.PersianDateTime;

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PersianMonthView
{

    public partial class PersianMonthViewControl : UserControl
    {
        private ToolTip _toolTip = new ToolTip();

        public PersianMonthViewControl()
        {
            InitializeComponent();
            //_toolTip.
            _toolTip.SetToolTip(lnkLblPreviousYear, "سال قبل");
            _toolTip.SetToolTip(lnkLblPreviousMonth, "ماه قبل");
            _toolTip.SetToolTip(lnkLblNextMonth, "ماه بعد");
            _toolTip.SetToolTip(lnkLblNextYear, "سال بعد");
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
                lstMonths.Items.Add(currentDate.AddDays(-currentDate.GetDayOfYear + (i * 30)+10).MonthName.ToString());
                if (lstMonths.Items[lstMonths.Items.Count - 1].ToString() == monthNow)
                {
                    lstMonths.SelectedIndex = i;

                }
            }

            //lstYear.TopIndex = lstYear.SelectedIndex;

            lstYear.DrawItem += lstYear_DrawItem;
            lstMonths.DrawItem += lstMonths_DrawItem;
            lstDays.DrawItem += LstDays_DrawItem;

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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);


        }
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            int newGridWidth = Convert.ToInt32(Math.Round((this.Width - _gridControlWidthMargin - 3) / 7f) * 7);
            int thisWidth = newGridWidth + _gridControlWidthMargin + 3;
            int newGridHeight = Convert.ToInt32(Math.Round((this.Height - _gridControlHeightMargin - PersianDatePicker.ColumnHeadersHeight - 2) / 5f) * 5);
            int thisHeight = newGridHeight + PersianDatePicker.ColumnHeadersHeight + _gridControlHeightMargin + 2;
            this.Size = new Size(thisWidth, thisHeight);
            PersianDatePicker.Width = this.Width - _gridControlWidthMargin;
            PersianDatePicker.Height = this.Height - _gridControlHeightMargin;
            PersianDatePicker.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            int rowHeight = (PersianDatePicker.Height - 3) / 5;
            PersianDatePicker.RowTemplate.Height = (PersianDatePicker.Height - PersianDatePicker.ColumnHeadersHeight - 2) / 5;
            lblSelectedDate.MaximumSize = new Size(this.Width - _labelControlWidthMargin, _labelControlMaxHeight);
            lblSelectedDate.MinimumSize = new Size(this.Width - _labelControlWidthMargin, _labelControlMinHeight);
            lblSelectedDate.Width = this.Width - _labelControlWidthMargin;

            base.Update();
            base.Refresh();
            base.Invalidate();
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


        //private DataGridView PersianDatePicker; // The calendar grid
        //private Label lblSelectedDate;
        //private LinkLabel lnkLblPreviousYear, lnkLblPreviousMonth, lnkLblNextMonth, lnkLblNextYear, lnklblToday;




        //public DataTable FillMonthView(DateTime _date)
        //{
        //    currentDate = new PersianDateTime(_date);
        //    lblSelectedDate.Text = $"{currentDate.GetLongDayOfWeekName} {currentDate.Day} {currentDate.GetLongMonthName} {currentDate.Year}";

        //    var monthDays = currentDate.GetMonthDays;
        //    var monthFirstDay = currentDate.GetPersianDateOfLastDayOfMonth().AddDays(1 - monthDays);
        //    var firstDayofweekIndex = Convert.ToInt32(monthFirstDay.PersianDayOfWeek);

        //    DataTable _month = new DataTable();

        //    // Create Columns for Persian Week Days
        //    for (int i = 0; i < 7; i++)
        //    {
        //        var weekDayName = new PersianDateTime(currentDate.GetFirstDayOfWeek().ToDateTime());
        //        _month.Columns.Add(new DataColumn
        //        {
        //            DataType = typeof(int),
        //            ColumnName = weekDayName.DayOfWeek.ToString(),
        //            Caption = weekDayName.GetLongDayOfWeekName
        //        });
        //    }

        //    for (int i = 0; i < 5; i++) _month.Rows.Add();

        //    // Place Day Numbers in Correct Cells
        //    for (int i = 0; i < monthDays; i++)
        //    {
        //        int row = (i + firstDayofweekIndex) / 7;
        //        int column = (i + firstDayofweekIndex) % 7;
        //        _month.Rows[row][column] = i + 1;
        //    }

        //    return _month;
        //}
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
                // label text
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

            this.Tag="Loaded";

            // Perform any logic that requires the control to be fully loaded
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


        }
        private void LstDays_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                // Convert English numbers to Persian numbers before drawing
                string persianText = ConvertToPersianNumbers(lstDays.Items[e.Index].ToString());

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(persianText, e.Font, textBrush, e.Bounds);
                }

            }

            e.DrawFocusRectangle();

        }
        private void lstMonths_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                
                string persianText = lstMonths.Items[e.Index].ToString();

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(persianText, e.Font, textBrush, e.Bounds);
                }
            }

            e.DrawFocusRectangle();
        }
        private void lstYear_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                // Convert English numbers to Persian numbers before drawing
                string persianText = ConvertToPersianNumbers(lstYear.Items[e.Index].ToString());

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(persianText, e.Font, textBrush, e.Bounds);
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
            lstYear.Height = 200;
            lstYear.Visible = true;
            // 🔥 Ensure the selected item is in the middle of the ListBox
            int visibleItems = lstYear.Height / lstYear.ItemHeight; // How many items fit
            int middleIndex = Math.Max(0, lstYear.SelectedIndex - (visibleItems / 2) + 2);
            lstYear.TopIndex = middleIndex; // 🔥 Scroll to center the selected item

            lstMonths.Height = 200;
            lstMonths.Visible = true;

            visibleItems = lstMonths.Height / lstYear.ItemHeight; // How many items fit
            middleIndex = Math.Max(0, lstMonths.SelectedIndex - (visibleItems / 2) + 2);
            lstMonths.TopIndex = middleIndex; // 🔥 Scroll to center the selected item

            // Add the global mouse click detector
            Application.AddMessageFilter(new GlobalClickDetector(new Control[] { lstYear, lstMonths, lstDays }, HidePopups));
            this.ResumeLayout();
        }
        


        private void HidePopups()
        {
            this.SuspendLayout();
            lstYear.Visible = false;
            lstMonths.Visible = false;
            lstDays.Visible = false;
            this.ResumeLayout();
        }

        private void lstMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();
            if (this.Tag.ToString() == "Loading") { return; }
            lstDays.Height = 200;
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
    }
}