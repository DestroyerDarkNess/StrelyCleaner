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
    public partial class CustomItemClick : UserControl
    {
        public CustomItemClick()
        {
            InitializeComponent();
        }


        public void SetIcon(Image IconEx) { panel1.BackgroundImage = IconEx;  }

        public void SetText(string TextEx) { label5.Text = TextEx; }

        public void SetSubInfo1(string TextEx) { label7.Text = TextEx; label7.Visible = true; }
        public void SetSubInfo2(string TextEx) { label6.Text = TextEx; label6.Visible = true; }

        private void label5_Click(object sender, EventArgs e)
        {
            if (this.Enabled == true)  OnClick(e); 
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            if (this.Enabled == true) OnMouseHover(e);
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            if (this.Enabled == true) OnMouseLeave(e);
        }
    }
}
