#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILTER_AVERAGE		"average"
#define FILTER_GAUSS3		"gauss3"
#define FILTER_GAUSS5		"gauss5"
#define FILTER_SOBELX		"sobelx"
#define FILTER_SOBELY		"sobely"
#define FILTER_GRAY			"grey"

typedef unsigned int DWORD;
typedef long LONG;
typedef unsigned short WORD;
typedef unsigned char BYTE;

#pragma pack(1)

typedef struct tagBITMAPINFOHEADER{
	DWORD      biSize;
	LONG       biWidth;
	LONG       biHeight;
	WORD       biPlanes;
	WORD       biBitCount;
	DWORD      biCompression;
	DWORD      biSizeImage;
	LONG       biXPelsPerMeter;
	LONG       biYPelsPerMeter;
	DWORD      biClrUsed;
	DWORD      biClrImportant;
} BITMAPINFOHEADER;

typedef struct tagBITMAPFILEHEADER
{
	WORD    bfType;
	DWORD   bfSize;
	WORD    bfReserved1;
	WORD    bfReserved2;
	DWORD   bfOffBits;
} BITMAPFILEHEADER;

typedef struct tagRGB
{
	unsigned char rgbBlue;
	unsigned char rgbGreen;
	unsigned char rgbRed;
} RGB;

#pragma pack()

int read_image_file(char* input_file, BITMAPFILEHEADER* file_header, BITMAPINFOHEADER* info_header, RGB** pImg, char** pPal, int* pPalSize, int* pStride)
{
	FILE* fp;
	int k;

	if (NULL == (fp = fopen(input_file, "rb")))
	{
		printf("Can't open the input file.\n");
		return 0;
	}
	fread(file_header, sizeof(BITMAPFILEHEADER), 1, fp);
	fread(info_header, sizeof(BITMAPINFOHEADER), 1, fp);
	if (info_header->biBitCount != 24 && info_header->biBitCount != 32)
	{
		fclose(fp);
		return 0;
	}

	*pPalSize = file_header->bfOffBits - sizeof(BITMAPFILEHEADER) - sizeof(BITMAPINFOHEADER);
	if (*pPalSize > 0)
	{
		*pPal = (char*)malloc(*pPalSize);
		fread(*pPal, 1, *pPalSize, fp);
	}

	*pImg = (RGB*)malloc(info_header->biWidth * info_header->biHeight * sizeof(RGB));
	*pStride = (4 - (info_header->biWidth * (info_header->biBitCount / 8)) % 4) & 3;

	k = 0;
	for (int i = 0 ; i < info_header->biHeight ; i++)
	{
		for (int j = 0 ; j < info_header->biWidth ; j++)
		{
			fread(*pImg + k, sizeof(RGB), 1, fp);
			k = k + 1;
			if (info_header->biBitCount == 32)
				fseek(fp, 1, SEEK_CUR);
		}
		fseek(fp, *pStride, SEEK_CUR);
	}

	fclose(fp);
	return 1;
}

