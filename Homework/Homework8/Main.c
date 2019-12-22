#define _CRT_SECURE_NO_WARNINGS
#include "bmp.h"
#include "filter.h"

#include <stdio.h>

void printUsage() 
{

    const char description[] = "Usage: progname <input> <filter_index> <output>"
                               "\n\n\tavaialable filters: \n"
                               "\t\t0. Averaging filter 3x3\n"
                               "\t\t1. Gaussian averaging filter 3x3\n"
                               "\t\t2. Sobel filter by X\n"
                               "\t\t3. Sobel filter by Y\n"
                               "\t\t4. GrayScale\n";

    printf("%s\n", description);
}

int toNumber(const char* buffer, int* num) 
{

    if (!buffer || !num)
        return 0;

    (*num) = 0;
    int digits = 0;
    while (*buffer)
	{
        int c = *buffer++;
        if (!(c >= '0' && c <= '9'))
            return 0;
        (*num) = (*num) * 10 + (c - '0');
        ++digits;
    }
    return digits;
}

int main(int argc, const char** argv) 
{

    int filterIndex = 0;

    if (argc != 4 ||
            toNumber(argv[2], &filterIndex) != 1 ||
            !(filterIndex >= 0 && filterIndex < 5)) 
	{
        printUsage();
        return 0;
    }
    printf("\n");

    const char* inputFile = argv[1];
    const char* outputFile = argv[3];

    BMP* bmp = bmpRead(inputFile);
    if (!bmp)
        return -1;

    switch (filterIndex) 
	{
        case 0:
            averageFilter(bmp);
            break;
        case 1:
            gaussianAvgerageFilter(bmp);
            break;
        case 2:
            SobelXFilter(bmp);
            break;
        case 3:
            SobelYFilter(bmp);
            break;
        case 4:
            greyFilter(bmp);
            break;
        default:
            break;
    }

    if (bmpSaveAs(bmp, outputFile))
        return -1;

    bmpClose(bmp);
    printf("[*] %s closed\n", inputFile);
    return 0;
}