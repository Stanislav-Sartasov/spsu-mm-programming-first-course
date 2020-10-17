using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Filter.ClientApp
{
    internal static class SecondaryFunctions
    {
        internal static void FillStructOfImage(BitmapSource image, out StructOfImage structOfImage)
        {
            byte[] pixels = null;
            var bytesPerPixel = (image.Format.BitsPerPixel + 7) / 8;
            ImageToByteArray(image, out pixels);
            structOfImage = new StructOfImage();
            structOfImage.Height = image.PixelHeight;
            structOfImage.Width = image.PixelWidth;
            structOfImage.DpiX = image.DpiX;
            structOfImage.DpiY = image.DpiY;
            structOfImage.Palette = null;
            structOfImage.MyPixelFormat = image.Format;
            structOfImage.Pixels = pixels;
            structOfImage.Stride = bytesPerPixel * image.PixelWidth;
        }

        internal static bool ImageToByteArray(BitmapSource image, out byte[] result)
        {
            try
            {
                int width = image.PixelWidth;
                int height = image.PixelHeight;
                int stride = width * ((image.Format.BitsPerPixel + 7) / 8);
                result = new byte[height * stride];
                image.CopyPixels(result, stride, 0);
                
                return true;
            }
            catch (Exception ex)
            {
                
                result = default;
                return false;
            }
        }

        internal static bool ByteArrayToImage(byte[] pixels, StructOfImage format, out BitmapSource result)
        {
            try
            {
                result = BitmapSource.Create(format.Width,
                                            format.Height,
                                            format.DpiX,
                                            format.DpiY,
                                            format.MyPixelFormat,
                                            format.Palette,
                                            pixels,
                                            format.Stride);

                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
        public static bool TryParseIp(string ipForConnection, out IPEndPoint iP)
        {
            string[] temp = ipForConnection.Split(new char[] { '.', ':' });
            if (temp.Length != 5)
            {
                iP = null;
                return false;
            }
            else
            {
                for (int k = 0; k < 5; k++)
                {
                    foreach (char c in temp[k])
                        if (c < '0' || '9' < c)
                        {
                            iP = null;
                            return false;
                        }
                }
                List<byte> result = new List<byte>();
                for (int j = 0; j < 4; j++)
                    result.Add(byte.Parse(temp[j]));
                UInt32 ip = BitConverter.ToUInt32(result.ToArray(), 0);
                UInt16 port = UInt16.Parse(temp[4]);
                iP = new IPEndPoint(ip, port);
                return true;
            }
        }

        public static byte[] TranslateToByteArrayUnicode(string message, byte source)
        {
            byte[] temp = Encoding.Unicode.GetBytes(message.ToString());
            byte[] result = new byte[temp.Length + 1];
            result[0] = source; // 0 - user, 1 - connect, 2 - disconnect
            Array.Copy(temp, 0, result, 1, temp.Length);
            return result;
        }

        public static string TranslateByteArrayToStringUnicode(byte[] message)
        {
            string result = Encoding.Unicode.GetString(message, 1, message.Length - 1); // message[0] - source

            return result;
        }
    }
}
