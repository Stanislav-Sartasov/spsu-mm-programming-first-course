using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.ServiceModel;
using System.Runtime.InteropServices;

namespace Filter.Server
{
    public class Picture
    {
        private volatile bool isWorking;

        private int bytesPerPixel;

        private uint height;
        private uint width;

        private byte[,,] bitMapImage;

        public void Stop()
        {
            isWorking = false;
        }

        private void IncomingImageToBitMapImage(byte[] incomingImage, uint height, uint width)
        {
            this.height = height;
            this.width = width;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < bytesPerPixel; k++)
                    {
                        bitMapImage[i, j, k] = incomingImage[i * width * bytesPerPixel + j * bytesPerPixel + k];
                    }
                }
            }
        }

        private byte[] BitMapImageToOutgoingImage()
        {
            byte[] outgoingImage = new byte[height * width * bytesPerPixel];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < bytesPerPixel; k++)
                    {
                        outgoingImage[i * width * bytesPerPixel + j * bytesPerPixel + k] = bitMapImage[i, j, k];
                    }
                }
            }

            return outgoingImage;
        }

        public bool ApplyFilter(Bitmap image, string filter, uint width)
        {
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            int byteCount = bitmapData.Stride * image.Height;
            byte[] arrayOfPixels = new byte[byteCount];

            bytesPerPixel = bitmapData.Stride / image.Width;

            Marshal.Copy(bitmapData.Scan0, arrayOfPixels, 0, byteCount);
            image.UnlockBits(bitmapData);

            isWorking = true;
            bitMapImage = new byte[image.Height, width, bytesPerPixel];

            IncomingImageToBitMapImage(arrayOfPixels, (uint)bitmapData.Height, (uint) bitmapData.Width);

            bool result = Filter(filter);

            byte[] resultArray = BitMapImageToOutgoingImage();

            Marshal.Copy(resultArray, 0, bitmapData.Scan0, resultArray.Length);

            return result;
        }   


        public bool Filter(string mode)
        {
            int[,] matrix = new int[3, 3];
            switch (mode)
            {
                case "ColorWB":
                    {
                        return ColorWB();
                    }
                case "Averaging":
                    {
                        for (int i = 0; i < 3; i++)
                            for (int j = 0; j < 3; j++)
                                matrix[i, j] = 1;
                        return MatrixFilter(matrix);
                    }
                case "Gauss3":
                    {
                        matrix[0, 0] = 1; matrix[0, 1] = 2; matrix[0, 2] = 1;
                        matrix[1, 0] = 2; matrix[1, 1] = 4; matrix[1, 2] = 2;
                        matrix[2, 0] = 1; matrix[2, 1] = 2; matrix[2, 2] = 1;
                        return MatrixFilter(matrix);
                    }
                case "SobelX":
                    {
                        matrix[0, 0] = -1; matrix[0, 1] = 0; matrix[0, 2] = 1;
                        matrix[1, 0] = -2; matrix[1, 1] = 0; matrix[1, 2] = 2;
                        matrix[2, 0] = -1; matrix[2, 1] = 0; matrix[2, 2] = 1;
                        return MatrixFilterSobel(matrix);
                    }
                case "SobelY":
                    {
                        matrix[0, 0] = -1; matrix[0, 1] = -2; matrix[0, 2] = -1;
                        matrix[1, 0] = 0; matrix[1, 1] = 0; matrix[1, 2] = 0;
                        matrix[2, 0] = 1; matrix[2, 1] = 2; matrix[2, 2] = 1;
                        return MatrixFilterSobel(matrix);
                    }
                default:
                    {
                        return false;
                    }
            }

        }

        private bool ColorWB()
        {
            try
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        byte result = (byte)((299 * bitMapImage[i, j, 0] + 587 * bitMapImage[i, j, 1] + 114 * bitMapImage[i, j, 2]) / 1000);
                        bitMapImage[i, j, 0] = result;
                        bitMapImage[i, j, 1] = result;
                        bitMapImage[i, j, 2] = result;
                    }

                    if (isWorking)
                    {
                        try
                        {
                            OperationContext.Current
                                .GetCallbackChannel<IFilterServiceCallback>()
                                .ProgressCallback(100 * i / (int)height);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        private bool MatrixFilter(int[,] matrix)
        {
            int[,] steps = new int[9, 2];
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    steps[(i + 1) * 3 + j + 1, 0] = i;
                    steps[(i + 1) * 3 + j + 1, 1] = j;
                }

            byte[,,] bitMapImageCopy = new byte[height, width, bytesPerPixel];

            try
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int[] result;

                        result = Convolution(steps, i, j, bitMapImage, height, width, matrix);

                        bitMapImageCopy[i, j, 0] = (byte)(result[0] / result[3]);
                        bitMapImageCopy[i, j, 1] = (byte)(result[1] / result[3]);
                        bitMapImageCopy[i, j, 2] = (byte)(result[2] / result[3]);
                    }

                    if (isWorking)
                    {
                        try
                        {
                            OperationContext.Current
                                .GetCallbackChannel<IFilterServiceCallback>()
                                .ProgressCallback(100 * i / (int)height);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < bytesPerPixel; k++)
                        bitMapImage[i, j, k] = bitMapImageCopy[i, j, k];

            return true;
        }

        private bool MatrixFilterSobel(int[,] matrix)
        {
            int[,] steps = new int[9, 2];
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    steps[(i + 1) * 3 + j + 1, 0] = i;
                    steps[(i + 1) * 3 + j + 1, 1] = j;
                }

            byte[,,] bitMapImageCopy = new byte[height, width, bytesPerPixel];

            try
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        int[] result;

                        result = Convolution(steps, i, j, bitMapImage, height, width, matrix);

                        bitMapImageCopy[i, j, 0] = (byte)((Math.Abs(result[0]) + Math.Abs(result[1]) + Math.Abs(result[2])) / 3);
                        bitMapImageCopy[i, j, 1] = (byte)((Math.Abs(result[0]) + Math.Abs(result[1]) + Math.Abs(result[2])) / 3);
                        bitMapImageCopy[i, j, 2] = (byte)((Math.Abs(result[0]) + Math.Abs(result[1]) + Math.Abs(result[2])) / 3);
                    }

                    if (isWorking)
                    {
                        try
                        {
                            OperationContext.Current
                                .GetCallbackChannel<IFilterServiceCallback>()
                                .ProgressCallback(100 * i / (int)height);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < bytesPerPixel; k++)
                        bitMapImage[i, j, k] = bitMapImageCopy[i, j, k] > 255 ? (byte)255 : bitMapImageCopy[i, j, k] < 0 ? (byte) 0 : bitMapImageCopy[i, j, k];

            return true;
        }

        private int[] Convolution(int[,] steps, int i, int j, byte[,,] bitMapImage, uint height, uint width, int[,] matrix)
        {
            int[] result = new int[4] { 0, 0, 0, 0 };

            for (int step = 0; step < 9; step++)
                if (i + steps[step, 0] >= 0 && i + steps[step, 0] < height && j + steps[step, 1] >= 0 && j + steps[step, 1] < width)
                {
                    result[0] += bitMapImage[i + steps[step, 0], j + steps[step, 1], 0] * matrix[steps[step, 0] + 1, steps[step, 1] + 1];
                    result[1] += bitMapImage[i + steps[step, 0], j + steps[step, 1], 1] * matrix[steps[step, 0] + 1, steps[step, 1] + 1];
                    result[2] += bitMapImage[i + steps[step, 0], j + steps[step, 1], 2] * matrix[steps[step, 0] + 1, steps[step, 1] + 1];
                    result[3] += matrix[steps[step, 0] + 1, steps[step, 1] + 1];
                }

            return result;
        }

        public bool PictureComparsion(Picture firstPicture, Picture secondPicture)
        {
            if ((firstPicture.height != secondPicture.height) || (firstPicture.width != secondPicture.height))
                return false;

            for (int i = 0; i < firstPicture.height; i++)
                for (int j = 0; j < firstPicture.width; j++)
                    for (int k = 0; k < bytesPerPixel; k++)
                        if (firstPicture.bitMapImage[i, j, k] != secondPicture.bitMapImage[i, j, k])
                            return false;

            return true;
        }
    }
}