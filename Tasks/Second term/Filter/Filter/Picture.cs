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
        private BitMapFileHeader file = new BitMapFileHeader();
        private BitMapInfoHeader info = new BitMapInfoHeader();
        private uint height;
        private uint width;
        private byte[,,] processedPicture;
        private const double pi = 3.1415926535897932384626433832795;

        public void Read(string path)
        {
            FileStream input = new FileStream(path, FileMode.Open);
            BinaryReader picture = new BinaryReader(input);

            file.bfType = picture.ReadUInt16();
            file.bfSize = picture.ReadUInt32();
            file.bfReserved1 = picture.ReadUInt16();
            file.bfReserved2 = picture.ReadUInt16();
            file.bfOffBits = picture.ReadUInt32();

            info.biSize = picture.ReadUInt32();
            info.biWidth = picture.ReadUInt32();
            info.biHeight = picture.ReadUInt32();
            info.biPlanes = picture.ReadUInt16();
            info.biBitCount = picture.ReadUInt16();
            info.biCompression = picture.ReadUInt32();
            info.biSizeImage = picture.ReadUInt32();
            info.biXPelsPerMeter = picture.ReadUInt32();
            info.biYPelsPerMeter = picture.ReadUInt32();
            info.biClrUsed = picture.ReadUInt32();
            info.biClrImportant = picture.ReadUInt32();

            height = info.biHeight;
            width = info.biWidth;
            processedPicture = new byte[height, width, 3];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    processedPicture[i, j, 0] = picture.ReadByte();
                    processedPicture[i, j, 1] = picture.ReadByte();
                    processedPicture[i, j, 2] = picture.ReadByte();
                }
            picture.Close();
            input.Close();
        }

        public void Write(string path)
        {
            FileStream output = new FileStream(path, FileMode.Create);
            BinaryWriter picture = new BinaryWriter(output);

            picture.Write(file.bfType);
            picture.Write(file.bfSize);
            picture.Write(file.bfReserved1);
            picture.Write(file.bfReserved2);
            picture.Write(file.bfOffBits);

            picture.Write(info.biSize);
            picture.Write(info.biWidth);
            picture.Write(info.biHeight);
            picture.Write(info.biPlanes);
            picture.Write(info.biBitCount);
            picture.Write(info.biCompression);
            picture.Write(info.biSizeImage);
            picture.Write(info.biXPelsPerMeter);
            picture.Write(info.biYPelsPerMeter);
            picture.Write(info.biClrUsed);
            picture.Write(info.biClrImportant);

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    picture.Write(processedPicture[i, j, 0]);
                    picture.Write(processedPicture[i, j, 1]);
                    picture.Write(processedPicture[i, j, 2]);
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
                        GreyFilter(picture.processedPicture, picture.height, picture.width);
                        break;
                    }
                case "Averaging":
                    {
                        Averaging(picture.processedPicture, picture.height, picture.width);
                        break;
                    }
                case "Gauss3":
                    {
                        Gauss3(picture.processedPicture, picture.height, picture.width);
                        break;
                    }
                case "SobelX":
                    {
                        Sobel(picture.processedPicture, picture.height, picture.width, "SobelX");
                        break;
                    }
                case "SobelY":
                    {
                        Sobel(picture.processedPicture, picture.height, picture.width, "SobelY");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid filter name.");
                        return;
                    }
            }
        }
        private static void GreyFilter(byte[,,] processedPicture, uint height, uint width)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    //byte x = (byte)(0.299 * picture.processedPicture[i, j, 0] + 0.587 * picture.processedPicture[i, j, 1] + 0.114 * picture.processedPicture[i, j, 2]);
                    //byte x = (byte)((2126 * processedPicture[i, j, 0] + 7152 * processedPicture[i, j, 1] + 722 * processedPicture[i, j, 2]) / 10000);
                    //byte x = (byte)(0.212 * picture.processedPicture[i, j, 0] + 0.701 * picture.processedPicture[i, j, 1] + 0.087 * picture.processedPicture[i, j, 2]);
                    //byte x = (byte)(0.2989 * picture.processedPicture[i, j, 0] + 0.5870 * picture.processedPicture[i, j, 1] + 0.1140 * picture.processedPicture[i, j, 2]);
                    //byte x = (byte)(0.243 * picture.processedPicture[i, j, 0] + 0.41 * picture.processedPicture[i, j, 1] + 0.347 * picture.processedPicture[i, j, 2]);
                    byte x = (byte)((processedPicture[i, j, 0] + processedPicture[i, j, 1] + processedPicture[i, j, 2]) / 3);
                    processedPicture[i, j, 0] = x;
                    processedPicture[i, j, 1] = x;
                    processedPicture[i, j, 2] = x;
                }
        }
        private static void Sobel(byte[,,] processedPicture, uint height, uint width, string type)
        {
            byte[,,] tempPicture = new byte[height, width, 3];
            int[,] directions = new int[3, 3];
            switch (type)
            {
                case "SobelY":
                    {
                        directions[0, 0] = -1;
                        directions[0, 1] = -2;
                        directions[0, 2] = -1;
                        directions[1, 0] = 0;
                        directions[1, 1] = 0;
                        directions[1, 2] = 0;
                        directions[2, 0] = 1;
                        directions[2, 1] = 2;
                        directions[2, 2] = 1;
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
                    int[] result = new int[3] { 0, 0, 0 };
                    for (int iterDirX = 0; iterDirX < size; iterDirX++)
                        for (int iterDirY = 0; iterDirY < size; iterDirY++)
                            if ((i + iterDirX - 1) >= 0 && (i + iterDirX - 1) <= (height - 1) && (j + iterDirY - 1) >= 0 && (j + iterDirY - 1) <= (width - 1))
                            {
                                for (int k = 0; k < 3; k++)
                                    result[k] += (processedPicture[i + iterDirX - 1, j + iterDirY - 1, k] * directions[iterDirX, iterDirY]);
                            }
                    for (int k = 0; k < 3; k++)
                    {
                        result[k] = result[k] > 255 ? 255 : result[k];
                        result[k] = result[k] < 0 ? 0 : result[k];
                        tempPicture[i, j, k] = (byte)result[k];
                    }
                }
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < 3; k++)
                        processedPicture[i, j, k] = tempPicture[i, j, k];
            return;
        }

        private static void Gauss3(byte[,,] processedPicture, uint height, uint width)
        {
            int size = 3;
            int[,] directions = new int[3, 3] { { 1, 2, 1 },
                                                { 2, 4, 2 },
                                                { 1, 2, 1 } };
            byte[,,] tempPicture = new byte[height, width, 3];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    int[] result = new int[3] { 0, 0, 0 };
                    int divider = 0;
                    for (int iterDirX = 0; iterDirX < size; iterDirX++)
                        for (int iterDirY = 0; iterDirY < size; iterDirY++)
                            if ((i + iterDirX - 1) >= 0 && (i + iterDirX - 1) <= (height - 1) && (j + iterDirY - 1) >= 0 && (j + iterDirY - 1) <= (width - 1))
                            {
                                for (int k = 0; k < 3; k++)
                                    result[k] += (processedPicture[i + iterDirX - 1, j + iterDirY - 1, k] * directions[iterDirX, iterDirY]);
                                divider += directions[iterDirX, iterDirY];
                            }
                    for (int k = 0; k < 3; k++)
                        tempPicture[i, j, k] = (byte)(result[k] / divider);
                }
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < 3; k++)
                        processedPicture[i, j, k] = tempPicture[i, j, k];
            return;
        }

        private static void Averaging(byte[,,] processedPicture, uint height, uint width)
        {
            int[,] directions = { { 1, 1, 1 },
                                  { 1, 1, 1 },
                                  { 1, 1, 1 } };
            byte[,,] tempPicture = new byte[height, width, 3];
            int size = 3;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    int[] result = new int[3] { 0, 0, 0 };
                    int divider = 0;
                    for (int iterDirX = 0; iterDirX < size; iterDirX++)
                        for (int iterDirY = 0; iterDirY < size; iterDirY++)
                            if ((i + iterDirX - 1) >= 0 && (i + iterDirX - 1) <= (height - 1) && (j + iterDirY - 1) >= 0 && (j + iterDirY - 1) <= (width - 1))
                            {
                                for (int k = 0; k < 3; k++)
                                    result[k] += (processedPicture[i + iterDirX - 1, j + iterDirY - 1, k] * directions[iterDirX, iterDirY]);
                                divider += directions[iterDirX, iterDirY];
                            }
                    for (int k = 0; k < 3; k++)
                        tempPicture[i, j, k] = (byte)(result[k] / divider);
                }
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < 3; k++)
                        processedPicture[i, j, k] = tempPicture[i, j, k];
            return;
        }


        //public static void Filter(Picture picture, string type)
        //{

        //}

        public static int ImageComparison(Picture pictureOne, Picture pictureTwo)
        {
            if (pictureOne.height != pictureTwo.height || pictureOne.width != pictureTwo.width)
                return -1;
            uint height = pictureOne.height;
            uint width = pictureOne.width;
            int diff = 0;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    if ((pictureOne.processedPicture[i, j, 0] != pictureTwo.processedPicture[i, j, 0]) ||
                        (pictureOne.processedPicture[i, j, 1] != pictureTwo.processedPicture[i, j, 1]) ||
                        (pictureOne.processedPicture[i, j, 2] != pictureTwo.processedPicture[i, j, 2]))
                        diff++;
            return diff;
        }
    }
}