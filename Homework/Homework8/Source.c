#include <stdio.h>
#include <malloc.h>

#include "Header.h"


void average3x3Filter(RGB** ptrRGB, unsigned int height, unsigned int width, RGB** ptrNewRGB) {
	for (int i = 1; i < height - 1; i++) {
		for (int j = 1; j < width - 1; j++) 	{
			unsigned int red = 0, green = 0, blue = 0;

			for (int k = -1; k < 2; k++) {
				for (int l = -1; l < 2; l++) {
					red += ptrRGB[i + k][j + l].red;
					green += ptrRGB[i + k][j + l].green;
					blue += ptrRGB[i + k][j + l].blue;
				}
			}


			ptrNewRGB[i][j].red = (unsigned char)(red / 9);
			ptrNewRGB[i][j].green = (unsigned char)(green / 9);
			ptrNewRGB[i][j].blue = (unsigned char)(blue / 9);
		}
	}
}


void gauss3x3Filter(RGB** ptrRGB, unsigned int height, unsigned int width, RGB** ptrNewRGB) {
	unsigned int matrix[3][3] = { { 1, 2, 1 },\
								  { 2, 4, 2 },\
								  { 1, 2, 1 } };


	for (int i = 1; i < height - 1; i++) {
		for (int j = 1; j < width - 1; j++) {
			unsigned int red = 0, green = 0, blue = 0;

			for (int k = 0; k < 3; k++)	{
				for (int l = 0; l < 3; l++)	{
					red += ptrRGB[i + k - 1][j + l - 1].red * matrix[k][l];
					green += ptrRGB[i + k - 1][j + l - 1].green * matrix[k][l];
					blue += ptrRGB[i + k - 1][j + l - 1].blue * matrix[k][l];
				}
			}


			ptrNewRGB[i][j].red = (unsigned char)(red / 16);
			ptrNewRGB[i][j].green = (unsigned char)(green / 16);
			ptrNewRGB[i][j].blue = (unsigned char)(blue / 16);
		}
	}
}


void useSobelAnyFilter(RGB** ptrRGB, unsigned int i, unsigned int j, unsigned int filterType, RGB** ptrNewRGB) {
	int anyMatrix[3][3];
	int red = 0, green = 0, blue = 0;


	switch (filterType) {
	case 1:
		anyMatrix[0][0] = 1; //{ 1,  2,  1}
		anyMatrix[0][1] = 2; //{ 0,  0,  0}
		anyMatrix[0][2] = 1; //{-1, -2, -1}
		anyMatrix[1][0] = 0;
		anyMatrix[1][1] = 0;
		anyMatrix[1][2] = 0;
		anyMatrix[2][0] = -1;
		anyMatrix[2][1] = -2;
		anyMatrix[2][2] = -1;
		break; 
	case 2:
		anyMatrix[0][0] = -1; //{-1, 0, 1}
		anyMatrix[0][1] = 0;  //{-2, 0, 2}
		anyMatrix[0][2] = 1;  //{-1, 0, 1}
		anyMatrix[1][0] = -2;
		anyMatrix[1][1] = 0;
		anyMatrix[1][2] = 2;
		anyMatrix[2][0] = -1;
		anyMatrix[2][1] = 0;
		anyMatrix[2][2] = 1;
		break;
	default:
		return;
	}


	for (int i = 0; i < 9; i++)	{
		RGB* ptrTmpRGB = &ptrRGB[i - 1 + i / 3][j - 1 + i % 3];

		red += ptrTmpRGB->red * anyMatrix[i / 3][i % 3];
		green += ptrTmpRGB->green * anyMatrix[i / 3][i % 3];
		blue += ptrTmpRGB->blue * anyMatrix[i / 3][i % 3];
	}


	if (red < 0) {
		red = 0;
	}
	else if (red > 255) {
		red = 255;
	}

	if (green < 0) {
		green = 0;
	}
	else if (green > 255) {
		green = 255;
	}

	if (blue < 0) {
		blue = 0;
	}
	else if (blue > 255) {
		blue = 255;
	}


	ptrNewRGB[i][j].red = (unsigned char)(red);
	ptrNewRGB[i][j].green = (unsigned char)(green);
	ptrNewRGB[i][j].blue = (unsigned char)(blue);
}


