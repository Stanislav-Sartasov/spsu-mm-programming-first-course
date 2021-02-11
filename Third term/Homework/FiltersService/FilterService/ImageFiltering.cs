using System;
using System.Drawing;
using System.ServiceModel;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using FilterServiceContract;

namespace FilterService
{
    class ImageFiltering
    {
        private volatile bool isWorking;

        public void Stop() => isWorking = false;

        public bool ApplyFilter(Bitmap img, string filterName)
        {
            isWorking = true;
            if (filterName =="BlackAndWhite")
            {
                return ApplyBWFilter(img);
            }
            else
            {
                var matrix = MatrixCreator.CreateMatrix(filterName);
                return ApplyKernelFIlter(img, matrix); 
            }
        }

        public bool ApplyKernelFIlter(Bitmap img, double[,] kernel)
        {
            int frameWidth = (kernel.GetLength(0) - 1) / 2;
            BitmapData bitmapData = img
                .LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int bytesPerPixel = Image.GetPixelFormatSize(img.PixelFormat) / 8;
            int stride = bitmapData.Stride;
            int byteCount = stride * img.Height;
            byte[] oldPixels = new byte[byteCount];
            byte[] newPixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, oldPixels, 0, byteCount);
            int heightInPixels = bitmapData.Height;
            int widthInBytes = bitmapData.Width * bytesPerPixel;
            double blueValue, greenValue, redValue;

            try
            {
                for (int height = 0; height < heightInPixels; height++)
                {
                    int currentLine = height * stride;
                    for (int width = 0; width < widthInBytes; width += bytesPerPixel)
                    {
                        blueValue = oldPixels[currentLine + width];
                        greenValue = oldPixels[currentLine + width + 1];
                        redValue = oldPixels[currentLine + width + 2];

                        if (height >= frameWidth
                            && width >= frameWidth * bytesPerPixel
                            && height < (heightInPixels - frameWidth)
                            && width < (widthInBytes - frameWidth * bytesPerPixel))
                        {
                            blueValue = greenValue = redValue = 0;
                            for (int y = -frameWidth; y <= frameWidth; y++)
                            {
                                for (int x = -frameWidth; x <= frameWidth; x++)
                                {
                                    double factor = kernel[y + frameWidth, x + frameWidth];
                                    int index = stride * (height + y) + width + x * bytesPerPixel;
                                    blueValue += oldPixels[index] * factor;
                                    greenValue += oldPixels[index + 1] * factor;
                                    redValue += oldPixels[index + 2] * factor;
                                }
                            }
                        }

                        newPixels[currentLine + width] = ToByte(blueValue);
                        newPixels[currentLine + width + 1] = ToByte(greenValue);
                        newPixels[currentLine + width + 2] = ToByte(redValue);
                    }
                    try
                    {
                        if (isWorking)
                        {
                            OperationContext.Current
                                .GetCallbackChannel<IFilterServiceCallback>()
                                .ProgressCallback(100 * height / heightInPixels);
                        }
                        else 
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                img.UnlockBits(bitmapData);
            }
            Marshal.Copy(newPixels, 0, ptrFirstPixel, newPixels.Length);
            return true;
        }

        public bool ApplyBWFilter(Bitmap img)
        {
            BitmapData bitmapData = img
                .LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int bytesPerPixel = Image.GetPixelFormatSize(img.PixelFormat) / 8;
            int stride = bitmapData.Stride;
            int byteCount = stride * img.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, byteCount);
            int heightInPixels = bitmapData.Height;
            int widthInBytes = bitmapData.Width * bytesPerPixel;
            double blueValue, greenValue, redValue;
            byte pxValue;

            try
            {
                for (int height = 0; height < heightInPixels; height++)
                {
                    
                    int currentLine = height * stride;
                    for (int width = 0; width < widthInBytes; width += bytesPerPixel)
                    {
                        blueValue = pixels[currentLine + width];
                        greenValue = pixels[currentLine + width + 1];
                        redValue = pixels[currentLine + width + 2];
                        pxValue = ToByte((blueValue + greenValue + redValue) / 3);

                        pixels[currentLine + width] = pxValue;
                        pixels[currentLine + width + 1] = pxValue;
                        pixels[currentLine + width + 2] = pxValue;
                    }
                    if (isWorking)
                    {
                        try
                        {
                            OperationContext.Current
                                .GetCallbackChannel<IFilterServiceCallback>()
                                .ProgressCallback(100 * height / heightInPixels);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            finally
            {
                img.UnlockBits(bitmapData);
            }
            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            return true;
        }

        public static byte ToByte(double value)
        {
            if (value > 255)
                return 255;
            else if (value < 0)
                return 0;
            else
                return (byte)value;
        }
    }
}
