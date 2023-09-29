using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StrelyCleaner.Core
{
   

    public static class Extensions
    {
        public static Point CenterForm(this Form FormParent, Form Form_to_Center)
        {
            Point FormLocation = new Point();
            FormLocation.X = (FormParent.Left + (FormParent.Width - Form_to_Center.Width) / (int)2); // set the X coordinates.
            FormLocation.Y = (FormParent.Top + (FormParent.Height - Form_to_Center.Height) / (int)2); // set the Y coordinates.
            return FormLocation; // return the Location to the Form it was called from.
        }

        public static Icon ToIcon(this Image image, int width, int height)
        {
            if (image == null)
            {
                return null;
            }

            if (width <= 0 || height <= 0)
            {
                return null;
            }

            // Crear un nuevo Bitmap con el tamaño especificado.
            Bitmap bitmap = new Bitmap(image, new Size(width, height));

            // Utilizar MemoryStream para guardar el Bitmap como un formato ICO.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Icon);

                // Crear un Icono a partir del MemoryStream.
                return new Icon(memoryStream);
            }
        }
    }

}
