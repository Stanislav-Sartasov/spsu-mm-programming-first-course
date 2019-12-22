// ./bin/Debug/task8 input.bmp SobelX output.bmp
#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>

const double pi = 3.1415926535897932384626433832795;
const double sigma = 0.6;

#pragma pack(push, 1)
struct bmp_file
{
    unsigned short bf_type;
    unsigned int bf_size;
    unsigned short bf_reversed_one;
    unsigned short bf_reversed_two;
    unsigned int bf_off_bits;
};

struct bmp_info
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
};
#pragma pack(pop)

void add_filter(unsigned char* input, int size, int fl, int height, int width, double* mask)
{
    unsigned char* output = (unsigned char*)malloc(3 * height * width * sizeof(char));
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            double result[3] = {0, 0, 0};
            double d = 0;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if ((i + y - 1) >= 0  && (i + y - 1) < height && (j + x - 1) >= 0 && (j + x - 1) < width)
                    {
                        d += mask[y * size + x];
                        for (int k = 0; k < 3; k++)
                            result[k] += input[((i + y - 1) * width + j + x - 1) * 3 + k] * mask[y * size + x];
                    }
                }
            }
            if (fl)
            {
                for (int k = 0; k < 3; k++)
                    output[(i * width + j) * 3 + k] = (unsigned char)(result[k] / d);
            }
            else
            {
                int x = 0;
                if ((result[0] + result[1] + result[2]) > 384)
                    x = 255;
                for (int k = 0; k < 3; k++)
                    output[(i * width + j) * 3 + k] = x;
            }
        }
    }
    for (int i = 0; i < height * width * 3; i++)
        input[i] = output[i];
    free(output);
}

void averaging(unsigned char* input, int height, int width, int size)
{
    double *mask = (double *) malloc(size * size * sizeof(double));
    for (int i = 0; i < size * size ; i++)
        mask[i] = 1;
    add_filter(input, size, 1, height, width, mask);
}

void gauss(unsigned char* input, int height, int width, int size)
{
    double *mask = (double *) malloc(size * size * sizeof(double));
    for (int x = 0; x < size; x++)
        for (int y = 0; y < size; y++)
            mask[x * size + y] = 1.0 / sqrt(2 * pi * sigma) * exp(-(x * x + y * y) / (2 * sigma * sigma));
    add_filter(input, size, 1, height, width, mask);
}

void sobelX(unsigned char* input, int height, int width, int size)
{
    int a[9] = {-1, 0, 1, -2, 0, 2, -1, 0, 1};
    double *mask = (double *) malloc(size * size * sizeof(double));
    for (int i = 0; i < size * size; i++)
        mask[i] = a[i];
    add_filter(input, size, 0, height, width, mask);
}

void sobelY(unsigned char* input, int height, int width, int size)
{
    int a[9] =  {-1, -2, -1, 0, 0, 0, 1, 2, 1};
    double *mask = (double *) malloc(size * size * sizeof(double));
    for (int i = 0; i < size * size; i++)
        mask[i] = a[i];
    add_filter(input, size, 0, height, width, mask);
}

void toGrey(unsigned char* input, int height, int width)
{
    for (int i = 0; i < height * width; i++)
    {
        unsigned char res = (2126 * input[i * 3] + 7152 * input[i * 3 + 1] + 722 * input[i * 3 + 2]) / 10000;
        for (int k = 0; k < 3; k++)
            input[i * 3 + k] = res;
    }
}

int main(int argc, char* argv[])
{
    /*int argc = 4;
    char* argv[argc];
    argv[1] = "input1.bmp";
    argv[2] = "SobelY";
    argv[3] = "output2.bmp";*/

    FILE* fin;
	FILE* fout;

    fin = fopen(argv[1], "rb");
    fout = fopen(argv[3], "wb");

    if (argc != 4)
    {
        printf("Check the parameters are correct");
        exit(-1);
    }
    if (fin == NULL)
    {
        printf("Input file open failed");
        exit(-1);
    }
    if (fout == NULL)
    {
        printf("Output file create failed");
        exit(-1);
    }

    struct bmp_file file_header;
    struct bmp_info info_header;
    fread(&file_header, sizeof(file_header), 1, fin);
    fread(&info_header, sizeof(info_header), 1, fin);
    unsigned char* input = (unsigned char*)malloc(info_header.size_image);
    fseek(fin, file_header.bf_off_bits, SEEK_SET);
    fread(input, 1, info_header.size_image, fin);

    if (!strcmp(argv[2], "Averaging3"))
        averaging(input, info_header.height, info_header.widht, 3);
    else if (strcmp(argv[2], "Averaging5") == 0)
        averaging(input, info_header.height, info_header.widht, 5);
    else if (!strcmp(argv[2], "Gauss3"))
        gauss(input, info_header.height, info_header.widht, 3);
    else if (!strcmp(argv[2], "Gauss5"))
        gauss(input, info_header.height, info_header.widht, 5);
    else if (!strcmp(argv[2], "SobelX"))
        sobelX(input, info_header.height, info_header.widht, 3);
    else if (!strcmp(argv[2], "SobelY"))
        sobelY(input, info_header.height, info_header.widht, 3);
    else if (!strcmp(argv[2], "ToGrey"))
        toGrey(input, info_header.height, info_header.widht);
    else
    {
        printf("Check filter name\n");
        exit(-1);
    }
    printf("Cool!\n");
    fwrite(&file_header, sizeof(file_header), 1, fout);
    fwrite(&info_header, sizeof(info_header), 1, fout);
    for (int i = 0; i < info_header.size_image; i++)
        fwrite(&input[i], 1, 1, fout);
    free(input);
    fclose(fin);
    fclose(fout);
    return 0;
}
