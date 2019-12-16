#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#define pi 3.1415926535897932384626433832795

typedef unsigned char rgb_24[3];

void fprint_bit_unsigned_int(FILE* file, unsigned int count)
{
	for (int i = 0; i <= 24; i += 8)
	{
		fprintf(file, "%c", (count & (255 << i)) >> i);
		fflush(file);
	}
}

void fprint_bit_unsigned_short(FILE* file, unsigned short count)
{
	fprintf(file, "%c", count & 255);
	fflush(file);
	fprintf(file, "%c", (count & (255 << 8)) >> 8);
	fflush(file);
}

void skip_and_write(FILE* file_in, FILE* file_out, unsigned int count)
{
	for (unsigned i = 0; i < count; i++)
		fprintf(file_out, "%c", fgetc(file_in));
}

unsigned int fget_int(FILE* file)
{
	char c[4];
	for (int i = 0; i < 4; i++)
		c[i] = fgetc(file);
	return *((unsigned int*)& c);
}

int read_and_write_bmp(FILE* file_in, FILE* file_out, unsigned int* width, unsigned int* height, rgb_24** image, unsigned char** alpha)
{
	if (fgetc(file_in) != 'B')
	{
		fclose(file_in);
		fclose(file_out);
		return 1;
	}
	if (fgetc(file_in) != 'M')
	{
		fclose(file_in);
		fclose(file_out);
		return 1;
	}
	fprintf(file_out, "BM");
	skip_and_write(file_in, file_out, 8);
	unsigned int image_begin = fget_int(file_in);
	fprint_bit_unsigned_int(file_out, image_begin);
	skip_and_write(file_in, file_out, 4);
	*width = fget_int(file_in);
	fprint_bit_unsigned_int(file_out, *width);
	*height = fget_int(file_in);
	fprint_bit_unsigned_int(file_out, *height);
	skip_and_write(file_in, file_out, 2);
	unsigned short bit_count = fgetc(file_in);
	bit_count |= (unsigned short)fgetc(file_in) << 8;
	fprint_bit_unsigned_short(file_out, bit_count);
	skip_and_write(file_in, file_out, image_begin - 30);
	if (bit_count == 32)
		* alpha = (char*)malloc(*width * *height);
	else
		*alpha = 0;
	*image = (rgb_24*)malloc(sizeof(rgb_24) * *width * *height);
	for (unsigned i = 0; i < *height; i++)
	{
		for (unsigned j = 0; j < *width; j++)
		{
			for (int c = 2; c >= 0; c--)
				* (*(*image + i * *width + j) + c) = fgetc(file_in);
			if (bit_count == 32)
				* alpha[i * *width + j] = fgetc(file_in);
		}
		for (unsigned j = 0; j < (4 - ((*width * (bit_count / 8)) % 4)) % 4; j++)
			fgetc(file_in);
	}
	fclose(file_in);
	return 0;
}

double use_mask(rgb_24** image, unsigned int width, unsigned int height, unsigned x, unsigned y, int c, double* mask, unsigned sz, double max_sum)
{
	int m = sz / 2;
	double sum = 0;
	for (int i = -m; i <= m; i++)
		for (int j = -m; j <= m; j++)
			if ((y + i) >= 0 && (y + i) < height && (x + j) >= 0 && (x + j) < width)
				sum += (double)(*(*(*image + (y + i) * width + x + j) + c)) * mask[(i + m) * sz + j + m];
			else
				max_sum -= mask[(i + m) * sz + j + m];
	return sum / max_sum;
}

