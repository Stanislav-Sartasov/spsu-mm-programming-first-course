#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>
#include <string.h>
#define min(x, y) x < y ? x : y


void inputCheck(int argc, char* argv[])
{
	if (argc != 4 || !(strcmp(argv[2], "Averaging") == 0 || strcmp(argv[2], "Gauss3") == 0 || strcmp(argv[2], "Gauss5") == 0 || strcmp(argv[2], "Sobel") == 0
		|| strcmp(argv[2], "SobelX") == 0 || strcmp(argv[2], "SobelY") == 0 || strcmp(argv[2], "ColorWB") == 0) || fopen(argv[1], "rb") == NULL ||
		fopen(argv[3], "rb") == NULL)
	{
		printf("Invalid input. Try again.");
		exit(0);
	}
}

#pragma pack(push, 1)
struct BITMAPFILEHEADER
{
	unsigned short	bfType;
	unsigned int bfSize;
	unsigned short bfReserved1;
	unsigned short bfReserved2;
	unsigned int bfOffBits;
};

struct BITMAPINFOHEADER {
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

void averageFilter(unsigned char* bitMapImage, int height, int width)
{
	unsigned char* bitMapImageCopy = (unsigned char*)malloc(3 * height * width * sizeof(unsigned char));
	for (int i = 0; i < height * width; i++)
		bitMapImageCopy[i] = bitMapImage[i];

	int steps[9][2] = { -1, -1, -1, 0, -1, 1, 0, -1, 0, 0, 0, 1, 1, -1, 1, 0, 1, 1 };

	for (int i = 0; i < height; i++)
		for (int j = 0; j < width; j++)
		{
			int result[3] = { 0, 0, 0 };
			int divider = 0;

			for (int step = 0; step < 9; step++)
				if (i + steps[step][0] >= 0 && i + steps[step][0] < height && j + steps[step][1] >= 0 && j + steps[step][1] < width)
				{ 
					result[0] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3];
					result[1] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 1];
					result[2] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 2];
					divider += 1;
				}

			bitMapImageCopy[(i * width + j) * 3] = result[0] / divider;
			bitMapImageCopy[(i * width + j) * 3 + 1] = result[1] / divider;
			bitMapImageCopy[(i * width + j) * 3 + 2] = result[2] / divider;
		}

	for (int i = 0; i < height * width * 3; i++)
		bitMapImage[i] = bitMapImageCopy[i];
}

void gaussFilter(unsigned char* bitMapImage, int height, int width, int size)
{
	double pi = 3.1415926535897932384626433832795;
	double sigma = 0.6;
	double* gaussian = (double*)malloc((2 * size + 1) * (2 * size + 1) * sizeof(double));

	for (int x = -size; x < size + 1; x++)
		for (int y = -size; y < size + 1; y++)
			gaussian[(x + size) * (2 * size + 1) + y + size] = 1 / sqrt(2 * pi * sigma) * exp(-(x * x + y * y) / (2 * sigma * sigma));

	unsigned char* bitMapImageCopy = (unsigned char*)malloc(3 * height * width * sizeof(unsigned char));
	for (int i = 0; i < height * width; i++)
		bitMapImageCopy[i] = bitMapImage[i];

	if (size == 1)
	{
		int steps[9][2] = { -1, -1, -1, 0, -1, 1, 0, -1, 0, 0, 0, 1, 1, -1, 1, 0, 1, 1 };

		for (int i = 0; i < height; i++)
			for (int j = 0; j < width; j++)
			{
				double result[3] = { 0, 0, 0 };
				double divisor = 0;

				for (int step = 0; step < 9; step++)
					if (i + steps[step][0] >= 0 && i + steps[step][0] < height && j + steps[step][1] >= 0 && j + steps[step][1] < width)
					{
						result[0] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3] * gaussian[(steps[step][0] + 1) * (2 * size + 1) + steps[step][1] + 1];
						result[1] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 1] * gaussian[(steps[step][0] + 1) * (2 * size + 1) + steps[step][1] + 1];
						result[2] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 2] * gaussian[(steps[step][0] + 1) * (2 * size + 1) + steps[step][1] + 1];
						divisor += gaussian[(steps[step][0] + 1) * (2 * size + 1) + steps[step][1] + 1];
					}

				bitMapImageCopy[(i * width + j) * 3] = (unsigned char)(result[0] / divisor);
				bitMapImageCopy[(i * width + j) * 3 + 1] = (unsigned char)(result[1] / divisor);
				bitMapImageCopy[(i * width + j) * 3 + 2] = (unsigned char)(result[2] / divisor);
			}
	}
	else if (size == 2)
	{
		int steps[25][2] = { -2, -2, -2, -1, -2, 0, -2, 1, -2, 2, -1, -2, -1, -1, -1, 0, -1, 1, -1, 2, 0, -2, 0, -1, 0, 0, 0, 1, 0, 2, 1, -2, 1, -1, 1, 0, 1, 1, 1, 2, 2, -2, 2, -1, 2, 0, 2, 1, 2, 2 };

		for (int i = 0; i < height; i++)
			for (int j = 0; j < width; j++)
			{
				double result[3] = { 0, 0, 0 };
				double divisor = 0;

				for (int step = 0; step < 25; step++)
					if (i + steps[step][0] >= 0 && i + steps[step][0] < height && j + steps[step][1] >= 0 && j + steps[step][1] < width)
					{
						result[0] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3] * gaussian[(steps[step][0] + 2) * (2 * size + 1) + steps[step][1] + 2];
						result[1] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 1] * gaussian[(steps[step][0] + 2) * (2 * size + 1) + steps[step][1] + 2];
						result[2] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 2] * gaussian[(steps[step][0] + 2) * (2 * size + 1) + steps[step][1] + 2];
						divisor += gaussian[(steps[step][0] + 2) * (2 * size + 1) + steps[step][1] + 2];
					}

				bitMapImageCopy[(i * width + j) * 3] = (unsigned char)(result[0] / divisor);
				bitMapImageCopy[(i * width + j) * 3 + 1] = (unsigned char)(result[1] / divisor);
				bitMapImageCopy[(i * width + j) * 3 + 2] = (unsigned char)(result[2] / divisor);
			}
	}

	for (int i = 0; i < height * width * 3; i++)
		bitMapImage[i] = bitMapImageCopy[i];
}

