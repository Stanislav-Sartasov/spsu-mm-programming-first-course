#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>
#include <string.h>
#include <stdlib.h>
#include <malloc.h>
#define to_byte(x) x > 128 ? 255 : 0
FILE* fileIn;
FILE* fileOut;


#pragma pack(push, 1)
struct BITMAPFILEHEADER
{
	unsigned short bfType;
	unsigned int bfSize;
	unsigned short bfReserved1;
	unsigned short bfReserved2;
	unsigned int bfOffBits;
};

struct BITMAPINFOHEADER
{
	unsigned int biSize;
	unsigned int  biWidth;
	unsigned int  biHeight;
	unsigned short  biPlanes;
	unsigned short  biBitCount;
	unsigned int biCompression;
	unsigned int biSizeImage;
	unsigned int  biXPelsPerMeter;
	unsigned int  biYPelsPerMeter;
	unsigned int biClrUsed;
	unsigned int biClrImportant;
};
#pragma pack(pop)

void convolution(unsigned char* bitMapImage, double height, int width, char* mode)
{
	if (strcmp(mode, "BlackWhite") == 0)
	{
		for (int i = 0; i < height * width; i++)
		{
			unsigned char result = (299 * bitMapImage[i * 3] + 587 * bitMapImage[i * 3 + 1] + 114 * bitMapImage[i * 3 + 2]) / 1000;
			bitMapImage[i * 3] = result;
			bitMapImage[i * 3 + 1] = result;
			bitMapImage[i * 3 + 2] = result;
		}
		return;
	}

	double* bitMapImageCopy = (double*)malloc(3 * height * width * sizeof(double));

	int size = 3;

	double matrix3x3[4][9] =
	{
		{  1 / 16.0, 2 / 16.0, 1 / 16.0, 2 / 16.0, 4 / 16.0, 2 / 16.0, 1 / 16.0, 2 / 16.0, 1 / 16.0},
		{ -1, 0, 1, -2,  0, 2, -1, 0, 1 },
		{ -1, -2,-1, 0, 0, 0, 1, 2, 1 },
		{  1, 1, 1, 1, 1, 1, 1, 1, 1 }
	};
	double matrix5x5[1][25] =
	{
		{
			1 / 256.0, 4 / 256.0, 6 / 256.0, 4 / 256.0, 1 / 256.0,
			4 / 256.0, 16 / 256.0, 24 / 256.0, 16 / 256.0, 4 / 256.0,
			6 / 256.0, 24 / 256.0, 36 / 256.0, 24 / 256.0, 6 / 256.0,
			4 / 256.0, 16 / 256.0, 24 / 256.0, 16 / 256.0, 4 / 256.0,
			1 / 256.0, 4 / 256.0, 6 / 256.0, 4 / 256.0, 1 / 256.0
		}
	};

	double* selected = 0;
	int isSobel = 0;

	if (strcmp(mode, "Averaging") == 0)
	{
		selected = matrix3x3[3];
	}
	else if (strcmp(mode, "Gauss3") == 0)
	{
		selected = matrix3x3[0];
	}
	else if (strcmp(mode, "Gauss5") == 0)
	{
		selected = matrix5x5[0];
	}
	else if (strcmp(mode, "SobelX") == 0)
	{
		isSobel = 1;
		selected = matrix3x3[2];
	}
	else if (strcmp(mode, "SobelY") == 0)
	{
		isSobel = 1;
		selected = matrix3x3[1];
	}

	for (int h = 0; h < height; h++)
	{
		for (int w = 0; w < width; w++)
		{

			double result[3] = { 0, 0, 0 };
			double divider = 0;
			for (int x = 0; x < size; x++)
				for (int y = 0; y < size; y++)
					if ((h + x - 1) >= 0 && (h + x - 1) <= (height - 1) && (w + y - 1) >= 0 && (w + y - 1) <= (width - 1))
					{
						result[0] += bitMapImage[((h + x - 1) * width + w + y - 1) * 3 + 0] * selected[x * size + y];
						result[1] += bitMapImage[((h + x - 1) * width + w + y - 1) * 3 + 1] * selected[x * size + y];
						result[2] += bitMapImage[((h + x - 1) * width + w + y - 1) * 3 + 2] * selected[x * size + y];
					}
			if (isSobel)
			{
				bitMapImageCopy[(h * width + w) * 3] = to_byte(result[0]);
				bitMapImageCopy[(h * width + w) * 3 + 1] = to_byte(result[1]);
				bitMapImageCopy[(h * width + w) * 3 + 2] = to_byte(result[2]);
			}
			else
			{
				bitMapImageCopy[(h * width + w) * 3] = (unsigned char)(result[0]);
				bitMapImageCopy[(h * width + w) * 3 + 1] = (unsigned char)(result[1]);
				bitMapImageCopy[(h * width + w) * 3 + 2] = (unsigned char)(result[2]);
			}

		}
	}


	for (int i = 0; i < height * width * 3; i++)
		bitMapImage[i] = bitMapImageCopy[i];
	free(bitMapImageCopy);
}

int main(int argc, char* argv[])
{
	if (argc != 4 || !(strcmp(argv[2], "Averaging") == 0 || strcmp(argv[2], "Gauss3") == 0 || strcmp(argv[2], "Gauss5") == 0
		|| strcmp(argv[2], "SobelX") == 0 || strcmp(argv[2], "SobelY") == 0 || strcmp(argv[2], "BlackWhite") == 0) || fopen_s(&fileIn, argv[1], "rb") != 0)
	{
		printf("Invalid input. Try again.");
		exit(-1);
	}

	fopen_s(&fileIn, argv[1], "rb");
	fopen_s(&fileOut, argv[3], "wb");

	struct BITMAPFILEHEADER bitMapFileHeader;
	struct BITMAPINFOHEADER bitMapInfoHeader;

	fread(&bitMapFileHeader, sizeof(bitMapFileHeader), 1, fileIn);
	fread(&bitMapInfoHeader, sizeof(bitMapInfoHeader), 1, fileIn);

	unsigned char* bitMapImage = (unsigned char*)malloc(bitMapInfoHeader.biSizeImage);

	fseek(fileIn, bitMapFileHeader.bfOffBits, SEEK_SET);
	fread(bitMapImage, 1, bitMapInfoHeader.biSizeImage, fileIn);

	convolution(bitMapImage, bitMapInfoHeader.biHeight, bitMapInfoHeader.biWidth, argv[2]);

	fwrite(&bitMapFileHeader, sizeof(bitMapFileHeader), 1, fileOut);
	fwrite(&bitMapInfoHeader, sizeof(bitMapInfoHeader), 1, fileOut);

	for (unsigned int i = 0; i < bitMapInfoHeader.biSizeImage; i++)
		fwrite(&bitMapImage[i], 1, 1, fileOut);

	free(bitMapImage);
	fclose(fileIn);
	fclose(fileOut);
	return 0;
}