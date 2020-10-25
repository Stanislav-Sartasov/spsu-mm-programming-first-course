#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include "mman.h"
#include <sys/stat.h>
#include <sys/types.h>
#include <stdlib.h>
#include <fcntl.h>
#include <string.h>
#include <io.h>


char* trim_left(char* string)
{
	while ((*string == ' ') || (*string == '\t'))
	{
		string++;
	}
	return string;
}

int trim_right(char* string)
{
	char* start = string;
	char* frs = string;  // указатель на последний не пробел
	while (*string != '\n')
	{
		if ((*string != ' ') && (*string != '\t'))
		{
			frs = string;
		}
		string++;
	}
	int count = frs - start + 1;
	return count;
}

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

int max_char(char* text)
{
	int count = 0;
	while (*text != '\n')
	{
		count++;
		text++;
	}
	return ++count;
}

int cmp(const void* p1, const void* p2)
{
	const char* s1, * s2;

	s1 = *(char**)p1;
	s2 = *(char**)p2;
	return -strcmp(s1, s2);
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

	char** text = (char**)malloc(max_lines(src, statbuf.st_size) * sizeof(char*));
	text[0] = src;
	char* current_char = src + 1;
	int lines = 0;

	while (current_char != src + statbuf.st_size)
	{
		if ((*(current_char - 1) == '\n') && *current_char == '\n')
		{
			current_char++;
			continue;
		}
		if (*(current_char - 1) == '\n')
		{
			text[++lines] = current_char;
		}
		current_char++;
	}

	for (int i = 0; i < lines; i++)
	{
		text[i] = trim_left(text[i]);
	}

	qsort(text, lines, sizeof(char*), cmp);
	int p = 10;
	for (int i = 0; i < lines; i++)
	{
		_write(fdout, text[i],trim_right(text[i])/*max_char(text[i])*/);
		_write(fdout, text[i] + max_char(text[i]) - 1, 1);
	}

	munmap(src, statbuf.st_size);
	_close(fdin);
	_close(fdout);
	free(text);
	return 0;
}