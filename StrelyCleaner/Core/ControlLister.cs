using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Drawing;

namespace StrelyCleaner.Core
{
    

    public class ControlLister
    {
        private Orientation _Orientation = System.Windows.Forms.Orientation.Horizontal;
        public Orientation OrientationControls
        {
            get
            {
                return _Orientation;
            }
            set
            {
                _Orientation = value;
            }
        }

        public Point Margen
        {
            get
            {
                return MargenP;
            }
            set
            {
                MargenP = value;
            }
        }

        private int _MaxItems = 0;
        public int MaxItemInLine
        {
            get
            {
                return _MaxItems;
            }
            set
            {
                _MaxItems = value;
            }
        }



        private int LocationX = 4;
        private int LocationY = 4;
        private int SeparationX = 4;
        private int SeparationY = 4;
        private int XSizeCupon = 0;
        private int YSizeCupon = 0;
        private Point MargenP = new Point(4, 4);

        private int ControlsCount = 0;



        public void Add(Panel ContainerControl, Control ControlEx, bool LimitedLocation = false)
        {
            if (ContainerControl != null)
            {
                ControlEx.Visible = false;

                Control TheLastControl = null;

                if (ContainerControl.Controls.Count != 0)
                    TheLastControl = ContainerControl.Controls[ContainerControl.Controls.Count - 1];

                if (TheLastControl == null)
                {
                    ContainerControl.Controls.Add(ControlEx);
                    ControlEx.Location = new Point(MargenP.X, MargenP.Y);

                    XSizeCupon = ContainerControl.Width - (ContainerControl.Controls[0].Location.X + ContainerControl.Controls[0].Width);
                    YSizeCupon = ContainerControl.Height - (ContainerControl.Controls[0].Location.X + ContainerControl.Controls[0].Height);
                }
                else
                {
                    int NewPostX = 0;
                    int NewPostY = 0;

                    if (_Orientation == Orientation.Horizontal)
                    {
                        if (LimitedLocation == false)
                        {
                            NewPostX = TheLastControl.Location.X + TheLastControl.Width + SeparationX;
                            NewPostY = TheLastControl.Location.Y;
                        }
                        else if (XSizeCupon >= (ControlEx.Width + SeparationX))
                        {
                            NewPostX = TheLastControl.Location.X + TheLastControl.Width + SeparationX;
                            NewPostY = TheLastControl.Location.Y;
                        }
                        else
                        {
                            NewPostX = MargenP.X;
                            NewPostY = TheLastControl.Location.Y + TheLastControl.Height + SeparationY;
                        }
                    }
                    else if (_Orientation == Orientation.Vertical)
                    {
                        if (LimitedLocation == false)
                        {
                            NewPostX = TheLastControl.Location.X;
                            NewPostY = TheLastControl.Location.Y + TheLastControl.Height + SeparationY;
                        }
                        else if (YSizeCupon >= (ControlEx.Height + SeparationY))
                        {
                            NewPostX = TheLastControl.Location.X;
                            NewPostY = TheLastControl.Location.Y + TheLastControl.Height + SeparationY;
                        }
                        else
                        {
                            NewPostX = TheLastControl.Location.X + TheLastControl.Width + SeparationX;
                            NewPostY = MargenP.Y;
                        }
                    }

                    LocationX = NewPostX;
                    LocationY = NewPostY;


                    ContainerControl.Controls.Add(ControlEx);
                    ControlEx.Location = new Point(LocationX, LocationY);

                    TheLastControl = ContainerControl.Controls[ContainerControl.Controls.Count - 1];
                    XSizeCupon = ContainerControl.Width - (TheLastControl.Location.X + ContainerControl.Controls[0].Width);
                    YSizeCupon = ContainerControl.Height - (TheLastControl.Location.Y + ContainerControl.Controls[0].Height);

                    ControlsCount += 1;
                }

                ControlEx.Visible = true;
            }
        }

        private int GetInlineItemsCount(Panel Container)
        {
            int ItemInline = 0;
            Control TheLastControl = Container.Controls[Container.Controls.Count - 1];

            foreach (Control ItemN in Container.Controls)
            {
                if (_Orientation == Orientation.Horizontal)
                {
                    if (ItemN.Location.X == TheLastControl.Location.X)
                        ItemInline += 1;
                }
                else if (_Orientation == Orientation.Vertical)
                {
                    if (ItemN.Location.Y == TheLastControl.Location.Y)
                        ItemInline += 1;
                }
            }

            return ItemInline;
        }
    }

}