void fromColorToBlackAndWhiteFilter(unsigned char* bitMapImage, int height, int width)
{
	for (int i = 0; i < height * width; i++)
	{
		unsigned char result = (299 * bitMapImage[i * 3] + 587 * bitMapImage[i * 3 + 1] + 114 * bitMapImage[i * 3 + 2]) / 1000;
		bitMapImage[i * 3] = result;
		bitMapImage[i * 3 + 1] = result;
		bitMapImage[i * 3 + 2] = result;
	}
}

void sobelFilter(unsigned char* bitMapImage, int height, int width, char mode)
{
	char sobelX[3][3] = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
	char sobelY[3][3] = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };

	unsigned char* bitMapImageCopy = (unsigned char*)malloc(3 * height * width * sizeof(unsigned char));
	for (int i = 0; i < height * width; i++)
		bitMapImageCopy[i] = bitMapImage[i];

	int steps[9][2] = { -1, -1, -1, 0, -1, 1, 0, -1, 0, 0, 0, 1, 1, -1, 1, 0, 1, 1 };

	if (mode == 'x')
		for (int i = 0; i < height; i++)
			for (int j = 0; j < width; j++)
			{
				int result[3] = { 0, 0, 0 };

				for (int step = 0; step < 9; step++)
					if (i + steps[step][0] >= 0 && i + steps[step][0] < height && j + steps[step][1] >= 0 && j + steps[step][1] < width)
					{
						result[0] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3] * sobelX[steps[step][0] + 1][steps[step][1] + 1];
						result[1] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 1] * sobelX[steps[step][0] + 1][steps[step][1] + 1];
						result[2] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 2] * sobelX[steps[step][0] + 1][steps[step][1] + 1];
					}

				bitMapImageCopy[(i * width + j) * 3] = result[0] > 255 ? 255 : (result[0] < 0 ? 0 : result[0]);
				bitMapImageCopy[(i * width + j) * 3 + 1] = result[1] > 255 ? 255 : (result[1] < 0 ? 0 : result[1]);
				bitMapImageCopy[(i * width + j) * 3 + 2] = result[2] > 255 ? 255 : (result[2] < 0 ? 0 : result[2]);
			}
	else if (mode == 'y')
		for (int i = 0; i < height; i++)
			for (int j = 0; j < width; j++)
			{
				int result[3] = { 0, 0, 0 };

				for (int step = 0; step < 9; step++)
					if (i + steps[step][0] >= 0 && i + steps[step][0] < height && j + steps[step][1] >= 0 && j + steps[step][1] < width)
					{
						result[0] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3] * sobelY[steps[step][0] + 1][steps[step][1] + 1];
						result[1] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 1] * sobelY[steps[step][0] + 1][steps[step][1] + 1];
						result[2] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 2] * sobelY[steps[step][0] + 1][steps[step][1] + 1];
					}

				bitMapImageCopy[(i * width + j) * 3] = result[0] > 255 ? 255 : (result[0] < 0 ? 0 : result[0]);
				bitMapImageCopy[(i * width + j) * 3 + 1] = result[1] > 255 ? 255 : (result[1] < 0 ? 0 : result[1]);
				bitMapImageCopy[(i * width + j) * 3 + 2] = result[2] > 255 ? 255 : (result[2] < 0 ? 0 : result[2]);
			}
	else if (mode == '0')
		for (int i = 0; i < height; i++)
			for (int j = 0; j < width; j++)
			{
				int result1[3] = { 0, 0, 0 };
				int result2[3] = { 0, 0, 0 };

				for (int step = 0; step < 9; step++)
					if (i + steps[step][0] >= 0 && i + steps[step][0] < height && j + steps[step][1] >= 0 && j + steps[step][1] < width)
					{
						result1[0] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3] * sobelX[steps[step][0] + 1][steps[step][1] + 1];
						result1[1] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 1] * sobelX[steps[step][0] + 1][steps[step][1] + 1];
						result1[2] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 2] * sobelX[steps[step][0] + 1][steps[step][1] + 1];
						result2[0] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3] * sobelY[steps[step][0] + 1][steps[step][1] + 1];
						result2[1] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 1] * sobelY[steps[step][0] + 1][steps[step][1] + 1];
						result2[2] += bitMapImage[((i + steps[step][0]) * width + j + steps[step][1]) * 3 + 2] * sobelY[steps[step][0] + 1][steps[step][1] + 1];
					}

				int result11 = (299 * abs(result1[0]) + 587 * abs(result1[1]) + 114 * abs(result1[2])) / 1000;
				int result22 = (299 * abs(result2[0]) + 587 * abs(result2[1]) + 114 * abs(result2[2])) / 1000;

				bitMapImageCopy[(i * width + j) * 3] = min((int)sqrt(result11 * result11 + result22 * result22), 255);
				bitMapImageCopy[(i * width + j) * 3 + 1] = min((int)sqrt(result11 * result11 + result22 * result22), 255);
				bitMapImageCopy[(i * width + j) * 3 + 2] = min((int)sqrt(result11 * result11 + result22 * result22), 255);
			}

	fromColorToBlackAndWhiteFilter(bitMapImageCopy, height, width);

	for (int i = 0; i < height * width * 3; i++)
		bitMapImage[i] = bitMapImageCopy[i] > 200 ? 255 : 0;
}

