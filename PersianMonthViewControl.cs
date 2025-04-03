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
using System.Linq;
using System.Drawing.Drawing2D;


namespace PersianMonthView
{

    public partial class PersianMonthViewControl : UserControl
    {
        private const int _gridControlWidthMargin = 10;
        private const int _gridControlHeightMargin = 61;
        private const int _labelControlWidthMargin = 10;
        private const int _labelControlMinHeight = 15;
        private const int _labelControlMaxHeight = 100;
        private const int _ColumnsNumber = 7;
        private const int _RowsNumber = 5;
        public PersianMonthViewControl()
        {
            InitializeComponent();
            //toolTip.
            _toolTip.SetToolTip(lnkLblPreviousYear, "سال قبل");
            _toolTip.SetToolTip(lnkLblPreviousMonth, "ماه قبل");
            _toolTip.SetToolTip(lnkLblNextMonth, "ماه بعد");
            _toolTip.SetToolTip(lnkLblNextYear, "سال بعد");
            PersianDatePicker.BackgroundColor = _defaultBackGroundColor;
            //_SecondBackColor= Color.Honeydew;
            // Prevent select by canceling selection
            PersianDatePicker.SelectionChanged += (s, eArgs) =>
            {
                PersianDatePicker.ClearSelection();
            };
            //
            PersianDatePicker.DataSource = FillMonthView(DateTime.Now);
            InitializePersianDGV(PersianDatePicker);
            PersianDatePicker.CellMouseEnter += PersianDatePicker_CellMouseEnter;
            PersianDatePicker.CellMouseLeave += PersianDatePicker_CellMouseLeave;
            PersianDatePicker.ClearSelection();

            lstYear.BeginUpdate();
            lstMonths.BeginUpdate();
            lstYear.Items.Clear();
            PersianDateTime currentDate = new PersianDateTime(DateTime.Now);
            string yearNow = currentDate.ToString("yyyy");

            for (int i = 10; i > -90; i--)
            {
                lstYear.Items.Add(currentDate.AddYears(i).ToString("yyyy"));
                if (lstYear.Items[lstYear.Items.Count - 1].ToString() == yearNow)
                    lstYear.SelectedIndex = 10 - i;
            }
            string monthNow = currentDate.ToString("MMMM");
            for (int i = 0; i < 12; i++)
            {
                lstMonths.Items.Add(currentDate.AddDays(-currentDate.GetDayOfYear + (i * 30) + 10).MonthName.ToString());
                if (lstMonths.Items[lstMonths.Items.Count - 1].ToString() == monthNow)
                    lstMonths.SelectedIndex = i;
            }

            PersianDatePicker.CellClick += PersianDatePicker_CellClick;
            PersianDatePicker.CellToolTipTextNeeded += PersianDatePicker_CellToolTipTextNeeded;
            lstYear.DrawItem += lstYear_DrawItem;
            lstMonths.DrawItem += lstMonths_DrawItem;
            lstDays.DrawItem += LstDays_DrawItem;
            lstYear.EndUpdate();
            lstMonths.EndUpdate();
        }
        public event EventHandler<CellClickedEventArgs> CellClicked;

        private void PersianDatePicker_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ) return;

