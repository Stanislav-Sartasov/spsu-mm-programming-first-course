#define _CRT_SECURE_NO_WARNINGS
#define M_PI		3.14159265358979323846
#include <stdio.h>
#include <math.h>
#include <string.h>
#include <stdlib.h>
#include <malloc.h>
FILE* fileIn;
FILE* fileOut;

double sigma = 0.6;

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

	double matrix[4][9] =
	{
		{  1, 2, 1, 2, 4, 2, 1, 2, 1},
		{ -1, 0, 1, -2,  0, 2, -1, 0, 1 },
		{ -1, -2,-1, 0, 0, 0, 1, 2, 1 },
		{  1, 1, 1, 1, 1, 1, 1, 1, 1 }
	};

	double* selected = 0;
	int isSobel = 0;

	if (strcmp(mode, "Averaging") == 0)
	{
		selected = matrix[3];
	}
	else if (strcmp(mode, "Gauss3") == 0)
	{
		selected = matrix[0];
	}
	else if (strcmp(mode, "SobelX") == 0)
	{
		selected = matrix[2];
		isSobel = 1;
	}
	else if (strcmp(mode, "SobelY") == 0)
	{
		selected = matrix[1];
		isSobel = 1;
	}

	int bc = 3;
	for (int j = 0; j < 3; j++)
	{
		for (int i = bc * width + j; i < (width * (height - 1)) * 3; i += 3)
		{
			if ((i % (width * bc) < bc) || ((i + bc) % (width * bc) < bc))
				continue;
			double pixel_value = selected[0] * bitMapImage[i + bc * width - bc] + selected[1] * bitMapImage[i + bc * width] + selected[2] * bitMapImage[i + bc * width + bc]
				+ selected[3] * bitMapImage[i - bc] + selected[4] * bitMapImage[i] + selected[5] * bitMapImage[i + bc] + selected[6] * bitMapImage[i - bc * width - bc]
				+ selected[7] * bitMapImage[i - bc * width] + selected[8] * bitMapImage[i - bc * width + bc];
			if (isSobel)
			{
				if (pixel_value > 255)
					pixel_value = 255;
				if (pixel_value < 0)
					pixel_value = 0;
			}
			else
			{
				pixel_value /= 16;
			}
			bitMapImageCopy[i] = (unsigned char)pixel_value;
		}
	}

	for (int i = 0; i < height * width * bc; i++)
		bitMapImage[i] = bitMapImageCopy[i];
	free(bitMapImageCopy);
}

int main(int argc, char* argv[])
{
	if (argc != 4 || !(strcmp(argv[2], "Averaging") == 0 || strcmp(argv[2], "Gauss3") == 0 || strcmp(argv[2], "Gauss5") == 0 || strcmp(argv[2], "Sobel") == 0
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