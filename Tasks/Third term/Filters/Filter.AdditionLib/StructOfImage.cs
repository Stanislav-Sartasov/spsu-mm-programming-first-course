using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Filter.AdditionLib
{
    public struct StructOfImage
    {
        public int Height;
        public int Width;
        public double DpiX;
        public double DpiY;
        public PixelFormat MyPixelFormat;
        public BitmapPalette Palette;
        public byte[] Pixels;
        public int Stride;
    }
}
