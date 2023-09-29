using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrelyCleaner.Core
{

        public class ScrollManager : IDisposable
        {
            private Guna.UI2.WinForms.Helpers.PanelScrollHelper vScrollHelperMain; // Guna.UI2.Lib.ScrollBar.PanelScrollHelper
            private Panel ControlTarget = null/* TODO Change to default(_) if this is not a reference type */;



            public ScrollManager(Panel ControlA, Control[] ScrollBarArray, bool AutoSizeScroll = false)
            {
                ControlTarget = ControlA;
                foreach (Control ScrollBar in ScrollBarArray)
                {
                    if (ScrollBar is Guna.UI2.WinForms.Guna2VScrollBar)
                    {
                        Guna.UI2.WinForms.Guna2VScrollBar PatchScroll = ScrollBar as Guna.UI2.WinForms.Guna2VScrollBar;
                        vScrollHelperMain = new Guna.UI2.WinForms.Helpers.PanelScrollHelper(ControlA, PatchScroll, AutoSizeScroll);
                    }
                    else if (ScrollBar is Guna.UI2.WinForms.Guna2HScrollBar)
                    {
                        Guna.UI2.WinForms.Guna2HScrollBar PatchScroll = ScrollBar as Guna.UI2.WinForms.Guna2HScrollBar;
                        vScrollHelperMain = new Guna.UI2.WinForms.Helpers.PanelScrollHelper(ControlA, PatchScroll, AutoSizeScroll);
                    }
                }

                vScrollHelperMain.UpdateScrollBar();

                ControlA.Resize += Control_Resize;
            }

            public void UpdateScroll()
            {
                vScrollHelperMain.UpdateScrollBar();
            }



            private void Control_Resize(object sender, EventArgs e)
            {
                if (vScrollHelperMain != null)
                    vScrollHelperMain.UpdateScrollBar();
            }




            /// <summary>
            ///         ''' To detect redundant calls when disposing.
            ///         ''' </summary>
            private bool IsDisposed = false;
            /// <summary>
            ///         ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            ///         ''' </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            ///         ''' Releases unmanaged and - optionally - managed resources.
            ///         ''' </summary>
            ///         ''' <param name="IsDisposing">
            ///         ''' <c>true</c> to release both managed and unmanaged resources; 
            ///         ''' <c>false</c> to release only unmanaged resources.
            ///         ''' </param>
            protected void Dispose(bool IsDisposing)
            {
                if (IsDisposed == false)
                {
                    if (IsDisposing == true)
                    {
                        if (this.ControlTarget != null)
                        {
                            {
                                var withBlock = this.ControlTarget;
                                withBlock.Resize -= Control_Resize;
                            }

                            vScrollHelperMain.Dispose();
                        }
                    }
                }

                IsDisposed = true;
            }
        }

}
