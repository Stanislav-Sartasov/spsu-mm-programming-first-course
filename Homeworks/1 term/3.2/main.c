#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <malloc.h>
#include <string.h>

#pragma pack(push, 1)
struct bmp_file
{
	unsigned short bf_type;
	unsigned int bf_size;
	unsigned short bf_reversed_one;
	unsigned short bf_reversed_two;
	unsigned int bf_off_bits;
};

struct bmp_info
{
	unsigned int size;
	unsigned int width;
	unsigned int height;
	unsigned short planes;
	unsigned short bit_count;
	unsigned int compression;
	unsigned int size_image;
	unsigned int x_pels_per_meter;
	unsigned int y_pels_per_meter;
	unsigned int color_used;
	unsigned int color_important;
};
#pragma pack(pop)

void layout(unsigned char* inImage, double* bit, int height, int width, int size, int sobel)
{
	unsigned char* outImage = (unsigned char*)malloc(3 * height * width * sizeof(char));

	for (int i = 0; i < height; i++)
	{
		for (int j = 0; j < width; j++)
		{
			double rgb[3] = { 0, 0, 0 };
			double a = 0;

			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					if (((i + y - 1) >= 0) && ((i + y - 1) < height) && ((j + x - 1) >= 0) && ((j + x - 1) < width))
					{
						a = a + bit[y * size + x];
						for (int k = 0; k < 3; k++)
						{
							rgb[k] = rgb[k] + inImage[((i + y - 1) * width + j + x - 1) * 3 + k] * bit[y * size + x];
						}
					}
				}
			}
			
			if (sobel == 0)
			{
				for (int k = 0; k < 3; k++)
				{
					outImage[(i * width + j) * 3 + k] = (unsigned char)(rgb[k] / a);
				}
			}
			else
			{
				int x = 0;
				if ((rgb[0] + rgb[1] + rgb[2]) > 384)
				{
					x = 255;
				}

				for (int k = 0; k < 3; k++)
				{
					outImage[(i * width + j) * 3 + k] = x;
				}
			}
		}
	}

	for (int i = 0; i < height * width * 3; i++)
	{
		inImage[i] = outImage[i];
	}

	free(outImage);
}

void medianFilter(unsigned char* inImage, int height, int width)
{
	int size = 3;
	double* bit = (double*)malloc(size * size * sizeof(double));

	for (int i = 0; i < size * size; i++)
	{
		bit[i] = 1;
	}

	layout(inImage, bit, height, width, size, 0);

	free(bit);
}

void gaussFilter(char type[], unsigned char *inImage, int height, int width)
{
	int size;
	if (strcmp(type, "gauss3") == 0)
	{
		size = 3;
	}
	if (strcmp(type, "gauss5") == 0)
	{
		size = 5;
	}
	double* bit = (double*)malloc(size * size * sizeof(double));

	double sig = 0.6, pi = 3.141592653589793238462643383279;
	
	for (int x = 0; x < size; x++)
	{
		for (int y = 0; y < size; y++)
		{
			bit[x * size + y] = 1.0 / sqrt(2 * pi * sig) * exp( -(x * x + y * y) / (2 * sig * sig) );
		}
	}

	layout(inImage, bit, height, width, size, 0);

	free(bit);
}

void sobelFilter(char type[], unsigned char *inImage, int height, int width)
{
	int size = 3;
	double* bit = (double*)malloc(size * size * sizeof(double));

	if (strcmp(type, "sobelX") == 0)
	{
		int mat[9] = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };

		for (int i = 0; i < size * size; i++)
		{
			bit[i] = mat[i];
		}
	}
	else
	{
		int mat[9] = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };

		for (int i = 0; i < size * size; i++)
		{
			bit[i] = mat[i];
		}
	}

	layout(inImage, bit, height, width, size, 1);

	free(bit);
}

void greyFilter(unsigned char *inImage, int height, int width)
{
	for (int i = 0; i < height * width; i++)
	{
		unsigned char out = (2126 * inImage[i * 3] + 7152 * inImage[i * 3 + 1] + 722 * inImage[i * 3 + 2]) / 10000;
		for (int k = 0; k < 3; k++)
		{
			inImage[i * 3 + k] = out;
		}
	}
}

