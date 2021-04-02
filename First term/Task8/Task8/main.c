#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>


#pragma pack(push, 1)

typedef struct bitMapFileHeader
{
	unsigned short bfType;
	unsigned int bfSize;
	unsigned short bfReserved1;
	unsigned short bfReserved2;
	unsigned int bfOffBits;
} bitMapFileHeader;


typedef struct bitMapInfoHeader
{
	unsigned int biSize;
	unsigned int biWidth;
	unsigned int biHeight;
	unsigned short biPlanes;
	unsigned short biBitCount;
	unsigned int  biCompression;
	unsigned int  biSizeImage;
	unsigned int biXPelsPerMeter;
	unsigned int biYPelsPerMeter;
	unsigned int  biClrUsed;
	unsigned int  biClrImportant;
} bitMapInfoHeader;

#pragma pack(pop)

void greyScale(unsigned char* mas, unsigned int biSizeImage)
{
	for (int i = 0; i < biSizeImage / 3; i++)
	{
		int grey = (int)((mas[i * 3] + mas[i * 3 + 1] + mas[i * 3 + 2]) / 3);
		mas[i * 3] = grey;
		mas[i * 3 + 1] = grey;
		mas[i * 3 + 2] = grey;
	}
}

void negativ(unsigned char* mas, unsigned int biSizeImage)
{
	for (int i = 0; i < biSizeImage / 3; i++)
	{
		int grey = (int)((mas[i * 3] + mas[i * 3 + 1] + mas[i * 3 + 2]) / 3);
		mas[i * 3] = 255 - mas[i * 3];
		mas[i * 3 + 1] = 255 - mas[i * 3 + 1];
		mas[i * 3 + 2] = 255 - mas[i * 3 + 2];
	}
}

void multByConvMatrix(double* core, unsigned char* mas, unsigned int biWidth, unsigned int biHeight, int div)
{
	unsigned char* block = (unsigned char*)calloc(3 * biHeight * biWidth, sizeof(unsigned char));
	double r = 0;
	double g = 0;
	double b = 0;
	for (int i = 0; i < biHeight; i++)
	{
		for (int j = 0; j < biWidth; j++)
		{
			for (int s = 0; s < 3; s++)
			{
				for (int t = 0; t < 3; t++)
				{
					if ((i + s - 1 >= 0) && (i + s - 1 < biHeight) && (j + t - 1 >= 0) && (j + t - 1) < biWidth)
					{
						r += mas[((i + s - 1) * biWidth + j + t - 1) * 3] * core[s * 3 + t];
						g += mas[((i + s - 1) * biWidth + j + t - 1) * 3 + 1] * core[s * 3 + t];
						b += mas[((i + s - 1) * biWidth + j + t - 1) * 3 + 2] * core[s * 3 + t];
					}
				}
			}
			if (div && block)
			{
				block[(i * biWidth + j) * 3] = (unsigned char)(r / div);
				block[(i * biWidth + j) * 3 + 1] = (unsigned char)(g / div);
				block[(i * biWidth + j) * 3 + 2] = (unsigned char)(b / div);
			}
			else
			{
				double x = 0;
				if (r + g + b > 384)
					x = 255;
				else
					x = (double)(((double)abs((int)r) + (double)abs((int)b) + (double)abs((int)g)) / 3);
				block[(i * biWidth + j) * 3] = (unsigned char)x;
				block[(i * biWidth + j) * 3 + 1] = (unsigned char)x;
				block[(i * biWidth + j) * 3 + 2] = (unsigned char)x;
			}
			r = g = b = 0;

		}

	}
	for (int i = 1; i < biHeight - 1; i++)
	{
		for (int j = 1; j < biWidth - 1; j++)
		{
			mas[(i * biWidth + j) * 3] = block[(i * biWidth + j) * 3];
			mas[(i * biWidth + j) * 3 + 1] = block[(i * biWidth + j) * 3 + 1];
			mas[(i * biWidth + j) * 3 + 2] = block[(i * biWidth + j) * 3 + 2];
		}
	}
	free(block);
}

void averaging(unsigned char* mas, unsigned int biHeight, unsigned int biWidth)
{
	double core[9] = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
	multByConvMatrix(core, mas, biWidth, biHeight, 9);
}

void gauss(unsigned char* mas, unsigned int biHeight, unsigned int biWidth)
{
	double core[9] = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
	multByConvMatrix(core, mas, biWidth, biHeight, 16);
}

void sobelY(unsigned char* mas, unsigned int biHeight, unsigned int biWidth)
{
	double core[9] = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
	multByConvMatrix(core, mas, biWidth, biHeight, 0);
}

void sobelX(unsigned char* mas, unsigned int biHeight, unsigned int biWidth)
{
	double core[9] = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
	multByConvMatrix(core, mas, biWidth, biHeight, 0);
}

void output(unsigned char* mas, unsigned int biSizeImage, FILE* output_file)
{
	for (int i = 0; i < biSizeImage; i++)
	{
		fwrite(&mas[i], sizeof(char), 1, output_file);
	}
}

int main(int argc, char* argv[])
{
	if (argc != 4)
	{
		printf("Incorrect input. Wrong number of input arguments.");
		exit(EXIT_FAILURE);
	}


	FILE* input_file;

	if ((input_file = (fopen(argv[1], "rb"))) == NULL)
	{
		printf("Incorrect input. Invalid input file name.");
		exit(EXIT_FAILURE);
	}

	bitMapFileHeader bMFH;
	bitMapInfoHeader bMIH;

	fread(&bMFH, sizeof(bMFH), 1, input_file);
	fread(&bMIH, sizeof(bMIH), 1, input_file);

	unsigned char* mas = malloc(bMIH.biSizeImage * sizeof(unsigned char));
	if (mas)
	{
		fread(mas, sizeof(unsigned char), bMIH.biSizeImage, input_file);
	}
	char* filter = argv[2];

	if (strcmp(filter, "grayScale") == 0)
	{
		greyScale(mas, bMIH.biSizeImage);
	}
	else if (strcmp(filter, "averaging") == 0)
	{
		averaging(mas, bMIH.biHeight, bMIH.biWidth);
	}
	else if (strcmp(filter, "negativ") == 0)
	{
		negativ(mas, bMIH.biSizeImage);
	}
	else if (strcmp(filter, "gauss") == 0)
	{
		gauss(mas, bMIH.biHeight, bMIH.biWidth);
	}
	else if (strcmp(filter, "sobelX") == 0)
	{
		sobelX(mas, bMIH.biHeight, bMIH.biWidth);
	}
	else if (strcmp(filter, "sobelY") == 0)
	{
		sobelY(mas, bMIH.biHeight, bMIH.biWidth);
	}
	else
	{
		printf("This filter does not exist.");
		fclose(input_file);
		free(mas);
		exit(EXIT_FAILURE);
	}

	FILE* output_file = fopen(argv[3], "w+b");
	fwrite(&bMFH, sizeof(bMFH), 1, output_file);
	fwrite(&bMIH, sizeof(bMIH), 1, output_file);
	output(mas, bMIH.biSizeImage, output_file);

	free(mas);
	fclose(input_file);
	fclose(output_file);
	return 0;
}