void sobelAnyFilter(RGB** ptrRGB, unsigned int height, unsigned int width, unsigned int filterType, RGB** ptrNewRGB) {
	for (int i = 1; i < height - 3; i++) {
		for (int j = 1; j < width - 3; j++) {
			if (filterType == 1) {
				useSobelAnyFilter(ptrRGB, i, j, 1, ptrNewRGB);
			}
			else if (filterType == 2) {
				useSobelAnyFilter(ptrRGB, i, j, 2, ptrNewRGB);
			}
		}
	}
}


void greyFilter(RGB** ptrRGB, unsigned int height, unsigned int width, RGB** ptrNewRGB) {
	for (int i = 0; i < height; i++) {
		for (int j = 0; j < width; j++) {
			unsigned char newColor = (ptrRGB[i][j].red + ptrRGB[i][j].green + ptrRGB[i][j].blue) / 3;


			ptrNewRGB[i][j].red = newColor;
			ptrNewRGB[i][j].green = newColor;
			ptrNewRGB[i][j].blue = newColor;
		}
	}
}



RGB** filterStart(fileHeader* fileHeaderPtr, fileInfo* fileInfoPtr, FILE* fileInput, unsigned int* padLine, char** palette, unsigned int* paletteSize) {
	fread(fileHeaderPtr, sizeof(fileHeader), 1, fileInput);
	fread(fileInfoPtr, sizeof(fileInfo), 1, fileInput);

	if (fileInfoPtr->biBitCount != 32 && fileInfoPtr->biBitCount != 24) {
		return(NULL);
	}

	*paletteSize = fileHeaderPtr->bfOffBits - sizeof(fileHeader) - sizeof(fileInfo);

	if (*paletteSize != 0) {
		*palette = (char*)malloc(paletteSize);
		fread(*palette, *paletteSize, 1, fileInput);
	}

	RGB** ptrRGB = (RGB**)calloc(sizeof(RGB*), fileInfoPtr->biHeight);

	for (int i = 0; i < fileInfoPtr->biHeight; i++) {
		ptrRGB[i] = (RGB*)calloc(sizeof(RGB), fileInfoPtr->biWidth);
	}

	*padLine = (4 - (fileInfoPtr->biWidth * (fileInfoPtr->biBitCount / 8)) % 4) & 3;

	for (int i = 0; i < fileInfoPtr->biHeight; i++) {
		for (int j = 0; j < fileInfoPtr->biWidth; j++) {
			ptrRGB[i][j].red = (unsigned char)getc(fileInput);
			ptrRGB[i][j].green = (unsigned char)getc(fileInput);
			ptrRGB[i][j].blue = (unsigned char)getc(fileInput);


			if (fileInfoPtr->biBitCount == 32) {
				getc(fileInput);
			}
		}

		for (int k = 0; k < *padLine; k++) {
			getc(fileInput);
		}
	}


	fclose(fileInput);
	return(ptrRGB);
}


RGB** cpyRGB(RGB** ptrRGB, unsigned int height, unsigned int width) {
	RGB** ptrNewRgb = (RGB**)calloc(sizeof(RGB*), height);

	for (int i = 0; i < height; i++) {
		ptrNewRgb[i] = (RGB*)calloc(sizeof(RGB*), width);
		memcpy(ptrNewRgb[i], ptrRGB[i], sizeof(RGB) * width);
	}


	return(ptrNewRgb);
}


void freeOldRGB(RGB** ptrRGB, fileInfo* fileInfoPtr) {
	unsigned int i;

	for (i = 0; i < fileInfoPtr->biHeight; i++)	{
		free(ptrRGB[i]);
	}


	free(ptrRGB);
}


