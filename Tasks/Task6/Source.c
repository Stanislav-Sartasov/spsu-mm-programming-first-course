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
	char* s1 = *(char**)p1;
	char* s2 = *(char**)p2;
	for (int i = 0; s1[i] != '\r' && s2[i] != '\r' && s1[i] != '\n' && s2[i] != '\n'; i++)
	{		
		if (s1[i] < s2[i])
			return -1;
		if (s1[i] > s2[i])
			return 1;
	}
	return 0;
}

void checkInput(int argc, char* argv[], int fileIn, int fileOut)
{
	if (argc != 3)
	{
		printf("Check the number of input parameter");
		exit(-1);
	}
	else if ((fileIn = open(argv[1], O_RDWR)) == -1)
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

int lengthString(char* string)
{
	int result = 1;
	for (int i = 2; string[i] != '\n'; i++)
		result++;
	return result;
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

	for (int i = 0; i < stat.st_size / sizeof(char); i++)
		if (map[i] == '\n')
			countLines++;

	char** strings = (char**)malloc((countLines + 1)  * sizeof(char*));

	strings[0] = &map[0];
	for (int i = 0, counter = 1; i < stat.st_size / sizeof(char);)
	{
		while (map[i] != '\n' && map[i] != '\r')
			i += 1;

		strings[counter] = &map[i + 2];

		if (map[i] == '\r')
			i++;
		i++;
		counter++;
	}
	qsort(strings, countLines, sizeof(char*), cmp);

	char endl = '\n';
	for (int i = 0; i < countLines; i++)
	{
		write(fileOut, strings[i], lengthString(strings[i]));
		write(fileOut, &endl, sizeof(char));
	}

	free(strings);
	munmap(map, stat.st_size);
	close(fileIn);
	close(fileOut);
	return 0;
}