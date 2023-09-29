using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace StrelyCleaner.Controls
{
   

    public class NeonEffectProvider
    {
        private Control control;
        private Color neonColor = Color.Lime;
        private int blurRadius = 5;

        public NeonEffectProvider(Control control)
        {
            this.control = control;
            //ApplyNeonEffect();
            control.Paint += Control_Paint;
        }

        public Color NeonColor
        {
            get { return neonColor; }
            set
            {
                neonColor = value;
                control.Invalidate();
            }
        }

        public int BlurRadius
        {
            get { return blurRadius; }
            set
            {
                blurRadius = value;
                control.Invalidate();
            }
        }

        private void Control_Paint(object sender, PaintEventArgs e)
        {
            ApplyNeonEffect();
        
       }

        private void ApplyNeonEffect()
        {
            Bitmap bmp = new Bitmap(control.Width, control.Height);
            control.DrawToBitmap(bmp, new Rectangle(0, 0, control.Width, control.Height));

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);

                    if (pixelColor != Color.Transparent)
                    {
                        Color newColor = Color.FromArgb(
                            Math.Min(pixelColor.A + neonColor.A, 255),
                            Math.Min(pixelColor.R + neonColor.R, 255),
                            Math.Min(pixelColor.G + neonColor.G, 255),
                            Math.Min(pixelColor.B + neonColor.B, 255)
                        );

                        for (int i = -blurRadius; i <= blurRadius; i++)
                        {
                            for (int j = -blurRadius; j <= blurRadius; j++)
                            {
                                int newX = x + i;
                                int newY = y + j;

                                if (newX >= 0 && newX < bmp.Width && newY >= 0 && newY < bmp.Height)
                                {
                                    bmp.SetPixel(newX, newY, newColor);
                                }
                            }
                        }
                    }
                }
            }
            control.BackgroundImageLayout = ImageLayout.Stretch;
           control.BackgroundImage = bmp;
        }
    }

}
