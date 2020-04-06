using System;
using System.IO;

namespace Filter
{
    public class Picture
    {
        private struct BitMapFileHeader
        {
            public ushort bfType;
            public uint bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public uint bfOffBits;
        }
        private struct BitMapInfoHeader
        {
            public uint biSize;
            public uint biWidth;
            public uint biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public uint biXPelsPerMeter;
            public uint biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }
        private BitMapFileHeader File = new BitMapFileHeader();
        private BitMapInfoHeader Info = new BitMapInfoHeader();
        private uint Height;
        private uint Width;
        private byte[,,] ProcessedPicture;
        private const double pi = 3.1415926535897932384626433832795;

        public void Read(string path)
        {
            FileStream input = new FileStream(path, FileMode.Open);
            BinaryReader picture = new BinaryReader(input);

            File.bfType = picture.ReadUInt16();
            File.bfSize = picture.ReadUInt32();
            File.bfReserved1 = picture.ReadUInt16();
            File.bfReserved2 = picture.ReadUInt16();
            File.bfOffBits = picture.ReadUInt32();

            Info.biSize = picture.ReadUInt32();
            Info.biWidth = picture.ReadUInt32();
            Info.biHeight = picture.ReadUInt32();
            Info.biPlanes = picture.ReadUInt16();
            Info.biBitCount = picture.ReadUInt16();
            Info.biCompression = picture.ReadUInt32();
            Info.biSizeImage = picture.ReadUInt32();
            Info.biXPelsPerMeter = picture.ReadUInt32();
            Info.biYPelsPerMeter = picture.ReadUInt32();
            Info.biClrUsed = picture.ReadUInt32();
            Info.biClrImportant = picture.ReadUInt32();

            Height = Info.biHeight;
            Width = Info.biWidth;
            ProcessedPicture = new byte[Height, Width, 3];
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    ProcessedPicture[i, j, 0] = picture.ReadByte();
                    ProcessedPicture[i, j, 1] = picture.ReadByte();
                    ProcessedPicture[i, j, 2] = picture.ReadByte();
                }
            picture.Close();
            input.Close();
        }

