#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define FILTER_AVERAGE		"average"
#define FILTER_GAUSS3		"gauss3"
#define FILTER_GAUSS5		"gauss5"
#define FILTER_SOBELX		"sobelx"
#define FILTER_SOBELY		"sobely"
#define FILTER_GRAY			"gray"


#pragma pack(push, 1)

typedef struct 
{
	unsigned int biSize;
	unsigned int biWidth;
	unsigned int biHeight;
	unsigned short biPlanes;
	unsigned short biBitCount;
	unsigned int biCompression;
	unsigned int biSizeImage;
	unsigned int biXPelsPerMeter;
	unsigned int biYPelsPerMeter;
	unsigned int biClrUsed;
	unsigned int biClrImportant;
} BitmapInfoHeader;

typedef struct 
{
	unsigned short bfType;
	unsigned int bfSize;
	unsigned short bfReserved1;
	unsigned short bfReserved2;
	unsigned int bfOffBits;
} BitmapFileHeader;

typedef struct 
{
	unsigned char blue;
	unsigned char green;
	unsigned char red;
} RGB;

#pragma pack(pop)

typedef struct
{
	BitmapFileHeader file_header;
	BitmapInfoHeader info_header;
	char *palatee;
	size_t palatee_size;
	RGB *image_data;
} BitmapImage;


int read_image_file(char *input_file, BitmapImage *image)
{
	FILE *fp = fopen(input_file, "rb");
	if (!fp) 
	{
		printf("Can't open the input file.\n");
		return -1;
	}

	BitmapFileHeader *file_header = &image->file_header;
	fread(file_header, sizeof(BitmapFileHeader), 1, fp);
	if (file_header->bfType != 0x4D42)
	{
		fclose(fp);
		printf("Input file is not in bmp format.\n");
		return -1;
	}
	BitmapInfoHeader *info_header = &image->info_header;
	fread(info_header, sizeof(BitmapInfoHeader), 1, fp);

	/*if (info_header->biBitCount != 24 && info_header->biBitCount != 32)
	{
		fclose(fp);
		printf("Input file is neither 32-bit or 24-bit.\n");
		return 0;
	}*/

	fseek(fp, file_header->bfOffBits, SEEK_SET);

	RGB *image_data = (RGB *)malloc(info_header->biWidth * info_header->biHeight * sizeof(RGB));

	if (!image_data)
	{
		free(image);
		fclose(fp);
		printf("Couldnt allocate memory to store image");
		return -1;
	}

	//storing palatee
	size_t palateeSize = file_header->bfOffBits - sizeof(BitmapFileHeader) - sizeof(BitmapInfoHeader);
	image->palatee = NULL;
	image->palatee_size = 0;
	if (palateeSize > 0)
	{
		image->palatee = (char *)malloc(palateeSize);
		image->palatee_size = palateeSize;
		fread(image->palatee, 1, palateeSize, fp);
	}

	int k = 0;
	for (int i = 0; i < info_header->biHeight; i++)
	{
		for (int j = 0; j < info_header->biWidth; j++)
		{
			fread(image_data + k++, sizeof(RGB), 1, fp);
		}
	}
	image->image_data = image_data;
	fclose(fp);
	return 0;

}

int write_image_file(char *output_file, BitmapImage *image) 
{
	FILE *fp = fopen(output_file, "wb");
	if (!fp) 
	{
		printf("Can't open the output file");
		return -1;
	}
	fwrite(&image->file_header, sizeof(BitmapFileHeader), 1, fp);
	fwrite(&image->info_header, sizeof(BitmapInfoHeader), 1, fp);
	if (image->palatee)
		fwrite(&image->palatee, image->palatee_size, 1, fp);
	for (int i = 0; i < image->info_header.biWidth * image->info_header.biHeight; i++)
		fwrite(&image->image_data[i], sizeof(RGB), 1, fp);
	fclose(fp);
	return 0;
}

void apply_matrix_to_filter(RGB *img, int width, int height, int *matrix, int dim) 
{
	int i, j, k, l;
	int r, g, b;
	RGB *t;
	int gap, weight;

	t = (RGB *)malloc(width * height * sizeof(RGB));
	memcpy(t, img, width * height * sizeof(RGB));

	gap = (dim - 1) / 2;

	weight = 0;
	for (i = -gap; i <= gap; i++)
	{
		for (j = -gap; j <= gap; j++)
		{
			weight += matrix[(gap + i) * dim + gap + j];
		}
	}

	for (i = gap; i < height - gap; i++)
	{
		for (j = gap; j < width - gap; j++) 
		{
			r = 0;
			g = 0;
			b = 0;
			for (k = -gap; k <= gap; k++)
			{
				for (l = -gap; l <= gap; l++) 
				{
					r += t[(i + k) * width + j + l].red * matrix[(gap + k) * dim + gap + l];
					g += t[(i + k) * width + j + l].green * matrix[(gap + k) * dim + gap + l];
					b += t[(i + k) * width + j + l].blue * matrix[(gap + k) * dim + gap + l];
				}
			}

			img[i * width + j].red = r / weight;
			img[i * width + j].green = g / weight;
			img[i * width + j].blue = b / weight;
		}
	}

	free(t);
}

