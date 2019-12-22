#define _CRT_SECURE_NO_WARNINGS
#include "bmp.h"
#include <stdint.h>
#include <stdlib.h>
#include <string.h>

#define BITMAPINFOHEADER_SIZE       (40)
#define BITMAPV4HEADER_SIZE         (108)
#define BITMAPV5HEADER_SIZE         (124)

size_t fReadOffset(void* buffer, size_t size, size_t count, FILE* stream,
        size_t offset) 
{

    if (!stream)
        return 0;
    fseek(stream, offset, SEEK_SET);
    return fread(buffer, size, count, stream);
}

size_t fWriteOffset(void* buffer, size_t size, size_t count, FILE* stream,
                    size_t offset)
{

    if (!stream)
        return 0;
    fseek(stream, offset, SEEK_SET);
    return fwrite(buffer, size, count, stream);
}

BMP* bmpRead(const char* path) 
{
    FILE* fp = fopen(path, "rb");
    if (!fp) {
        printf("[*] bmpRead: cannot open file %s\n", path);
        return NULL;
    }

    // present in Bitmap file header
    u32 bmpSize;
    u32 pixelArrayOffset;

    // present in Bitmap info header
    u32 bmpInfoHeaderSize;
    i32 width;
    i32 height;
    u16 bpp;
    u32 compression;
    u32 origin;
    u8* pixelArray = NULL;

#define READ_VAR(var, size, offset) {\
    if (fReadOffset(var, size, 1, fp, offset) != 1){\
        fclose(fp);\
        return NULL;\
    }\
}
    READ_VAR(&bmpSize,          4, 0x0002)
    READ_VAR(&pixelArrayOffset, 4, 0x000A)

    READ_VAR(&bmpInfoHeaderSize, 4, 0x000E)
    if (!(bmpInfoHeaderSize == BITMAPINFOHEADER_SIZE ||
          bmpInfoHeaderSize == BITMAPV4HEADER_SIZE   ||
          bmpInfoHeaderSize == BITMAPV5HEADER_SIZE)) {
        printf("[*] not supported: unsupported info header\n");
        fclose(fp);
        return NULL;
    }
    READ_VAR(&width,       4, 0x0012)
    READ_VAR(&height,      4, 0x0016)
    READ_VAR(&bpp,         2, 0x001C)
    READ_VAR(&compression, 4, 0x001E)

    if (height < 0) 
	{
        // origin is upper-left
        origin = 1;
        height = -height;
    } else 
	{
        // origin is lower-left
        origin = 0;
    }

    printf("\tfile size:   %u\n", bmpSize);
    printf("\twidth:       %d\n", width);
    printf("\theight:      %d\n", height);
    printf("\tbpp:         %u\n", bpp);
    printf("\tcompression: %u\n", compression);
    printf("\n");

    int err = 0;
    if (!(bpp == 24 || bpp == 32))
        err = 1;
    else if (compression != 0)
        err = 2;
    else if (!(pixelArray = malloc(width * height * (bpp >> 3u))))
        err = 3;

    if (err)
	{
        switch (err)
		{
            case 1:
                printf("[*] not supported: bmp not 24-bit or 32-bit\n");
                break;
            case 2:
                printf("[*] not supported: compression detected\n");
                break;
            case 3:
                printf("[*] cannot allocate memory\n");
                break;
        }
        fclose(fp);
        return NULL;
    }

    u32 rowSize = (width * (bpp >> 3u));
    rowSize += (rowSize % 4) ? (4 - rowSize % 4): 0;

    // copy pixelArray from file to memory
    u32 pixelArrayIndex = 0;
    for (int i=0; i < height; ++i)
	{
        u32 row = (origin) ? (i) : (height - 1 - i);
        u32 index = pixelArrayOffset + (rowSize * row);
        fReadOffset(&pixelArray[pixelArrayIndex], 1, (width * (bpp >> 3u)), fp, index);
        pixelArrayIndex += (width * (bpp >> 3u));
    }

    BMP* bmp = malloc(sizeof(BMP));
    if (bmp == NULL) 
	{
        printf("[*] cannot allocate memory\n");
        fclose(fp);
        free(pixelArray);
        return NULL;
    }

    bmp->fp = fp;
    bmp->fileSize = bmpSize;
    bmp->pixelArrayOffset = pixelArrayOffset;
    bmp->infoHeaderSize = bmpInfoHeaderSize;
    bmp->width = width;
    bmp->height = height;
    bmp->bpp = bpp;
    bmp->compression = compression;
    bmp->origin = origin;
    bmp->pixSize = (bpp >> 3u);
    bmp->pixelArray = pixelArray;

    return bmp;
}

int  bmpSaveAs(BMP* bmp, const char* path)
{
    if (!bmp)
        return 1;

    u8* outMem = malloc(bmp->fileSize);
    if (!outMem) {
        printf("[*] BmpSaveAs: cannot allocate memory\n");
        return 1;
    }

    // create exact duplicate of original bmp file
    if (fReadOffset(outMem, 1, bmp->fileSize, bmp->fp, 0) != bmp->fileSize)
	{
        printf("[*] BmpSaveAs: cannot copy file\n");
        free(outMem);
        return 1;
    }

    // copy the modified pixel data to memory
    u32 rowSize = (bmp->width * (bmp->bpp >> 3u));
    rowSize += (rowSize % 4) ? (4 - rowSize % 4): 0;

    u32 srcIndex = 0;
    for (int i=0; i < bmp->height; ++i)
	{
        u32 row = (bmp->origin) ? (i) : (bmp->height - 1 - i);
        u32 dstIndex = bmp->pixelArrayOffset + (rowSize * row);
        size_t nBytes = (bmp->width * (bmp->bpp >> 3u));
        memcpy(&outMem[dstIndex], &bmp->pixelArray[srcIndex], nBytes);
        srcIndex += nBytes;
    }

    // open new bmp file
    FILE* outFP = fopen(path, "wb");
    if (!outFP)
	{
        printf("[*] bmpSaveAs: cannot create file\n");
        free(outMem);
        return 1;
    }

    // save memory to new bmp file
    if (fWriteOffset(&outMem[0], 1, bmp->fileSize, outFP, 0) != bmp->fileSize)
	{
        printf("[*] bmpSaveAs: cannot write to file\n");
        free(outMem);
        fclose(outFP);
        return 1;
    }

    fclose(outFP);
    free(outMem);
    printf("[*] bmpSaveAs: file saved at %s\n", path);
    return 0;
}

void bmpClose(BMP* bmp) 
{
    if (!bmp)
        return;

    fclose(bmp->fp);
    free(bmp->pixelArray);
    free(bmp);
}


