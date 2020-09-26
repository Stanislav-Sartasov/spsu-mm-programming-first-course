
#include <stdio.h>
#pragma pack(push)
#pragma pack(1)
#include <string.h>

typedef struct
{
	unsigned short bfType;
	unsigned int bfSize;
	unsigned short bfReserved1;
	unsigned short bfReserved2;
	unsigned int bfOfBits;
} BMPHEADER;


struct BMPINFO
{
	unsigned int biSize;
	unsigned int biWidth;
	unsigned int biHeight;
	unsigned short biPlanes;
	unsigned short biBitCount;
	unsigned int biCompression;
	unsigned int biSizeImage;
	unsigned int biXPelsPerMeter;
	unsigned int biYPelsPerMeter;
	unsigned int biColorUsed;
	unsigned int biColorImportant;
};

struct RGBQUAD
{
	unsigned char rgbBlue;
	unsigned char rgbGreen;
	unsigned char rgbRed;
	unsigned char rgbReserved;
};

double** getMatrix(unsigned int size)
{
	double** matrix = new double*[size];
	for (int x = 0; x < size; x++)
		matrix[x] = new double[size];
	return matrix;
}


unsigned char* filterApply(unsigned int* imageData, int bytesCount, int mx,
	int my, int mx4, double*** matrixis, unsigned int filterMatrixSize, RGBQUAD* colorData)
{
	unsigned char* temp = new unsigned char[mx4*my];
	double r, g, b;
	unsigned char* ptr = (unsigned char*)imageData;
	double** redFilterMatrix = matrixis[0];
	double** greenFilterMatrix = matrixis[1];
	double** blueFilterMatrix = matrixis[2];

	int usedColorIndexes[256];
	for (int i = 0; i < 256; i++)
		usedColorIndexes[i] = -1;
	int maxUsedIndexNumber = 0;

	for (int y = my - 1; y >= 0; y--)
	{
		unsigned char* tempRow = temp + mx4 * y;
		for (int x = 0; x < mx; x++)
		{
			int filterCenter = filterMatrixSize / 2;
			r = 0;
			g = 0;
			b = 0;

			for (int xf = -filterCenter; xf < (int)filterMatrixSize - filterCenter; ++xf)
			{

				for (int yf = -filterCenter; yf < (int)filterMatrixSize - filterCenter; yf++)
				{

					if (x + xf < 0 || x + xf >= mx || y + yf < 0 || y + yf >= my)
						continue;
					int offSet = -mx * 4 * yf + 4 * xf;

					if (filterMatrixSize > 1)
					{
						r += *(ptr + offSet)* redFilterMatrix[xf + filterCenter][yf + filterCenter];
						g += *(ptr + offSet + 1) * greenFilterMatrix[xf + filterCenter][yf + filterCenter];
						b += *(ptr + offSet + 2)* blueFilterMatrix[xf + filterCenter][yf + filterCenter];
					}
					else
					{
						double color = *(ptr + offSet) * redFilterMatrix[xf + filterCenter][yf + filterCenter] +
							*(ptr + offSet + 1) * greenFilterMatrix[xf + filterCenter][yf + filterCenter] +
							*(ptr + offSet + 2)* blueFilterMatrix[xf + filterCenter][yf + filterCenter];
						r = color;
						g = color;
						b = color;

					}

				}
			}

			if (r > 255)
				r = 255;
			if (r < 0)
				r = 0;
			if (b > 255)
				b = 255;
			if (b < 0)
				b = 0;
			if (g > 255)
				g = 255;
			if (g < 0)
				g = 0;

			if (colorData == NULL)
			{
				*(tempRow + 2) = (unsigned char)r;
				*(tempRow + 1) = (unsigned char)g;
				*tempRow = (unsigned char)b;
			}
			else
			{
				int findedColorIndex = -1;
				for (int colorIndex = 0; colorIndex < 256; colorIndex++)
				{
					if (colorData[colorIndex].rgbRed == r
						&& colorData[colorIndex].rgbBlue == b &&
						colorData[colorIndex].rgbGreen == g)
					{
						findedColorIndex = colorIndex;
						for (int colorIndexNumber = 0; colorIndexNumber < 256; colorIndexNumber++)
						{
							if (usedColorIndexes[colorIndexNumber] == findedColorIndex)
								break;
							else if (usedColorIndexes[colorIndexNumber] == -1)
							{
								usedColorIndexes[colorIndexNumber] = findedColorIndex;
								break;
							}
						}
						break;
					}


				}
				int freeIndex = 0;
				while (findedColorIndex == -1)
				{
					for (int colorIndex = 0; colorIndex < 256; colorIndex++)
					{
						if (usedColorIndexes[colorIndex] == freeIndex)
							freeIndex++;
						else if (usedColorIndexes[colorIndex] == -1)
						{
							findedColorIndex = freeIndex;
							colorData[freeIndex].rgbRed = (unsigned char)r;
							colorData[freeIndex].rgbGreen = (unsigned char)g;
							colorData[freeIndex].rgbBlue = (unsigned char)b;
							usedColorIndexes[maxUsedIndexNumber++] = findedColorIndex;
							break;
						}
					}

				}
				*tempRow = (unsigned char)findedColorIndex;


			}
			tempRow += bytesCount;
			ptr += 4;

		}
	}
	return temp;
}



