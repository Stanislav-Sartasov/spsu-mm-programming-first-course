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
	return strcmp(*(char**)ptr2, *(char**)ptr1);
}

int len(char* string)
{
	int counter = 0;
	for (int i = 0; string[i] != '\n' && string[i] != '\r'; i++)
		counter++;
	return counter;
}

int spacesInFront(char* string)
{
	int i = 0;
	while (string[i] == ' ')
	{
		i++;
	}
	return i;
}

int spacesInBack(char* string, int len)
{
	int i = len - 1;
	while (i > 0 && string[i] == ' ')
	{
		i--;
	}
	return i;
}

char* deleteSpaces(char* copy, char* string, int front, int back)
{
	for (int i = 0; i < back - front + 1; i++)
		copy[i] = string[i + front];
	return copy;
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
			j++;
		j++;
		k++;
	}

	qsort(strings, lines, sizeof(char*), compare);
	char endl = '\n';
	for (int i = 0; i < lines; i++)
	{
		int front = spacesInFront(strings[i]);
		int back = spacesInBack(strings[i], len(strings[i]));
		char* copy = (char*)malloc(sizeof(char) * (back - front + 1));
		copy = deleteSpaces(copy, strings[i], front, back);
		write(fOut, copy, back - front + 1);
		write(fOut, &endl, sizeof(char));
		free(copy);
	}

	free(strings);
	munmap(map, stat.st_size);
	close(fIn);
	close(fOut);
	return 0;
}