void set_average_filter(RGB* img, int width, int height)
{
	int i, j;
	int r, g, b;
	RGB* t;

	t = (RGB*)malloc(width * height * sizeof(RGB));
	memcpy(t, img, width * height * sizeof(RGB));

	for (i = 1 ; i < height-1 ; i++)
	{
		for (j = 1 ; j < width-1 ; j++)
		{
			r = t[(i-1)*width+j-1].rgbRed + t[(i-1)*width+j].rgbRed + t[(i-1)*width+j+1].rgbRed + 
				t[(i-0)*width+j-1].rgbRed + t[(i-0)*width+j].rgbRed + t[(i-0)*width+j+1].rgbRed + 
				t[(i+1)*width+j-1].rgbRed + t[(i+1)*width+j].rgbRed + t[(i+1)*width+j+1].rgbRed;

			g = t[(i-1)*width+j-1].rgbGreen + t[(i-1)*width+j].rgbGreen + t[(i-1)*width+j+1].rgbGreen + 
				t[(i-0)*width+j-1].rgbGreen + t[(i-0)*width+j].rgbGreen + t[(i-0)*width+j+1].rgbGreen + 
				t[(i+1)*width+j-1].rgbGreen + t[(i+1)*width+j].rgbGreen + t[(i+1)*width+j+1].rgbGreen;

			b = t[(i-1)*width+j-1].rgbBlue + t[(i-1)*width+j].rgbBlue + t[(i-1)*width+j+1].rgbBlue + 
				t[(i-0)*width+j-1].rgbBlue + t[(i-0)*width+j].rgbBlue + t[(i-0)*width+j+1].rgbBlue + 
				t[(i+1)*width+j-1].rgbBlue + t[(i+1)*width+j].rgbBlue + t[(i+1)*width+j+1].rgbBlue;

			img[i*width+j].rgbRed = r / 9;
			img[i*width+j].rgbGreen = g / 9;
			img[i*width+j].rgbBlue = b / 9;
		}
	}

	free(t);
}

void set_gauss_filter_3(RGB* img, int width, int height)
{
	int i, j;
	int r, g, b;
	RGB* t;
	int gauss3[3][3] = { {1, 2, 1}, {2, 4, 2}, {1, 2, 1} };

	t = (RGB*)malloc(width * height * sizeof(RGB));
	memcpy(t, img, width * height * sizeof(RGB));

	for (i = 1 ; i < height - 1 ; i++)
	{
		for(j = 1 ; j < width - 1 ; j++)
		{
			r = t[(i-1)*width+j-1].rgbRed * gauss3[0][0] + t[(i-1)*width+j].rgbRed * gauss3[0][1] + t[(i-1)*width+j+1].rgbRed * gauss3[0][2] + 
				t[(i-0)*width+j-1].rgbRed * gauss3[1][0] + t[(i-0)*width+j].rgbRed * gauss3[1][1] + t[(i-0)*width+j+1].rgbRed * gauss3[1][2] + 
				t[(i+1)*width+j-1].rgbRed * gauss3[2][0] + t[(i+1)*width+j].rgbRed * gauss3[2][1] + t[(i+1)*width+j+1].rgbRed * gauss3[2][2];

			g = t[(i-1)*width+j-1].rgbGreen * gauss3[0][0] + t[(i-1)*width+j].rgbGreen * gauss3[0][1] + t[(i-1)*width+j+1].rgbGreen * gauss3[0][2] + 
				t[(i-0)*width+j-1].rgbGreen * gauss3[1][0] + t[(i-0)*width+j].rgbGreen * gauss3[1][1] + t[(i-0)*width+j+1].rgbGreen * gauss3[1][2] + 
				t[(i+1)*width+j-1].rgbGreen * gauss3[2][0] + t[(i+1)*width+j].rgbGreen * gauss3[2][1] + t[(i+1)*width+j+1].rgbGreen * gauss3[2][2];

			b = t[(i-1)*width+j-1].rgbBlue * gauss3[0][0] + t[(i-1)*width+j].rgbBlue * gauss3[0][1] + t[(i-1)*width+j+1].rgbBlue * gauss3[0][2] + 
				t[(i-0)*width+j-1].rgbBlue * gauss3[1][0] + t[(i-0)*width+j].rgbBlue * gauss3[1][1] + t[(i-0)*width+j+1].rgbBlue * gauss3[1][2] + 
				t[(i+1)*width+j-1].rgbBlue * gauss3[2][0] + t[(i+1)*width+j].rgbBlue * gauss3[2][1] + t[(i+1)*width+j+1].rgbBlue * gauss3[2][2];

			img[i*width+j].rgbRed = r / 16;
			img[i*width+j].rgbGreen = g / 16;
			img[i*width+j].rgbBlue = b / 16;
		}
	}

	free(t);
}