void set_average_filter(RGB *img, int width, int height) 
{
	int matrix[] = 
	{
			1, 1, 1,
			1, 1, 1,
			1, 1, 1
	};

	apply_matrix_to_filter(img, width, height, matrix, 3);
}

void set_gauss_filter_3(RGB *img, int width, int height)
{
	int gauss3[] =
	{
			1, 2, 1,
			2, 4, 2,
			1, 2, 1
	};

	apply_matrix_to_filter(img, width, height, gauss3, 3);
}

void set_gauss_filter_5(RGB *img, int width, int height)
{
	int gauss5[] =
	{
			1, 4, 6, 4, 1,
			4, 16, 24, 16, 4,
			6, 24, 36, 24, 6,
			4, 16, 24, 16, 4,
			1, 4, 6, 4, 1
	};

	apply_matrix_to_filter(img, width, height, gauss5, 5);
}

void apply_sobel_filter(RGB *img, int width, int height, int *matrix)
{
	int i, j, k, l;
	int r, g, b;
	RGB *t;

	t = (RGB *)malloc(width * height * sizeof(RGB));
	memcpy(t, img, width * height * sizeof(RGB));

	for (i = 1; i < height - 1; i++)
	{
		for (j = 1; j < width - 1; j++)
		{
			r = 0;
			g = 0;
			b = 0;
			for (k = -1; k <= 1; k++)
			{
				for (l = -1; l <= 1; l++)
				{
					r += t[(i + k) * width + j + l].red * matrix[(1 + k) * 3 + 1 + l];
					g += t[(i + k) * width + j + l].green * matrix[(1 + k) * 3 + 1 + l];
					b += t[(i + k) * width + j + l].blue * matrix[(1 + k) * 3 + 1 + l];
				}
			}

			if (r < 0) r = 0;
			if (r > 255) r = 255;
			if (g < 0) g = 0;
			if (g > 255) g = 255;
			if (b < 0) b = 0;
			if (b > 255) b = 255;

			img[i * width + j].red = r;
			img[i * width + j].green = g;
			img[i * width + j].blue = b;
		}
	}

	free(t);
}

void set_sobel_filter_x(RGB *img, int width, int height)
{
	int sobelx[] = 
	{
			1, 2, 1,
			0, 0, 0,
			-1, -2, -1
	};

	apply_sobel_filter(img, width, height, sobelx);
}

void set_sobel_filter_y(RGB *img, int width, int height)
{
	int sobely[] =
	{
			-1, 0, 1,
			-2, 0, 2,
			-1, 0, 1
	};

	apply_sobel_filter(img, width, height, sobely);
}

void set_grey_filter(RGB *img, int width, int height)
{
	int i, j;
	int val;

	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			val = (img[i * width + j].red + img[i * width + j].green + img[i * width + j].blue) / 3;
			img[i * width + j].red = val;
			img[i * width + j].green = val;
			img[i * width + j].blue = val;
		}
	}
}

int main(int argc, char *argv[])
{
	if (argc != 4) {
		printf("<exe_name> FILTER_NAME INPUT_BMP OUTPUT_BMP\n");
		printf("for example\n");
		printf("a.out %s imput.bmp output.bmp\n", FILTER_AVERAGE);
		printf("a.out %s imput.bmp output.bmp\n", FILTER_GAUSS3);
		printf("a.out %s imput.bmp output.bmp\n", FILTER_GAUSS5);
		printf("a.out %s imput.bmp output.bmp\n", FILTER_SOBELX);
		printf("a.out %s imput.bmp output.bmp\n", FILTER_SOBELY);
		printf("a.out %s imput.bmp output.bmp\n", FILTER_GRAY);
		return -1;
	}

	BitmapImage image;
	if (read_image_file(argv[2], &image) == -1)
	{
		printf("Invalid input file.");
		return -1;
	}

	if (strcmp(argv[1], FILTER_AVERAGE) == 0)
	{
		set_average_filter(image.image_data, image.info_header.biWidth, image.info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_GAUSS3) == 0) 
	{
		set_gauss_filter_3(image.image_data, image.info_header.biWidth, image.info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_GAUSS5) == 0)
	{
		set_gauss_filter_5(image.image_data, image.info_header.biWidth, image.info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_SOBELX) == 0)
	{
		set_sobel_filter_x(image.image_data, image.info_header.biWidth, image.info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_SOBELY) == 0)
	{
		set_sobel_filter_y(image.image_data, image.info_header.biWidth, image.info_header.biHeight);
	}
	else if (strcmp(argv[1], FILTER_GRAY) == 0)
	{
		set_grey_filter(image.image_data, image.info_header.biWidth, image.info_header.biHeight);

	}
	else 
	{
		printf("The filter name is invalid.\n");
		return -1;
	}
	if (write_image_file(argv[3], &image) == -1)
	{
		printf("Invalid output file.");
		free(image.image_data);
		return -1;
	}
	free(image.image_data);
	return 0;
}