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

void apply_matrix_to_filter(RGB* img, int width, int height, int* matrix, int dim)
{
	int i, j, k, l;
	int r, g, b;
	RGB* t;
	int gap, weight;

	t = (RGB*)malloc(width * height * sizeof(RGB));
	memcpy(t, img, width * height * sizeof(RGB));

	gap = (dim - 1) / 2;

	weight = 0;
	for (i = -gap ; i <= gap ; i++)
	{
		for (j = -gap; j <= gap ; j++)
		{
			weight += matrix[(gap+i)*dim+gap+j];
		}
	}

	for (i = gap ; i < height - gap ; i++)
	{
		for(j = gap ; j < width - gap ; j++)
		{
			r = 0; g = 0; b = 0;
			for (k = -gap ; k <= gap ; k++)
			{
				for (l = -gap ; l <= gap ; l++)
				{
					r += t[(i+k)*width+j+l].rgbRed   * matrix[(gap+k)*dim+gap+l];
					g += t[(i+k)*width+j+l].rgbGreen * matrix[(gap+k)*dim+gap+l];
					b += t[(i+k)*width+j+l].rgbBlue  * matrix[(gap+k)*dim+gap+l];
				}
			}

			img[i*width+j].rgbRed = r / weight;
			img[i*width+j].rgbGreen = g / weight;
			img[i*width+j].rgbBlue = b / weight;
		}
	}

	free(t);
}

void set_average_filter(RGB* img, int width, int height)
{
	int matrix[] = {
		1, 1, 1, 
		1, 1, 1, 
	    1, 1, 1
	};

	apply_matrix_to_filter(img, width, height, matrix, 3);
}

void set_gauss_filter_3(RGB* img, int width, int height)
{
	int gauss3[] = {
		1, 2, 1,
		2, 4, 2,
		1, 2, 1
	};

	apply_matrix_to_filter(img, width, height, gauss3, 3);
}

void set_gauss_filter_5(RGB* img, int width, int height)
{
	int gauss5[] = {
		1, 4,  6,  4,  1,
		4, 16, 24, 16, 4,
		6, 24, 36, 24, 6,
		4, 16, 24, 16, 4,
		1, 4,  6,  4,  1
	};

	apply_matrix_to_filter(img, width, height, gauss5, 5);
}

void apply_sobel_filter(RGB* img, int width, int height, int* matrix)
{
	int i, j, k, l;
	int r, g, b;
	RGB* t;

	t = (RGB*)malloc(width * height * sizeof(RGB));
	memcpy(t, img, width * height * sizeof(RGB));

	for (i = 1 ; i < height - 1 ; i++)
	{
		for(j = 1 ; j < width - 1 ; j++)
		{
			r = 0; g = 0; b = 0;
			for (k = -1 ; k <= 1 ; k++)
			{
				for (l = -1 ; l <= 1 ; l++)
				{
					r += t[(i+k)*width+j+l].rgbRed   * matrix[(1+k)*3+1+l];
					g += t[(i+k)*width+j+l].rgbGreen * matrix[(1+k)*3+1+l];
					b += t[(i+k)*width+j+l].rgbBlue  * matrix[(1+k)*3+1+l];
				}
			}

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

void set_sobel_filter_x(RGB* img, int width, int height)
{
	int sobelx[] = {
		1,  2,  1,
		0,  0,  0,
		-1, -2, -1
	};

	apply_sobel_filter(img, width, height, sobelx);
}

void set_sobel_filter_y(RGB* img, int width, int height)
{
	int sobely[] = {
		-1, 0, 1,
		-2, 0, 2,
		-1, 0, 1
	};

	apply_sobel_filter(img, width, height, sobely);
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