void set_gauss_filter_5(RGB* img, int width, int height)
{
	int i, j;
	int r, g, b;
	RGB* t;
	int gauss5[5][5] = { {1, 4,  6,  4,  1}, {4, 16, 24, 16, 4}, {6, 24, 36, 24, 6}, {4, 16, 24, 16, 4}, {1, 4,  6,  4,  1} };

	t = (RGB*)malloc(width * height * sizeof(RGB));
	memcpy(t, img, width * height * sizeof(RGB));

	for (i = 2 ; i < height - 2 ; i++)
	{
		for(j = 2 ; j < width - 2 ; j++)
		{
			r = t[(i-2)*width+j-2].rgbRed * gauss5[0][0] + t[(i-2)*width+j-1].rgbRed * gauss5[0][1] + t[(i-2)*width+j].rgbRed * gauss5[0][2] + t[(i-2)*width+j+1].rgbRed * gauss5[0][3] + t[(i-2)*width+j+2].rgbRed * gauss5[0][4] + 
				t[(i-1)*width+j-2].rgbRed * gauss5[1][0] + t[(i-1)*width+j-1].rgbRed * gauss5[1][1] + t[(i-1)*width+j].rgbRed * gauss5[1][2] + t[(i-1)*width+j+1].rgbRed * gauss5[1][3] + t[(i-1)*width+j+2].rgbRed * gauss5[1][4] + 
				t[(i-0)*width+j-2].rgbRed * gauss5[2][0] + t[(i-0)*width+j-1].rgbRed * gauss5[2][1] + t[(i-0)*width+j].rgbRed * gauss5[2][2] + t[(i-0)*width+j+1].rgbRed * gauss5[2][3] + t[(i-0)*width+j+2].rgbRed * gauss5[2][4] + 
				t[(i+1)*width+j-2].rgbRed * gauss5[3][0] + t[(i+1)*width+j-1].rgbRed * gauss5[3][1] + t[(i+1)*width+j].rgbRed * gauss5[3][2] + t[(i+1)*width+j+1].rgbRed * gauss5[3][3] + t[(i+1)*width+j+2].rgbRed * gauss5[3][4] + 
				t[(i+2)*width+j-2].rgbRed * gauss5[4][0] + t[(i+2)*width+j-1].rgbRed * gauss5[4][1] + t[(i+2)*width+j].rgbRed * gauss5[4][2] + t[(i+2)*width+j+1].rgbRed * gauss5[4][3] + t[(i+2)*width+j+2].rgbRed * gauss5[4][4];

			g = t[(i-2)*width+j-2].rgbGreen * gauss5[0][0] + t[(i-2)*width+j-1].rgbGreen * gauss5[0][1] + t[(i-2)*width+j].rgbGreen * gauss5[0][2] + t[(i-2)*width+j+1].rgbGreen * gauss5[0][3] + t[(i-2)*width+j+2].rgbGreen * gauss5[0][4] + 
				t[(i-1)*width+j-2].rgbGreen * gauss5[1][0] + t[(i-1)*width+j-1].rgbGreen * gauss5[1][1] + t[(i-1)*width+j].rgbGreen * gauss5[1][2] + t[(i-1)*width+j+1].rgbGreen * gauss5[1][3] + t[(i-1)*width+j+2].rgbGreen * gauss5[1][4] + 
				t[(i-0)*width+j-2].rgbGreen * gauss5[2][0] + t[(i-0)*width+j-1].rgbGreen * gauss5[2][1] + t[(i-0)*width+j].rgbGreen * gauss5[2][2] + t[(i-0)*width+j+1].rgbGreen * gauss5[2][3] + t[(i-0)*width+j+2].rgbGreen * gauss5[2][4] + 
				t[(i+1)*width+j-2].rgbGreen * gauss5[3][0] + t[(i+1)*width+j-1].rgbGreen * gauss5[3][1] + t[(i+1)*width+j].rgbGreen * gauss5[3][2] + t[(i+1)*width+j+1].rgbGreen * gauss5[3][3] + t[(i+1)*width+j+2].rgbGreen * gauss5[3][4] + 
				t[(i+2)*width+j-2].rgbGreen * gauss5[4][0] + t[(i+2)*width+j-1].rgbGreen * gauss5[4][1] + t[(i+2)*width+j].rgbGreen * gauss5[4][2] + t[(i+2)*width+j+1].rgbGreen * gauss5[4][3] + t[(i+2)*width+j+2].rgbGreen * gauss5[4][4];

			b = t[(i-2)*width+j-2].rgbBlue * gauss5[0][0] + t[(i-2)*width+j-1].rgbBlue * gauss5[0][1] + t[(i-2)*width+j].rgbBlue * gauss5[0][2] + t[(i-2)*width+j+1].rgbBlue * gauss5[0][3] + t[(i-2)*width+j+2].rgbBlue * gauss5[0][4] + 
				t[(i-1)*width+j-2].rgbBlue * gauss5[1][0] + t[(i-1)*width+j-1].rgbBlue * gauss5[1][1] + t[(i-1)*width+j].rgbBlue * gauss5[1][2] + t[(i-1)*width+j+1].rgbBlue * gauss5[1][3] + t[(i-1)*width+j+2].rgbBlue * gauss5[1][4] + 
				t[(i-0)*width+j-2].rgbBlue * gauss5[2][0] + t[(i-0)*width+j-1].rgbBlue * gauss5[2][1] + t[(i-0)*width+j].rgbBlue * gauss5[2][2] + t[(i-0)*width+j+1].rgbBlue * gauss5[2][3] + t[(i-0)*width+j+2].rgbBlue * gauss5[2][4] + 
				t[(i+1)*width+j-2].rgbBlue * gauss5[3][0] + t[(i+1)*width+j-1].rgbBlue * gauss5[3][1] + t[(i+1)*width+j].rgbBlue * gauss5[3][2] + t[(i+1)*width+j+1].rgbBlue * gauss5[3][3] + t[(i+1)*width+j+2].rgbBlue * gauss5[3][4] + 
				t[(i+2)*width+j-2].rgbBlue * gauss5[4][0] + t[(i+2)*width+j-1].rgbBlue * gauss5[4][1] + t[(i+2)*width+j].rgbBlue * gauss5[4][2] + t[(i+2)*width+j+1].rgbBlue * gauss5[4][3] + t[(i+2)*width+j+2].rgbBlue * gauss5[4][4];

			img[i*width+j].rgbRed = r / 256;
			img[i*width+j].rgbGreen = g / 256;
			img[i*width+j].rgbBlue = b / 256;
		}
	}

	free(t);
}

