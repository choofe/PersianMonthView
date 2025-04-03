using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianMonthView
{
    public class CellClickedEventArgs
    {
        public int RowIndex { get; }
        public int ColumnIndex { get; }
        public object Value { get; }
        public PersianDateTime PersianDate { get; }
        public DateTime GregorianDate { get; }
        public CellClickedEventArgs(int row, int col, object value, PersianDateTime persian,DateTime gregorian)
        {
            RowIndex = row;
            ColumnIndex = col;
            Value = value;
            PersianDate = persian;
            GregorianDate = gregorian;
        }
    }
}