int main(int argc, char* argv[])
{
	inputCheck(argc, argv);

	FILE* fileIn = fopen(argv[1], "rb");
	FILE* fileOut = fopen(argv[3], "wb");
	struct BITMAPFILEHEADER bitMapFileHeader;
	struct BITMAPINFOHEADER bitMapInfoHeader;

	fread(&bitMapFileHeader, sizeof(bitMapFileHeader), 1, fileIn);
	fread(&bitMapInfoHeader, sizeof(bitMapInfoHeader), 1, fileIn);

	unsigned char* bitMapImage = (unsigned char*)malloc(bitMapInfoHeader.biSizeImage);

	fseek(fileIn, bitMapFileHeader.bfOffBits, SEEK_SET);
	fread(bitMapImage, 1, bitMapInfoHeader.biSizeImage, fileIn);

	fwrite(&bitMapFileHeader, sizeof(bitMapFileHeader), 1, fileOut);
	fwrite(&bitMapInfoHeader, sizeof(bitMapInfoHeader), 1, fileOut);

	if (strcmp(argv[2], "Averaging") == 0)
		averageFilter(bitMapImage, bitMapInfoHeader.biHeight, bitMapInfoHeader.biWidth);
	else if (strcmp(argv[2], "Gauss3") == 0)
		gaussFilter(bitMapImage, bitMapInfoHeader.biHeight, bitMapInfoHeader.biWidth, 1);
	else if (strcmp(argv[2], "Gauss5") == 0)
		gaussFilter(bitMapImage, bitMapInfoHeader.biHeight, bitMapInfoHeader.biWidth, 2);
	else if (strcmp(argv[2], "Sobel") == 0)
		sobelFilter(bitMapImage, bitMapInfoHeader.biHeight, bitMapInfoHeader.biWidth, '0');
	else if (strcmp(argv[2], "SobelX") == 0)
		sobelFilter(bitMapImage, bitMapInfoHeader.biHeight, bitMapInfoHeader.biWidth, 'x');
	else if (strcmp(argv[2], "SobelY") == 0)
		sobelFilter(bitMapImage, bitMapInfoHeader.biHeight, bitMapInfoHeader.biWidth, 'y');
	else if (strcmp(argv[2], "ColorWB") == 0)
		fromColorToBlackAndWhiteFilter(bitMapImage, bitMapInfoHeader.biHeight, bitMapInfoHeader.biWidth);

	for (int i = 0; i < bitMapInfoHeader.biSizeImage; i++)
		fwrite(&bitMapImage[i], 1, 1, fileOut);

	printf("The program worked successfully");
	free(bitMapImage);
	fclose(fileIn);
	fclose(fileOut);
	return 0;
}