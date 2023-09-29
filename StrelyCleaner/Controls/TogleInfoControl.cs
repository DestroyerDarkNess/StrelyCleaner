using StrelyCleaner.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrelyCleaner.Controls
{
    public partial class TogleInfoControl : UserControl
    {
        public TogleInfoControl()
        {
            InitializeComponent();
        }

        public Action OnAction = null;

        public Action OffAction = null;

        public Func<bool> GetFunc = null;

        public Color OnColor = Color.DarkGreen; //Color.DarkSlateGray;
        public Color OffColor = Color.Red;

        private  void guna2ToggleSwitch1_Click(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked == true) {
                ExecuteAsync(OnAction);
            } else {
                ExecuteAsync(OffAction);
            }

            Utilities.Sleep(1);

            UpdateValue();
        }

        public void UpdateValue() {
            bool ReturnVal = (bool)GetFunc.Invoke();
            guna2ToggleSwitch1.Checked = ReturnVal;
        }

        private void ExecuteAsync(Action Action) {

            if (Action != null) {
                Thread t = new Thread(Action.Invoke);
                t.Priority = ThreadPriority.Highest;
                t.Start();
            }

        }

        public void SetName(string Nick) { label2.Text = Nick; }
        public void SetDescription(string Des) { label1.Text = Des; }
        public void SetValue(bool Check) { guna2ToggleSwitch1.Checked = Check; }
        public void SetColor(Color ColorEx) { guna2GradientPanel1.FillColor2 = ColorEx; }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked == true) { guna2GradientPanel1.FillColor2 = OnColor; } else { guna2GradientPanel1.FillColor2 = OffColor; }
        }
    }
}
