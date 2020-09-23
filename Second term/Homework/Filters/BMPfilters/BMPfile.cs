using System;
using System.IO;

namespace BMPfilters
{
	public class BMPfile
	{
		public struct BitmapHeader
		{
			public byte B1 { get; internal set; }
			public byte B2 { get; internal set; }
			public uint BfSize { get; internal set; }
			public ushort BfReserved1 { get; internal set; }
			public ushort BfReserved2 { get; internal set; }
			public uint BfOffBits { get; internal set; }
		}

		public struct BitmapHeaderInfo
		{
			public uint Size { get; internal set; }
			public uint Width { get; internal set; }
			public uint Height { get; internal set; }
			public ushort Planes { get; internal set; }
			public ushort BitCount { get; internal set; }
			public uint Compression { get; internal set; }
			public uint SizeImage { get; internal set; }
			public uint XPelsPerMeter { get; internal set; }
			public uint YPelsPerMeter { get; internal set; } 
			public uint ColorsUsed { get; internal set; }
			public uint ColorsImportant { get; internal set; }
		}

		private int numOfChannels;
		public int NumOfChannels
		{
			get
			{
				return numOfChannels;
			}
			set
			{
				if (value != 32 && value != 24)
					throw new ArgumentException("The bitcount must be 32 or 24!");
				else
					numOfChannels = value / 8;
			}
		}

		public struct Pixel
		{
			public byte[] Channel { get; set; }
		}

		public BitmapHeader HeaderBMP { get; private set; }

		public BitmapHeaderInfo HeaderInfoBMP { get; private set; }

		public byte[] ColorTable { get; private set; }

		public Pixel[,] pixels;

		public BMPfile(string path)
		{
			if (File.Exists(path))
			{
				using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
				{
					HeaderBMP = new BitmapHeader
					{
						B1 = reader.ReadByte(),
						B2 = reader.ReadByte(),
						BfSize = reader.ReadUInt32(),
						BfReserved1 = reader.ReadUInt16(),
						BfReserved2 = reader.ReadUInt16(),
						BfOffBits = reader.ReadUInt32()
					};

					if (HeaderBMP.B1 != 0x42 || HeaderBMP.B2 != 0x4d)
						throw new Exception("Wrong file format!");

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
					for (int color = 0; color < HeaderBMP.BfOffBits - 54; color++)
						ColorTable[color] = reader.ReadByte();
					NumOfChannels = HeaderInfoBMP.BitCount;
					pixels = new Pixel[HeaderInfoBMP.Height, HeaderInfoBMP.Width];

					for (int height = 0; height < HeaderInfoBMP.Height; height++)
					{
						for (int width = 0; width < HeaderInfoBMP.Width; width++)
						{
							pixels[height, width].Channel = new byte[NumOfChannels];
							for (int numOfChannel = 0; numOfChannel < NumOfChannels; numOfChannel++)
							{
								pixels[height, width].Channel[numOfChannel] = reader.ReadByte();
							}
						}
					}
				}
			}
			else
			{
				Console.WriteLine("File cannot be read! Try again!");
				Environment.Exit(0);
			}
			
		}

		public void SetPixels(Filter filter)
		{
			pixels = filter.ApplyFilter(pixels);
		}

		public void WriteBMP(string path)
		{
			if (Directory.Exists(Path.GetDirectoryName(path)))
			{
				using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
				{
					writer.Write(HeaderBMP.B1);
					writer.Write(HeaderBMP.B2);
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

					for (int height = 0; height < HeaderInfoBMP.Height; height++)
					{
						for (int width = 0; width < HeaderInfoBMP.Width; width++)
						{

							for (int numOfChannel = 0; numOfChannel < NumOfChannels; numOfChannel++)
							{
								writer.Write(pixels[height, width].Channel[numOfChannel]);
							}
						}
					}
				};
			}
			else
			{
				Console.WriteLine("File cannot be write! Try again!");
				Environment.Exit(0);
			}
		}
	}
}
