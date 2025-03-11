using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersianMonthView
{
    public class GlobalClickDetector : IMessageFilter
    {
        private readonly Control[] _popupControls; // The ListBoxes (or any popups)
        private readonly Action _onClickOutside; // Action to perform when clicked outside

        public GlobalClickDetector(Control[] popupControls, Action onClickOutside)
        {
            _popupControls = popupControls;
            _onClickOutside = onClickOutside;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x201) // WM_LBUTTONDOWN (Left Mouse Button Click)
            {
                Point clickPosition = Control.MousePosition;

                // 🔥 Check if click is inside ANY of the popups
                foreach (var control in _popupControls)
                {
                    if (control.Bounds.Contains(control.Parent.PointToClient(clickPosition)))
                    {
                        return false; // Click is inside, DO NOT close popups
                    }
                }

                // If click is outside all popups, hide them
                _onClickOutside?.Invoke();
                return false;
            }
            return false;
        }
    }


}
