using System;
using System.IO;

namespace Picture
{
	public class BMP
	{
		public struct BitmapHeader
		{
			public ushort BfType;
			public uint BfSize;
			public ushort BfReserved1;
			public ushort BfReserved2;
			public uint BfOffBits;
		}
		public struct BitmapHeaderInfo
		{
			public uint Size;
			public uint Width;
			public uint Height;
			public ushort Planes;
			public ushort BitCount;
			public uint Compression;
			public uint SizeImage;
			public uint XPelsPerMeter;
			public uint YPelsPerMeter;
			public uint ColorsUsed;
			public uint ColorsImportant;
		}

		public BitmapHeader HeaderBMP;
		public BitmapHeaderInfo HeaderInfoBMP = new BitmapHeaderInfo();
		public uint width, height;

		public byte[] ColorTable { get; private set; }
		public byte [,,] pixels;
		
		public void ReadBMP(string path)
		{
			if (File.Exists(path))
			{
				BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));

				HeaderBMP = new BitmapHeader
				{
					BfType = reader.ReadUInt16(),
					BfSize = reader.ReadUInt32(),
					BfReserved1 = reader.ReadUInt16(),
					BfReserved2 = reader.ReadUInt16(),
					BfOffBits = reader.ReadUInt32()
				};

				HeaderInfoBMP = new BitmapHeaderInfo
				{
					Size = reader.ReadUInt32(),
					Width = reader.ReadUInt32(),
					Height = reader.ReadUInt32(),
					Planes = reader.ReadUInt16(),
					BitCount = reader.ReadUInt16(),
					Compression = reader.ReadUInt32(),
					SizeImage = reader.ReadUInt32(),
					XPelsPerMeter = reader.ReadUInt32(),
					YPelsPerMeter = reader.ReadUInt32(),
					ColorsUsed = reader.ReadUInt32(),
					ColorsImportant = reader.ReadUInt32()
				};

				ColorTable = new byte[HeaderBMP.BfOffBits - 54];
				for (int i = 0; i < HeaderBMP.BfOffBits - 54; i++)
					ColorTable[i] = reader.ReadByte();


				width = HeaderInfoBMP.Width;
				height = HeaderInfoBMP.Height;;
				pixels = new byte[height, width, 3];
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						pixels[i, j, 0] = reader.ReadByte();
						pixels[i, j, 1] = reader.ReadByte();
						pixels[i, j, 2] = reader.ReadByte();
					}
				}
				reader.Close();
			}
			else
			{
				throw new Exception("Can't read file.");
			}
		}

		public void WriteBMP(string path)
		{
			if (Directory.Exists(Path.GetDirectoryName(path)))
			{
				BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create));
				writer.Write(HeaderBMP.BfType);
				writer.Write(HeaderBMP.BfSize);
				writer.Write(HeaderBMP.BfReserved1);
				writer.Write(HeaderBMP.BfReserved2);
				writer.Write(HeaderBMP.BfOffBits);
				writer.Write(HeaderInfoBMP.Size);
				writer.Write(HeaderInfoBMP.Width);
				writer.Write(HeaderInfoBMP.Height);
				writer.Write(HeaderInfoBMP.Planes);
				writer.Write(HeaderInfoBMP.BitCount);
				writer.Write(HeaderInfoBMP.Compression);
				writer.Write(HeaderInfoBMP.SizeImage);
				writer.Write(HeaderInfoBMP.XPelsPerMeter);
				writer.Write(HeaderInfoBMP.YPelsPerMeter);
				writer.Write(HeaderInfoBMP.ColorsUsed);
				writer.Write(HeaderInfoBMP.ColorsImportant);

				for (int color = 0; color < HeaderBMP.BfOffBits - 54; color++)
					writer.Write(ColorTable[color]);

				for (int i = 0; i < HeaderInfoBMP.Height; i++)
				{
					for (int j = 0; j < HeaderInfoBMP.Width; j++)
					{
						writer.Write(pixels[i, j, 0]);
						writer.Write(pixels[i, j, 1]);
						writer.Write(pixels[i, j, 2]);
					}
				}
				writer.Close();
			}
			else
			{
				throw new Exception("Can't write file.");
			}
		}
	}
}
