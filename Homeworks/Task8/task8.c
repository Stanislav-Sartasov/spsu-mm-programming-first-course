#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILTER_AVERAGE		"average"
#define FILTER_GAUSS3		"gauss3"
#define FILTER_GAUSS5		"gauss5"
#define FILTER_SOBELX		"sobelx"
#define FILTER_SOBELY		"sobely"
#define FILTER_GRAY			"gray"

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

RGB** get_bmp_image_data(char* input_file, BITMAPFILEHEADER* bmpHeader, BITMAPINFOHEADER* bmpInfo, int* vect, char** palette, unsigned int *paletteSize)
{
	FILE* ifp;
	RGB** pixel;

	if (NULL == (ifp = fopen(input_file, "rb")))
	{
		printf("Can't open the input file.\n");
		return 0;
	}

	fread(bmpHeader, sizeof(BITMAPFILEHEADER), 1, ifp);
	fread(bmpInfo, sizeof(BITMAPINFOHEADER), 1, ifp);
	if (bmpInfo->biBitCount != 24 && bmpInfo->biBitCount != 32)
	{
		fclose(ifp);
		return 0;
	}

	*paletteSize = bmpHeader->bfOffBits - sizeof(BITMAPFILEHEADER) - sizeof(BITMAPINFOHEADER);
	if (*paletteSize)
	{
		*palette = (char*)malloc((int)paletteSize);	
		fread(*palette, *paletteSize, 1, ifp);
	}
	pixel = (RGB**)calloc(sizeof(RGB*), bmpInfo->biHeight);
	if (pixel == 0)
	{
		printf("Can't read palette information.\n");
		fclose(ifp);
		return 0;
	}
	for (int i = 0; i < bmpInfo->biHeight; i++)
	{
		pixel[i] = (RGB*)calloc(sizeof(RGB), bmpInfo->biWidth);
	}

	*vect = (4 - (bmpInfo->biWidth * (bmpInfo->biBitCount / 8)) % 4) & 3;
	for (int i = 0; i < bmpInfo->biHeight; i++)
	{
		for (int j = 0; j < bmpInfo->biWidth; j++)
		{
			pixel[i][j].rgbBlue = (unsigned char)getc(ifp);
			pixel[i][j].rgbGreen = (unsigned char)getc(ifp);
			pixel[i][j].rgbRed = (unsigned char)getc(ifp);
			if (bmpInfo->biBitCount == 32)
			{
				getc(ifp);
			}
		}
		for (int k = 0; k < *vect; k++)
		{
			getc(ifp);
		}
	}

	fclose(ifp);
	return pixel;
}

RGB** copy_bmp_data(RGB** rgb, int height, int width)
{
	RGB** out_rgb;

	out_rgb = (RGB**)calloc(sizeof(RGB*), height);
	for (int i = 0; i < height; i++)
	{
		out_rgb[i] = (RGB*)calloc(sizeof(RGB), width);
		memcpy(out_rgb[i], rgb[i], sizeof(RGB) * width);
	}
	return out_rgb;
}

void put_bmp_file_data(char* output_file, BITMAPFILEHEADER* bmpHeader, BITMAPINFOHEADER* bmpInfo, RGB** out_rgb, int vect, char* palette, unsigned int paletteSize)
{
	int i, j, k;
	FILE* ofp;

	if ((ofp = (fopen(output_file, "wb"))) == NULL)
	{
		printf("Can't open the output file");
		return;
	}
	fwrite(bmpHeader, sizeof(BITMAPFILEHEADER), 1, ofp);
	fwrite(bmpInfo, sizeof(BITMAPINFOHEADER), 1, ofp);

	if (paletteSize)
	{
		fwrite(palette, paletteSize, 1, ofp);
	}

	for (i = 0; i < bmpInfo->biHeight; i++)
	{
		for (j = 0; j < bmpInfo->biWidth; j++)
		{
			fwrite(&out_rgb[i][j], sizeof(RGB), 1, ofp);
			if (bmpInfo->biBitCount == 32)
			{
				putc(0, ofp);
			}
		}

		for (k = 0; k < vect; k++)
		{
			fputc(0, ofp);
		}
	}

	fclose(ofp);
}

void average_filter(RGB** rgb, RGB** out_rgb, int height, int width)
{
	int i, j;

	for (i = 1; i < height - 1; i++)
	{
		for (j = 1; j < width - 1; j++)
		{
			unsigned int green = 0, blue = 0, red = 0;
			for (int k = -1; k < 2; k++)
			{
				for (int m = -1; m < 2; m++)
				{
					red += rgb[i + k][j + m].rgbRed;
					green += rgb[i + k][j + m].rgbGreen;
					blue += rgb[i + k][j + m].rgbBlue;
				}
			}

			out_rgb[i][j].rgbRed = (unsigned char)(red / 9);
			out_rgb[i][j].rgbGreen = (unsigned char)(green / 9);
			out_rgb[i][j].rgbBlue = (unsigned char)(blue / 9);
		}
	}
}