void completeFilterFinish(RGB** ptrNewRGB, int padLine, fileHeader* fileHeaderPtr, fileInfo* fileInfoPtr, FILE* fileOutput, char* palette, unsigned int paletteSize) {
	unsigned int i, j, k;

	fwrite(fileHeaderPtr, sizeof(fileHeader), 1, fileOutput);
	fwrite(fileInfoPtr, sizeof(fileInfo), 1, fileOutput);

	if (paletteSize != 0) {
		fwrite(palette, paletteSize, 1, fileOutput);
	}

	for (i = 0; i < fileInfoPtr->biHeight; i++)	{
		for (j = 0; j < fileInfoPtr->biWidth; j++) {
			fwrite(&ptrNewRGB[i][j], sizeof(RGB), 1, fileOutput);

			if (fileInfoPtr->biBitCount == 32) {
				putc(0, fileOutput);
			}
		}

		for (k = 0; k < padLine; k++) {
			putc(0, fileOutput);
		}
	}


	fclose(fileOutput);
}


void beginFilterFinish(RGB*** ptrNewRGB, int padLine, fileHeader** fileHeaderPtr, fileInfo** fileInfoPtr, char* palette, unsigned int paletteSize, FILE* fileOutput) {
	fileInfo* tmpFileInfoPtr = *fileInfoPtr;

	completeFilterFinish(*ptrNewRGB, padLine, *fileHeaderPtr, *fileInfoPtr, fileOutput, palette, paletteSize);

	//for (i = 0; i < tmpFileInfoPtr->biHeight; i++)
	//{
	//	free(ptrNewRGB[i]);
	//}

	free(*ptrNewRGB);
	free(*fileHeaderPtr);
	free(*fileInfoPtr);
}


int useFilter(FILE* fileInput, unsigned int filterType, FILE* fileOutput) {
	fileHeader* fileHeaderPtr = (fileHeader*)malloc(sizeof(fileHeader));
	fileInfo* fileInfoPtr = (fileInfo*)malloc(sizeof(fileInfo));
	char* palette;
	unsigned int padLine, paletteSize, i;
	RGB** ptrRGB = filterStart(fileHeaderPtr, fileInfoPtr, fileInput, &padLine, &palette, &paletteSize);

	if (ptrRGB == NULL)	{
		return(0);
	}

	if (filterType == 1) {
		RGB** ptrNewRGB = cpyRGB(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth);
		average3x3Filter(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth, ptrNewRGB);
		freeOldRGB(ptrRGB, fileInfoPtr);
		beginFilterFinish(&ptrNewRGB, padLine, &fileHeaderPtr, &fileInfoPtr, palette, paletteSize, fileOutput);
		return(1);
	}
	else if (filterType == 2) {
		RGB** ptrNewRGB = cpyRGB(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth);
		gauss3x3Filter(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth, ptrNewRGB);
		freeOldRGB(ptrRGB, fileInfoPtr);
		beginFilterFinish(&ptrNewRGB, padLine, &fileHeaderPtr, &fileInfoPtr, palette, paletteSize, fileOutput);
		return(1);
	}
	else if (filterType == 3) {
		RGB** ptrNewRGB = cpyRGB(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth);
		sobelAnyFilter(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth, 1, ptrNewRGB);
		freeOldRGB(ptrRGB, fileInfoPtr);
		beginFilterFinish(&ptrNewRGB, padLine, &fileHeaderPtr, &fileInfoPtr, palette, paletteSize, fileOutput);
		return(1);
	}
	else if (filterType == 4) {
		RGB** ptrNewRGB = cpyRGB(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth);
		sobelAnyFilter(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth, 2, ptrNewRGB);
		freeOldRGB(ptrRGB, fileInfoPtr);
		beginFilterFinish(&ptrNewRGB, padLine, &fileHeaderPtr, &fileInfoPtr, palette, paletteSize, fileOutput);
		return(1);
	}
	else if (filterType == 5) {
		RGB** ptrNewRGB = cpyRGB(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth);
		greyFilter(ptrRGB, fileInfoPtr->biHeight, fileInfoPtr->biWidth, ptrNewRGB);
		freeOldRGB(ptrRGB, fileInfoPtr);
		beginFilterFinish(&ptrNewRGB, padLine, &fileHeaderPtr, &fileInfoPtr, palette, paletteSize, fileOutput);
		return(1);
	}
	else {
		free(fileHeaderPtr);
		free(fileInfoPtr);
		free(ptrRGB);
		return(0);
	}
}