void set_sobel_filter_x(RGB* img, int width, int height)
{
	int i, j;
	int r, g, b;
	RGB* t;
	int sobelx[3][3] = { {1,  2,  1}, {0,  0,  0}, {-1, -2, -1} };

	t = (RGB*)malloc(width * height * sizeof(RGB));
	memcpy(t, img, width * height * sizeof(RGB));

	for (i = 1 ; i < height - 1 ; i++)
	{
		for(j = 1 ; j < width - 1 ; j++)
		{
			r = t[(i-1)*width+j-1].rgbRed * sobelx[0][0] + t[(i-1)*width+j].rgbRed * sobelx[0][1] + t[(i-1)*width+j+1].rgbRed * sobelx[0][2] + 
				t[(i-0)*width+j-1].rgbRed * sobelx[1][0] + t[(i-0)*width+j].rgbRed * sobelx[1][1] + t[(i-0)*width+j+1].rgbRed * sobelx[1][2] + 
				t[(i+1)*width+j-1].rgbRed * sobelx[2][0] + t[(i+1)*width+j].rgbRed * sobelx[2][1] + t[(i+1)*width+j+1].rgbRed * sobelx[2][2];

			g = t[(i-1)*width+j-1].rgbGreen * sobelx[0][0] + t[(i-1)*width+j].rgbGreen * sobelx[0][1] + t[(i-1)*width+j+1].rgbGreen * sobelx[0][2] + 
				t[(i-0)*width+j-1].rgbGreen * sobelx[1][0] + t[(i-0)*width+j].rgbGreen * sobelx[1][1] + t[(i-0)*width+j+1].rgbGreen * sobelx[1][2] + 
				t[(i+1)*width+j-1].rgbGreen * sobelx[2][0] + t[(i+1)*width+j].rgbGreen * sobelx[2][1] + t[(i+1)*width+j+1].rgbGreen * sobelx[2][2];

			b = t[(i-1)*width+j-1].rgbBlue * sobelx[0][0] + t[(i-1)*width+j].rgbBlue * sobelx[0][1] + t[(i-1)*width+j+1].rgbBlue * sobelx[0][2] + 
				t[(i-0)*width+j-1].rgbBlue * sobelx[1][0] + t[(i-0)*width+j].rgbBlue * sobelx[1][1] + t[(i-0)*width+j+1].rgbBlue * sobelx[1][2] + 
				t[(i+1)*width+j-1].rgbBlue * sobelx[2][0] + t[(i+1)*width+j].rgbBlue * sobelx[2][1] + t[(i+1)*width+j+1].rgbBlue * sobelx[2][2];

			if (r < 0) r = 0;
			if (r > 255) r = 255;
			if (g < 0) g = 0;
			if (g > 255) g = 255;
			if (b < 0) b = 0;
			if (b > 255) b = 255;

			img[i*width+j].rgbRed = r;
			img[i*width+j].rgbGreen = g;
			img[i*width+j].rgbBlue = b;
		}
	}

	free(t);
}

