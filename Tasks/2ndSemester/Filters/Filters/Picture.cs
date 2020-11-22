using System;
using System.IO;
using System.Text.Json;

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

        private BitMapFileHeader bitMapFileHeader = new BitMapFileHeader();
        private BitMapInfoHeader bitMapInfoHeader = new BitMapInfoHeader();
        private byte[,,] bitMapImage;

        public void Read(string path)
        {
            FileStream input = new FileStream(path, FileMode.Open);
            BinaryReader picture = new BinaryReader(input);

            bitMapFileHeader.bfType = picture.ReadUInt16();
            bitMapFileHeader.bfSize = picture.ReadUInt32();
            bitMapFileHeader.bfReserved1 = picture.ReadUInt16();
            bitMapFileHeader.bfReserved2 = picture.ReadUInt16();
            bitMapFileHeader.bfOffBits = picture.ReadUInt32();

            bitMapInfoHeader.biSize = picture.ReadUInt32();
            bitMapInfoHeader.biWidth = picture.ReadUInt32();
            bitMapInfoHeader.biHeight = picture.ReadUInt32();
            bitMapInfoHeader.biPlanes = picture.ReadUInt16();
            bitMapInfoHeader.biBitCount = picture.ReadUInt16();
            bitMapInfoHeader.biCompression = picture.ReadUInt32();
            bitMapInfoHeader.biSizeImage = picture.ReadUInt32();
            bitMapInfoHeader.biXPelsPerMeter = picture.ReadUInt32();
            bitMapInfoHeader.biYPelsPerMeter = picture.ReadUInt32();
            bitMapInfoHeader.biClrUsed = picture.ReadUInt32();
            bitMapInfoHeader.biClrImportant = picture.ReadUInt32();

            bitMapImage = new byte[bitMapInfoHeader.biHeight, bitMapInfoHeader.biWidth, 3];

            for (int i = 0; i < bitMapInfoHeader.biHeight; i++)
                for (int j = 0; j < bitMapInfoHeader.biWidth; j++)
                {
                    bitMapImage[i, j, 0] = picture.ReadByte();
                    bitMapImage[i, j, 1] = picture.ReadByte();
                    bitMapImage[i, j, 2] = picture.ReadByte();
                }

            picture.Close();
            input.Close();
        }

        public void Write(string path)
        {
            FileStream output = new FileStream(path, FileMode.Create);
            BinaryWriter picture = new BinaryWriter(output);

            picture.Write(bitMapFileHeader.bfType);
            picture.Write(bitMapFileHeader.bfSize);
            picture.Write(bitMapFileHeader.bfReserved1);
            picture.Write(bitMapFileHeader.bfReserved2);
            picture.Write(bitMapFileHeader.bfOffBits);

            picture.Write(bitMapInfoHeader.biSize);
            picture.Write(bitMapInfoHeader.biWidth);
            picture.Write(bitMapInfoHeader.biHeight);
            picture.Write(bitMapInfoHeader.biPlanes);
            picture.Write(bitMapInfoHeader.biBitCount);
            picture.Write(bitMapInfoHeader.biCompression);
            picture.Write(bitMapInfoHeader.biSizeImage);
            picture.Write(bitMapInfoHeader.biXPelsPerMeter);
            picture.Write(bitMapInfoHeader.biYPelsPerMeter);
            picture.Write(bitMapInfoHeader.biClrUsed);
            picture.Write(bitMapInfoHeader.biClrImportant);

            for (int i = 0; i < bitMapInfoHeader.biHeight; i++)
                for (int j = 0; j < bitMapInfoHeader.biWidth; j++)
                {
                    picture.Write(bitMapImage[i, j, 0]);
                    picture.Write(bitMapImage[i, j, 1]);
                    picture.Write(bitMapImage[i, j, 2]);
                }

            picture.Close();
            output.Close();
        }

        public static void Filter(Picture picture, string mode)
        {
            int[,] matrix = new int[3,3];
            switch (mode)
            {
                case "ColorWB":
                    {
                        ColorWB(picture.bitMapImage, picture.bitMapInfoHeader.biHeight, picture.bitMapInfoHeader.biWidth);
                        break;
                    }
                case "Averaging":
                    {
                        for (int i = 0; i < 3; i++)
                            for (int j = 0; j < 3; j++)
                                matrix[i, j] = 1;
                        MatrixFilter(picture.bitMapImage, picture.bitMapInfoHeader.biHeight, picture.bitMapInfoHeader.biWidth, matrix);
                        break;
                    }
                case "Gauss3":
                    {
                        matrix[0, 0] = 1; matrix[0, 1] = 2; matrix[0, 2] = 1;
                        matrix[1, 0] = 2; matrix[1, 1] = 4; matrix[1, 2] = 2;
                        matrix[2, 0] = 1; matrix[2, 1] = 2; matrix[2, 2] = 1;
                        MatrixFilter(picture.bitMapImage, picture.bitMapInfoHeader.biHeight, picture.bitMapInfoHeader.biWidth, matrix);
                        break;
                    }
                case "SobelX":
                    {
                        matrix[0, 0] = -1; matrix[0, 1] = 0; matrix[0, 2] = 1;
                        matrix[1, 0] = -2; matrix[1, 1] = 0; matrix[1, 2] = 2;
                        matrix[2, 0] = -1; matrix[2, 1] = 0; matrix[2, 2] = 1;
                        MatrixFilterSobel(picture.bitMapImage, picture.bitMapInfoHeader.biHeight, picture.bitMapInfoHeader.biWidth, matrix);
                        break;
                    }
                case "SobelY":
                    {
                        matrix[0, 0] = -1; matrix[0, 1] = -2; matrix[0, 2] = -1;
                        matrix[1, 0] = 0; matrix[1, 1] = 0; matrix[1, 2] = 0;
                        matrix[2, 0] = 1; matrix[2, 1] = 2; matrix[2, 2] = 1;
                        MatrixFilterSobel(picture.bitMapImage, picture.bitMapInfoHeader.biHeight, picture.bitMapInfoHeader.biWidth, matrix);
                        break;
                    }
            }
        }

        private static void ColorWB(byte[,,] bitMapImage, uint height, uint width)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    byte result = (byte)((299 * bitMapImage[i, j, 0] + 587 * bitMapImage[i, j, 1] + 114 * bitMapImage[i, j, 2]) / 1000);
                    bitMapImage[i, j, 0] = result;
                    bitMapImage[i, j, 1] = result;
                    bitMapImage[i, j, 2] = result;
                }
        }

        private static void MatrixFilter(byte[,,] bitMapImage, uint height, uint width, int[,] matrix)
        {
            int[,] steps = new int[9, 2];
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    steps[(i + 1) * 3 + j + 1, 0] = i;
                    steps[(i + 1) * 3 + j + 1, 1] = j;
                }

            byte[,,] bitMapImageCopy = new byte[height, width, 3];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    int[] result;

                    result = Convolution(steps, i, j, bitMapImage, height, width, matrix);

                    bitMapImageCopy[i, j, 0] = (byte)(result[0] / result[3]);
                    bitMapImageCopy[i, j, 1] = (byte)(result[1] / result[3]);
                    bitMapImageCopy[i, j, 2] = (byte)(result[2] / result[3]);
                }

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < 3; k++)
                        bitMapImage[i, j, k] = bitMapImageCopy[i, j, k];
        }

        private static void MatrixFilterSobel(byte[,,] bitMapImage, uint height, uint width, int[,] matrix)
        {
            int[,] steps = new int[9, 2];
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    steps[(i + 1) * 3 + j + 1, 0] = i;
                    steps[(i + 1) * 3 + j + 1, 1] = j;
                }

            byte[,,] bitMapImageCopy = new byte[height, width, 3];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    int[] result;

                    result = Convolution(steps, i, j, bitMapImage, height, width, matrix);

                    bitMapImageCopy[i, j, 0] = (byte)((Math.Abs(result[0]) + Math.Abs(result[1]) + Math.Abs(result[2])) / 3);
                    bitMapImageCopy[i, j, 1] = (byte)((Math.Abs(result[0]) + Math.Abs(result[1]) + Math.Abs(result[2])) / 3);
                    bitMapImageCopy[i, j, 2] = (byte)((Math.Abs(result[0]) + Math.Abs(result[1]) + Math.Abs(result[2])) / 3);
                }

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    for (int k = 0; k < 3; k++)
                        bitMapImage[i, j, k] = bitMapImageCopy[i, j, k] > 128 ? (byte)255 : (byte)0;
        }

        private static int[] Convolution(int[,] steps, int i, int j, byte[,,] bitMapImage, uint height, uint width, int[,] matrix)
        {
            int[] result = new int[4] { 0, 0, 0, 0};

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

        public static int PictureComparsion(Picture firstPicture, Picture secondPicture) // 0 - not equal, 1 - equal
        {
            if ((firstPicture.bitMapInfoHeader.biHeight != secondPicture.bitMapInfoHeader.biHeight) || (firstPicture.bitMapInfoHeader.biWidth != secondPicture.bitMapInfoHeader.biWidth))
                return 0;

            for (int i = 0; i < firstPicture.bitMapInfoHeader.biHeight; i++)
                for (int j = 0; j < firstPicture.bitMapInfoHeader.biWidth; j++)
                    for (int k = 0; k < 3; k++)
                        if (firstPicture.bitMapImage[i, j, k] != secondPicture.bitMapImage[i, j, k])
                            return 0;

            return 1;
        }
    }
}