            var cell = PersianDatePicker.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Value == null || cell.Value == DBNull.Value) return;
            string[] parts = cell.Value.ToString().Split('|');
            int persianDay=Convert.ToInt16(ConvertToRomanNumbers(parts.ElementAtOrDefault(0)));
            int persianMonth = selectedPersianDateTime.GetMonthEnum(selectedPersianDateTime.GetLongMonthName);
            int persianYear = selectedPersianDateTime.Year;
            int gregDay = 1;
            if (parts.ElementAtOrDefault(1).Length < 3)
                gregDay = Convert.ToInt16(parts.ElementAtOrDefault(1));
            int gregMonth = SelectedDateObject.Month;
            int gregYear = SelectedDateObject.Year;
            string persianDate = ConvertToPersianNumbers(persianYear + "/" + persianMonth + "/" + persianDay);
            string gregDate = gregYear + "/" + gregMonth + "/" + gregDay;
            string customToolTip = persianDate + "\n" + gregDate;
            e.ToolTipText = customToolTip;
        }

        private void PersianDatePicker_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Value == null) return;
            
            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);
            string[] parts = e.Value.ToString().Split('|');
            string mainDate = parts.ElementAtOrDefault(0);
            string blGregDate = parts.ElementAtOrDefault(1);
            string brHijriDate = parts.ElementAtOrDefault(2);
            Rectangle rect = e.CellBounds;
            var mainFont = new Font("Tahoma", 12, FontStyle.Bold);
            var smallFont = new Font("Tahoma", 7, FontStyle.Regular);
            var brush = new SolidBrush(e.CellStyle.ForeColor);

            Pen gridLinePen = new Pen(new SolidBrush(e.CellStyle.ForeColor), 0.5f);
            Pen highLightgBorderPen = new Pen(new SolidBrush(Color.PaleGreen), 1);
            if (e.RowIndex==0) 
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right, e.CellBounds.Top);
            Color mainColor = e.CellStyle.ForeColor;
            Color blColor = e.CellStyle.ForeColor;
            Point rectCenter = OffsetObjectFromCenter(rect, mainDate, mainFont, 0, 0);
            
                
            if (e.ColumnIndex == 6) mainColor = Color.Green;
            if (blGregDate != null && blGregDate.Length > 3) // > 3 new year is 4 digit
            {
                rectCenter = OffsetObjectFromCenter(rect, mainDate, mainFont, 4, 0); //move text 4 pixels to right!
                blColor = Color.DarkGreen;
            }
            else if (blGregDate != null && blGregDate.Length == 3) //short month names are always 3 letters
            {
                rectCenter = OffsetObjectFromCenter(rect, mainDate, mainFont, 2, 0); //move text 2 pixels to right!
                blColor = Color.Purple;
            }
            //////////Alternative to PlaceHolder:///////////////
            //TextRenderer.DrawText(e.Graphics, main, mainFont,
            //rect, e.CellStyle.ForeColor, TextFormatFlags.HorizontalCenter + 1 | TextFormatFlags.VerticalCenter);
            ////////////////////////////////////////////////////

            TextRenderer.DrawText(e.Graphics, mainDate, mainFont, rectCenter, mainColor);
            TextRenderer.DrawText(e.Graphics, blGregDate, smallFont,
                new Rectangle(rect.Left + 1, rect.Bottom - 16, rect.Width / 2, 15),
                blColor, TextFormatFlags.Bottom | TextFormatFlags.Left);

            TextRenderer.DrawText(e.Graphics, brHijriDate, smallFont,
                new Rectangle(rect.Right - rect.Width / 2, rect.Bottom - 15, rect.Width / 2 - 2, 15),
                e.CellStyle.ForeColor, TextFormatFlags.Bottom | TextFormatFlags.Right);
        }
        /// <summary>
        /// Aligns the obj center to bound center and moves it 
        /// horizontally by Xoffset and vertically by Yoffset.
        /// </summary>
        /// <param name="bound">
        /// The boundary within the object will be centered
        /// </param>
        /// <param name="obj">
        /// The string object to center
        /// </param>
        /// <param name="font">
        /// The font which Obj is rendering the string
        /// </param>
        /// <param name="Xoffset">
        /// Horizontal offset from bound width center - Negative values move obj to left [default = 0]
        /// </param>
        /// <param name="Yoffset">
        /// Vertical offset from bound height center - Negative values move obj up [default = 0]
        /// </param>
        /// <returns></returns>

        Point OffsetObjectFromCenter(Rectangle bound, string obj, Font font, int Xoffset = 0, int Yoffset = 0)
        {
            int rectCenterX = Convert.ToInt16(bound.X + bound.Width / 2);
            int rectCenterY = Convert.ToInt16(bound.Y + bound.Height / 2);
            int objCenterX = Convert.ToInt16(TextRenderer.MeasureText(obj, font).Width / 2);
            int objCenterY = Convert.ToInt16(TextRenderer.MeasureText(obj, font).Height / 2);

            return new Point(rectCenterX - objCenterX + Xoffset, rectCenterY - objCenterY + Yoffset);
        }

        private void PersianDatePicker_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = PersianDatePicker.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (cell.Value == null || cell.Value == DBNull.Value) return;
            string[] parts = cell.Value.ToString().Split('|');
            //if (!int.TryParse(parts.ElementAtOrDefault(0), out int day)) return;
            //var current = LongStringToPersianDateTime(lblSelectedDate.Text);
            //var selected = new PersianDateTime(current.Year, current.Month, day);
            // highlight and internal updates
            //HighlightDate(PersianDatePicker, selected.ToDateTime());
            //  Raise custom event
            CellClicked?.Invoke(this, 
                new CellClickedEventArgs
                (e.RowIndex, e.ColumnIndex, cell.Value, selectedPersianDateTime, SelectedDateObject)
                );
        }

    }
}