using StrelyCleaner.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrelyCleaner.Controls
{
    public partial class OptionContainer : UserControl
    {

        public ControlLister Listener_Sys = new ControlLister { OrientationControls = Orientation.Horizontal, Margen = new Point(5, 10) };

        public int YMax = 200;

        private int baseY;
        public OptionContainer()
        {
            InitializeComponent();
            baseY = panel1.Height;
            panel1.Height = baseY - 1;
        }

        private void guna2Button2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2Button2.Checked) { this.Height = YMax; } else {  this.Height = 23; }
        }



        public void AddCheck(Control ControlEx, bool LimitLocation = false) {
            Listener_Sys.Add(PanelContainer, ControlEx, LimitLocation);
        }

        public void SetTitle(string TextTitle)
        {
            label2.Text = TextTitle;
        }

    }
}
