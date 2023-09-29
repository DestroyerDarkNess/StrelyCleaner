
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StrelyCleaner.Controls
{
    // /*               *\
    // |#* RichTextLabel *#|
    // \*               */
    // 
    // // By Elektro H@cker
    // 
    // Description:
    // ............
    // · A RichTextbox used as a Label to set text using various colors.
    // 
    // Methods:
    // ........
    // · AppendText (Overload)

    // Examples:
    // RichTextLabel1.AppendText("My ", Color.White, , New Font("Arial", 12, FontStyle.Bold))
    // RichTextLabel1.AppendText("RichText-", Color.White, , New Font("Arial", 12, FontStyle.Bold))
    // RichTextLabel1.AppendText("Label", Color.YellowGreen, Color.Black, New Font("Lucida console", 16, FontStyle.Italic))


    public class RichTextLabel : RichTextBox
    {
        public RichTextLabel()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.Enabled = false;
            base.Size = new Size(200, 20);
            base.BorderStyle = BorderStyle.None;
            base.Multiline = false;
        }


        /// <summary>
        ///     ''' Turn the control backcolor to transparent.
        ///     ''' </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = (cp.ExStyle | 32);
                return cp;
            }
        }



        // AcceptsTab
        // Just hidden from the designer and editor.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AcceptsTab
        {
            get
            {
                return base.AcceptsTab;
            }
            set
            {
                base.AcceptsTab = false;
            }
        }

        // AutoWordSelection
        // Just hidden from the designer and editor.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AutoWordSelection
        {
            get
            {
                return base.AutoWordSelection;
            }
            set
            {
                base.AutoWordSelection = false;
            }
        }

        // BackColor
        // Not hidden, but little hardcoded 'cause the createparams transparency.
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.SelectionStart = 0;
                base.SelectionLength = base.TextLength;
                base.SelectionBackColor = value;
                base.BackColor = value;
            }
        }

        // BorderStyle
        // Just hidden from the designer and editor.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new BorderStyle BorderStyle
        {
            get
            {
                return base.BorderStyle;
            }
            set
            {
                base.BorderStyle = BorderStyle.None;
            }
        }

        // Cursor
        // Hidden from the designer and editor,
        // because while the control is disabled the cursor always be the default even if changed.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                base.Cursor = Cursors.Default;
            }
        }

        // Enabled
        // Hidden from the but not from the editor,
        // because to prevent exceptions when doing loops over a control collection to disable/enable controls.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = false;
            }
        }

        // HideSelection
        // Just hidden from the designer and editor.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool HideSelection
        {
            get
            {
                return base.HideSelection;
            }
            set
            {
                base.HideSelection = true;
            }
        }

        // MaxLength
        // Just hidden from the designer and editor.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int MaxLength
        {
            get
            {
                return base.MaxLength;
            }
            set
            {
                base.MaxLength = 2147483646;
            }
        }

        // ReadOnly
        // Just hidden from the designer and editor.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }
            set
            {
                base.ReadOnly = true;
            }
        }

        // ScrollBars
        // Just hidden from the designer and editor.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new RichTextBoxScrollBars ScrollBars
        {
            get
            {
                return base.ScrollBars;
            }
            set
            {
                base.ScrollBars = RichTextBoxScrollBars.None;
            }
        }

        // ShowSelectionMargin
        // Just hidden from the designer and editor.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool ShowSelectionMargin
        {
            get
            {
                return base.ShowSelectionMargin;
            }
            set
            {
                base.ShowSelectionMargin = false;
            }
        }

        // TabStop
        // Just hidden from the designer and editor.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool TabStop
        {
            get
            {
                return base.TabStop;
            }
            set
            {
                base.TabStop = false;
            }
        }



        /// <summary>
        ///     ''' Append text to the current text.
        ///     ''' </summary>
        ///     ''' <param name="text">The text to append</param>
        ///     ''' <param name="forecolor">The font color</param>
        ///     ''' <param name="backcolor">The Background color</param>
        ///     ''' <param name="font">The font of the appended text</param>
        public void AppendText(string text, Color forecolor, Color backcolor, Font font = null/* TODO Change to default(_) if this is not a reference type */)
        {
            Int32 index = base.TextLength;
            base.AppendText(text);
            base.SelectionStart = index;
            base.SelectionLength = base.TextLength - index;
            base.SelectionColor = forecolor;

            if (backcolor != Color.Black)
                base.SelectionBackColor = backcolor;
            else
                base.SelectionBackColor = DefaultBackColor;

            if (font != null)
                base.SelectionFont = font;

            // Reset selection
            base.SelectionStart = base.TextLength;
            base.SelectionLength = 0;
        }

        public void AdjustRichTextBoxSize()
        {
            Size newSize = TextRenderer.MeasureText(base.Text, base.Font);
            newSize.Width += base.Margin.Horizontal;
            newSize.Height += base.Margin.Vertical;

            base.Size = newSize;
        }

    }

}