void create_gauss_33(RGB** rgb, RGB** out_rgb, int x, int y)
{
	const int matrix33[3][3] = { {1, 2, 1}, {2, 4, 2}, {1, 2, 1} };
	int pop = 16;
	int green = 0, blue = 0, red = 0;
	int i;

	for (i = 0; i < 3 * 3; i++)
	{
		RGB* pix = &rgb[y - 1 + i / 3][x - 1 + i % 3];

		blue += pix->rgbBlue * matrix33[i / 3][i % 3];
		green += pix->rgbGreen * matrix33[i / 3][i % 3];
		red += pix->rgbRed * matrix33[i / 3][i % 3];
	}
	out_rgb[y][x].rgbRed = (unsigned char)(red / pop);
	out_rgb[y][x].rgbBlue = (unsigned char)(blue / pop);
	out_rgb[y][x].rgbGreen = (unsigned char)(green / pop);
}

void gauss_filter_33(RGB** rgb, RGB** out_rgb, int height, int width)
{
	int i, j;

	for (i = 3 / 2; i < height - 3; i++)
	{
		for (j = 3 / 2; j < width - 3; j++)
		{
			create_gauss_33(rgb, out_rgb, j, i);
		}
	}
}

void create_gauss_55(RGB** rgb, RGB** out_rgb, int x, int y)
{
	const int matrix55[5][5] = { {1, 4,  6,  4,  1}, {4, 16, 24, 16, 4}, {6, 24, 36, 24, 6}, {4, 16, 24, 16, 4}, {1, 4,  6,  4,  1} };
	int pop = 256;
	int green = 0, blue = 0, red = 0;
	int i;

	for (i = 0; i < 5 * 5; i++)
	{
		RGB* pix = &rgb[y - 2 + i / 5][x - 2 + i % 5];

		blue += pix->rgbBlue * matrix55[i / 5][i % 5];
		green += pix->rgbGreen * matrix55[i / 5][i % 5];
		red += pix->rgbRed * matrix55[i / 5][i % 5];
	}

	out_rgb[y][x].rgbRed = (unsigned char)(red / pop);
	out_rgb[y][x].rgbBlue = (unsigned char)(blue / pop);
	out_rgb[y][x].rgbGreen = (unsigned char)(green / pop);
}

void gauss_filter_55(RGB** rgb, RGB** out_rgb, int height, int width)
{
	int i, j;

	for (i = 5 / 2; i < height - 5; i++)
	{
		for (j = 5 / 2; j < width - 5; j++)
		{
			create_gauss_55(rgb, out_rgb, j, i);
		}
	}
}

void create_sobel_x(RGB** rgb, RGB** out_rgb, int x, int y)
{
	const int matrix[3][3] = { {1,  2,  1}, {0,  0,  0}, {-1, -2, -1} };
	int green = 0, blue = 0, red = 0;
	int i;

	for (i = 0; i < 9; i++)
	{
		RGB* pix = &rgb[y - 1 + i / 3][x - 1 + i % 3];

		blue += pix->rgbBlue * matrix[i / 3][i % 3];
		green += pix->rgbGreen * matrix[i / 3][i % 3];
		red += pix->rgbRed * matrix[i / 3][i % 3];
	}

	if (red < 0)
		red = 0;
	if (red > 255)
		red = 255;

	if (blue < 0)
		blue = 0;
	if (blue > 255)
		blue = 255;

	if (green < 0)
		green = 0;
	if (green > 255)
		green = 255;

	out_rgb[y][x].rgbRed = (unsigned char)red;
	out_rgb[y][x].rgbBlue = (unsigned char)blue;
	out_rgb[y][x].rgbGreen = (unsigned char)green;
}

void sobel_x_filter(RGB** rgb, RGB** out_rgb, int height, int width)
{
	for (int i = 1; i < height - 3; i++)
	{
		for (int j = 1; j < width - 3; j++)
		{
			create_sobel_x(rgb, out_rgb, j, i);
		}
	}
}

