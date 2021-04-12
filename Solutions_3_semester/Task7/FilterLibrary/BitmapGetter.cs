using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace FilterLibrary
{
    public static class BitmapGetter
    {
        public static Bitmap GetBitmap(byte[] image)
		{
            return new Bitmap(new MemoryStream(image));
        }
        public static byte[] GetArray(Bitmap image)
		{
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        public static byte[] GetRGBPixel(Bitmap image, int x, int y)
		{
            Color color = image.GetPixel(x, y);
            return new byte[] { color.R, color.G, color.B };
        }
        public static Color GetColor(byte[] color)
		{
            return Color.FromArgb(color[0], color[1], color[2]);
		}
    }
}
