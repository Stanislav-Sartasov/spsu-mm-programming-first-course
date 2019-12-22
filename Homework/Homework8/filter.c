
#include "filter.h"
#include <stdlib.h>

static float max_p(float a, float b) 
{
    return (a > b) ? a : b;
}

static float min_p(float a, float b) 
{
    return (a < b) ? a : b;
}

// wrap pixel to range [0, 255]
static Pixel wrapPixel(Pixel pixel) {
    pixel.r = min_p(255, max_p(0, pixel.r));
    pixel.g = min_p(255, max_p(0, pixel.g));
    pixel.b = min_p(255, max_p(0, pixel.b));
    return pixel;
}

// return the index of pixel[r, c] in pixelArray
static u32 pixelIndex(BMP* bmp, int r, int c) 
{
    u32 rowSize = (bmp->width * bmp->pixSize);
    return (rowSize * r) + (c * bmp->pixSize);
}

static Pixel readPixel(BMP* bmp, int r, int c)
{
    u32 index = pixelIndex(bmp, r, c);
    Pixel pixel = {.r = (float)bmp->pixelArray[index + 2],
                   .g = (float)bmp->pixelArray[index + 1],
                   .b = (float)bmp->pixelArray[index + 0]};
    return pixel;
}

static void writePixel(BMP* bmp, int r, int c, Pixel pixel) 
{
    u32 index = pixelIndex(bmp, r, c);
    bmp->pixelArray[index + 2] = (u8)pixel.r;
    bmp->pixelArray[index + 1] = (u8)pixel.g;
    bmp->pixelArray[index + 0] = (u8)pixel.b;
}

void Convolution(Pixel* inPixelMatrix, Kernel* kernel, int w, int h,
        Pixel* outPixelMatrix)
{

    // loop through all the pixel in bmp
    for(int r=0; r<h; ++r) {
        for (int c = 0; c < w; ++c)
		{
            Pixel sum = {0, 0, 0};

            // loop through neighboring pixels
            for(int i=-1; i <= 1; ++i)
			{
                for(int j=-1; j <= 1; ++j)
				{

                    // neighboring pixel index
                    int cr = (r + i);
                    int cc = (c + j);

                    if (cr >= 0 && cr < h && cc >= 0 && cc < w)
					{
                        // neighboring pixel within image edges
                        Pixel pix = inPixelMatrix[cr * w + cc];
                        sum.r += pix.r * kernel->m[i+1][j+1];
                        sum.g += pix.g * kernel->m[i+1][j+1];
                        sum.b += pix.b * kernel->m[i+1][j+1];
                    } else {
                        // neighboring pixel outside image edge, use the current pixel
                        Pixel pix = inPixelMatrix[r * w + c];
                        sum.r += pix.r * kernel->m[i+1][j+1];
                        sum.g += pix.g * kernel->m[i+1][j+1];
                        sum.b += pix.b * kernel->m[i+1][j+1];
                    }
                }
            }
            // save the sum
            outPixelMatrix[r * w + c] = sum;
        }
    }
}

Pixel* bmpToPixelMatrix(BMP* bmp)
{
    size_t size = bmp->width * bmp->height * sizeof(Pixel);
    Pixel* pixelMatrix = malloc(size);
    if (!pixelMatrix)
	{
        printf("[*] cannot allocate memory\n");
        return NULL;
    }
    for (int r = 0; r < bmp->height; ++r)
	{
        for (int c = 0; c < bmp->width; ++c)
		{
            pixelMatrix[r * bmp->width + c] = readPixel(bmp, r, c);
        }
    }
    return pixelMatrix;
}

void savePixelMatrixToBmp(BMP* bmp, Pixel* pixelMatrix)
{
    for (int r=0; r<bmp->height; ++r) {
        for(int c=0; c<bmp->width; ++c) {
            Pixel pixel = pixelMatrix[r * bmp->width + c];
            writePixel(bmp, r, c, wrapPixel(pixel));
        }
    }
}

void applyFilter(BMP* bmp, Kernel* kernel) 
{
    Pixel* inPixelMatrix  = bmpToPixelMatrix(bmp);
    Pixel* outPixelMatrix = malloc(bmp->width * bmp->height * sizeof(Pixel));

    if (!inPixelMatrix || !outPixelMatrix)
	{
        free(inPixelMatrix);
        free(outPixelMatrix);
        return;
    }

    Convolution(inPixelMatrix, kernel, bmp->width, bmp->height, outPixelMatrix);

    savePixelMatrixToBmp(bmp, outPixelMatrix);
    free(inPixelMatrix);
    free(outPixelMatrix);
}

void averageFilter(BMP* bmp) 
{
    printf("[*] performin_pg averaging filter 3x3, 1 iteration\n");

    Kernel k;
    k.m[0][0] = k.m[0][1] = k.m[0][2] = 1 / 9.0f;
    k.m[1][0] = k.m[1][1] = k.m[1][2] = 1 / 9.0f;
    k.m[2][0] = k.m[2][1] = k.m[2][2] = 1 / 9.0f;

    applyFilter(bmp, &k);
}

void gaussianAvgerageFilter(BMP* bmp)
{
    printf("[*] performin_pg gaussian averaging filter 3x3, 1 iteration\n");

    // 2d gaussian filter
    Kernel k = {.m = {{1/16.0f, 2/16.0f, 1/16.0f},
                      {2/16.0f, 4/16.0f, 2/16.0f},
                      {1/16.0f, 2/16.0f, 1/16.0f}}};

    applyFilter(bmp, &k);
}

void SobelXFilter(BMP* bmp)
{
    printf("[*] performin_pg sobel filter by X\n");
    Kernel k = {.m = {{-1, 0, 1},
                      {-2, 0, 2},
                      {-1, 0, 1}}};

    applyFilter(bmp, &k);
}

void SobelYFilter(BMP* bmp) 
{
    printf("[*] performin_pg sobel filter by Y\n");
    Kernel k = {.m = {{1, 2, 1},
                      {0, 0, 0},
                      {-1, -2, -1}}};

    applyFilter(bmp, &k);
}

void greyFilter(BMP* bmp) 
{
    printf("[*] performin_pg grey filter\n");

    for (int r=0; r<bmp->height; ++r) 
	{
        for(int c=0; c<bmp->width; ++c) 
		{
            Pixel pixel = readPixel(bmp, r, c);
            float n = pixel.r + pixel.g + pixel.b;
            pixel.r = n / 3.0f;
            pixel.g = n / 3.0f;
            pixel.b = n / 3.0f;
            writePixel(bmp, r, c, wrapPixel(pixel));
        }
    }
}
