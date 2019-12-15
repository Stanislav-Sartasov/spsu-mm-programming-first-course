#include <stdio.h>
#include <Windows.h>


typedef BITMAPFILEHEADER fileHeader;
typedef BITMAPINFOHEADER fileInfo;


typedef struct RedGreenBlue
{
	unsigned char red;
	unsigned char green;
	unsigned char blue;
} RGB;


/*
	Applies the filter to the bmp file.
	@param startFile:	the input file.
	@param filter:		the filter applied.
	@param finalFile:	the output file.
*/
int useFilter(FILE* startFile, unsigned int filter, FILE* finalFile);


/*
	Applies the Average 3x3 filter.
	@param ptrRGB:		the old RBG properties.
	@param height:		the height of the image.
	@param width:		the width of the image.
	@param ptrNewRGB:	the new RGB propertie will be stored here.
*/
void average3x3Filter(RGB** ptrRGB, unsigned int height, unsigned int width, RGB** ptrNewRGB);


/*
	Applies the Gauss 3x3 filter.
	@param ptrRGB:		the old RBG properties.
	@param height:		the height of the image.
	@param width:		the width of the image.
	@param ptrNewRGB:	the new RGB propertie will be stored here.
*/
void gauss3x3Filter(RGB** ptrRGB, unsigned int height, unsigned int width, RGB** ptrNewRGB);


/*
	Applies the Sobel filter.
	@param ptrRGB:		the old RBG properties.
	@param height:		the height of the image.
	@param width:		the width of the image.
	@param filterType:	the filter type (x or y).
	@param ptrNewRGB:	the new RGB propertie will be stored here.
*/
void sobelAnyFilter(RGB** ptrRGB, unsigned int height, unsigned int width, unsigned int filterType, RGB** ptrNewRGB);

/*
	Applies the Gray filter.
	@param ptrRGB:		the old RBG properties.
	@param height:		the height of the image.
	@param width:		the width of the image.
*/
void greyFilter(RGB** ptrRGB, unsigned int height, unsigned int width);