#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include "mman.h"
#include <sys/stat.h>
#include <sys/types.h>
#include <stdlib.h>
#include <fcntl.h>
#include <string.h>
#include <io.h>


int max_lines(char* src, int size)
{
	int count = 0;
	for (int i = 0; i < size; i++)
	{
		if (src[i] == '\n')
			count++;
	}
	return count;
}

int lenght_string(char* src, int size)
{
	int max_lenght = 0;
	int count = 0;
	for (int i = 0; i < size; i++)
	{
		if (src[i] != '\n')
		{
			count++;
		}
		else if (count > max_lenght)
		{
			max_lenght = count;
			count = 0;
		}
		else
			count = 0;
	}
	return max_lenght;
}

int cmp(const void* p1, const void* p2)	
{
	const char* s1, * s2;

	s1 = *(char**)p1;
	s2 = *(char**)p2;
	return strcmp(s1, s2);
}

int main(int argc, char* argv[])
{
	int fdin, fdout;
	char* src;
	struct stat statbuf;

	if (argc != 3)
	{
		printf("Wrong input");
		exit(-1);
	}

	if ((fdin = _open(argv[1], O_RDWR)) < 0)
	{
		printf("Unable to open %s for reading", argv[1]);
		exit(-1);
	}
		

	if ((fdout = _open(argv[2], O_RDWR | O_CREAT | O_TRUNC, S_IWRITE)) < 0)
	{
		printf("Unable to open %s for writing", argv[2]);
		exit(-1);
	}
	

	if (fstat(fdin, &statbuf) < 0)
	{
		printf("Fstat error");
		exit(-1);
	}

	if ((src = mmap(0, statbuf.st_size, PROT_READ | PROT_WRITE, MAP_SHARED, fdin, 0)) == MAP_FAILED)
	{
		printf("Error in calling mmap function");
		exit(-1);
	}
	
	int lines = max_lines(src, statbuf.st_size);

	char** text = (char**)malloc(lines * sizeof(char*));

	int max_lenght = lenght_string(src, statbuf.st_size);
	char* strings = (char*)malloc(max_lenght * sizeof(char));
	int k = 0;

	int j, i, ctr;
	int y = 0;

	for (i = 0; i < statbuf.st_size; i++)
	{
		j = 0;

		while (k < statbuf.st_size)
		{
			if (src[k] == '\n')
			{
				strings[j] = '\n';
				j++;
				k++;
				break;
			}

			strings[j] = src[k];
			j++;
			k++;
		}
		text[i] = _strdup(strings);
		if (k == statbuf.st_size)
		{
			break;
		}
	}
	ctr = i;
	
	qsort(text, ctr, sizeof(char*), cmp);

	for (i = 0; i < ctr; i++)
	{
		char* block = text[i];

		while (*block != '\n')
		{
			y++;
			block++;
		}

		if (*block == '\n')
		{
			y++;
		}
	}

	char* sort_src = (char**)malloc(y * sizeof(char*));
	k = 0;

	for (i = 0; i < ctr; i++)
	{
		char* block = text[i];

		while (*block != '\n')
		{
			sort_src[k] = *block;
			k++;
			block++;
		}

		if (*block == '\n')
		{
			sort_src[k] = '\n';
			k++;
		}

		free(text[i]);
	}

	_write(fdout, sort_src, y);
	munmap(src, statbuf.st_size);
	_close(fdin);
	_close(fdout);
	free(sort_src);
	free(text);
	return 0;
}