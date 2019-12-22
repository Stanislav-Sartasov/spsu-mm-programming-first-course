#ifndef TASKS_FILTER_H
#define TASKS_FILTER_H

#include "bmp.h"

#undef min
#undef max

typedef struct 
{
    float m[3][3];
} Kernel;

typedef struct 
{
    float r, g, b;
} Pixel;

void Convolution(Pixel* inPixelMatrix, Kernel* kernel, int w, int h,
        Pixel* outPixelMatrix);

void applyFilter(BMP* bmp, Kernel* kernel);

// filtering functions
void averageFilter(BMP* bmp);
void gaussianAvgerageFilter(BMP* bmp);
void SobelXFilter(BMP* bmp);
void SobelYFilter(BMP* bmp);
void greyFilter(BMP* bmp);

#endif //TASKS_FILTER_H
