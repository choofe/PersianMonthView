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

            lstYear.DrawItem += lstYear_DrawItem;
            lstMonths.DrawItem += lstMonths_DrawItem;
            lstDays.DrawItem += LstDays_DrawItem;
            lstYear.EndUpdate();
            lstMonths.EndUpdate();
        }

    }
}