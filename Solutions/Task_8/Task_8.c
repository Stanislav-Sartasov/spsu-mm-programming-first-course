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
	return *((unsigned int*)&c);
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
		*alpha = (char*)malloc(*width * *height);
	else
		*alpha = 0;
	*image = (rgb_24*)malloc(sizeof(rgb_24) * *width * *height);
	for (unsigned i = 0; i < *height; i++)
	{
		for (unsigned j = 0; j < *width; j++)
		{
			for (int c = 2; c >= 0; c--)
				*(*(*image + i * *width + j) + c) = fgetc(file_in);
			if (bit_count == 32)
				*alpha[i * *width + j] = fgetc(file_in);
		}
		for (unsigned j = 0; j < (4 - ((*width * (bit_count / 8)) % 4)) % 4; j++)
			fgetc(file_in);
	}
	fclose(file_in);
	return 0;
}

void median(rgb_24** image, unsigned int width, unsigned int height, int m_size)
{
	unsigned char* arr = (unsigned char*)malloc(sizeof(unsigned char) * m_size * m_size);
	if (!arr)
		printf("no");
	int size;
	int m = m_size / 2;
	rgb_24* new_image = (rgb_24*)malloc(sizeof(rgb_24) * height * width);
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
			for (int c = 0; c < 3; c++)
			{
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
			}
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
			for (int c = 0; c < 3; c++)
				* (*(*image + y * width + x) + c) = new_image[y * width + x][c];
	free(new_image);
	free(arr);
}

