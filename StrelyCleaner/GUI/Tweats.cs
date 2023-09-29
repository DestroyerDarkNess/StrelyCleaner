using StrelyCleaner.Controls;
using StrelyCleaner.Core;
using StrelyCleaner.Core.Cleaner.Apps;
using StrelyCleaner.Core.Cleaner.Browser;
using StrelyCleaner.Core.Cleaner.Folders;
using StrelyCleaner.Core.Cleaner.Systems;
using StrelyCleaner.Core.Interfaces;
using StrelyCleaner.Core.Tweats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrelyCleaner.GUI
{
    public partial class Tweats : Form, IRenderForm
    {


        private ScrollManager panelFX2Scroll = null;
        private bool initialized = false;

        public Tweats()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            this.BackColor = Color.Transparent;

            panelFX2Scroll = new ScrollManager(panelFX2, new Control[] { guna2VScrollBar1 }, true);
        }

        public void BeginFrame()
        {
            if (panelFX1.Visible == false && initialized == false) { if (General != null) { initialized = true; panelFX1.Visible = true; guna2Button1.Checked = true; guna2Panel1.Visible = true;  } }
            if (Global_Instances.ProcessProvider != null)
            {
                if (Global_Instances.ProcessProvider.Enabled == true) { Global_Instances.ProcessProvider.Enabled = false; }
            }
        }


        List<ITweat> General = null;
        List<ITweat> Explorer = null;
        List<ITweat> Windows = null;
        List<ITweat> Telemetry = null;
        private bool Initialized = false;

        public void UpdateRenderData()
        {
            if (Initialized == false)
            {
                Initialized = true;

                if (General == null)
                {
                    General = new List<ITweat>();

                    General.Add(new Network_Throttling());

                    var MDefender = new Defender();
                    General.Add(MDefender);

                    if (Global_Instances.RenderUI != null && Global_Instances.RenderUI.Global_App_Services != null)
                    {
                        Global_Instances.RenderUI.Global_App_Services.DefenderSvKiller = MDefender.Get();
                    }

                    General.Add(new Core.Tweats.Services());
                    General.Add(new Remote_Assistance());
                }

                if (Explorer == null)
                {
                    Explorer = new List<ITweat>();

                    Explorer.Add(new Explorer());
                    Explorer.Add(new Explorer_AutoComplete());
                }

                if (Windows == null)
                {
                    Windows = new List<ITweat>();

                    Windows.Add(new LowLatency());
                    Windows.Add(new Timeouts());
                }

                if (Telemetry == null)
                {
                    Telemetry = new List<ITweat>();

                    Telemetry.Add(new Telemetry_Services());

                }

                if (Global_Instances.Lite == true) {
                    this.Invoke(new Action(() =>
                    {
                        BeginFrame();
                    }));
                }
            }
        }

        private void guna2Button1_CheckedChanged(object sender, EventArgs e)
        {

            if (General == null || guna2Button1.Checked == false) { return; }
            panelFX2.Controls.Clear();
            LoadTweatEnabler(panelFX2, General);

        }

        private void guna2Button6_CheckedChanged(object sender, EventArgs e)
        {
            if (Explorer == null || guna2Button6.Checked == false) { return; }
            panelFX2.Controls.Clear();
            LoadTweatEnabler(panelFX2, Explorer);
        }

        private void guna2Button2_CheckedChanged(object sender, EventArgs e)
        {
            if (Windows == null || guna2Button2.Checked == false) { return; }
            panelFX2.Controls.Clear();
            LoadTweatEnabler(panelFX2, Windows);
        }

        private void guna2Button3_CheckedChanged(object sender, EventArgs e)
        {
            if (Telemetry == null || guna2Button3.Checked == false) { return; }
            panelFX2.Controls.Clear();
            LoadTweatEnabler(panelFX2, Telemetry);
        }

        private  void LoadTweatEnabler(Panel PanelContainer, List<ITweat> Data)
        {
            Utilities.Sleep(1);

            ControlLister Listener_Sys = new ControlLister { OrientationControls = Orientation.Vertical, Margen = new Point(10, 10) };
            foreach (var ItemInfo in Data)
            {
                Control TweatOptionInfo = CreateOption(ItemInfo);
                Listener_Sys.Add(PanelContainer, TweatOptionInfo, false);
                TweatOptionInfo.Visible = true;
            }


            panelFX2Scroll.UpdateScroll();
        }

        private Control CreateOption(ITweat TweatOption)
        {
            TogleInfoControl ControlEx = new TogleInfoControl();
            ControlEx.OnAction = () => { try { TweatOption.Optimize(); } catch { }  };
            ControlEx.OffAction = () => { try { TweatOption.Disabled(); } catch { } }; 
            ControlEx.GetFunc = () => TweatOption.Get();
            ControlEx.SetName(TweatOption.id);
            string Descrip = TweatOption.Description;
            if (String.IsNullOrEmpty(Descrip) == false) { Descrip = Descrip + "  " + "Off restores system settings to default values, undoing previous optimizations. (If you already have this optimization, then this option will appear active)."; } 
            ControlEx.SetDescription(Descrip );
            ControlEx.UpdateValue();

            return ControlEx;
        }

        private void Tweats_Load(object sender, EventArgs e)
        {
            if (Global_Instances.Lite == true) { this.UpdateRenderData(); }
        }
    }
}
