#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

char comp(const char* i, const char* j)
{
	return *i - *j;
}

#pragma pack(push, 1)
struct bmpHeader
{
	unsigned char	b1, b2;			// BM
	unsigned int	bfSize;			// размер файла в байтах	
	unsigned short	bfReserved1;	// резерв    
	unsigned short	bfReserved2;	// резерв 
	unsigned int	bfOffBits;		// смещение до байтов изображения
};

struct bmpHeaderInfo
{
	unsigned int size;              // длина заголовка
	unsigned int width;             // ширина
	unsigned int height;            // высота
	unsigned short planes;          // число плоскостей
	unsigned short bitCount;        // глубина цвета
	unsigned int compression;       // тип компрессии
	unsigned int sizeImage;         // размер изображения
	unsigned int xPelsPerMeter;     // горизонтальное разрешение
	unsigned int yPelsPerMeter;     // вертикальное разрешение
	unsigned int colorsUsed;        // 0 - макс
	unsigned int colorsImportant;   // число основных цветов
};
#pragma pack(pop)

void blackAndWhite(unsigned char* bitmap, int width, int height, int bitcount)
{
	for (int i = 0; i < width * height * (bitcount / 8); i += bitcount / 8)
	{
		unsigned char mid = (bitmap[i] + bitmap[i + 1] + bitmap[i + 2]) / 3;
		for (int j = 0; j < 3; j++)
			bitmap[i + j] = mid;
	}
}

void medianFilter(unsigned char* bitmap, int width, int height, int bitcount)
{
	int bc = bitcount / 8;
	unsigned char* bitmapcopy = (unsigned char*)malloc(bc * height * width * sizeof(unsigned char));
	for (int i = 0; i < height * width * bc; i++)
		bitmapcopy[i] = bitmap[i];
	for (int j = 0; j < 3; j++)
	{
		for (int i = bc * width + j; i < (width * (height - 1)) * bc; i += bc)
		{
			if ((i % (width * bc) < bc) || ((i + bc) % (width * bc) < bc))
				continue;
			unsigned char* median = (unsigned char*)malloc(sizeof(char) * 9);
			median[0] = bitmap[i];
			median[1] = bitmap[i + bc * width];
			median[2] = bitmap[i + bc];
			median[3] = bitmap[i - bc];
			median[4] = bitmap[i - bc * width];
			median[5] = bitmap[i + bc * width + bc];
			median[6] = bitmap[i + bc * width - bc];
			median[7] = bitmap[i - bc * width - bc];
			median[8] = bitmap[i - bc * width + bc];
			qsort(median, 9, sizeof(char), comp);
			bitmapcopy[i] = median[4];
			free(median);
		}
	}
	for (int i = 0; i < height * width * bc; i++)
		bitmap[i] = bitmapcopy[i];
	free(bitmapcopy);
}

void filtersByKey(unsigned char* bitmap, int width, int height, int bitcount, int k)
{
	int bc = bitcount / 8;
	unsigned char* bitmapcopy = (unsigned char*)malloc(bc * height * width * sizeof(unsigned char));
	for (int i = 0; i < height * width * bc; i++)
		bitmapcopy[i] = bitmap[i];

	double matrix[3][9] = { {-1, -2, -1,  0,  0,  0,  1,  2,  1},
							{-1,  0,  1, -2,  0,  2, -1,  0,  1},
							{ 1,  2,  1,  2,  4,  2,  1,  2,  1} };

	for (int j = 0; j < 3; j++)
	{
		for (int i = bc * width + j; i < (width * (height - 1)) * bc; i += bc)
		{
			if ((i % (width * bc) < bc) || ((i + bc) % (width * bc) < bc))
				continue;
			double pixel_value = matrix[k][0] * bitmap[i + bc * width - bc] + matrix[k][1] * bitmap[i + bc * width] + matrix[k][2] * bitmap[i + bc * width + bc]
				+ matrix[k][3] * bitmap[i - bc] + matrix[k][4] * bitmap[i] + matrix[k][5] * bitmap[i + bc] + matrix[k][6] * bitmap[i - bc * width - bc]
				+ matrix[k][7] * bitmap[i - bc * width] + matrix[k][8] * bitmap[i - bc * width + bc];
			if (k < 2)
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
			bitmapcopy[i] = (unsigned char)pixel_value;
		}
	}
	for (int i = 0; i < height * width * bc; i++)
		bitmap[i] = bitmapcopy[i];
	free(bitmapcopy);
}

int main(int argc, char* argv[])
{
	if (argc != 4)
	{
		printf("Enter 4 parameters!");
		return -1;
	}

	char* nameOfFilter[5] = { "sobelFilterX", "sobelFilterY", "gaussianFilter", "blackAndWhite", "medianFilter" };
	void (*filter[4])(void);
	filter[0] = &filtersByKey;
	filter[1] = &blackAndWhite;
	filter[2] = &medianFilter;

	FILE* fin;
	FILE* fout;

	struct bmpHeader bmpheader;
	struct bmpHeaderInfo bmpinfo;

	char b[] = ".bmp";
	for (int i = 1; i < 5; i++)
	{
		if (argv[3][strlen(argv[3]) - i] != b[strlen(b) - i] || argv[1][strlen(argv[1]) - i] != b[strlen(b) - i])
		{
			printf("Invalid name of file!");
			return -1;
		}
	}

	int check = 0;
	int numOfFilter;
	for (int i = 0; i < 5; i++)
	{
		if (strcmp(nameOfFilter[i], argv[2]) == 0)
		{
			check = 1;
			numOfFilter = i;
		}
	}

	if (!check)
	{
		printf("Invalid name of filter");
		return -1;
	}

	if ((fin = fopen(argv[1], "rb")) == NULL)
	{
		printf("Invalid file input!");
		return -1;
	}
	fout = fopen(argv[3], "wb");

	fread(&bmpheader, sizeof(bmpheader), 1, fin);
	fread(&bmpinfo, sizeof(bmpinfo), 1, fin);
	unsigned char* bitmap = (unsigned char*)malloc(bmpinfo.sizeImage);

	unsigned char* colorTable = (unsigned char*)malloc(bmpheader.bfOffBits - 54);

	fread(colorTable, 1, bmpheader.bfOffBits - 54, fin);

	fseek(fin, bmpheader.bfOffBits, SEEK_SET);
	fread(bitmap, 1, bmpinfo.sizeImage, fin);

	fwrite(&bmpheader, sizeof(bmpheader), 1, fout);
	fwrite(&bmpinfo, sizeof(bmpinfo), 1, fout);

	for (int i = 0; i < bmpheader.bfOffBits - 54; i++)
	{
		fwrite(&colorTable[i], 1, 1, fout);
	}

	if (numOfFilter < 3)
		filter[0](bitmap, bmpinfo.width, bmpinfo.height, bmpinfo.bitCount, numOfFilter);
	else
		filter[numOfFilter - 2](bitmap, bmpinfo.width, bmpinfo.height, bmpinfo.bitCount, numOfFilter);

	for (int i = 0; i < bmpinfo.sizeImage; i++)
	{
		fwrite(&bitmap[i], 1, 1, fout);
	}

	free(bitmap);
	free(colorTable);

	fclose(fin);
	fclose(fout);
	printf("Complete!\n");
	system("pause");
	return 0;
}