void gaussian(rgb_24** image, unsigned int width, unsigned int height, int size, double sigma)
{
	double sum = 0.0;
	double sum_kern = 0.0;
	double* kern = (double*)malloc(sizeof(double) * size * size);
	int m = size / 2;
	for (int i = -m; i <= m; i++)
		for (int j = -m; j <= m; j++)
			kern[(i + m) * size + j + m] = exp(-(pow(i, 2.0) + pow(j, 2.0)) / (2 * pow(sigma, 2.0))) / (2 * pi * pow(sigma, 2.0));
	rgb_24* new_image = (rgb_24*)malloc(sizeof(rgb_24) * height * width);
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
			for (int c = 0; c < 3; c++)
			{
				sum = 0.0;
				sum_kern = 0.0;
				for (int i = -m; i <= m; i++)
					for (int j = -m; j <= m; j++)
						if ((y + i) >= 0 && (y + i) < height && (x + j) >= 0 && (x + j) < width)
						{
							sum += (double)(*(*(*image + (y + i) * width + x + j) + c)) * kern[(i + m) * size + j + m];
							sum_kern += kern[(i + m) * size + j + m];
						}
				new_image[y * width + x][c] = (unsigned char)(sum / sum_kern);
			}
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
			for (int c = 0; c < 3; c++)
				*(*(*image + y * width + x) + c) = new_image[y * width + x][c];
	free(new_image);
	free(kern);
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
	int g_x = 0;
	int g_y = 0;
	double shade;
	double shade_coefficient[3] = { 0.299, 0.587, 0.114 };
	for (unsigned y = 0; y < height; y++)
		for (unsigned x = 0; x < width; x++)
		{
			shade = 0;
			for (int c = 0; c < 3; c++)
				if (y > 0 && y < height - 1 && x > 0 && x < width - 1)
					if (type == 'x')
					{
						g_x = 0;
						for (int i = -1; i <= 1; i += 2)
							for (int j = -1; j <= 1; j++)
								g_x += *(*(*image + (y + i) * width + x + j) + c) * i * (2 - j * j);
						shade += abs(g_x) * shade_coefficient[c];
					}
					else if (type == 'y')
					{
						g_y = 0;
						for (int j = -1; j <= 1; j += 2)
							for (int i = -1; i <= 1; i++)
								g_y += *(*(*image + (y + i) * width + x + j) + c) * j * (2 - i * i);
						shade += abs(g_y) * shade_coefficient[c];
					}
					else
					{
						g_x = 0;
						g_y = 0;
						for (int i = -1; i <= 1; i += 2)
							for (int j = -1; j <= 1; j++)
								g_x += *(*(*image + (y + i) * width + x + j) + c) * i * (2 - j * j);
						for (int j = -1; j <= 1; j += 2)
							for (int i = -1; i <= 1; i++)
								g_y += *(*(*image + (y + i) * width + x + j) + c) * j * (2 - i * i);
						shade += sqrt(g_x * g_x + g_y * g_y) * shade_coefficient[c];
					}
			if (shade > threshold)
				shade = 255;
			else
				shade = 0;
			for (int c = 0; c < 3; c++)
				new_image[y * width + x][c] = shade;
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

int main(int argc, char** argv)
{
	FILE* file_in;
	FILE* file_out;
	rgb_24* image;
	unsigned char* alpha;
	unsigned int bit_count;
	unsigned int width;
	unsigned int height;
	int sz = 3;
	double sg = 0.6;
	double th = 255 / 2.0;

	if (argc < 4)
	{
		if (argc == 2)
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
				return 0;
			}
		if (argc == 1)
		{
			char* c = 0;
			char* str = 0;
			char* modifier = 0;
			char* function_name = 0;
			int i;
			char quotes;
			int str_beg;
			char start_flag;
			printf("\n\tthis program allows you to use certain filters for bmp-24 and bmp-32 images\n");
			printf("\tinput format: <input file name> <filter with modificators> <output file name>\n");
			printf("\tenter <help> for details\n");
			printf("\tenter <exit> for finish\n\n");
			for (;;)
			{
				start_flag = 0;
				for (;;)
				{
					printf("MyInst > ");
					if (start_flag)
					{
						free(c);
						free(str);
						free(modifier);
						free(function_name);
					}
					start_flag = 1;
					i = 0;
					quotes = 0;
					c = (char*)malloc(sizeof(char) * 256);
					str = (char*)malloc(sizeof(char) * 256);
					modifier = (char*)malloc(sizeof(char) * 256);
					function_name = (char*)malloc(sizeof(char) * 256);
					do
					{
						c[i++] = getchar();
						if (i % 255 == 0)
							c = (char*)realloc(c, sizeof(char) * (255 * (i / 255) + 1));
					} while (c[i - 1] != '\n');

					i = 0;
					if (c[i] == '\n')
						continue;
					while (c[i] == ' ' || c[i] == '\t')
						i++;
					if (c[i] == '"')
					{
						quotes = 1;
						i++;
					}
					if (c[i] == '\n')
					{
						printf("invalid input\n");
						continue;
					}
					str_beg = i;
					while (((c[i] != ' ' && c[i] != '\t' && !quotes) || (c[i] != '"' && quotes)) && c[i] != '\n')
					{
						str[i - str_beg] = c[i];
						i++;
						if ((i - str_beg) % 255 == 0 && (i - str_beg) != 0)
							str = (char*)realloc(str, sizeof(char) * (255 * ((i - str_beg) / 255) + 1));
					}
					str[i - str_beg] = '\0';
					if (c[i] == '"')
						i++;
					if (compare(str, "exit"))
						return 0;
					if (compare(str, "help"))
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
						printf("\t\tsz = 3\n\t\tsg = 0.6\n\t\tth = 2.0\n");
						printf("\n\tenter <exit> for finish\n\n");
						continue;
					}
					if (c[i] == '\n')
					{
						printf("invalid input\n");
						continue;
					}
					if (fopen_s(&file_in, str, "rb") != 0)
					{
						printf("invalid inputing file name\n");
						continue;
					}
					free(str);
					str = (char*)malloc(sizeof(char) * 256);


					sz = 3;
					sg = 0.6;
					th = 255 / 2;

					while (c[i] == ' ' || c[i] == '\t')
						i++;
					if (c[i] == '\n')
					{
						printf("invalid input\n");
						fclose(file_in);
						continue;
					}
					str_beg = i;
					while (c[i] != ' ' && c[i] != '\t' && c[i] != '/' && c[i] != '\n')
					{
						function_name[i - str_beg] = c[i];
						if (c[i] >= 'A' && c[i] <= 'Z')
							function_name[i - str_beg] = c[i] - 'A' + 'a';
						else if (c[i] >= 'a' && c[i] <= 'z')
							function_name[i - str_beg] = c[i];
						else if (c[i] != '_')
							c[i + 1] = '\n';
						i++;
						if ((i - str_beg) % 255 == 0)
							function_name = (char*)realloc(function_name, sizeof(char) * (255 * ((i - str_beg) / 255) + 1));
					}
					function_name[i - str_beg] = '\0';

					if (!compare(function_name, "median")
						&& !compare(function_name, "gaussian")
						&& !compare(function_name, "sobel")
						&& !compare(function_name, "sobel_x")
						&& !compare(function_name, "sobel_y")
						&& !compare(function_name, "shade"))
					{
						printf("invalid filter input\n");
						fclose(file_in);
						continue;
					}

					while (c[i] == ' ' || c[i] == '\t')
						i++;

					while (c[i] == '/')
					{
						i++;
						str_beg = i;
						while (c[i] != ' ' && c[i] != '\t' && c[i] != '\n' && (c[i] < '0' || c[i] > '9') && c[i] != '=')
						{
							modifier[i - str_beg] = c[i];
							i++;
							if ((i - str_beg) % 255 == 0)
								modifier = (char*)realloc(modifier, sizeof(char) * (255 * ((i - str_beg) / 255) + 1));
						}
						modifier[i - str_beg] = '\0';
						while (c[i] == ' ' || c[i] == '\t')
							i++;
						if (c[i] == '=')
							i++;
						while (c[i] == ' ' || c[i] == '\t')
							i++;
						free(str);
						str = (char*)malloc(sizeof(char) * 256);
						str_beg = i;
						while ((c[i] >= '0' && c[i] <= '9') || c[i] == '.')
						{
							str[i - str_beg] = c[i];
							i++;
							if ((i - str_beg) % 255 == 0)
								str = (char*)realloc(str, sizeof(char) * (255 * ((i - str_beg) / 255) + 1));
						}
						str[i - str_beg] = '\0';
						if (c[i] != ' ' && c[i] != '\t' && c[i] != '/')
							c[i] = '\n';
						if (modifier[2] != '\0')
							c[i] = '\n';
						if (modifier[0] == 's')
						{
							if (modifier[1] == 'z' && atoi(str) != 0)
								sz = atoi(str) - 1 + atoi(str) % 2;
							else if (modifier[1] == 'g' && atof(str) != 0)
								sg = atof(str);
							else
								c[i] = '\n';
						}
						else if (modifier[0] == 't' && modifier[1] == 'h' && atof(str) != 0)
							th = 255 / atof(str);
						else
							c[i] = '\n';
						while (c[i] == ' ' || c[i] == '\t')
							i++;
					}
					if (c[i] == '\n')
					{
						printf("invalid modificator input\n");
						fclose(file_in);
						continue;
					}

					quotes = 0;
					if (c[i] == '"')
					{
						quotes = 1;
						i++;
					}
					str_beg = i;
					while (((c[i] != ' ' && c[i] != '\t' && !quotes) || (c[i] != '"' && quotes)) && c[i] != '\n')
					{
						str[i - str_beg] = c[i];
						i++;
						if ((i - str_beg) % 255 == 0 && (i - str_beg) != 0)
							str = (char*)realloc(str, sizeof(char) * (255 * ((i - str_beg) / 255) + 1));
					}
					str[i - str_beg] = '\0';
					if (c[i] == '\n' && quotes)
					{
						printf("invalid input\n");
						fclose(file_in);
						continue;
					}
					if (c[i] == '"')
						i++;
					while (c[i] == ' ' || c[i] == '\t')
						i++;
					if (c[i] != '\n')
					{
						printf("invalid input\n");
						fclose(file_in);
						continue;
					}
					if (fopen_s(&file_out, str, "wb") != 0)
					{
						printf("invalid outputing file name\n");
						fclose(file_in);
						continue;
					}
					break;
				}
				free(c);
				free(str);
				free(modifier);

				if (!read_and_write_bmp(file_in, file_out, &width, &height, &image, &alpha))
				{
					if (alpha)
						bit_count = 32;
					else
						bit_count = 24;

					if (compare(function_name, "median"))
						median(&image, width, height, sz);
					else if (compare(function_name, "gaussian"))
						gaussian(&image, width, height, sz, sg);
					else if (compare(function_name, "sobel"))
						sobel_xy(0, &image, width, height, th);
					else if (compare(function_name, "sobel_x"))
						sobel_xy('x', &image, width, height, th);
					else if (compare(function_name, "sobel_y"))
						sobel_xy('y', &image, width, height, th);
					else if (compare(function_name, "shade"))
						shade(&image, width, height);

					free(function_name);

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
					free(function_name);
					printf("input file reading error\n");
				}
			}
		}
		return -1;
	}
	if (fopen_s(&file_in, argv[1], "rb"))
	{
		printf("can't open file '%s'\n", argv[1]);
		return -1;
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
		return -1;
	}

	int i = 3;
	while (argv[i][0] == '/' && (i + 3) < argc)
	{
		if (argv[i + 1][0] != '=')
		{
			printf("modificator invalid input\n");
			fclose(file_in);
			return -1;
		}
		char mod[2];
		for (int j = 0; j < 2; j++)
		{
			mod[j] = argv[i][j + 1];
			if (mod[j] == '\0')
			{
				printf("modificator invalid input\n");
				fclose(file_in);
				return -1;
			}
		}
		if (argv[i][3] != '\0')
		{
			printf("modificator invalid input\n");
			fclose(file_in);
			return -1;
		}
		i += 2;
		for (int j = 0; argv[i][j] != '\0'; j++)
			if (!((argv[i][j] >= '0' && argv[i][j] <= '9') || argv[i][j] == '.'))
			{
				printf("modificator invalid input\n");
				fclose(file_in);
				return -1;
			}
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
				return -1;
			}
		}
		else if (mod[0] == 't' && mod[1] == 'h')
			th = 255 / atof(argv[i]);
		else
		{
			printf("modificator invalid input\n");
			fclose(file_in);
			return -1;
		}
		++i;
	}

	if (fopen_s(&file_out, argv[i], "wb"))
	{
		printf("invalid input\n");
		fclose(file_in);
		return -1;
	}

	if (i + 1 < argc)
	{
		printf("invalid input\n");
		fclose(file_in);
		fclose(file_out);
		return -1;
	}

	if (!read_and_write_bmp(file_in, file_out, &width, &height, &image, &alpha))
	{
		if (alpha)
			bit_count = 32;
		else
			bit_count = 24;

		if (compare(argv[2], "median"))
			median(&image, width, height, sz);
		else if (compare(argv[2], "gaussian"))
			gaussian(&image, width, height, sz, sg);
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