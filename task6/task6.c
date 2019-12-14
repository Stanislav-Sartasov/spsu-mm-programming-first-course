#include <stdio.h>
#include <fcntl.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <stdlib.h>
#include "mman.h"
#include <string.h>

int compare(const void* a, const void* b)
{
	const char** pa, ** pb;

	pa = (const char**)a;
	pb = (const char**)b;
	return (strcmp(*pa, *pb));
}



int main(int argc, char* argv[])
{
	
	int fdin, fdout;
	void* src;
	struct stat stat;
	
	if (argc != 3)
	{
		exit(-1);
	}	
	if ((fdin = open(argv[1], O_RDWR)) < 0)
		exit(-1);
	if ((fdout = open(argv[2], O_RDWR | O_CREAT | O_TRUNC, S_IWRITE)) < 0)
		exit(-1);
	
	fstat(fdin, &stat);
	int size = stat.st_size;
	int amount = size / sizeof(char);
	
	src = mmap(0, size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fdin, 0);
	char* arr = src;
	int n = 0;
	
	for (int i = 0; i < amount; i++)
	{
		if ((arr[i] == '\n'))
		{
			n += 1;
			
		}
	}

	char** strings = (char**)malloc(n * sizeof(char*));
	int k = 0;
	int j = 0;
	while (j < amount)
	{
		strings[k] = &arr[j];
		while (arr[j] != '\n')
		{
			
			j += 1;
		}
		j += 1;
		k += 1;
		
	}
	
	qsort(strings, n, sizeof(char*), compare);
	
	char end = '\n';
	for (int k = 0; k < n; k++)
	{
		for (int i = 0; *(strings[k] + i) != '\n'; i++)
		{
			write(fdout, (strings[k] + i), sizeof(char));
		}
		write(fdout, &end, sizeof(char));
	}

	free(strings);
	munmap(src, size);
	close(fdin);
	close(fdout);

	return 0;
}