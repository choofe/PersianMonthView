using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersianMonthView
{
    public partial class PersianMonthViewControl : UserControl
    {
        private Color previousColor; // Store the original color

        private void PersianDatePicker_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 )
            {
                DataGridViewCell cell = PersianDatePicker.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value == null || cell.Value == DBNull.Value) { return; }
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
                if (cell.Value == null || cell.Value == DBNull.Value) { return; }
                // Restore the stored original color
                cell.Style.BackColor = previousColor;
            }
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
                { cellValue = Int16.Parse(ConvertPersianToRomanNumbers(PersianDatePicker.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Value.ToString()).Split('|').ElementAtOrDefault(0)); }
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
        private void lblSelectedDate_Click(object sender, EventArgs e)
        {
            ShowPopup();

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
