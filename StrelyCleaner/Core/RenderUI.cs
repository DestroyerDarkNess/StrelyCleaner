using Guna.UI2.WinForms;
using ProcessHacker.Common;
using StrelyCleaner.Core.Interfaces;
using StrelyCleaner.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrelyCleaner.Core
{
    public class RenderUI
    {
        public Core.Services Global_App_Services = null;

        public GUI.Hardware Hardware_UI;

        public GUI.Antivirus Antivirus_UI;

        public GUI.Optimizer Optimizer_UI;

        public GUI.Cleaner Cleaner_UI;

        public GUI.Tweats Tweats_UI;

        public GUI.Settings Settings_UI;

        public float FPS = 0;

        public bool LockFramesPerSecond = false;
        public bool VSync = false;

        public int BackEndThreads = 1;

        private int frameCount = 0;
        private bool RenderFrame = false;
        private Stopwatch stopwatch = null;
        private long lastFrameTicks = 0;

       

        public RenderUI(bool WithServices = false) {

            if (WithServices) { Global_App_Services = new Services(); }

            Hardware_UI = new GUI.Hardware() { TopLevel = false, Text = "0", Visible = false }; 

            Antivirus_UI = new GUI.Antivirus() { TopLevel = false, Text = "1", Visible = false }; 

            Optimizer_UI = new GUI.Optimizer() { TopLevel = false, Text = "2", Visible = false }; 

            Cleaner_UI = new GUI.Cleaner() { TopLevel = false, Text = "3", Visible = false }; 

            Tweats_UI = new GUI.Tweats() { TopLevel = false, Text = "4", Visible = false };

            Settings_UI = new GUI.Settings() { TopLevel = false, Text = "5", Visible = false }; 

            //Hardware_UI.SetDoubleBuffered(true);
            //Antivirus_UI.SetDoubleBuffered(true);
            //Optimizer_UI.SetDoubleBuffered(true);
            //Cleaner_UI.SetDoubleBuffered(true);
            //Tweats_UI.SetDoubleBuffered(true);
            //Settings_UI.SetDoubleBuffered(true);

            stopwatch = new Stopwatch();
            LockFramesPerSecond = Boolean.Parse(Global_Instances.AppSettings.ReadIni("Settings", "LockFramesPerSecond", "True"));
            VSync = Boolean.Parse(Global_Instances.AppSettings.ReadIni("Settings", "VSync", "True"));
            int ValThreads = int.Parse(Global_Instances.AppSettings.ReadIni("Settings", "BackEndThreads", "1"));
            BackEndThreads = ValThreads;

        }

        public bool UIRenderData(Form UI)
        {
            try {
               
                if (VSync == true) { if (RenderFrame == false) { return false; } }
             

                if (UI == null) { return false; }

                if (UI.Visible == true)     {  RenderDataAsync(UI); }

                return true;

            } catch { return false; }

        }

        public void ChangeToView(Form UI)
        {

            //if (VSync == true && UI.Text == "2") { VSync = false; } else { VSync = true; }

            UI.BringToFront();

            foreach (Control item in UI.Parent.Controls) {
                if (string.Equals(item.Name, UI.Name, StringComparison.OrdinalIgnoreCase) == true) { continue; }
                item.Visible = false;
                item.SendToBack();
            }
            UI.Visible = true;
            UI.Invalidate();
            UI.Refresh();
            UI.Update();
        }

            public bool Render(Form UI) {
            //try {
            if (UI == null) { return false; }

            if (stopwatch.IsRunning == false) { stopwatch.Start(); }

            UpdateFrameData();
            if (LockFramesPerSecond == false) { RenderFrameView(); } else { Utilities.Sleep(30 + BackEndThreads, Utilities.Measure.Milliseconds); }
           

            //switch (UI.Text)
            //{
            //    case "0":

            //        if (Hardware_UI!= null && Hardware_UI.Visible == false) {
                        
            //          if (Antivirus_UI != null) Antivirus_UI.Visible = false;
            //            if (Optimizer_UI != null) Optimizer_UI.Visible = false;
            //            if (Tweats_UI != null) Tweats_UI.Visible = false;
            //            if (Cleaner_UI != null) Cleaner_UI.Visible = false;
            //            if (Settings_UI != null) Settings_UI.Visible = false;

            //            Hardware_UI.Visible = true;
            //            Hardware_UI.BringToFront();
            //        }

            //        break;
            //    case "1":

            //        if (Antivirus_UI != null && Antivirus_UI.Visible == false)
            //        {

            //            if (Hardware_UI != null) Hardware_UI.Visible = false;
            //            if (Optimizer_UI != null) Optimizer_UI.Visible = false;
            //            if (Tweats_UI != null) Tweats_UI.Visible = false;
            //            if (Cleaner_UI != null) Cleaner_UI.Visible = false;
            //            if (Settings_UI != null) Settings_UI.Visible = false;

            //            Antivirus_UI.Visible = true;
            //            Antivirus_UI.BringToFront();

            //        }

            //        break;
            //    case "2":

            //        if (Optimizer_UI != null && Optimizer_UI.Visible == false)
            //        {

            //            if (Hardware_UI != null) Hardware_UI.Visible = false;
            //            if (Antivirus_UI != null) Antivirus_UI.Visible = false;
            //            if (Tweats_UI != null) Tweats_UI.Visible = false;
            //            if (Cleaner_UI != null) Cleaner_UI.Visible = false;
            //            if (Settings_UI != null) Settings_UI.Visible = false;

            //            Optimizer_UI.Visible = true;
            //            Optimizer_UI.BringToFront();

            //        }

            //        break;
            //    case "3":

            //        if (Cleaner_UI != null && Cleaner_UI.Visible == false)
            //        {


            //            if (Hardware_UI != null) Hardware_UI.Visible = false;
            //            if (Antivirus_UI != null) Antivirus_UI.Visible = false;
            //            if (Optimizer_UI != null) Optimizer_UI.Visible = false;
            //            if (Tweats_UI != null) Tweats_UI.Visible = false;
            //            if (Settings_UI != null) Settings_UI.Visible = false;

            //            Cleaner_UI.Visible = true;
            //            Cleaner_UI.BringToFront();
            //        }

            //        break;
            //    case "4":

            //        if (Tweats_UI != null && Tweats_UI.Visible == false)
            //        {


            //            if (Hardware_UI != null) Hardware_UI.Visible = false;
            //            if (Antivirus_UI != null) Antivirus_UI.Visible = false;
            //            if (Optimizer_UI != null) Optimizer_UI.Visible = false;
            //            if (Cleaner_UI != null) Cleaner_UI.Visible = false;
            //            if (Settings_UI != null) Settings_UI.Visible = false;

            //            Tweats_UI.Visible = true;
            //            Tweats_UI.BringToFront();
            //        }

            //        break;
            //    case "5":

            //        if (Settings_UI != null && Settings_UI.Visible == false)
            //        {


            //            if (Hardware_UI != null) Hardware_UI.Visible = false;
            //            if (Antivirus_UI != null) Antivirus_UI.Visible = false;
            //            if (Optimizer_UI != null) Optimizer_UI.Visible = false;
            //            if (Cleaner_UI != null) Cleaner_UI.Visible = false;
            //            if (Tweats_UI != null) Tweats_UI.Visible = false;

            //            Settings_UI.Visible = true;
            //            Settings_UI.BringToFront();
            //        }

            //        break;
            //    default:
            //        // code block
            //        break;
            //}


            //UI.Parent.Visible = true;
         
            return true;

            //} catch { return false; }


        }




        private void UpdateFrameData() {
            frameCount++;

            long currentTicks = stopwatch.ElapsedTicks;
            long elapsedTicksSinceLastFrame = currentTicks - lastFrameTicks;
            long targetFrameInterval = Stopwatch.Frequency / BackEndThreads;


            if (elapsedTicksSinceLastFrame >= targetFrameInterval)
            {
                lastFrameTicks = currentTicks;
                
                RenderFrame = true;
            }
            else
            {

                RenderFrame = false;
            }

            if (stopwatch.ElapsedMilliseconds >= 1000)
            {
               
                float fps = frameCount / (stopwatch.ElapsedMilliseconds / 1000.0f);

                FPS = fps; //($"FPS: {fps:F2}");

                stopwatch.Restart();
                frameCount = 0;
                lastFrameTicks = 0;

                if (LockFramesPerSecond == true) { RenderFrameView();} 

            } else { }

        }

        private void RenderFrameView()
        {
            try {
                IRenderForm RenderView = (IRenderForm)Core.Global_Instances.MainUI.CurrentPageView;
                RenderView.BeginFrame();
            } catch { }
        }

        private void RenderDataAsync(Form CurrentPageView)
        {
            IRenderForm RenderView = (IRenderForm)CurrentPageView;
            RenderView.UpdateRenderData();
        }


    }
}
