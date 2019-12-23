#include "filter.h"

void gauss(struct img*, struct BITMAPINFOHEADER *);


int main(int argc, char* argv[])
{
    FILE *fIn, *fOut;
    if (argc != 4 || !(strcmp(argv[2], "median") == 0 || strcmp(argv[2], "gauss") == 0 || strcmp(argv[2], "sobelX") == 0
            || strcmp(argv[2], "sobelY") == 0 || strcmp(argv[2], "sobelAll") == 0 || strcmp(argv[2], "gray") == 0) || fopen_s(&fIn, argv[1], "rb") != 0)
        {
            if ((argc == 2) && (strcmp(argv[1], "/help") == 0))
                printf("Comands: median, gray, gauss, sobelX, sobelY, sobelAll\npatern of input: <filein.bmp> <comand> <fileout.bmp>\nExample of input: in.bmp gauss out.bmp\n");
            else
                printf("Something is wrong, try again or try to use \"/help\"\n ");
            return 0;
        }
    fopen_s(&fIn, argv[1], "rb");
    fopen_s(&fOut, argv[3], "wb");

    struct BITMAPFILEHEADER bmpFileH;
    struct BITMAPINFOHEADER bmpInfoH;
    struct img image;
    unsigned int paleteSize;
    char *palete = NULL;
    unsigned int padLine;

    getReady(fIn, &bmpFileH, &bmpInfoH, &image, &paleteSize, palete, &padLine);
    fclose(fIn);
    ////////////
    filter(&image, argv[2]);
    ///////////
    getDone(fOut, bmpFileH, bmpInfoH, &image, paleteSize, palete, padLine);
    fclose(fOut);

    printf("done good");
    return 0;
}
