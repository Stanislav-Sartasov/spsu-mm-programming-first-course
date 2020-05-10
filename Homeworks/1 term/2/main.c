#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <string.h>
#include <fcntl.h>
#include <sys/stat.h>
#include "mmf.h"

void copyAdress(char** str, char* input)
{
	int i = 0, jInp = 0, jStr = 0;
	while (input[i])
	{
		if (input[i] == '\n')
		{
			str[jStr] = &input[jInp];
			jInp = i + 1;
			jStr++;
		}
		i++;
	}
}

int inputCheck(int argc, char* argv[], int fileIn, FILE* fileOut)
{
	short fl = 0;
	if (fileIn == -1)
	{
		printf("The input file open failed\n");
		fl = 1;
	}
	if (fileOut == NULL)
	{
		printf("The output file create failed\n");
		fl = 1;
	}
	if (fl == 1)
	{
		printf("Please, try again\n");
		exit(0);
	}
}

int comp(const void* s1, const void* s2)
{
	const char* a = *(char**)s1;
	const char* b = *(char**)s2;
	return strcmp(a, b);
}

int main(int argc, char* argv[])
{
	if (argc != 3)
	{
		printf("Invalid number of parameters");
		return 1;
	}

	int fileIn = _open(argv[1], O_RDWR, 0);
	FILE* fileOut = fopen(argv[2], "a");

	inputCheck(argc, argv, fileIn, fileOut);

	struct stat inf;
	fstat(fileIn, &inf);
	int size = inf.st_size;

	char* input = mmap(0, size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fileIn, 0);
	if (input == MAP_FAILED)
	{
		printf("The mmap function calling failed\nPlease, try again\n");
		exit(0);
	}

	int inputLength = strlen(input);
	int numStr = 0;
	for (int i = 0; i < inputLength; i++)
	{
		if (input[i] == '\n')
		{
			numStr++;
		}
	}

	char** str = (char**)malloc(numStr * sizeof(char*));
	if (str == NULL)
	{
		printf("The memory allocation failed\nPlease, try again\n");
		exit(0);
	}

	copyAdress(str, input);

	qsort(str, numStr, sizeof(char*), comp);

	for (int i = 0; i < numStr; i++)
	{
		int j = 0;
		if (str[i])
		{
			while (str[i][j] != '\n')
			{
				fputc(str[i][j], fileOut);
				j++;
			}
		}
		fputc('\n', fileOut);
	}

	_close(fileIn);
	fclose(fileOut);
	free(str);
	munmap(input, inputLength);

	printf("The file sorted\n");
	return 0;
}