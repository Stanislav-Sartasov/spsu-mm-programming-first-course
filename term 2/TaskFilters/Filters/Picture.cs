using System.IO;


namespace TaskFilters
{
    class Picture
    {
		struct BitMapHeaderFile
		{
			public ushort bfType;
			public uint bfSize;
			public ushort bfReserved1;
			public ushort bfReserved2;
			public uint bfOffBits;
		}

		struct BitMapHeaderInfo
		{
			public uint biSize;
			public uint biWidht;
			public uint biHeight;
			public ushort biPlanes;
			public ushort biCount;
			public uint biCompression;
			public uint biSizeImage;
			public uint biXPelsPerMeter;
			public uint biYPelsPerMeter;
			public uint biColorUsed;
			public uint biColorImportant;
		}

		private BitMapHeaderFile HeaderFile;
		private BitMapHeaderInfo HeaderInfo;
		public uint width, height;
		public byte[,,] pixels;
		public void BMPRead(FileStream InputPicture)
		{
			BinaryReader ReadPicture = new BinaryReader(InputPicture);
			
			HeaderFile.bfType = ReadPicture.ReadUInt16();
			HeaderFile.bfSize = ReadPicture.ReadUInt32();
			HeaderFile.bfReserved1 = ReadPicture.ReadUInt16();
			HeaderFile.bfReserved2 = ReadPicture.ReadUInt16();
			HeaderFile.bfOffBits = ReadPicture.ReadUInt32();

			HeaderInfo.biSize = ReadPicture.ReadUInt32();
			HeaderInfo.biWidht = ReadPicture.ReadUInt32();
			HeaderInfo.biHeight = ReadPicture.ReadUInt32();
			HeaderInfo.biPlanes = ReadPicture.ReadUInt16();
			HeaderInfo.biCount = ReadPicture.ReadUInt16();
			HeaderInfo.biCompression = ReadPicture.ReadUInt32();
			HeaderInfo.biSizeImage = ReadPicture.ReadUInt32();
			HeaderInfo.biXPelsPerMeter = ReadPicture.ReadUInt32();
			HeaderInfo.biYPelsPerMeter = ReadPicture.ReadUInt32();
			HeaderInfo.biColorUsed = ReadPicture.ReadUInt32();
			HeaderInfo.biColorImportant = ReadPicture.ReadUInt32();

			height = HeaderInfo.biHeight;
			width = HeaderInfo.biWidht;
			pixels = new byte[height, width, 3];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					pixels[i, j, 0] = ReadPicture.ReadByte();
					pixels[i, j, 1] = ReadPicture.ReadByte();
					pixels[i, j, 2] = ReadPicture.ReadByte();
				}
			}
			InputPicture.Close();
		}

		public void BMPWrite(FileStream OutputPicture)
		{
			BinaryWriter WritePicture = new BinaryWriter(OutputPicture);

			WritePicture.Write(HeaderFile.bfType);
			WritePicture.Write(HeaderFile.bfSize);
			WritePicture.Write(HeaderFile.bfReserved1);
			WritePicture.Write(HeaderFile.bfReserved2);
			WritePicture.Write(HeaderFile.bfOffBits);

			WritePicture.Write(HeaderInfo.biSize);
			WritePicture.Write(HeaderInfo.biWidht);
			WritePicture.Write(HeaderInfo.biHeight);
			WritePicture.Write(HeaderInfo.biPlanes);
			WritePicture.Write(HeaderInfo.biCount);
			WritePicture.Write(HeaderInfo.biCompression);
			WritePicture.Write(HeaderInfo.biSizeImage);
			WritePicture.Write(HeaderInfo.biXPelsPerMeter);
			WritePicture.Write(HeaderInfo.biYPelsPerMeter);
			WritePicture.Write(HeaderInfo.biColorUsed);
			WritePicture.Write(HeaderInfo.biColorImportant);

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					WritePicture.Write(pixels[i, j, 0]);
					WritePicture.Write(pixels[i, j, 1]);
					WritePicture.Write(pixels[i, j, 2]);
				}
			}
			OutputPicture.Close();


		}
    }
}
