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
    }
}
