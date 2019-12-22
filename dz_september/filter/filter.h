#pragma once
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#pragma pack(push, 1)
struct BITMAPFILEHEADER
{
    unsigned short	bfType;
    unsigned int bfSize;
    unsigned short bfReserved1;
    unsigned short bfReserved2;
    unsigned int bfOffBits;
};

struct BITMAPINFOHEADER
{
    unsigned int biSize;
    unsigned int  biWidth;
    unsigned int  biHeight;
    unsigned short  biPlanes;
    unsigned short  biBitCount;
    unsigned int biCompression;
    unsigned int biSizeImage;
    unsigned int  biXPelsPerMeter;
    unsigned int  biYPelsPerMeter;
    unsigned int biClrUsed;
    unsigned int biClrImportant;
};
#pragma pack(pop)

struct pix
{
    unsigned char a[3];
};

struct img
{
    struct pix **bits;
    unsigned int len, wid;
};

void filter(struct img *, char *);
int getReady(FILE *fIn, struct BITMAPFILEHEADER *bmpFileH, struct BITMAPINFOHEADER *bmpInfoH, struct img *image, unsigned int *paleteSize, char *palete, unsigned int *padLine);
int getDone(FILE *fOut, struct BITMAPFILEHEADER bmpFileH, struct BITMAPINFOHEADER bmpInfoH, struct img *image, unsigned int paleteSize, char *palete, unsigned int padLine);
