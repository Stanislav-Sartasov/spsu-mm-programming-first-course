#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <math.h>

double sigma = 0.8;
double pi = 3.14159265358979323846264338;

FILE* file_in;
FILE* file_out;

double gauss_func(double a, double b)
{
	double ans = (1 / sqrt(2 * pi * sigma) * exp(-(a * a + b * b) / (2 * sigma * sigma)));
	return ans;
}

void correct_input(int argc, char* argv[])
{
	if ((argc != 4) || ((strcmp(argv[2], "Averaging") != 0) && (strcmp(argv[2], "Gauss3") != 0)
		&& (strcmp(argv[2], "Gauss5") != 0) && (strcmp(argv[2], "SobelX") != 0) &&
		(strcmp(argv[2], "SobelY") != 0) && (strcmp(argv[2], "Grayscale") != 0))
		|| (fopen_s(&file_in, argv[1], "rb") != 0))
	{
		printf("Incorrect input.");
		exit(0);
	}
}



#pragma pack(push, 1)
struct bmh_file
{
	unsigned short	bf_type;
	unsigned int bf_size;
	unsigned short bf_reversed_one;
	unsigned short bf_reversed_two;
	unsigned int bf_off_bits;
} file_header;

struct bmh_info 
{
	unsigned int size;
	unsigned int widht;
	unsigned int height;
	unsigned short planes;
	unsigned short bit_count;
	unsigned int compression;
	unsigned int size_image;
	unsigned int x_pels_per_meter;
	unsigned int y_pels_per_meter;
	unsigned int color_used;
	unsigned int color_important;
} info_header;
#pragma pack(pop)


void filters(unsigned char* image, int height, int width, char* type)
{
	if (strcmp(type, "Grayscale") == 0)
	{
		for (int i = 0; i < height * width; i++)
		{
			unsigned char res = (image[i * 3] * 299 + image[i * 3 + 1] * 587 + image[i * 3 + 2] * 114) / 1000;
			image[i * 3] = res;
			image[i * 3 + 1] = res;
			image[i * 3 + 2] = res;
		}
		return;
	}

	int size;
	if (strcmp(type, "Gauss5") == 0)
	{
		size = 2;
	}
	else size = 1;

	double** kernel = (double**)(malloc(sizeof(double*) * (2 * size + 1)));
	for (int i = 0; i < 2 * size + 1; i++)
	{
		kernel[i] = (double*)(malloc(sizeof(double) * (2 * size + 1)));
	}
	if (strcmp(type, "Averaging") == 0)
	{
		for (int i = 0; i < 2 * size + 1; i++)
		{
			for (int j = 0; j < 2 * size + 1; j++)
			{
				kernel[i][j] = 1;
			}
		}
	}
	else if ((strcmp(type, "Gauss3") == 0) || (strcmp(type, "Gauss5") == 0))
	{
		for (int i = -size; i <= size; i++)
		{
			for (int j = -size; j <= size; j++)
			{
				kernel[i + size][j + size] = gauss_func(i, j);
			}
		}
	}
	else if (strcmp(type, "SobelX") == 0)
	{
		double mask[9] = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
		for (int i = 0; i < 9; i++)
		{
			kernel[i / 3][i % 3] = mask[i];
		}
	}
	else if (strcmp(type, "SobelY") == 0)
	{
		double mask[9] = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
		for (int i = 0; i < 9; i++)
		{
			kernel[i / 3][i % 3] = mask[i];
		}
	}
	
	unsigned char* copy_bitmap_image = (unsigned char*)malloc(sizeof(char) * 3 * height * width);
	for (int x = 0; x < height; x++)
	{
		for (int y = 0; y < width; y++)
		{
			double res[3] = { 0, 0, 0 };
			double sum = 0.0;
			for (int i = 0; i < 2 * size + 1; i++)
			{
				for (int j = 0; j < 2 * size + 1; j++)
				{
					if ((x + i - 1) >= 0 && (x + i - 1) < height && (y + j - 1) >= 0 && (y + j - 1) < width)
					{
						res[0] += image[((x + i - 1) * width + y + j - 1) * 3] * kernel[i][j];
						res[1] += image[((x + i - 1) * width + y + j - 1) * 3 + 1] * kernel[i][j];
						res[2] += image[((x + i - 1) * width + y + j - 1) * 3 + 2] * kernel[i][j];
						sum += kernel[i][j];
					}
				}
			}
			if (strcmp(type, "SobelX") == 0 || strcmp(type, "SobelY") == 0)
			{
				unsigned char result = (abs(res[0]) + abs(res[1]) + abs(res[2])) / 3;
				for (int i = 0; i < 3; i++)
				{
					copy_bitmap_image[(x * width + y) * 3 + i] = result;
				}
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					copy_bitmap_image[(x * width + y) * 3 + i] = abs(res[i]) / sum;
				}
			}
		}
	}
	
	for (int i = 0; i < height * width * 3; i++)
	{
		image[i] = copy_bitmap_image[i];
	}
	free(kernel);
	free(copy_bitmap_image);
}

int main(int argc, char* argv[])
{
	correct_input(argc, argv);
	fopen_s(&file_in, argv[1], "rb");
	fopen_s(&file_out, argv[3], "wb");

	struct bmh_file file_header;
	struct bmh_info info_header;
	fread(&file_header, sizeof(file_header), 1, file_in);
	fread(&info_header, sizeof(info_header), 1, file_in);

	unsigned char* bitmap_image = (unsigned char*)(malloc(info_header.size_image));
	fseek(file_in, file_header.bf_off_bits, SEEK_SET);
	fread(bitmap_image, 1, file_header.bf_size, file_in);
	
	filters(bitmap_image, info_header.height, info_header.widht, argv[2]);
	
	fwrite(&file_header, sizeof(file_header), 1, file_out);
	fwrite(&info_header, sizeof(info_header), 1, file_out);
	for (int i = 0; i < info_header.size_image; i++)
	{
		fwrite(&bitmap_image[i], 1, 1, file_out);

	}

	free(bitmap_image);
	fclose(file_in);
	fclose(file_out);
	return 0;
}