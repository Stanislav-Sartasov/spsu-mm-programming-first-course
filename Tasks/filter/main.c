#include <stdio.h>
#include <math.h>
#include <string.h>
#include <stdlib.h>
#define min(x, y) x < y ? x : y
#define max(x, y) x > y ? x : y
#define step(x) x > 128 ? 255 : 0

#define pi 3.1415926535897932384626433832795

FILE* fileIn;
FILE* fileOut;

#pragma pack(push, 1)
struct bmh_file
{
	unsigned short	bf_type;
	unsigned int bf_size;
	unsigned short bf_reversed_one;
	unsigned short bf_reversed_two;
	unsigned int bf_off_bits;
};

struct bmh_info {
	unsigned int size;
	unsigned int  widht;
	unsigned int  height;
	unsigned short  planes;
	unsigned short  bit_count;
	unsigned int compression;
	unsigned int size_image;
	unsigned int  x_pels_per_meter;
	unsigned int  y_pels_per_meter;
	unsigned int color_used;
	unsigned int color_important;
};
#pragma pack(pop)

void correct_input(int argc, char* argv[])
{
    int fl = 0;
    if (!(argc == 4 && (strcmp(argv[2], "Averaging") == 0 || strcmp(argv[2], "Gauss3") == 0 || strcmp(argv[2], "Gauss5") == 0
            || strcmp(argv[2], "SobelX") == 0 || strcmp(argv[2], "SobelY") == 0 || strcmp(argv[2], "ToGrey") == 0) && fopen_s(&fileIn, argv[1], "rb") == 0))
        fl = 1;
    char extension[] = ".bmp";
    for (int i = 1; i < 5; i++)
        if (argv[3][strlen(argv[3]) - i] != extension[4 - i] || argv[1][strlen(argv[1]) - i] != extension[4 - i])
            fl = 1;
    if (fl)
    {
        printf("Invalid input. Try again.\n");
        exit(0);
    }
}

void filter(unsigned char* input_binary_image, int height, int width, char type[])
{
    if (strcmp(type, "ToGrey") == 0)
    {
        for (int i = 0; i < height * width; i++)
        {
            //unsigned char result = (299 * input_binary_image[i * 3] + 587 * input_binary_image[i * 3 + 1] + 114 * input_binary_image[i * 3 + 2]) / 1000;
            unsigned char result = (input_binary_image[i * 3] + input_binary_image[i * 3 + 1] + input_binary_image[i * 3 + 2]) / 3;
            input_binary_image[i * 3] = result;
            input_binary_image[i * 3 + 1] = result;
            input_binary_image[i * 3 + 2] = result;
        }
        return;
    }
    int size = 3;
    int fl = 1;
    double* directions = (double*)malloc(size * size * sizeof(double));
    if (strcmp(type, "Gauss3") == 0 || strcmp(type, "Gauss5") == 0) //gauss
    {
        double sigma = 0.6;
        if (type == "Gauss5") // gauss 5*5
        {
            size = 5;
            directions = (double*)realloc(directions, size * size * sizeof(double));
        }
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                directions[x * size + y] = 1 / sqrt(2 * pi * sigma) * exp(-(x * x + y * y) / (2 * sigma * sigma));
    }
    if (strcmp(type, "Averaging") == 0) // average
    {
        int c[9]  = {1, 1, 1, 1, 1, 1, 1, 1, 1};
        for(int i = 0; i < 9; i++)
            directions[i] = c[i];
    }
    if (strcmp(type, "SobelX") == 0) // sobel x
    {
        fl = 0;
        int a[9] = {-1, 0, 1, -2, 0, 2, -1, 0, 1};
        for(int i = 0; i < 9; i++)
            directions[i] = a[i];
    }
    if (strcmp(type, "SobelY") == 0) // sobel y
    {
        fl = 0;
        int b[9] = {-1, -2, -1, 0, 0, 0, 1, 2, 1};
        for(int i = 0; i < 9; i++)
            directions[i] = b[i];
    }
    unsigned char* image_copy = (unsigned char*)malloc(3 * height * width * sizeof(char));
    for (int i = 0; i < height; i++)
        for (int j = 0; j < width; j++)
        {
            double result[3] = {0, 0, 0};
            double divider = 0;
            for (int iter_dir_x = 0; iter_dir_x < size; iter_dir_x++)
                for (int iter_dir_y = 0; iter_dir_y < size; iter_dir_y++)
                    if ((i + iter_dir_x - 1) >= 0  && (i + iter_dir_x - 1) <= (height - 1) && (j + iter_dir_y - 1) >= 0 && (j + iter_dir_y - 1) <= (width - 1))
                    {
                        result[0] += input_binary_image[((i + iter_dir_x - 1) * width + j + iter_dir_y - 1) * 3 + 0] * directions[iter_dir_x * size + iter_dir_y];
                        result[1] += input_binary_image[((i + iter_dir_x - 1) * width + j + iter_dir_y - 1) * 3 + 1] * directions[iter_dir_x * size + iter_dir_y];
                        result[2] += input_binary_image[((i + iter_dir_x - 1) * width + j + iter_dir_y - 1) * 3 + 2] * directions[iter_dir_x * size + iter_dir_y];
                        divider += directions[iter_dir_x * size + iter_dir_y];
                    }
            if (fl)
            {
                image_copy[(i * width + j) * 3] = (unsigned char)(result[0] / divider);
                image_copy[(i * width + j) * 3 + 1] = (unsigned char)(result[1] / divider);
                image_copy[(i * width + j) * 3 + 2] = (unsigned char)(result[2] / divider);
            }
            else
            {
                int x = step(((result[0] + result[1] + result[2]) / 3));
                image_copy[(i * width + j) * 3] = x;
                image_copy[(i * width + j) * 3 + 1] = x;
                image_copy[(i * width + j) * 3 + 2] = x;
            }
        }
	for (int i = 0; i < height * width * 3; i++)
		input_binary_image[i] = image_copy[i];
	free(image_copy);
}

int main(int argc, char* argv[])
{
	correct_input(argc, argv);
	fopen_s(&fileIn, argv[1], "rb");
	fopen_s(&fileOut, argv[3], "wb");
	struct bmh_file file_header;
	struct bmh_info info_header;
	fread(&file_header, sizeof(file_header), 1, fileIn);
	fread(&info_header, sizeof(info_header), 1, fileIn);
	unsigned char* input_binary_image = (unsigned char*)malloc(info_header.size_image);
	fseek(fileIn, file_header.bf_off_bits, SEEK_SET);
	fread(input_binary_image, 1, info_header.size_image, fileIn);

	filter(input_binary_image, info_header.height, info_header.widht, argv[2]);

    fwrite(&file_header, sizeof(file_header), 1, fileOut);
	fwrite(&info_header, sizeof(info_header), 1, fileOut);
	for (int i = 0; i < info_header.size_image; i++)
		fwrite(&input_binary_image[i], 1, 1, fileOut);
	printf("Success");
	free(input_binary_image);
	fclose(fileIn);
	fclose(fileOut);
	return 0;
}