        public void Write(string path)
        {
            FileStream output = new FileStream(path, FileMode.Create);
            BinaryWriter picture = new BinaryWriter(output);

            picture.Write(File.bfType);
            picture.Write(File.bfSize);
            picture.Write(File.bfReserved1);
            picture.Write(File.bfReserved2);
            picture.Write(File.bfOffBits);

            picture.Write(Info.biSize);
            picture.Write(Info.biWidth);
            picture.Write(Info.biHeight);
            picture.Write(Info.biPlanes);
            picture.Write(Info.biBitCount);
            picture.Write(Info.biCompression);
            picture.Write(Info.biSizeImage);
            picture.Write(Info.biXPelsPerMeter);
            picture.Write(Info.biYPelsPerMeter);
            picture.Write(Info.biClrUsed);
            picture.Write(Info.biClrImportant);

            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    picture.Write(ProcessedPicture[i, j, 0]);
                    picture.Write(ProcessedPicture[i, j, 1]);
                    picture.Write(ProcessedPicture[i, j, 2]);
                }
            picture.Close();
            output.Close();
        }

        public static void Filter(Picture picture, string type)
        {
            switch (type)
            {
                case "Grey":
                    {
                        GreyFilter(picture.ProcessedPicture, picture.Height, picture.Width);
                        break;
                    }
                case "Averaging":
                    {
                        Averaging(picture.ProcessedPicture, picture.Height, picture.Width);
                        break;
                    }
                case "Gauss3":
                    {
                        Gauss3(picture.ProcessedPicture, picture.Height, picture.Width);
                        break;
                    }
                case "SobelX":
                    {
                        Sobel(picture.ProcessedPicture, picture.Height, picture.Width, "SobelX");
                        break;
                    }
                case "SobelY":
                    {
                        Sobel(picture.ProcessedPicture, picture.Height, picture.Width, "SobelY");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid filter name.");
                        return;
                    }
            }
        }
        public static void GreyFilter(byte[,,] processedPicture, uint height, uint width)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    //byte x = (byte)(0.299 * picture.ProcessedPicture[i, j, 0] + 0.587 * picture.ProcessedPicture[i, j, 1] + 0.114 * picture.ProcessedPicture[i, j, 2]);
                    //byte x = (byte)((2126 * processedPicture[i, j, 0] + 7152 * processedPicture[i, j, 1] + 722 * processedPicture[i, j, 2]) / 10000);
                    //byte x = (byte)(0.212 * picture.ProcessedPicture[i, j, 0] + 0.701 * picture.ProcessedPicture[i, j, 1] + 0.087 * picture.ProcessedPicture[i, j, 2]);
                    //byte x = (byte)(0.2989 * picture.ProcessedPicture[i, j, 0] + 0.5870 * picture.ProcessedPicture[i, j, 1] + 0.1140 * picture.ProcessedPicture[i, j, 2]);
                    //byte x = (byte)(0.243 * picture.ProcessedPicture[i, j, 0] + 0.41 * picture.ProcessedPicture[i, j, 1] + 0.347 * picture.ProcessedPicture[i, j, 2]);
                    byte x = (byte)((processedPicture[i, j, 0] + processedPicture[i, j, 1] + processedPicture[i, j, 2]) / 3);
                    processedPicture[i, j, 0] = x;
                    processedPicture[i, j, 1] = x;
                    processedPicture[i, j, 2] = x;
                }
        }
        public static void Sobel(byte[,,] processedPicture, uint height, uint width, string type)
        {
            byte[,,] tempPicture = new byte[height, width, 3];
            double[,] directions = new double[3, 3];
            switch (type)
            {
                case "SobelY":
                    {
                        directions[0, 0] = 1;
                        directions[0, 1] = 2;
                        directions[0, 2] = 1;
                        directions[1, 0] = 0;
                        directions[1, 1] = 0;
                        directions[1, 2] = 0;
                        directions[2, 0] = -1;
                        directions[2, 1] = -2;
                        directions[2, 2] = -1;
                        break;
                    }
                case "SobelX":
                    {
                        directions[0, 0] = -1;
                        directions[0, 1] = 0;
                        directions[0, 2] = 1;
                        directions[1, 0] = -2;
                        directions[1, 1] = 0;
                        directions[1, 2] = 2;
                        directions[2, 0] = -1;
                        directions[2, 1] = 0;
                        directions[2, 2] = 1;
                        break;
                    }
            }
            int size = 3;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    double[] result = new double[3] { 0, 0, 0 };
                    for (int iter_dir_x = 0; iter_dir_x < size; iter_dir_x++)
                        for (int iter_dir_y = 0; iter_dir_y < size; iter_dir_y++)
                            if ((i + iter_dir_x - 1) >= 0 && (i + iter_dir_x - 1) <= (height - 1) && (j + iter_dir_y - 1) >= 0 && (j + iter_dir_y - 1) <= (width - 1))
                            {
                                for (int k = 0; k < 3; k++)
                                    result[k] += (processedPicture[i + iter_dir_x - 1, j + iter_dir_y - 1, k] * directions[iter_dir_x, iter_dir_y]);
                            }
                    for (int k = 0; k < 3; k++)
                    {
                        result[k] = result[k] > 255 ? 255 : result[k];
                        result[k] = result[k] < 0 ? 0 : result[k];
                        tempPicture[i, j, k] = (byte)(result[k]);
                    }
                }
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < 3; k++)
                        processedPicture[i, j, k] = tempPicture[i, j, k];
            return;
        }

        public static void Gauss3(byte[,,] processedPicture, uint height, uint width)
        {
            int size = 3;
            int[,] directions = new int[3, 3] { { 1, 2, 1 },
                                                { 2, 4, 2 },
                                                { 1, 2, 1 } };
            byte[,,] tempPicture = new byte[height, width, 3];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    double[] result = new double[3] { 0, 0, 0 };
                    double divider = 0;
                    for (int iter_dir_x = 0; iter_dir_x < size; iter_dir_x++)
                        for (int iter_dir_y = 0; iter_dir_y < size; iter_dir_y++)
                            if ((i + iter_dir_x - 1) >= 0 && (i + iter_dir_x - 1) <= (height - 1) && (j + iter_dir_y - 1) >= 0 && (j + iter_dir_y - 1) <= (width - 1))
                            {
                                for (int k = 0; k < 3; k++)
                                    result[k] += (processedPicture[i + iter_dir_x - 1, j + iter_dir_y - 1, k] * directions[iter_dir_x, iter_dir_y]);
                                divider += directions[iter_dir_x, iter_dir_y];
                            }
                    for (int k = 0; k < 3; k++)
                        tempPicture[i, j, k] = (byte)(result[k]);
                }
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < 3; k++)
                        processedPicture[i, j, k] = tempPicture[i, j, k];
            return;
        }

        public static void Averaging(byte[,,] processedPicture, uint height, uint width)
        {
            int[,] directions = { { 1, 1, 1 },
                                  { 1, 1, 1 },
                                  { 1, 1, 1 } };
            byte[,,] tempPicture = new byte[height, width, 3];
            int size = 3;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    double[] result = new double[3] { 0, 0, 0 };
                    double divider = 0;
                    for (int iter_dir_x = 0; iter_dir_x < size; iter_dir_x++)
                        for (int iter_dir_y = 0; iter_dir_y < size; iter_dir_y++)
                            if ((i + iter_dir_x - 1) >= 0 && (i + iter_dir_x - 1) <= (height - 1) && (j + iter_dir_y - 1) >= 0 && (j + iter_dir_y - 1) <= (width - 1))
                            {
                                for (int k = 0; k < 3; k++)
                                    result[k] += (processedPicture[i + iter_dir_x - 1, j + iter_dir_y - 1, k] * directions[iter_dir_x, iter_dir_y]);
                                divider += directions[iter_dir_x, iter_dir_y];
                            }
                    for (int k = 0; k < 3; k++)
                        tempPicture[i, j, k] = (byte)(result[k]);
                }
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < 3; k++)
                        processedPicture[i, j, k] = tempPicture[i, j, k];
            return;
        }
    }
}