double*** getMeanFilter(unsigned char size)
{
	double** matrix = getMatrix(size);
	for (int x = 0; x < size; x++)
		for (int y = 0; y < size; y++)
			matrix[x][y] = 1.0 / (size * size);
	double*** colorMatrixis = new double**[3];
	colorMatrixis[0] = matrix;
	colorMatrixis[1] = matrix;
	colorMatrixis[2] = matrix;
	return colorMatrixis;
};


double*** getGrayFilter()
{
	double** redMatrix = getMatrix(1);
	redMatrix[0] = new double[1];
	redMatrix[0][0] = 0.2125;

	double** greenMatrix = getMatrix(1);
	greenMatrix[0] = new double[1];
	greenMatrix[0][0] = 0.7154;

	double** blueMatrix = getMatrix(1);
	blueMatrix[0] = new double[1];
	blueMatrix[0][0] = 0.0721;

	double*** colorMatrixis = new double**[3];
	colorMatrixis[0] = redMatrix;
	colorMatrixis[1] = greenMatrix;
	colorMatrixis[2] = blueMatrix;
	return colorMatrixis;
}

double*** getGaussFilter(unsigned char size)
{
	double** matrix = getMatrix(size);
	if (size == 3)
	{
		matrix[0][0] = 1.0 / 16;
		matrix[0][1] = 2.0 / 16;
		matrix[0][2] = 1.0 / 16;
		matrix[1][0] = 2.0 / 16;
		matrix[1][1] = 4.0 / 16;
		matrix[1][2] = 2.0 / 16;
		matrix[2][0] = 1.0 / 16;
		matrix[2][1] = 2.0 / 16;
		matrix[2][2] = 1.0 / 16;
	}
	if (size == 5)
	{
		matrix[0][0] = 0.000789;
		matrix[0][1] = 0.006581;
		matrix[0][2] = 0.013347;
		matrix[0][3] = 0.006581;
		matrix[0][4] = 0.000789;
		matrix[1][0] = 0.006581;
		matrix[1][1] = 0.054901;
		matrix[1][2] = 0.111345;
		matrix[1][3] = 0.054901;
		matrix[1][4] = 0.006581;
		matrix[2][0] = 0.013347;
		matrix[2][1] = 0.111345;
		matrix[2][2] = 0.225821;
		matrix[2][3] = 0.111345;
		matrix[2][4] = 0.013347;
		matrix[3][0] = 0.006581;
		matrix[3][1] = 0.054901;
		matrix[3][2] = 0.111345;
		matrix[3][3] = 0.054901;
		matrix[3][4] = 0.006581;
		matrix[4][0] = 0.000789;
		matrix[4][1] = 0.006581;
		matrix[4][2] = 0.013347;
		matrix[4][3] = 0.006581;
		matrix[4][4] = 0.000789;
	}
	double*** colorMatrixis = new double**[3];
	colorMatrixis[0] = matrix;
	colorMatrixis[1] = matrix;
	colorMatrixis[2] = matrix;
	return colorMatrixis;
}


double*** getSobelFilterX()
{
	double** matrix = getMatrix(3);

	matrix[0][0] = -1;
	matrix[0][1] = 0;
	matrix[0][2] = 1;
	matrix[1][0] = -2;
	matrix[1][1] = 0;
	matrix[1][2] = 2;
	matrix[2][0] = -1;
	matrix[2][1] = 0;
	matrix[2][2] = 1;
	double*** colorMatrixis = new double**[3];
	colorMatrixis[0] = matrix;
	colorMatrixis[1] = matrix;
	colorMatrixis[2] = matrix;
	return colorMatrixis;
}

double*** getSobelFilterY()
{
	double** matrix = getMatrix(3);

	matrix[0][0] = -1;
	matrix[0][1] = -2;
	matrix[0][2] = -1;
	matrix[1][0] = 0;
	matrix[1][1] = 0;
	matrix[1][2] = 0;
	matrix[2][0] = 1;
	matrix[2][1] = 2;
	matrix[2][2] = 1;
	double*** colorMatrixis = new double**[3];
	colorMatrixis[0] = matrix;
	colorMatrixis[1] = matrix;
	colorMatrixis[2] = matrix;
	return colorMatrixis;
}


