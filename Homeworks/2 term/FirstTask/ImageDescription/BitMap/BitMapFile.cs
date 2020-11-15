using System;
using System.IO;
using System.Xml.Resolvers;

namespace FirstTask.ImageDescription
{
	public class BitMapFile
	{
		private struct FileHeader
		{
			public ushort bfType;
			public uint bfSize;
			public ushort bfReversedOne;
			public ushort bfReversedTwo;
			public uint bfOffBits;
		}
		private struct InfoHeader
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
			public uint biColorUsed;
			public uint biColorImportant;
		}

		private const int offSet = 54;

		private FileHeader head;
		private InfoHeader info;

		public byte[] ColorTable { get; set; }
		public byte[] PixelsBytes { get; set; }
		
		public long Height { get; private set; }
		public long Width { get; private set; }
		private long SizeOfImage { get; set; } 

		public void FileRead(string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("File not found\nPlease, try again\n");
				Environment.Exit(-1);
			}

			using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
			{
				head.bfType = reader.ReadUInt16();
				head.bfSize = reader.ReadUInt32();
				head.bfReversedOne = reader.ReadUInt16();
				head.bfReversedTwo = reader.ReadUInt16();
				head.bfOffBits = reader.ReadUInt32();

				info.biSize = reader.ReadUInt32();
				info.biWidth = reader.ReadUInt32();
				info.biHeight = reader.ReadUInt32();
				info.biPlanes = reader.ReadUInt16();
				info.biBitCount = reader.ReadUInt16();
				info.biCompression = reader.ReadUInt32();
				info.biSizeImage = reader.ReadUInt32();
				info.biXPelsPerMeter = reader.ReadUInt32();
				info.biYPelsPerMeter = reader.ReadUInt32();
				info.biColorUsed = reader.ReadUInt32();
				info.biColorImportant = reader.ReadUInt32();

				ColorTable = new byte[head.bfOffBits - offSet];
				for (int i = 0; i < head.bfOffBits - offSet; i++)
				{
					ColorTable[i] = reader.ReadByte();
				}

				Height = (long)info.biHeight;
				Width = (long)info.biWidth;
				SizeOfImage = (long)info.biSizeImage;

				PixelsBytes = new byte[SizeOfImage];
				for (long i = 0; i < SizeOfImage; i++)
				{
					PixelsBytes[i] = reader.ReadByte();
				}
			}
		}
		public void FileWrite(string path)
		{
			using (var writer = new BinaryWriter(File.Open(path, FileMode.Create)))
			{
				writer.Write(head.bfType);
				writer.Write(head.bfSize);
				writer.Write(head.bfReversedOne);
				writer.Write(head.bfReversedTwo);
				writer.Write(head.bfOffBits);

				writer.Write(info.biSize);
				writer.Write(info.biWidth);
				writer.Write(info.biHeight);
				writer.Write(info.biPlanes);
				writer.Write(info.biBitCount);
				writer.Write(info.biCompression);
				writer.Write(info.biSizeImage);
				writer.Write(info.biXPelsPerMeter);
				writer.Write(info.biYPelsPerMeter);
				writer.Write(info.biColorUsed);
				writer.Write(info.biColorImportant);

				for (int i = 0; i < head.bfOffBits - offSet; i++)
				{
					writer.Write(ColorTable[i]);
				}

				for (long i = 0; i < SizeOfImage; i++)
				{
					writer.Write(PixelsBytes[i]);
				}
			}
		}
	}
}