void megas(char type, rgb_24** image, unsigned int width, unsigned int height, int sz, double sg)
{
	unsigned char* arr = 0;
	double* kern = 0;
	switch (type)
	{
	case 'm':
		arr = (unsigned char*)malloc(sizeof(unsigned char) * sz * sz);
		break;
	case 'g':
		kern = (double*)malloc(sizeof(double) * sz * sz);
		break;
	default:
		return;
	}
	int size;
	int m = sz / 2;
	double sum_kern_max = 0.0;
	rgb_24* new_image = (rgb_24*)malloc(sizeof(rgb_24) * height * width);
	if (type == 'g')
		for (int i = -m; i <= m; i++)
			for (int j = -m; j <= m; j++)
			{
				kern[(i + m) * sz + j + m] = exp(-(pow(i, 2.0) + pow(j, 2.0)) / (2 * pow(sg, 2.0))) / (2 * pi * pow(sg, 2.0));
				sum_kern_max += kern[(i + m) * sz + j + m];
			}
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
			for (int c = 0; c < 3; c++)
			{
				switch (type)
				{
				case 'm':
					size = 0;
					for (unsigned i = y - m; i <= y + m; i++)
						for (unsigned j = x - m; j <= x + m; j++)
							if ((i >= 0) && (i < height) && (j >= 0) && (j < width))
							{
								size++;
								arr[size - 1] = *(*(*image + i * width + j) + c);
							}
					for (unsigned i = 0; i < size / 2 + 1; i++)
					{
						unsigned char min = arr[i];
						short ind = i;
						for (int j = 1; j < size; j++)
							if (min > arr[j])
							{
								min = arr[j];
								ind = j;
							}
						arr[ind] = arr[i];
						arr[i] = min;
					}
					new_image[y * width + x][c] = arr[size / 2];
					break;
				case 'g':
					new_image[y * width + x][c] = (unsigned char)use_mask(image, width, height, x, y, c, kern, sz, sum_kern_max);
					break;
				}
			}
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
			for (int c = 0; c < 3; c++)
				* (*(*image + y * width + x) + c) = new_image[y * width + x][c];
	free(new_image);
	switch (type)
	{
	case 'm':
		free(arr);
		break;
	case 'g':
		free(kern);
	}
}

void shade(rgb_24** image, unsigned int width, unsigned int height)
{
	double shade;
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
		{
			shade = 0.299 * (*(*(*image + y * width + x))) + 0.587 * (*(*(*image + y * width + x) + 1)) + 0.114 * (*(*(*image + y * width + x) + 2));
			for (int c = 0; c < 3; c++)
				*(*(*image + y * width + x) + c) = (unsigned char)shade;
		}
}

void sobel_xy(char type, rgb_24** image, unsigned int width, unsigned int height, double threshold)
{
	rgb_24* new_image = (rgb_24*)malloc(sizeof(rgb_24) * height * width);
	double g_x;
	double g_y;
	double g_x_mask[] = { 1, 2, 1, 0, 0, 0, -1, -2, -1 };
	double g_y_mask[] = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
	double shade;
	double shade_coefficient[3] = { 0.299, 0.587, 0.114 };
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
		{
			shade = 0;
			for (int c = 0; c < 3; c++)
				if (y > 0 && y < height - 1 && x > 0 && x < width - 1)
				{
					if (type != 'y')
						g_x = fabs(use_mask(image, width, height, x, y, c, g_x_mask, 3, 1));
					if (type != 'x')
						g_y = fabs(use_mask(image, width, height, x, y, c, g_y_mask, 3, 1));
					if (type == 'x')
						shade += g_x * shade_coefficient[c];
					else if (type == 'y')
						shade += g_y * shade_coefficient[c];
					else
						shade += sqrt(g_x * g_x + g_y * g_y) * shade_coefficient[c];
				}
			if (shade > threshold)
				shade = 255;
			else
				shade = 0;
			for (int c = 0; c < 3; c++)
				new_image[y * width + x][c] = (unsigned char)shade;
		}
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
			for (int c = 0; c < 3; c++)
				* (*(*image + y * width + x) + c) = new_image[y * width + x][c];
	free(new_image);
}

char compare(char* str1, char* str2)
{
	int i = 0;
	while (str1[i] != '\0' && str2[i] != '\0')
	{
		if (str1[i] != str2[i])
			return 0;
		++i;
	}
	if (str1[i] != str2[i])
		return 0;
	return 1;
}