int main(int argc, char **argv)
{
	printf("This prog takes .bmp file, applies filer and creates new .bmp file\nPlease choose a filter:\ngaussFilter\nmeanFilter\nsobelX\nsobelY\ngrayFilter\n");
	if (argc != 4)
	{
		printf("wrong args number");
		return 1;
	}
	char* inputFile = argv[1];
	char* filterName = argv[2];

	char* outputFile = argv[3];
	char fileData[1];
	BMPHEADER header;

	FILE* f;
	fopen_s(&f, inputFile, "rb");
	if (!f)
	{
		printf("Wrong input file");
		fclose(f);
		return 2;
	}
	unsigned int size;
	size = fread(&header, 1, sizeof(BMPHEADER), f);

	if (size != sizeof(BMPHEADER))
	{
		printf("Wrong BMP header");
		fclose(f);
		return 3;
	}

	BMPINFO info;

	size = fread(&info, 1, sizeof(BMPINFO), f);
	if (size != sizeof(BMPINFO))
	{
		printf("Wrong BMP info");
		fclose(f);
		return 4;
	}

	RGBQUAD* colorData = NULL;
	if (info.biBitCount <= 8)
	{
		colorData = new RGBQUAD[256];
		size = fread(colorData, 1, 1024, f);
		if (size != 1024)
		{
			printf("Wrong BMP pollete");
			fclose(f);
			return 8;
		}

	}

	int mx = info.biWidth;
	int my = info.biHeight;
	int bytesCount = info.biBitCount / 8;
	int mx4 = (bytesCount * mx + ((bytesCount*mx) % 4 ? 4 - (bytesCount*mx) % 4 : 0));

	unsigned char* temp = new unsigned char[mx4*my];

	size = fread(temp, 1, mx4*my, f);
	if (size != mx4 * my)
	{
		printf("Wrong BMP data");
		fclose(f);
		return 5;
	}

	fclose(f);
	unsigned int* imageData = new unsigned int[mx*my];
	unsigned char* ptr = (unsigned char*)imageData;
	unsigned int r, g, b;
	for (int y = my - 1; y >= 0; y--)
	{

		unsigned char* tempRow = temp + mx4 * y;

		for (int x = 0; x < mx; x++)
		{
			if (info.biBitCount <= 8)
			{
				r = colorData[*tempRow].rgbRed;
				b = colorData[*tempRow].rgbBlue;
				g = colorData[*tempRow].rgbGreen;
			}
			else
			{

				r = *(tempRow + 2);
				g = *(tempRow + 1);
				b = *tempRow;
			}
			*ptr++ = r;
			*ptr++ = g;
			*ptr++ = b;

			ptr++;
			tempRow += bytesCount;

		}
	}
	delete[] temp;

	double*** matrixis = NULL;
	int filterMatrixSize = 1;

	if (strcmp(filterName, "grayFilter") == 0)
	{
		printf("grayFilter applied");
		matrixis = getGrayFilter();
	}
	else if (strcmp(filterName, "sobelX") == 0)
	{
		printf("sobelX filter applied");
		filterMatrixSize = 3;
		matrixis = getSobelFilterX();
	}
	else if (strcmp(filterName, "gaussFilter") == 0)
	{
		printf("gaussFilter is chosen\nChoose matrix size:\n3(3x3) or 5(5x5)\n");
		scanf_s("%d", &filterMatrixSize);
		matrixis = getGaussFilter(filterMatrixSize);
		printf("gaussFilter size %dx%d applied", filterMatrixSize, filterMatrixSize);
	}
	else if (strcmp(filterName, "sobelY") == 0)
	{
		printf("sobelY applied");
		filterMatrixSize = 3;
		matrixis = getSobelFilterY();
	}
	else if (strcmp(filterName, "meanFilter") == 0)
	{
		printf("meanFilter is chosen\nEnter Matrix Size: ");
		scanf_s("%d", &filterMatrixSize);
		matrixis = getMeanFilter(filterMatrixSize);
		printf("meanFilter size %dx%d is applied", filterMatrixSize, filterMatrixSize);
	}
	else
	{
		printf("Wrong filter name");
		return 7;
	}
	temp = filterApply(imageData, bytesCount, mx, my, mx4, matrixis, filterMatrixSize, colorData);

	fopen_s(&f, outputFile, "wb");
	if (!f)
	{
		printf("Wrong input file");
		fclose(f);
		return 6;
	}

	fwrite(&header, 1, sizeof(BMPHEADER), f);

	fwrite(&info, 1, sizeof(BMPINFO), f);
	fwrite(temp, 1, mx4*my, f);
	fclose(f);
	delete[] temp;
	delete[] imageData;

	return 0;
}