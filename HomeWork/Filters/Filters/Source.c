#define _CRT_SECURE_NO_WARNINGS
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

void grey(unsigned char* bitmap, int width, int height, int bitcount)
{
    for (int i = 0; i < width * height * (bitcount / 8); i += bitcount / 8)
    {
        unsigned char mid = (bitmap[i] + bitmap[i + 1] + bitmap[i + 2]) / 3;
        for (int j = 0; j < 3; j++)
            bitmap[i + j] = mid;
    }
}

void filter(unsigned char* input, int height, int width, int choice)
{
    double* mask = (double*)malloc(3 * 3 * sizeof(double));
    int matrix[3][9] = { {1,  2,  1,  2,  4,  2,  1,  2,  1},
                            {-1,  0,  1, -2,  0,  2, -1,  0,  1},
                            {-1, -2, -1, 0, 0, 0, 1, 2, 1} }; 

    if (choice == 1)   // усред
    {
        for (int i = 0; i < 3 * 3; i++)
            mask[i] = 1;
    }
    else if (choice == 2) // гаус
    {
        for (int i = 0; i < 9; i++)
            mask[i] = matrix[0][i];
    }
    else if (choice == 3) // соболь Х
    {
        for (int i = 0; i < 3 * 3; i++)
            mask[i] = matrix[1][i];
    }
    else if (choice == 4)   //соболь У
    {
        for (int i = 0; i < 3 * 3; i++)
            mask[i] = matrix[2][i];
    }


    unsigned char* output = (unsigned char*)malloc(3 * height * width * sizeof(char));
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            double result[3] = { 0, 0, 0 };
            double d = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if ((i + y - 1) >= 0 && (i + y - 1) < height && (j + x - 1) >= 0 && (j + x - 1) < width)
                    {
                        d += mask[y * 3 + x];
                        for (int k = 0; k < 3; k++)
                            result[k] += input[((i + y - 1) * width + j + x - 1) * 3 + k] * mask[y * 3 + x];
                    }
                }
            }
            if (choice == 1 || choice == 2)
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

int main(int argc, char* argv[])
{

    int chek = 1;
    if (chek) printf("%d", 777);
    else printf("%d", 000);
    if (argc != 4)
    {
        printf("Wrong input");
        exit(-1);
    }

    FILE* fin;
    FILE* fout;

    fin = fopen(argv[1], "rb");
    fout = fopen(argv[3], "wb");

    if (fin == NULL)
    {
        printf("Input failed");
        exit(-1);
    }
    if (fout == NULL)
    {
        printf("Output failed");
        exit(-1);
    }


    struct bmp_file file_header;
    struct bmp_info info_header;
    fread(&file_header, sizeof(file_header), 1, fin);
    fread(&info_header, sizeof(info_header), 1, fin);

    unsigned char* bitmap = (unsigned char*)malloc(info_header.size_image);

    unsigned char* colorTable = (unsigned char*)malloc(file_header.bf_off_bits - 54);

    fread(colorTable, 1, file_header.bf_off_bits - 54, fin);

    fseek(fin, file_header.bf_off_bits, SEEK_SET);
    fread(bitmap, 1, info_header.size_image, fin);

    fwrite(&file_header, sizeof(file_header), 1, fout);
    fwrite(&info_header, sizeof(info_header), 1, fout);

    for (int i = 0; i < file_header.bf_off_bits - 54; i++)
    {
        fwrite(&colorTable[i], 1, 1, fout);
    }

    if (!strcmp(argv[2], "grey"))
        grey(bitmap, info_header.height, info_header.widht, info_header.bit_count);
    if (!strcmp(argv[2], "fveraging"))
        filter(bitmap, info_header.height, info_header.widht, 1);
     if (!strcmp(argv[2], "gauss"))
         filter(bitmap, info_header.height, info_header.widht, 2);
    if (!strcmp(argv[2], "sobolX"))
        filter(bitmap, info_header.height, info_header.widht, 3);
    if (!strcmp(argv[2], "sobolY"))
        filter(bitmap, info_header.height, info_header.widht, 4);



    for (int i = 0; i < info_header.size_image; i++)
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