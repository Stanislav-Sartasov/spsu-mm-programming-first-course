#ifndef IMAGE_H
#define IMAGE_H

#include <cstring>
#include <iostream>
#include <fstream>
#include <cmath>

template<typename T>
struct pix
{
    T a[3];
};

struct img
{
    struct pix<unsigned char> **bits;
    unsigned int len, wid;
};

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

class Image
{
private:

    struct BITMAPFILEHEADER bmpFileH;
    struct BITMAPINFOHEADER bmpInfoH;
    struct img image;
    unsigned int paleteSize;
    char *palete = NULL;
    unsigned int padLine;
    int **matrix, xMatr = 0;
    unsigned int pixelsAmount;
    unsigned int pixelsDone;

    void medPix(int z);
    struct pix<long> goMatrix(int i, int j);
    void sobel();

public:
    Image();
    ~Image();
    void createMatrix(int k);
    void openBMP(char *str);
    void closeBMP(char *str);
    void gary();
    void median();
    void gauss();
    void sobelY();
    void sobelX();
    void sobelAll();
};

#endif // IMAGE_H