void create_sobel_y(RGB** rgb, RGB** out_rgb, int x, int y)
{
	const int matrix[3][3] = { {-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1} };
	int green = 0, blue = 0, red = 0;

	for (int i = 0; i < 9; i++)
	{
		RGB* pix = &rgb[y - 1 + i / 3][x - 1 + i % 3];

		blue += pix->rgbBlue * matrix[i / 3][i % 3];
		green += pix->rgbGreen * matrix[i / 3][i % 3];
		red += pix->rgbRed * matrix[i / 3][i % 3];
	}

	if (red < 0)
		red = 0;
	if (red > 255)
		red = 255;

	if (blue < 0)
		blue = 0;
	if (blue > 255)
		blue = 255;

	if (green < 0)
		green = 0;
	if (green > 255)
		green = 255;

	out_rgb[y][x].rgbRed = (unsigned char)red;
	out_rgb[y][x].rgbBlue = (unsigned char)blue;
	out_rgb[y][x].rgbGreen = (unsigned char)green;
}


void sobel_y_filter(RGB** rgb, RGB** out_rgb, int height, int width)
{
	for (int i = 1; i < height - 3; i++)
	{
		for (int j = 1; j < width - 3; j++)
		{
			create_sobel_y(rgb, out_rgb, j, i);
		}
	}
}

void grey_filter(RGB** rgb, RGB** out_rgb, int height, int width)
{
	unsigned char color;

	for (int i = 0; i < height; i++)
	{
		for (int j = 0; j < width; j++)
		{
			color = (unsigned char)((rgb[i][j].rgbGreen + rgb[i][j].rgbBlue + rgb[i][j].rgbRed) / 3);

			out_rgb[i][j].rgbBlue = color;
			out_rgb[i][j].rgbGreen = color;
			out_rgb[i][j].rgbRed = color;
		}
	}
}


int main(int argc, char* argv[])
{
	int i;
	BITMAPFILEHEADER bmpHeader;
	BITMAPINFOHEADER bmpInfo;
	int vect;
	char* palette;
	unsigned int paletteSize;
	RGB** rgb;
	RGB** out_rgb;


	if (argc != 4)
	{
		printf("<exe_name> INPUT_BMP FILTER_NAME OUTPUT_BMP\n");
		printf("for example\n");
		printf("task8.exe imput.bmp %s output.bmp\n", FILTER_AVERAGE);
		printf("task8.exe imput.bmp %s output.bmp\n", FILTER_GAUSS3);
		printf("task8.exe imput.bmp %s output.bmp\n", FILTER_GAUSS5);
		printf("task8.exe imput.bmp %s output.bmp\n", FILTER_SOBELX);
		printf("task8.exe imput.bmp %s output.bmp\n", FILTER_SOBELY);
		printf("task8.exe imput.bmp %s output.bmp\n", FILTER_GRAY);
		return -1;
	}

	rgb = get_bmp_image_data(argv[1], &bmpHeader, &bmpInfo, &vect, &palette, &paletteSize);
	if (rgb == 0)
	{
		printf("The bmp file format is invalid.\n");
		return -1;
	}
	out_rgb = copy_bmp_data(rgb, bmpInfo.biHeight, bmpInfo.biWidth);

	if (strcmp(argv[2], FILTER_AVERAGE) == 0)
	{
		average_filter(rgb, out_rgb, bmpInfo.biHeight, bmpInfo.biWidth);
	}
	else if (strcmp(argv[2], FILTER_GAUSS3) == 0)
	{
		gauss_filter_33(rgb, out_rgb, bmpInfo.biHeight, bmpInfo.biWidth);
	}
	else if (strcmp(argv[2], FILTER_GAUSS5) == 0)
	{
		gauss_filter_55(rgb, out_rgb, bmpInfo.biHeight, bmpInfo.biWidth);
	}
	else if (strcmp(argv[2], FILTER_SOBELX) == 0)
	{
		sobel_x_filter(rgb, out_rgb, bmpInfo.biHeight, bmpInfo.biWidth);
	}
	else if (strcmp(argv[2], FILTER_SOBELY) == 0)
	{
		sobel_y_filter(rgb, out_rgb, bmpInfo.biHeight, bmpInfo.biWidth);
	}
	else if (strcmp(argv[2], FILTER_GRAY) == 0)
	{
		grey_filter(rgb, out_rgb, bmpInfo.biHeight, bmpInfo.biWidth);

	}
	else
	{
		printf("The filter name is invalid.\n");
		return -1;
	}

	put_bmp_file_data(argv[3], &bmpHeader, &bmpInfo, out_rgb, vect, palette, paletteSize);
	for (i = 0; i < bmpInfo.biHeight ; i++)
	{
		free(rgb[i]);
	}
	free(rgb);

	for (i = 0; i < bmpInfo.biHeight ; i++)
	{
		free(out_rgb[i]);
	}
	free(out_rgb);

	return 0;
}


