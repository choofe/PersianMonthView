using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            private set => base.Tag = value; //  derived classes can't modify it
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
    }
}
