using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersianMonthView
{
    public partial class PersianMonthViewControl : UserControl
    {
        private const int _gridControlWidthMargin = 10;
        private const int _gridControlHeightMargin = 83;
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
            lblSelectedDate.Width = this.Width -150- _labelControlWidthMargin;
            lblSelectedDate.Left =thisWidth/2-lblSelectedDate.Width/2;
            lblHijriMonthYear.Left = this.Width - 190;
            NavigateLocationUpdate();
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
                //this.Invalidate();
                //this.Refresh();
                //this.Update();
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.DoubleBuffered = true;
            this.Tag = "Loaded";
            // 🔥 Ensure styles & layout are applied immediately
            PersianDatePicker.CellPainting += PersianDatePicker_CellPainting;
            ResizeGridView();
            HighlightDate(PersianDatePicker, DateTime.Now);
            this.Invalidate();
            this.Refresh();
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            // at this point, all designer-set properties are applied
            Invalidate(); // or rerun any Hijri-date logic
        }

    }
}
