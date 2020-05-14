#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#define SIZE_MASK 3

#pragma pack(push, 1)

typedef struct bitMapFileHeader
{
    unsigned short bfType;
    unsigned int bfSize;
    unsigned short bfReversed1;
    unsigned short bfReversed2;
    unsigned  int bfOffBits;
}bitMapFileHeader;

typedef struct bitMapInfoHeader
{
    unsigned int biSize;
    unsigned int biWidth;
    unsigned int biHeight;
    unsigned short biPlanes;
    unsigned short biBitCount;
    unsigned int  biCompression;
    unsigned int  biSizeImage;
    unsigned int biXPelsPerMeter;
    unsigned int biYPelsPerMeter;
    unsigned int  biClrUsed;
    unsigned int  biClrImportant;
} bitMapInfoHeader;

#pragma pack(pop)


void toGrey(unsigned char* image, unsigned int height, unsigned int width)
{
    for (int i = 0; i < height * width; i++)
    {
        int brightness = (int)(image[i * 3] * 0.2126 + image[i * 3 + 1] * 0.7152 + image[i * 3 + 2] * 0.0722);
        if (brightness > 255)
        {
            image[i * 3] = 255;
            image[i * 3 + 1] = 255;
            image[i * 3 + 2] = 255;
        }
        image[i * 3] = brightness;
        image[i * 3 + 1] = brightness;
        image[i * 3 + 2] = brightness;
    }
}

void convolution(unsigned char* image, const double* nucleus, unsigned int height, unsigned int width, int dev)
{
    unsigned char* output = (unsigned char*)calloc(3 * height * width, sizeof(unsigned char));
    double r = 0;
    double g = 0;
    double b = 0;
    for (int h = 0; h < height; h++)
    {
        for (int w = 0; w < width; w++)
        {
            for (int my = 0; my < SIZE_MASK; my++)
            {
                for (int mx = 0; mx < SIZE_MASK; mx++)
                {
                    if ((h + my - 1) >= 0 && (h + my - 1) < height && (w + mx - 1) >= 0 && (w + mx - 1) < width)
                    {
                        r += image[((h + my - 1) * width + w + mx - 1) * 3] * nucleus[my * SIZE_MASK + mx];
                        g += image[((h + my - 1) * width + w + mx - 1) * 3 + 1] * nucleus[my * SIZE_MASK + mx];
                        b += image[((h + my - 1) * width + w + mx - 1) * 3 + 2] * nucleus[my * SIZE_MASK + mx];
                    }
                }
            }
            if (dev)
            {
                output[(h * width + w) * 3] = (unsigned char)(r / dev);
                output[(h * width + w) * 3 + 1] = (unsigned char)(g / dev);
                output[(h * width + w) * 3 + 2] = (unsigned char)(b / dev);
            }
            else
            {
                double x = 0;
                if (r + g + b > 384)
                    x = 255;
                else
                    x = (double)(((double)abs((int)r) + (double)abs((int)b) + (double)abs((int)g)) / 3);

                output[(h * width + w) * 3 + 0] = (unsigned char)x;
                output[(h * width + w) * 3 + 1] = (unsigned char)x;
                output[(h * width + w) * 3 + 2] = (unsigned char)x;
            }
            r = 0;
            g = 0;
            b = 0;
        }
    }

    for (int i = 0; i < height * width * 3; i++)
        image[i] = output[i];
    free(output);
}

void averaging(unsigned char* image, unsigned int height, unsigned int width)
{
    double nucleus[9] = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    convolution(image, nucleus, height, width, 9);
}

void gauss(unsigned char* image, unsigned int height, unsigned int width)
{
    double nucleus[9] = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
    convolution(image, nucleus, height, width, 16);
}

void sobelX(unsigned char* image, unsigned int height, unsigned int width)
{
    double nucleus[9] = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
    convolution(image, nucleus, height, width, 0);
}

void sobelY(unsigned char* image, unsigned int height, unsigned int width)
{
    double nucleus[9] = { 1, 2, 1, 0, 0, 0, -1, -2, -1 };
    convolution(image, nucleus, height, width, 0);
}


int main(int argc, char* argv[])
{
    if (argc != 4)
    {
        printf("Invalid number of parameters");
        exit(EXIT_FAILURE);
    }
    FILE* fin = fopen(argv[1], "rb");
    FILE* fout = fopen(argv[3], "wb");

    if (fin == NULL)
    {
        printf("Error opening input file\n");
        exit(EXIT_FAILURE);
    }
    if (fout == NULL)
    {
        printf("Error opening output file\n");
        exit(EXIT_FAILURE);
    }

    bitMapFileHeader bFileH;
    bitMapInfoHeader bMIH;
    fread(&bFileH, sizeof(bFileH), 1, fin);
    fread(&bMIH, sizeof(bMIH), 1, fin);
    // fseek(fin, bFileH.bfOffBits, SEEK_SET);

    unsigned char* image = (unsigned char*)calloc(bMIH.biWidth * bMIH.biHeight * 3, 1);
    fread(image, sizeof(unsigned char), bMIH.biWidth * bMIH.biHeight * 3, fin);

    if (image == NULL)
    {
        printf("Error in file\n");
    }

    char* filter = argv[2];
    if (strcmp(filter, "averaging") == 0)
    {
        averaging(image, bMIH.biHeight, bMIH.biWidth);
    }
    else if (strcmp(filter, "gauss") == 0)
    {
        gauss(image, bMIH.biHeight, bMIH.biWidth);
    }
    else if (strcmp(filter, "sobelx") == 0)
    {
        sobelX(image, bMIH.biHeight, bMIH.biWidth);
    }
    else if (strcmp(filter, "sobely") == 0)
    {
        sobelY(image, bMIH.biHeight, bMIH.biWidth);
    }
    else if (strcmp(filter, "togrey") == 0)
    {
        toGrey(image, bMIH.biHeight, bMIH.biWidth);
    }
    else
    {
        printf("Invalid input");
    }


    fwrite(&bFileH, sizeof(bFileH), 1, fout);
    fwrite(&bMIH, sizeof(bMIH), 1, fout);
    for (int i = 0; i < bMIH.biHeight * bMIH.biWidth * 3; i++)
    {
        fwrite(&image[i], sizeof(unsigned char), 1, fout);
    }
    free(image);
    fclose(fin);
    fclose(fout);
    printf("Successfully\n");
    return 0;
}