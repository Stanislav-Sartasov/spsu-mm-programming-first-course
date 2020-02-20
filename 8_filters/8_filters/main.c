#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>

unsigned char sobel(int x)
{
	return x > 128 ? 255 : 0;
}

void inputValidation(int argc, char* argv[])
{
	if (argc != 4)
	{
		printf("Wrong number of parameters.");
		exit(-1);
	}
	if (strcmp(argv[2], "Averaging") != 0 && strcmp(argv[2], "Gauss3") != 0 && strcmp(argv[2], "Gauss5") != 0 &&
		strcmp(argv[2], "Grey") != 0 && strcmp(argv[2], "SobelX") != 0 && strcmp(argv[2], "SobelY") != 0)
	{
		printf("Wrong filter name.");
		exit(-1);
	}
}

void applyFilter(unsigned char* image, int height, int width, char* mode)
{
	unsigned char* imageCopy = (unsigned char*)malloc(3 * height * width * sizeof(char));

	if (strcmp("Grey", mode) == 0)
	{
		for (int i = 0; i < width * height; i++)
		{
			unsigned char result = (2126 * image[i * 3] + 7152 * image[i * 3 + 1] + 722 * image[i * 3 + 2]) / 10000;
			image[3 * i] = result;
			image[3 * i + 1] = result;
			image[3 * i + 2] = result;
		}
		return;
	}

	const int size = (strcmp("Gauss5", mode) == 0) ? 25 : 9;
	int border = (strcmp("Gauss5", mode) == 0) ? 2 : 1,
		divisor = 1;
	int* mask = (int*)malloc(sizeof(int) * size);

	if (strcmp("Averaging", mode) == 0)
	{
		for (int i = 0; i < 9; i++)
			mask[i] = 1;
		divisor = 9;
	}
	else if (strcmp("SobelX", mode) == 0)
	{
		int temp[9] = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
		for (int i = 0; i < 9; i++)
			mask[i] = temp[i];
		divisor = 1;
	}
	else if (strcmp("SobelY", mode) == 0)
	{
		int temp[9] = { -1, 0, 1, -2, 0, 2, -1 ,0, 1 };
		for (int i = 0; i < 9; i++)
			mask[i] = temp[i];
		divisor = 1;
	}
	else if (strcmp("Gauss3", mode) == 0)
	{
		int temp[9] = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
		for (int i = 0; i < 9; i++)
			mask[i] = temp[i];
		divisor = 16;
	}
	else if (strcmp("Gauss5", mode) == 0)
	{
		int temp[25] =
		{
			1, 4, 6, 4, 1,
			4, 16, 24, 16, 4,
			6, 24, 36, 24, 6,
			4, 16, 24, 16, 4,
			1, 4, 6, 4, 1
		};
		for (int i = 0; i < 25; i++)
			mask[i] = temp[i];
		divisor = 256;
	}

	int flag = 0;
	if (strcmp("SobelX", mode) == 0 || strcmp("SobelY", mode) == 0)
		flag = 1;

	int red = 0;
	int green = 0;
	int blue = 0;
	int counter = 0;
	for (int i = border; i < height - border; i++)
		for (int j = border; j < width - border; j++)
		{
			red = green = blue = counter = 0;
			for (int v = -border; v <= border; v++)
				for (int w = -border; w <= border; w++)
				{
					red += mask[counter] * image[((i + v) * width + (j + w)) * 3];
					green += mask[counter] * image[((i + v) * width + (j + w)) * 3 + 1];
					blue += mask[counter] * image[((i + v) * width + (j + w)) * 3 + 2];
					counter++;
				}
			red /= divisor;
			green /= divisor;
			blue /= divisor;

			if (flag)
			{
				unsigned char result = sobel((red + green + blue) / 3);
				for (int k = 0; k < 3; k++)
					imageCopy[(i * width + j) * 3 + k] = result;
			}
			else
			{
				imageCopy[(i * width + j) * 3] = red;
				imageCopy[(i * width + j) * 3 + 1] = green;
				imageCopy[(i * width + j) * 3 + 2] = blue;
			}
		}

	for (int i = 0; i < height * width * 3; i++)
		image[i] = imageCopy[i];

	free(imageCopy);
	free(mask);
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

int main(int argc, char *argv[])
{
	printf("Example: \"input.bmp filter_name output.bmp\"\n");
	inputValidation(argc, argv);
	FILE* fIn;
	FILE* fOut;
	fIn = fopen(argv[1], "rb");
	fOut = fopen(argv[3], "wb");

	if (fIn == NULL)
	{
		printf("Cannot open the input file.");
		exit(-1);
	}
	if (fOut == NULL)
	{
		printf("Cannot open the output file.");
		exit(-1);
	}

	struct BITMAPFILEHEADER fileHeader;
	struct BITMAPINFOHEADER infoHeader;
	fread(&fileHeader, sizeof(fileHeader), 1, fIn);
	fread(&infoHeader, sizeof(infoHeader), 1, fIn);

	unsigned char* binaryInput = (unsigned char*)malloc(infoHeader.biSizeImage);
	if (binaryInput == NULL) exit(-1);
	fseek(fIn, fileHeader.bfOffBits, SEEK_SET);
	fread(binaryInput, 1, infoHeader.biSizeImage, fIn);

	applyFilter(binaryInput, infoHeader.biHeight, infoHeader.biWidth, argv[2]);

	fwrite(&fileHeader, sizeof(fileHeader), 1, fOut);
	fwrite(&infoHeader, sizeof(infoHeader), 1, fOut);

	for (unsigned int i = 0; i < infoHeader.biSizeImage; i++)
		fwrite(&binaryInput[i], 1, 1, fOut);

	free(binaryInput);
	fclose(fIn);
	fclose(fOut);

	return 0;
}