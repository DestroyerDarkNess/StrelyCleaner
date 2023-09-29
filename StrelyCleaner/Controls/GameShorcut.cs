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
    public partial class GameShorcut : UserControl
    {

        public string DataTag { get; set; }
        public GameShorcut()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (this.Enabled == true) OnClick(e);
        }

        public void SetName(string NameText) { guna2Button1.Text = NameText; }
        public void SetIcon(Image Icon) { guna2Button1.Image = Icon; }

        public void SetTooltip(string TooltipText) { guna2HtmlToolTip1.SetToolTip( guna2Button1, TooltipText); }

    }
}
