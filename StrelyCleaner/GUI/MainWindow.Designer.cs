using StrelyCleaner.Helpers;

namespace StrelyCleaner.GUI
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2VScrollBar1 = new Guna.UI2.WinForms.Guna2VScrollBar();
            this.PanelLayout = new System.Windows.Forms.Panel();
            this.CleanerButton = new Guna.UI2.WinForms.Guna2Button();
            this.FrameRateLabel = new System.Windows.Forms.Label();
            this.SettingsButton = new Guna.UI2.WinForms.Guna2Button();
            this.AntivirusButton = new Guna.UI2.WinForms.Guna2Button();
            this.TweatsButton = new Guna.UI2.WinForms.Guna2Button();
            this.OptimizerButton = new Guna.UI2.WinForms.Guna2Button();
            this.HardwareButton = new Guna.UI2.WinForms.Guna2Button();
            this.guna2VSeparator1 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.PanelContainer = new System.Windows.Forms.Panel();
            this.PanelHeader = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.guna2ControlBox2 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2Elipse2 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2HtmlToolTip1 = new Guna.UI2.WinForms.Guna2HtmlToolTip();
            this.guna2DragControl2 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.guna2DragControl3 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.guna2DragControl4 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.guna2ContextMenuStrip1 = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
            this.guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
            this.guna2Panel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PanelLayout.SuspendLayout();
            this.PanelHeader.SuspendLayout();
            this.guna2ContextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.guna2Panel1.BorderColor = System.Drawing.Color.White;
            this.guna2Panel1.BorderRadius = 5;
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.panel1);
            this.guna2Panel1.Controls.Add(this.PanelLayout);
            this.guna2Panel1.Controls.Add(this.PanelContainer);
            this.guna2Panel1.Controls.Add(this.PanelHeader);
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(886, 479);
            this.guna2Panel1.TabIndex = 1;
            this.guna2Panel1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.guna2VScrollBar1);
            this.panel1.Location = new System.Drawing.Point(858, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(27, 454);
            this.panel1.TabIndex = 4;
            // 
            // guna2VScrollBar1
            // 
            this.guna2VScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2VScrollBar1.FillColor = System.Drawing.Color.Transparent;
            this.guna2VScrollBar1.InUpdate = false;
            this.guna2VScrollBar1.LargeChange = 10;
            this.guna2VScrollBar1.Location = new System.Drawing.Point(2, 0);
            this.guna2VScrollBar1.Name = "guna2VScrollBar1";
            this.guna2VScrollBar1.ScrollbarSize = 21;
            this.guna2VScrollBar1.Size = new System.Drawing.Size(21, 454);
            this.guna2VScrollBar1.SmallChange = 4;
            this.guna2VScrollBar1.TabIndex = 3;
            this.guna2VScrollBar1.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))));
            this.guna2VScrollBar1.VisibleChanged += new System.EventHandler(this.guna2VScrollBar1_VisibleChanged);
            // 
            // PanelLayout
            // 
            this.PanelLayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelLayout.BackColor = System.Drawing.Color.Transparent;
            this.PanelLayout.Controls.Add(this.CleanerButton);
            this.PanelLayout.Controls.Add(this.FrameRateLabel);
            this.PanelLayout.Controls.Add(this.SettingsButton);
            this.PanelLayout.Controls.Add(this.AntivirusButton);
            this.PanelLayout.Controls.Add(this.TweatsButton);
            this.PanelLayout.Controls.Add(this.OptimizerButton);
            this.PanelLayout.Controls.Add(this.HardwareButton);
            this.PanelLayout.Controls.Add(this.guna2VSeparator1);
            this.PanelLayout.Location = new System.Drawing.Point(1, 24);
            this.PanelLayout.Name = "PanelLayout";
            this.PanelLayout.Size = new System.Drawing.Size(60, 454);
            this.PanelLayout.TabIndex = 1;
            // 
            // CleanerButton
            // 
            this.CleanerButton.BorderColor = System.Drawing.Color.DimGray;
            this.CleanerButton.BorderRadius = 5;
            this.CleanerButton.BorderThickness = 1;
            this.CleanerButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.CleanerButton.CheckedState.BorderColor = System.Drawing.Color.White;
            this.CleanerButton.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))), ((int)(((byte)(150)))));
            this.CleanerButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.CleanerButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.CleanerButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.CleanerButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.CleanerButton.FillColor = System.Drawing.Color.Transparent;
            this.CleanerButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CleanerButton.ForeColor = System.Drawing.Color.White;
            this.CleanerButton.Image = ((System.Drawing.Image)(resources.GetObject("CleanerButton.Image")));
            this.CleanerButton.ImageSize = new System.Drawing.Size(25, 25);
            this.CleanerButton.Location = new System.Drawing.Point(11, 180);
            this.CleanerButton.Name = "CleanerButton";
            this.CleanerButton.Size = new System.Drawing.Size(38, 39);
            this.CleanerButton.TabIndex = 6;
            this.CleanerButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.guna2HtmlToolTip1.SetToolTip(this.CleanerButton, "Cleaner");
            this.CleanerButton.CheckedChanged += new System.EventHandler(this.UINavButtons_CheckedChanged);
            // 
            // FrameRateLabel
            // 
            this.FrameRateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameRateLabel.BackColor = System.Drawing.Color.Transparent;
            this.FrameRateLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FrameRateLabel.ForeColor = System.Drawing.Color.White;
            this.FrameRateLabel.Location = new System.Drawing.Point(3, 392);
            this.FrameRateLabel.Name = "FrameRateLabel";
            this.FrameRateLabel.Size = new System.Drawing.Size(54, 55);
            this.FrameRateLabel.TabIndex = 5;
            this.FrameRateLabel.Text = "CPU";
            this.FrameRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsButton
            // 
            this.SettingsButton.BorderColor = System.Drawing.Color.DimGray;
            this.SettingsButton.BorderRadius = 5;
            this.SettingsButton.BorderThickness = 1;
            this.SettingsButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.SettingsButton.CheckedState.BorderColor = System.Drawing.Color.White;
            this.SettingsButton.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))), ((int)(((byte)(150)))));
            this.SettingsButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.SettingsButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.SettingsButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.SettingsButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.SettingsButton.FillColor = System.Drawing.Color.Transparent;
            this.SettingsButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SettingsButton.ForeColor = System.Drawing.Color.White;
            this.SettingsButton.Image = ((System.Drawing.Image)(resources.GetObject("SettingsButton.Image")));
            this.SettingsButton.ImageSize = new System.Drawing.Size(25, 25);
            this.SettingsButton.Location = new System.Drawing.Point(11, 292);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(38, 39);
            this.SettingsButton.TabIndex = 4;
            this.SettingsButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.guna2HtmlToolTip1.SetToolTip(this.SettingsButton, "Settings");
            this.SettingsButton.CheckedChanged += new System.EventHandler(this.UINavButtons_CheckedChanged);
            // 
            // AntivirusButton
            // 
            this.AntivirusButton.BorderColor = System.Drawing.Color.DimGray;
            this.AntivirusButton.BorderRadius = 5;
            this.AntivirusButton.BorderThickness = 1;
            this.AntivirusButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.AntivirusButton.CheckedState.BorderColor = System.Drawing.Color.White;
            this.AntivirusButton.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))), ((int)(((byte)(150)))));
            this.AntivirusButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.AntivirusButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.AntivirusButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.AntivirusButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.AntivirusButton.FillColor = System.Drawing.Color.Transparent;
            this.AntivirusButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AntivirusButton.ForeColor = System.Drawing.Color.White;
            this.AntivirusButton.Image = ((System.Drawing.Image)(resources.GetObject("AntivirusButton.Image")));
            this.AntivirusButton.ImageSize = new System.Drawing.Size(30, 30);
            this.AntivirusButton.Location = new System.Drawing.Point(11, 68);
            this.AntivirusButton.Name = "AntivirusButton";
            this.AntivirusButton.Size = new System.Drawing.Size(38, 39);
            this.AntivirusButton.TabIndex = 3;
            this.AntivirusButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.guna2HtmlToolTip1.SetToolTip(this.AntivirusButton, "Antivirus");
            this.AntivirusButton.CheckedChanged += new System.EventHandler(this.UINavButtons_CheckedChanged);
            // 
            // TweatsButton
            // 
            this.TweatsButton.BorderColor = System.Drawing.Color.DimGray;
            this.TweatsButton.BorderRadius = 5;
            this.TweatsButton.BorderThickness = 1;
            this.TweatsButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.TweatsButton.CheckedState.BorderColor = System.Drawing.Color.White;
            this.TweatsButton.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))), ((int)(((byte)(150)))));
            this.TweatsButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.TweatsButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.TweatsButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.TweatsButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.TweatsButton.FillColor = System.Drawing.Color.Transparent;
            this.TweatsButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TweatsButton.ForeColor = System.Drawing.Color.White;
            this.TweatsButton.Image = ((System.Drawing.Image)(resources.GetObject("TweatsButton.Image")));
            this.TweatsButton.ImageSize = new System.Drawing.Size(25, 25);
            this.TweatsButton.Location = new System.Drawing.Point(11, 237);
            this.TweatsButton.Name = "TweatsButton";
            this.TweatsButton.Size = new System.Drawing.Size(38, 39);
            this.TweatsButton.TabIndex = 2;
            this.TweatsButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.guna2HtmlToolTip1.SetToolTip(this.TweatsButton, "System Optimization Tweats");
            this.TweatsButton.CheckedChanged += new System.EventHandler(this.UINavButtons_CheckedChanged);
            // 
            // OptimizerButton
            // 
            this.OptimizerButton.BorderColor = System.Drawing.Color.DimGray;
            this.OptimizerButton.BorderRadius = 5;
            this.OptimizerButton.BorderThickness = 1;
            this.OptimizerButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.OptimizerButton.CheckedState.BorderColor = System.Drawing.Color.White;
            this.OptimizerButton.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))), ((int)(((byte)(150)))));
            this.OptimizerButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.OptimizerButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.OptimizerButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.OptimizerButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.OptimizerButton.FillColor = System.Drawing.Color.Transparent;
            this.OptimizerButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.OptimizerButton.ForeColor = System.Drawing.Color.White;
            this.OptimizerButton.Image = ((System.Drawing.Image)(resources.GetObject("OptimizerButton.Image")));
            this.OptimizerButton.ImageSize = new System.Drawing.Size(25, 25);
            this.OptimizerButton.Location = new System.Drawing.Point(11, 124);
            this.OptimizerButton.Name = "OptimizerButton";
            this.OptimizerButton.Size = new System.Drawing.Size(38, 39);
            this.OptimizerButton.TabIndex = 1;
            this.OptimizerButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.guna2HtmlToolTip1.SetToolTip(this.OptimizerButton, "Booster");
            this.OptimizerButton.CheckedChanged += new System.EventHandler(this.UINavButtons_CheckedChanged);
            // 
            // HardwareButton
            // 
            this.HardwareButton.BorderColor = System.Drawing.Color.DimGray;
            this.HardwareButton.BorderRadius = 5;
            this.HardwareButton.BorderThickness = 1;
            this.HardwareButton.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.HardwareButton.CheckedState.BorderColor = System.Drawing.Color.White;
            this.HardwareButton.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))), ((int)(((byte)(150)))));
            this.HardwareButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.HardwareButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.HardwareButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.HardwareButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.HardwareButton.FillColor = System.Drawing.Color.Transparent;
            this.HardwareButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.HardwareButton.ForeColor = System.Drawing.Color.White;
            this.HardwareButton.Image = ((System.Drawing.Image)(resources.GetObject("HardwareButton.Image")));
            this.HardwareButton.ImageSize = new System.Drawing.Size(30, 30);
            this.HardwareButton.Location = new System.Drawing.Point(11, 12);
            this.HardwareButton.Name = "HardwareButton";
            this.HardwareButton.Size = new System.Drawing.Size(38, 39);
            this.HardwareButton.TabIndex = 0;
            this.HardwareButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.guna2HtmlToolTip1.SetToolTip(this.HardwareButton, "Hardware");
            this.HardwareButton.CheckedChanged += new System.EventHandler(this.UINavButtons_CheckedChanged);
            // 
            // guna2VSeparator1
            // 
            this.guna2VSeparator1.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2VSeparator1.Location = new System.Drawing.Point(58, 0);
            this.guna2VSeparator1.Name = "guna2VSeparator1";
            this.guna2VSeparator1.Size = new System.Drawing.Size(2, 454);
            this.guna2VSeparator1.TabIndex = 0;
            // 
            // PanelContainer
            // 
            this.PanelContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelContainer.AutoScroll = true;
            this.PanelContainer.BackColor = System.Drawing.Color.Transparent;
            this.PanelContainer.Location = new System.Drawing.Point(60, 24);
            this.PanelContainer.Name = "PanelContainer";
            this.PanelContainer.Size = new System.Drawing.Size(824, 454);
            this.PanelContainer.TabIndex = 2;
            this.PanelContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelContainer_Paint);
            // 
            // PanelHeader
            // 
            this.PanelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelHeader.BorderRadius = 3;
            this.PanelHeader.Controls.Add(this.guna2ControlBox2);
            this.PanelHeader.Controls.Add(this.guna2ControlBox1);
            this.PanelHeader.Controls.Add(this.label2);
            this.PanelHeader.Controls.Add(this.label1);
            this.PanelHeader.Controls.Add(this.panel2);
            this.PanelHeader.Controls.Add(this.label3);
            this.PanelHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(21)))), ((int)(((byte)(80)))));
            this.PanelHeader.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(80)))));
            this.PanelHeader.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.PanelHeader.Location = new System.Drawing.Point(1, 2);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(884, 22);
            this.PanelHeader.TabIndex = 0;
            // 
            // guna2ControlBox2
            // 
            this.guna2ControlBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox2.ControlBoxStyle = Guna.UI2.WinForms.Enums.ControlBoxStyle.Custom;
            this.guna2ControlBox2.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.guna2ControlBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2ControlBox2.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox2.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox2.Location = new System.Drawing.Point(834, 0);
            this.guna2ControlBox2.Name = "guna2ControlBox2";
            this.guna2ControlBox2.Size = new System.Drawing.Size(25, 22);
            this.guna2ControlBox2.TabIndex = 3;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.CustomClick = true;
            this.guna2ControlBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(859, 0);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(25, 22);
            this.guna2ControlBox1.TabIndex = 2;
            this.guna2ControlBox1.Click += new System.EventHandler(this.guna2ControlBox1_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label2.Location = new System.Drawing.Point(134, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "Total SystemCare";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(40, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "StrelyCleaner";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(23, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(17, 22);
            this.panel2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 22);
            this.label3.TabIndex = 5;
            this.label3.Text = "   ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2DragControl1
            // 
            this.guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl1.TargetControl = this.label1;
            this.guna2DragControl1.UseTransparentDrag = true;
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.TargetControl = this;
            // 
            // guna2Elipse2
            // 
            this.guna2Elipse2.BorderRadius = 3;
            this.guna2Elipse2.TargetControl = this.PanelHeader;
            // 
            // guna2HtmlToolTip1
            // 
            this.guna2HtmlToolTip1.AllowLinksHandling = true;
            this.guna2HtmlToolTip1.AutomaticDelay = 0;
            this.guna2HtmlToolTip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(21)))));
            this.guna2HtmlToolTip1.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlToolTip1.MaximumSize = new System.Drawing.Size(0, 0);
            this.guna2HtmlToolTip1.ShowAlways = true;
            this.guna2HtmlToolTip1.UseGdiPlusTextRendering = true;
            // 
            // guna2DragControl2
            // 
            this.guna2DragControl2.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl2.TargetControl = this.label2;
            this.guna2DragControl2.UseTransparentDrag = true;
            // 
            // guna2DragControl3
            // 
            this.guna2DragControl3.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl3.TargetControl = this.PanelHeader;
            this.guna2DragControl3.UseTransparentDrag = true;
            // 
            // guna2DragControl4
            // 
            this.guna2DragControl4.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl4.TargetControl = this.panel2;
            this.guna2DragControl4.UseTransparentDrag = true;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.guna2ContextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "StrelyCleanner (version: 1.6.3)";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // guna2ContextMenuStrip1
            // 
            this.guna2ContextMenuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.guna2ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.guna2ContextMenuStrip1.Name = "guna2ContextMenuStrip1";
            this.guna2ContextMenuStrip1.RenderStyle.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(143)))), ((int)(((byte)(255)))));
            this.guna2ContextMenuStrip1.RenderStyle.BorderColor = System.Drawing.Color.Gainsboro;
            this.guna2ContextMenuStrip1.RenderStyle.ColorTable = null;
            this.guna2ContextMenuStrip1.RenderStyle.RoundedEdges = true;
            this.guna2ContextMenuStrip1.RenderStyle.SelectionArrowColor = System.Drawing.Color.White;
            this.guna2ContextMenuStrip1.RenderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.guna2ContextMenuStrip1.RenderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.guna2ContextMenuStrip1.RenderStyle.SeparatorColor = System.Drawing.Color.Gainsboro;
            this.guna2ContextMenuStrip1.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.guna2ContextMenuStrip1.Size = new System.Drawing.Size(104, 48);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.removeToolStripMenuItem.Text = "Show";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // guna2MessageDialog1
            // 
            this.guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            this.guna2MessageDialog1.Caption = null;
            this.guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            this.guna2MessageDialog1.Parent = this;
            this.guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Dark;
            this.guna2MessageDialog1.Text = null;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(21)))));
            this.ClientSize = new System.Drawing.Size(887, 480);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StrelyCleaner";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(21)))));
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.guna2Panel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.PanelLayout.ResumeLayout(false);
            this.PanelHeader.ResumeLayout(false);
            this.guna2ContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2GradientPanel PanelHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel PanelLayout;
        private System.Windows.Forms.Panel PanelContainer; //System.Windows.Forms.Panel
        private Guna.UI2.WinForms.Guna2Button HardwareButton;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse2;
        private Guna.UI2.WinForms.Guna2Button AntivirusButton;
        private Guna.UI2.WinForms.Guna2Button TweatsButton;
        private Guna.UI2.WinForms.Guna2Button OptimizerButton;
        private Guna.UI2.WinForms.Guna2Button SettingsButton;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator1;
        private Guna.UI2.WinForms.Guna2HtmlToolTip guna2HtmlToolTip1;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox2;
        private System.Windows.Forms.Label FrameRateLabel;
        private Guna.UI2.WinForms.Guna2Button CleanerButton;
        private Guna.UI2.WinForms.Guna2VScrollBar guna2VScrollBar1;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl3;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl4;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private Guna.UI2.WinForms.Guna2ContextMenuStrip guna2ContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private Guna.UI2.WinForms.Guna2ShadowForm guna2ShadowForm1;
        private Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
    }
}