void set_sobel_filter_y(RGB* img, int width, int height)
{
	int i, j;
	int r, g, b;
	RGB* t;
	int sobely[3][3] = { {-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1} };

	t = (RGB*)malloc(width * height * sizeof(RGB));
	memcpy(t, img, width * height * sizeof(RGB));

	for (i = 1 ; i < height - 1 ; i++)
	{
		for(j = 1 ; j < width - 1 ; j++)
		{
			r = t[(i-1)*width+j-1].rgbRed * sobely[0][0] + t[(i-1)*width+j].rgbRed * sobely[0][1] + t[(i-1)*width+j+1].rgbRed * sobely[0][2] + 
				t[(i-0)*width+j-1].rgbRed * sobely[1][0] + t[(i-0)*width+j].rgbRed * sobely[1][1] + t[(i-0)*width+j+1].rgbRed * sobely[1][2] + 
				t[(i+1)*width+j-1].rgbRed * sobely[2][0] + t[(i+1)*width+j].rgbRed * sobely[2][1] + t[(i+1)*width+j+1].rgbRed * sobely[2][2];

			g = t[(i-1)*width+j-1].rgbGreen * sobely[0][0] + t[(i-1)*width+j].rgbGreen * sobely[0][1] + t[(i-1)*width+j+1].rgbGreen * sobely[0][2] + 
				t[(i-0)*width+j-1].rgbGreen * sobely[1][0] + t[(i-0)*width+j].rgbGreen * sobely[1][1] + t[(i-0)*width+j+1].rgbGreen * sobely[1][2] + 
				t[(i+1)*width+j-1].rgbGreen * sobely[2][0] + t[(i+1)*width+j].rgbGreen * sobely[2][1] + t[(i+1)*width+j+1].rgbGreen * sobely[2][2];

			b = t[(i-1)*width+j-1].rgbBlue * sobely[0][0] + t[(i-1)*width+j].rgbBlue * sobely[0][1] + t[(i-1)*width+j+1].rgbBlue * sobely[0][2] + 
				t[(i-0)*width+j-1].rgbBlue * sobely[1][0] + t[(i-0)*width+j].rgbBlue * sobely[1][1] + t[(i-0)*width+j+1].rgbBlue * sobely[1][2] + 
				t[(i+1)*width+j-1].rgbBlue * sobely[2][0] + t[(i+1)*width+j].rgbBlue * sobely[2][1] + t[(i+1)*width+j+1].rgbBlue * sobely[2][2];

			if (r < 0) r = 0;
			if (r > 255) r = 255;
			if (g < 0) g = 0;
			if (g > 255) g = 255;
			if (b < 0) b = 0;
			if (b > 255) b = 255;

			img[i*width+j].rgbRed = r;
			img[i*width+j].rgbGreen = g;
			img[i*width+j].rgbBlue = b;
		}
	}

	free(t);
}

