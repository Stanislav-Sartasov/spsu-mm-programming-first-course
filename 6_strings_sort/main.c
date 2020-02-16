#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <fcntl.h>
#include "mman.h"
#include <string.h>

int compare(const void* ptr1, const void* ptr2)
{
	return strcmp(*(char**)ptr1, *(char**)ptr2);
}

int len(char* string)
{
	int counter = 1;
	for (int i = 2; string[i] != '\n'; i++)
		counter++;
	return counter;
}

int main(int argc, char* argv[])
{	
	int fIn, fOut;

	if (argc != 3)
	{
		printf("You entered a wrong number of parameters.");
		exit(-1);
	}
	if ((fIn = open(argv[1], O_RDWR)) == -1)
	{
		printf("Cannot open the input file.");
		exit(-1);
	}
	if ((fOut = open(argv[2], O_RDWR | O_CREAT | O_TRUNC, S_IWRITE)) == -1)
	{
		printf("Cannot open the output file.");
		exit(-1);
	}

	struct stat stat;
	fstat(fIn, &stat);

	char* map = mmap(0, stat.st_size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fIn, 0);
	if (map == MAP_FAILED)
	{
		printf("Failed calling mmap.");
		exit(-1);
	}

	int numOfChars = stat.st_size / sizeof(char);
	int lines = 0;
	for (int i = 0; i < numOfChars; i++)
	{
		if (map[i] == '\n')
			lines++;
	}

	char** strings = (char**)malloc(sizeof(char*) * (lines + 1));
	if (strings == NULL)
	{
		exit(-1);
	}

	int k = 0;
	int j = 0;
	while (j < numOfChars)
	{
		strings[k] = &map[j];
		while (map[j] != '\n' && j < numOfChars)
			j += 1;
		j += 1;
		k += 1;
	}
	qsort(strings, lines, sizeof(char*), compare);

	char endl = '\n';
	for (int i = 0; i < lines; i++)
	{
		write(fOut, strings[i], len(strings[i]));
		write(fOut, &endl, sizeof(char));
	}


	free(strings);
	munmap(map, stat.st_size);
	close(fIn);
	close(fOut);
	return 0;
}