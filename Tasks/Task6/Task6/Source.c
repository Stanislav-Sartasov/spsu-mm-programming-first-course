#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <malloc.h>
#include <sys/stat.h>
#include <sys/types.h>
#include "mman.h"
#include <string.h>

int cmp(const void* p1, const void* p2)
{
	return strcmp(*(char**)p1, *(char**)p2);
}

void checkInput(int argc, char* argv[], int fileIn, int fileOut)
{
	if (argc != 3)
	{
		printf("Check the number of input parameter");
		exit(-1);
	}
	else if ((fileIn = open(argv[1], O_RDONLY)) == -1)
	{
		printf("Input file failed to open");
		exit(-1);
	}
	else if ((fileOut = open(argv[2], O_RDWR | O_CREAT | O_TRUNC, S_IWRITE)) == -1)
	{
		printf("Output file failed to open");
		exit(-1);
	}
}

void print(int fileOut, char* string, int size)
{
	char endl = '\n\0';
	for (int i = 0; i < size; i++)
		write(fileOut, &string[i], sizeof(char));
	write(fileOut, &endl, sizeof(char));
}

int main(int argc, char* argv[])
{
	int fileIn, fileOut;

	checkInput(argc, argv, &fileIn, &fileOut);

	fileIn = open(argv[1], O_RDWR);
	fileOut = open(argv[2], O_RDWR | O_CREAT | O_TRUNC, S_IWRITE);

	struct stat stat;
	fstat(fileIn, &stat);

	char* map = mmap(0, stat.st_size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fileIn, 0);
	if (map == MAP_FAILED)
	{
		printf("Error when calling the mmap function");
		exit(-1);
	}

	int countLines = 0;
	int maxLengthofString = 0;
	int currentCounter = 0;

	for (int i = 0; i < stat.st_size / sizeof(char); i++)
		if (map[i] == '\n')
		{
			countLines++;
			maxLengthofString = i - currentCounter - 1 > maxLengthofString ? i - currentCounter - 1 : maxLengthofString;
			currentCounter = i + 1;
		}

	char** strings = (char**)malloc(countLines * sizeof(char*));
	int* lengthOfStrings = (int*)malloc(countLines * sizeof(int));

	for (int i = 0, counter = 0, previousCounter = 0; i < stat.st_size / sizeof(char);)
	{

		while (map[i] != '\n' && map[i] != '\r')
			i += 1;

		if (counter == 0)
			lengthOfStrings[counter] = i;
		else
			lengthOfStrings[counter] = i - previousCounter;

		previousCounter += lengthOfStrings[counter] + 2;

		char* currentLine = (char*)malloc(lengthOfStrings[counter] * sizeof(char));

		snprintf(currentLine, lengthOfStrings[counter] + 1, &map[i - lengthOfStrings[counter]]);
		strings[counter] = currentLine;

		if (map[i] == '\r')
			i++;
		i++;
		counter++;
	}

	qsort(strings, countLines, sizeof(char*), cmp);

	for (int i = 0; i < countLines; i++)
		print(fileOut, strings[i], strlen(strings[i]));

	free(strings);
	munmap(map, stat.st_size);
	close(fileIn);
	close(fileOut);
	return 0;
}