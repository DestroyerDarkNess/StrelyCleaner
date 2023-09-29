using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace StrelyCleaner.Core
{
    public class IconExtractor
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]  private static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)] private static extern bool DestroyIcon(IntPtr handle);

        public static Image ExtractIconFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("File Not Found", filePath);
                }

                IntPtr hIcon = ExtractIcon(IntPtr.Zero, filePath, 0);

                if (hIcon != IntPtr.Zero)
                {
                    Icon icon = Icon.FromHandle(hIcon);
                    Image image = icon.ToBitmap();
                    DestroyIcon(hIcon); 
                    return image;
                }
            }
            catch (Exception)
            {
                
            }

            return null;
        }

    }

}
