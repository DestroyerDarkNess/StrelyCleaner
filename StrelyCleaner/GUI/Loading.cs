using Guna.UI2.WinForms;
using StrelyCleaner.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StrelyCleaner.GUI
{
    public partial class Loading : Form
    {
        public Loading()
        {
            
            InitializeComponent();
        }

       public Point NewLocation;

        private void Loading_Load(object sender, EventArgs e)
        {

        }

        private void Loading_Shown(object sender, EventArgs e)
        {
            this.Location = NewLocation;
        }

        public void ClosedSplash() {
            this.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }

    }
}