void set_grey_filter(RGB* img, int width, int height)
{
	int i, j;
	int val;

	for (i = 0 ; i < height ; i++)
	{
		for (j = 0 ; j < width ; j++)
		{
			val = (img[i*width+j].rgbRed + img[i*width+j].rgbGreen + img[i*width+j].rgbBlue) / 3;
			img[i*width+j].rgbRed = val;
			img[i*width+j].rgbGreen = val;
			img[i*width+j].rgbBlue = val;
		}
	}
}

void write_image_file(char* output_file, BITMAPFILEHEADER* file_header, BITMAPINFOHEADER* info_header, RGB* img, char* pal, int palSize, int pStride)
{
	FILE* fp;

	if ((fp = (fopen(output_file, "wb"))) == NULL)
	{
		printf("Can't open the output file");
		return;
	}
	fwrite(file_header, sizeof(BITMAPFILEHEADER), 1, fp);
	fwrite(info_header, sizeof(BITMAPINFOHEADER), 1, fp);

	if (palSize)
	{
		fwrite(pal, 1, palSize, fp);
	}

	fwrite(img, sizeof(RGB), info_header->biHeight * info_header->biWidth, fp);

	fclose(fp);
}

int main(int argc, char* argv[])
{
	BITMAPFILEHEADER file_header;
	BITMAPINFOHEADER info_header;
	RGB* img;
	char* pal = 0;
	int pal_size, stride;

	if (argc != 4)
	{
		printf("<exe_name> FILTER_NAME INPUT_BMP OUTPUT_BMP\n");
		printf("for example\n");
		printf("task8.exe %s imput.bmp output.bmp\n", FILTER_AVERAGE);
		printf("task8.exe %s imput.bmp output.bmp\n", FILTER_GAUSS3);
		printf("task8.exe %s imput.bmp output.bmp\n", FILTER_GAUSS5);
		printf("task8.exe %s imput.bmp output.bmp\n", FILTER_SOBELX);
		printf("task8.exe %s imput.bmp output.bmp\n", FILTER_SOBELY);
		printf("task8.exe %s imput.bmp output.bmp\n", FILTER_GRAY);
		return -1;
	}

	if (0 == read_image_file(argv[2], &file_header, &info_header, &img, &pal, &pal_size, &stride))
	{
		printf("The bmp file is invalid.\n");
		return -1;
	}

	if (strcmp(argv[1], FILTER_AVERAGE) == 0)
	{
		set_average_filter(img, info_header.biWidth, info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_GAUSS3) == 0)
	{
		set_gauss_filter_3(img, info_header.biWidth, info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_GAUSS5) == 0)
	{
		set_gauss_filter_5(img, info_header.biWidth, info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_SOBELX) == 0)
	{
		set_sobel_filter_x(img, info_header.biWidth, info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_SOBELY) == 0)
	{
		set_sobel_filter_y(img, info_header.biWidth, info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_GRAY) == 0)
	{
		set_grey_filter(img, info_header.biWidth, info_header.biHeight);

	}
	else
	{
		printf("The filter name is invalid.\n");
		return -1;
	}

	write_image_file(argv[3], &file_header, &info_header, img, pal, pal_size, stride);

	if (img) free(img);
	if (pal) free(pal);

	return 0;
}


