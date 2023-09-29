using StrelyCleaner.Core;
using StrelyCleaner.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XylonV2;

namespace StrelyCleaner.Controls
{
    public partial class DeviceControl : UserControl
    {
        public Action OnAction = null;

        public DeviceControl()
        {
            InitializeComponent();

        }

        public string MainDir = string.Empty;

        public void ScanDrive() {
            guna2HtmlToolTip1.SetToolTip(panel1, MainDir);
            IEnumerable<string> Files = FileDirSearcher.GetFilePaths(dirPath: MainDir, searchOption: SearchOption.TopDirectoryOnly, null, fileExtPatterns: new string[]
     {
                 "*.cmd", "*.com",  "*.js", "*.vbs", "*.wsf", "*.py", "*.ps1", "*.lnk", "*.reg",  "*.vbscript",
                "*.wsh", "*.hta", "*.pdf"
     }, ignoreCase: true, throwOnError: false);

            if (Files.Count() == 0) { guna2GradientButton2.Text = "Scan"; guna2GradientButton2.Checked = true;
                guna2HtmlToolTip1.SetToolTip(guna2GradientButton2, "Your device is clean.");
            } else { guna2GradientButton2.Text = "Clean"; guna2GradientButton2.Checked = false;
                guna2HtmlToolTip1.SetToolTip(guna2GradientButton2, "Threats found, Clean recommended.");
            }
        }

        public void SetName(string DataText) { label7.Text = DataText; }

        public void SetSize(string DataText) { label6.Text = DataText; }

        public void SetPorcent(int Data) { guna2ProgressBar1.Value = Data; }

        public void SetTooltip(string DataText) {
            guna2HtmlToolTip1.SetToolTip(label7, DataText);
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            OnAction?.Invoke();
        }

        public bool FixUSB = false;

        private void guna2CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            FixUSB = guna2CheckBox5.Checked;
        }

        public bool FixWithoutScan = false;

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            FixWithoutScan = guna2CheckBox1.Checked;
        }
    }
}
