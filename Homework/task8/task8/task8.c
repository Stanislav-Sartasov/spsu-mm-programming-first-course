#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

char comp(const char* i, const char* j)
{
	return *i - *j;
}

#pragma pack(push, 1)
struct bmpheader
{
	unsigned char	b1, b2;			//BM
	unsigned int	bfSize;			//размер файла в байтах	
	unsigned short	bfReserved1;	//резерв    
	unsigned short	bfReserved2;	//резеов 
	unsigned int	bfOffBits;		//смещение до байтов изображения
};

struct bmpheaderinfo 
{
	unsigned int Size;              //длина заголовка
	unsigned int Width;             // ширина
	unsigned int Height;            // высота
	unsigned short Planes;          // число плоскостей
	unsigned short BitCount;        // глубина цвета
	unsigned int Compression;       // тип компрессии
	unsigned int SizeImage;         // размер изображения
	unsigned int XpelsPerMeter;     // горизонтальное разрешение
	unsigned int YpelsPerMeter;     // вертикальное разрешение
	unsigned int ColorsUsed;        // 0 - макс
	unsigned int ColorsImportant;   // число основных цветов
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

void sobelFilterX(unsigned char* bitmap, int width, int height, int bitcount)
{
	int bc = bitcount / 8;
	unsigned char* bitmapcopy = (unsigned char*)malloc(bc * height * width * sizeof(char));
	for (int i = 0; i < height * width * bc; i++)
		bitmapcopy[i] = bitmap[i];
	for (int j = 0; j < 3; j++)
	{
		for (int i = bc * width + j; i < (width * (height - 1)) * bc; i += bc)
		{
			if ((i % (width * bc) < bc) || ((i + bc) % (width * bc) < bc))
				continue;
			int x = bitmap[i - bc * width - bc] + 2 * bitmap[i - bc * width] + bitmap[i - bc * width + bc]
				- bitmap[i + bc * width - bc] - 2 * bitmap[i + bc * width] - bitmap[i + bc * width + bc];
			if (x > 255)
				x = 255;
			if (x < 0)
				x = 0;
			bitmapcopy[i] = x;
		}
	}
	for (int i = 0; i < height * width * bc; i++)
		bitmap[i] = bitmapcopy[i];
	free(bitmapcopy);
}

void sobelFilterY(unsigned char* bitmap, int width, int height, int bitcount)
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
			int y = (bitmap[i + bc * width + bc] + 2 * bitmap[i + bc] + bitmap[i - bc * width + bc]
				- bitmap[i + bc * width - bc] - 2 * bitmap[i - bc] - bitmap[i - bc * width - bc]);
			if (y > 255)
				y = 255;
			if (y < 0)
				y = 0;
			bitmapcopy[i] = y;
		}
	}
	for (int i = 0; i < height * width * bc; i++)
		bitmap[i] = bitmapcopy[i];
	free(bitmapcopy);
	
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

void gaussianFilter(unsigned char* bitmap, int width, int height, int bitcount)
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
			bitmapcopy[i] = 0.0625 * (4 * bitmapcopy[i] + 2 * (bitmapcopy[i + bc * width] + bitmapcopy[i + bc] + bitmapcopy[i - bc] + bitmapcopy[i - bc * width]) +
				1 * (bitmapcopy[i + bc * width + bc] + bitmapcopy[i + bc * width - bc] + bitmapcopy[i - bc * width - bc] + bitmapcopy[i - bc * width + bc]));
		}
		bitmapcopy[j] = 0.0625 * (4 * bitmapcopy[j] + 2 * (bitmapcopy[j + bc] + bitmapcopy[bc * width + j]) + bitmapcopy[bc * width + bc + j]);

		bitmapcopy[(width - 1) * bc + j] = 0.0625 * (4 * bitmapcopy[(width - 1) * bc + j]
			+ 2 * (bitmapcopy[(width - 2) * 3 + j] + bitmapcopy[2 * bc * width - bc + j]) + bitmapcopy[2 * bc * width - 2 * bc + j]);
		bitmapcopy[height * width * bc - bc + j] = 0.0625 * (4 * bitmapcopy[height * width * bc - bc + j]
			+ 2 * (bitmapcopy[height * width * bc - bc + j - bc] + bitmapcopy[height * (width - 1) * bc - bc + j]) + bitmapcopy[height * (width - 1) * bc - 2 * bc + j]);
	}
	for (int i = 0; i < height * width * bc; i++)
		bitmap[i] = bitmapcopy[i];
	free(bitmapcopy);
}

int main(int argc, char* argv[])
{
	char* name_of_filter[5] = { "blackAndWhite", "sobelFilterX", "sobelFilterY", "medianFilter", "gaussianFilter" };
	void (*filter[5])(void);
	filter[0] = &blackAndWhite;
	filter[1] = &sobelFilterX;
	filter[2] = &sobelFilterY;
	filter[3] = &medianFilter;
	filter[4] = &gaussianFilter;

	FILE* fin;
	FILE* fout;
	

	struct bmpheader bmpheader;
	struct bmpheaderinfo bmpinfo;

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
	int num_of_filter;
	for (int i = 0; i < 5; i++)
	{
		if (strcmp(name_of_filter[i], argv[2]) == 0)
		{
			check = 1;
			num_of_filter = i;
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

	unsigned int size = bmpinfo.Width * bmpinfo.Height * (bmpinfo.BitCount / 8);
	unsigned char* colorTable = (unsigned char*)malloc(bmpheader.bfOffBits - 54);

	fread(colorTable, 1, bmpheader.bfOffBits - 54, fin);

	unsigned char* bitmap = (unsigned char*)malloc(bmpinfo.SizeImage);
	fseek(fin, bmpheader.bfOffBits, SEEK_SET);
	fread(bitmap, 1, bmpinfo.SizeImage, fin);

	fwrite(&bmpheader, sizeof(bmpheader), 1, fout);
	fwrite(&bmpinfo, sizeof(bmpinfo), 1, fout);
	for (int i = 0; i < bmpheader.bfOffBits - 54; i++)
	{
		fwrite(&colorTable[i], 1, 1, fout);
	}

	filter[num_of_filter](bitmap, bmpinfo.Width, bmpinfo.Height, bmpinfo.BitCount);

	for (int i = 0; i < bmpinfo.SizeImage; i++)
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
