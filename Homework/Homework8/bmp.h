#ifndef TASKS_BMP_H
#define TASKS_BMP_H

#include <stdio.h>
#include <stdint.h>

#undef min
#undef max

typedef uint8_t  u8;
typedef uint16_t u16;
typedef uint32_t u32;
typedef int32_t  i32;

typedef struct 
{
    FILE* fp;

    // present in Bitmap file header
    u32 fileSize;
    u32 pixelArrayOffset;

    // present in Bitmap info header
    u32 infoHeaderSize;
    i32 width;
    i32 height;
    u16 bpp;
    u32 compression;

    u32 origin;
    u32 pixSize;
    u8* pixelArray;
} BMP;

BMP* bmpRead(const char* path);
int  bmpSaveAs(BMP* bmp, const char* path);
void bmpClose(BMP* bmp);

#endif //TASKS_BMP_H
