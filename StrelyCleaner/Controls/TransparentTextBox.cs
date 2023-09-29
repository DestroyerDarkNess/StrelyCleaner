using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrelyCleaner.Controls
{
    public class TransparentTextBox : TextBox //Guna.UI2.WinForms.Guna2TextBox
    {

        public TransparentTextBox() {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            
        }

    }
}