void filterSelect(unsigned char *binImage, int height, int width, char type[])
{
	if (strcmp(type, "grey") == 0)
	{
		greyFilter(binImage, height, width);
	}
	if (strcmp(type, "median") == 0)
	{
		medianFilter(binImage, height, width);
	}
	if ((strcmp(type, "gauss3") == 0) || (strcmp(type, "gauss5") == 0))
	{
		gaussFilter(type, binImage, height, width);
	}
	if ((strcmp(type, "sobelX") == 0) || (strcmp(type, "sobelY") == 0))
	{
		sobelFilter(type, binImage, height, width);
	}
}

void inputCheckNames(int argc, char *argv[])
{
	if ((argc == 2) && (strcmp(argv[1], "help") == 0))
	{
		printf("Commands: \"help\", \"median\", \"grey\", \"gauss3\", \"gauss5\", \"sobelX\", \"sobelY\"\nPattern of input: \"input.bmp\" \"command\" \"output.bmp\"\nExample: input.bmp gauss3 output.bmp\n");
		exit(0);
	}
	else
	{
		int fl = 0;
		if (!((strcmp(argv[2], "median") == 0) || (strcmp(argv[2], "gauss3") == 0) || (strcmp(argv[2], "gauss5") == 0) || (strcmp(argv[2], "sobelX") == 0) || (strcmp(argv[2], "sobelY") == 0) || (strcmp(argv[2], "grey") == 0)))
		{
			printf("Wrong name of the filter\n");
			fl = 1;
		}

		char ext[] = ".bmp";
		for (int i = 1; i < 5; i++)
		{
			if ((argv[3][strlen(argv[3]) - i] != ext[4 - i]) || (argv[1][strlen(argv[1]) - i] != ext[4 - i]))
			{
				printf("Wrong name of the output file\n");
				fl = 1;
				break;
			}
		}

		if (fl == 1)
		{
			printf("Please, try again or use \"help\"\n");
			exit(0);
		}
	}	
}

void inputCheckFiles(int argc, char *argv[], FILE *fileIn, FILE *fileOut)
{
	short fl = 0;
	if (fileIn == NULL)
	{
		printf("The input file open failed\n");
		fl = 1;
	}
	if (fileOut == NULL)
	{
		printf("The output file create failed\n");
		fl = 1;
	}
	if (fl == 1)
	{
		printf("Please, try again or use \"help\"\n");
		exit(0);
	}
}

int main(int argc, char *argv[])
{
	if (!((argc == 2) && (strcmp(argv[1], "help") == 0)) && (argc != 4))
	{
		printf("Wrong number of the incoming parameters\nPlease, try again or use \"help\"\n");
		exit(0);
	}

	inputCheckNames(argc, argv);

	FILE *fileIn = fopen(argv[1], "rb");
	FILE *fileOut = fopen(argv[3], "wb");

	inputCheckFiles(argc, argv, fileIn, fileOut);

	struct bmp_file fileHeader;
	struct bmp_info infoHeader;

	fread(&fileHeader, sizeof(fileHeader), 1, fileIn);
	fread(&infoHeader, sizeof(infoHeader), 1, fileIn);

	unsigned char *binImage = (unsigned char*)malloc(infoHeader.size_image);
	fseek(fileIn, fileHeader.bf_off_bits, SEEK_SET);
	fread(binImage, 1, infoHeader.size_image, fileIn);

	filterSelect(binImage, infoHeader.height, infoHeader.width, argv[2]);

	printf("Success!\n");

	fwrite(&fileHeader, sizeof(fileHeader), 1, fileOut);
	fwrite(&infoHeader, sizeof(infoHeader), 1, fileOut);

	for (int i = 0; i < infoHeader.size_image; i++)
	{
		fwrite(&binImage[i], 1, 1, fileOut);
	}

	free(binImage);
	fclose(fileIn);
	fclose(fileOut);
}