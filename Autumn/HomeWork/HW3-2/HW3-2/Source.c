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
	unsigned short	bfType;
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

void convolution(unsigned char* bitMapImage, int height, int width, char* mode)
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

	int size = strcmp(mode, "Gauss5") == 0 ? 2 : 1;
	int* steps = (int*)malloc(2 * (2 * size + 1) * (2 * size + 1) * sizeof(int));
	for (int i = -size; i <= size; i++)
		for (int j = -size; j <= size; j++)
		{
			steps[2 * ((i + size) * (2 * size + 1) + j + size)] = i;
			steps[2 * ((i + size) * (2 * size + 1) + j + size) + 1] = j;
		}

	double* matrix = (double*)malloc((2 * size + 1) * (2 * size + 1) * sizeof(double));

	if (strcmp(mode, "Averaging") == 0)
	{
		for (int i = 0; i < (2 * size + 1) * (2 * size + 1); i++)
			matrix[i] = 1;
	}
	else if (strcmp(mode, "Gauss3") == 0 || strcmp(mode, "Gauss5") == 0)
	{
		for (int x = -size; x < size + 1; x++)
			for (int y = -size; y < size + 1; y++)
				matrix[(x + size) * (2 * size + 1) + y + size] = 1 / sqrt(2 * M_PI * sigma) * exp(-(x * x + y * y) / (2 * sigma * sigma));
	}
	else if (strcmp(mode, "SobelX") == 0)
	{
		double source[] = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
		for (int i = 0; i < 9; i++)
			matrix[i] = source[i];
	}
	else if (strcmp(mode, "SobelY") == 0)
	{
		double source[] = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
		for (int i = 0; i < 9; i++)
			matrix[i] = source[i];
	}

	for (int i = 0; i < height; i++)
		for (int j = 0; j < width; j++)
		{
			double result[3] = { 0, 0, 0 };
			double divisor = 0;

			for (int step = 0; step < (2 * size + 1) * (2 * size + 1); step++)
				if (i + steps[2 * step] >= 0 && i + steps[2 * step] < height && j + steps[2 * step + 1] >= 0 && j + steps[2 * step + 1] < width)
				{
					result[0] += bitMapImage[((i + steps[2 * step]) * width + j + steps[2 * step + 1]) * 3] * matrix[(steps[2 * step] + size) * (2 * size + 1) + steps[2 * step + 1] + size];
					result[1] += bitMapImage[((i + steps[2 * step]) * width + j + steps[2 * step + 1]) * 3 + 1] * matrix[(steps[2 * step] + size) * (2 * size + 1) + steps[2 * step + 1] + size];
					result[2] += bitMapImage[((i + steps[2 * step]) * width + j + steps[2 * step + 1]) * 3 + 2] * matrix[(steps[2 * step] + size) * (2 * size + 1) + steps[2 * step + 1] + size];
					divisor += matrix[(steps[2 * step] + size) * (2 * size + 1) + steps[2 * step + 1] + size];
				}
			if (strcmp(mode, "SobelX") == 0 || strcmp(mode, "SobelY") == 0)
			{
				bitMapImageCopy[(i * width + j) * 3] = (abs(result[0]) + abs(result[1]) + abs(result[2])) / 3;
				bitMapImageCopy[(i * width + j) * 3 + 1] = (abs(result[0]) + abs(result[1]) + abs(result[2])) / 3;
				bitMapImageCopy[(i * width + j) * 3 + 2] = (abs(result[0]) + abs(result[1]) + abs(result[2])) / 3;
				
			}
			else
			{
				bitMapImageCopy[(i * width + j) * 3] = (double)(result[0] / divisor);
				bitMapImageCopy[(i * width + j) * 3 + 1] = (double)(result[1] / divisor);
				bitMapImageCopy[(i * width + j) * 3 + 2] = (double)(result[2] / divisor);
				bitMapImage[i] = bitMapImageCopy[i];
			}
		}

	for (int i = 0; i < height * width * 3; i++) 
	{
		if (strcmp(mode, "SobelX") == 0 || strcmp(mode, "SobelY") == 0)
			bitMapImage[i] = bitMapImageCopy[i] > 128 ? 255 : 0;
		else
			bitMapImage[i] = bitMapImageCopy[i];
	}

	free(matrix);
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