char** strgs(char** str_in, int* len)
{
	char* str = *str_in;
	char** arr = (char**)malloc(sizeof(char*));
	int ln = 0;
	for (int i = 0; str[i] != '\0' && str[i] != '\n';)
	{
		if (str[i] != ' ' && str[i] != '\t')
		{
			if (str[i] == '"')
			{
				++i;
				ln++;
				arr = (char**)realloc(arr, sizeof(char*) * ln);
				arr[ln - 1] = str + i;
				for (i; str[i] != '"' && str[i] != '\0' && str[i] != '\n'; i++);
				if (str[i] == '\0' || str[i] == '\n')
				{
					*len = 0;
					free(arr);
					return 0;
				}
				str[i] = '\0';
				++i;
			}
			else
			{
				ln++;
				arr = (char**)realloc(arr, sizeof(char*) * ln);
				arr[ln - 1] = str + i;
				for (; str[i] != '\0' && str[i] != '\n' && str[i] != ' ' && str[i] != '\t'; i++);
				if (str[i] != '\0')
				{
					str[i] = '\0';
					++i;
				}
			}
		}
		else
			++i;
	}
	*len = ln;
	return arr;
}

int main(int argc, char** argv)
{
	FILE* file_in;
	FILE* file_out;
	rgb_24* image;
	unsigned char* alpha;
	unsigned int bit_count;
	unsigned int width;
	unsigned int height;
	int sz;
	double sg;
	double th;
	char is_auto = 0;

	for (;;)
	{

		sz = 3;
		sg = 0.6;
		th = 255 / 2.0;

		char* str = 0;
		if (is_auto)
		{
			free(argv);
			free(str);
		}
		str = (char*)malloc(sizeof(char) * 3);
		str[0] = 'a';
		str[1] = ' ';

		if (argc < 4 || is_auto)
		{
			if (argc == 1 || is_auto)
			{
				if (!is_auto)
				{
					printf("\n\tthis program allows you to use certain filters for bmp-24 and bmp-32 images\n");
					printf("\tinput format: <input file name> <filter with modificators> <output file name>\n");
					printf("\tenter <help> for details\n");
					printf("\tenter <exit> for finish\n\n");
					is_auto = 1;
				}
				printf("My_inst > ");
				for (int i = 2;; i++)
				{
					str[i] = getchar();
					if (str[i] == '\n')
					{
						str[i] = '\0';
						break;
					}
					str = (char*)realloc(str, sizeof(char) * (i + 2));
				}
				argv = strgs(&str, &argc);
			}
			if (argc == 2)
			{
				if (compare(argv[1], "help"))
				{
					printf("\n\tfilters supported:\n\n");
					printf("\t<median>\n");
					printf("\t\t/sz - matrix size\n\n");
					printf("\t<gaussian>\n");
					printf("\t\t/sz - matrix size\n");
					printf("\t\t/sg - sigma\n\n");
					printf("\t<sobel> <sobel_x> <sobel_y>\n");
					printf("\t\t/th - threshold, pixels in shades of gray from (255 / th) is white, the rest are black\n");
					printf("\n\t<shade>\n");
					printf("\n\tyou can use modificators like <gaussian /sz = 5>\n");
					printf("\n\twithout modifiers standard values will be taken:\n");
					printf("\t\tsz = 3\n\t\tsg = 0.6\n\t\tth = 2.0\n\n");
					if (!is_auto)
						return 0;
					else
						continue;
				}
				else if (compare(argv[1], "exit"))
					return 0;
				else
				{
					if (is_auto)
					{
						printf("invalid input\n");
						continue;
					}
					else
						return -1;
				}
			}
			else if (argc == 1 && is_auto)
				continue;
			else if (argc < 4)
			{
				if (is_auto)
				{
					printf("invalid input\n");
					continue;
				}
				else
					return -1;
			}
		}
		if (fopen_s(&file_in, argv[1], "rb"))
		{
			printf("can't open file '%s'\n", argv[1]);
			if (!is_auto)
				return -1;
			else
				continue;
		}

		if (!compare(argv[2], "median")
			&& !compare(argv[2], "gaussian")
			&& !compare(argv[2], "sobel")
			&& !compare(argv[2], "sobel_x")
			&& !compare(argv[2], "sobel_y")
			&& !compare(argv[2], "shade"))
		{
			printf("invalid filter input\n");
			fclose(file_in);
			if (!is_auto)
				return -1;
			else
				continue;
		}

		int i = 3;
		char err = 0;
		while (argv[i][0] == '/' && (i + 3) < argc)
		{
			if (argv[i + 1][0] != '=')
			{
				printf("modificator invalid input\n");
				fclose(file_in);
				if (!is_auto)
					return -1;
				else
				{
					err = 1;
					break;
				}
			}
			char mod[2];
			for (int j = 0; j < 2; j++)
			{
				mod[j] = argv[i][j + 1];
				if (mod[j] == '\0')
				{
					printf("modificator invalid input\n");
					fclose(file_in);
					if (!is_auto)
						return -1;
					else
					{
						err = 1;
						break;
					}
				}
			}
			if (err)
				break;
			if (argv[i][3] != '\0')
			{
				printf("modificator invalid input\n");
				fclose(file_in);
				if (!is_auto)
					return -1;
				else
				{
					err = 1;
					break;
				}
			}
			i += 2;
			for (int j = 0; argv[i][j] != '\0'; j++)
				if (!((argv[i][j] >= '0' && argv[i][j] <= '9') || argv[i][j] == '.'))
				{
					printf("modificator invalid input\n");
					fclose(file_in);
					if (!is_auto)
						return -1;
					else
					{
						err = 1;
						break;
					}
				}
			if (err)
				break;
			if (mod[0] == 's')
			{
				if (mod[1] == 'z')
					sz = atoi(argv[i]) - 1 + atoi(argv[i]) % 2;
				else if (mod[1] == 'g')
					sg = atof(argv[i]);
				else
				{
					printf("modificator invalid input\n");
					fclose(file_in);
					if (!is_auto)
						return -1;
					else
					{
						err = 1;
						break;
					}
				}
			}
			else if (mod[0] == 't' && mod[1] == 'h')
				th = 255 / atof(argv[i]);
			else
			{
				printf("modificator invalid input\n");
				fclose(file_in);
				if (!is_auto)
					return -1;
				else
				{
					err = 1;
					break;
				}
			}
			++i;
		}
		if (err)
			continue;

		if (fopen_s(&file_out, argv[i], "wb"))
		{
			printf("invalid input\n");
			fclose(file_in);
			if (!is_auto)
				return -1;
			else
				continue;
		}

		if (i + 1 < argc)
		{
			printf("invalid input\n");
			fclose(file_in);
			fclose(file_out);
			if (!is_auto)
				return -1;
			else
				continue;
		}

		if (!read_and_write_bmp(file_in, file_out, &width, &height, &image, &alpha))
		{
			if (alpha)
				bit_count = 32;
			else
				bit_count = 24;

			if (compare(argv[2], "median"))
				megas('m', &image, width, height, sz, sg);
			else if (compare(argv[2], "gaussian"))
				megas('g', &image, width, height, sz, sg);
			else if (compare(argv[2], "sobel"))
				sobel_xy(0, &image, width, height, th);
			else if (compare(argv[2], "sobel_x"))
				sobel_xy('x', &image, width, height, th);
			else if (compare(argv[2], "sobel_y"))
				sobel_xy('y', &image, width, height, th);
			else if (compare(argv[2], "shade"))
				shade(&image, width, height);

			for (unsigned i = 0; i < height; i++)
			{
				for (unsigned j = 0; j < width; j++)
				{
					for (int c = 2; c >= 0; c--)
						fprintf(file_out, "%c", image[i * width + j][c]);
					if (alpha != 0)
						fprintf(file_out, "%c", 0);
				}
				for (unsigned j = 0; j < (4 - ((width * (bit_count / 8)) % 4)) % 4; j++)
					fprintf(file_out, "%c", 0);
			}
			free(image);
			if (!alpha)
				free(alpha);
			fclose(file_out);
		}
		else
		{
			printf("'%s' reading error\n", argv[0]);
		}
	}
}
