using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersianMonthView
{
    public partial class PersianMonthViewControl : UserControl
    {
        private void ResizeGridView()
        {
            if (PersianDatePicker.ColumnCount == 0 || PersianDatePicker.RowCount == 0)
                return;

            int columnWidth = PersianDatePicker.Width / _ColumnsNumber;
            int rowHeight = (PersianDatePicker.Height - PersianDatePicker.ColumnHeadersHeight) / _RowsNumber;

            foreach (DataGridViewColumn col in PersianDatePicker.Columns)
                col.Width = columnWidth;
            foreach (DataGridViewRow row in PersianDatePicker.Rows)
                row.Height = rowHeight;

            PersianDatePicker.RowTemplate.Height = rowHeight; // Ensure RowTemplate height is applied
            PersianDatePicker.Invalidate();
            PersianDatePicker.Refresh();
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
                    DataType = typeof(string),
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
            ///comment out for testing Miladi date
            //for (int i = 0; i < monthDays; i++)
            //{
            //    if (i + firstDayofweekIndex < 35)
            //    {
            //        var row = (i + firstDayofweekIndex) / 7;
            //        var column = (i + firstDayofweekIndex) % 7;
            //        _month.Rows[row][column] = i + 1;
            //    }
            //    else
            //    {
            //        var row = (i + firstDayofweekIndex - 35) / 7;
            //        var column = (i + firstDayofweekIndex - 35) % 7;
            //        _month.Rows[row][column] = i + 1;
            //    }
            //}
            DateTime gregDateDay = monthFirstDay.ToDateTime();
            lblGregMonth.Text = gregDateDay.ToString("MMM", CultureInfo.InvariantCulture) + '-' +
                gregDateDay.AddMonths(1).ToString("MMM", CultureInfo.InvariantCulture) + " " +
                gregDateDay.AddMonths(1).Year.ToString();
            for (int i = 0; i < monthDays; i++)
            {
                string persianDayString = ConvertToPersianNumbers((i + 1).ToString());
                string gregDayString = (gregDateDay.Day).ToString();
                
                if (gregDayString=="1" ) gregDayString = gregDateDay.ToString("MMM", CultureInfo.InvariantCulture);
                if (gregDayString == "Jan") gregDayString = gregDateDay.AddMonths(1).Year.ToString();
                string dayString = persianDayString + '|' + gregDayString;
                if (i + firstDayofweekIndex < 35)
                {
                    var row = (i + firstDayofweekIndex) / 7;
                    var column = (i + firstDayofweekIndex) % 7;
                    _month.Rows[row][column] = dayString;//(i + 1).ToString() + "|" + (gregDateDay.Day).ToString();
                }
                else
                {
                    var row = (i + firstDayofweekIndex - 35) / 7;
                    var column = (i + firstDayofweekIndex - 35) % 7;
                    _month.Rows[row][column] = dayString;
                }

                gregDateDay = gregDateDay.AddDays(1);
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
                    if (cell.Value != null && int.TryParse(ConvertToRomanNumbers(cell.Value.ToString()).Split('|').ElementAtOrDefault(0), out int day) && day == selectedDay)
                    {
                        cell.Style.BackColor = _dateHighLightColor;
                        cell.Style.ForeColor = Color.Black;
                        cell.Style.Font = new Font("Tahoma", 14f, FontStyle.Bold);
                        
                        return; // Exit loop after finding the date
                    }
                }
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

        private string ConvertToPersianNumbers(string input)
        {
            char[] persianDigits = { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
            for (int i = 0; i < 10; i++)
            {
                input = input.Replace(i.ToString(), persianDigits[i].ToString());
            }
            return input;
        }
        private string ConvertToRomanNumbers(string input)
        {
            char[] persianDigits = { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
            char[] romanDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (int i = 0; i < 10; i++)
            {
                input = input.Replace(persianDigits[i].ToString(), romanDigits[i].ToString());
            }
            return input;
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
