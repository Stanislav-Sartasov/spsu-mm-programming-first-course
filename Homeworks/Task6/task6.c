#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include "mman.h"
#include <fcntl.h>
#include <string.h>
#include <sys/stat.h>
#include <malloc.h>
#include <io.h>

// calculate the line's count in text and get all lines in text
int get_all_lines(char* text, char*** lines)
{
	int text_len, line_count = 0;
	int pos = 0, i;

	if (lines == 0)
	{
		return -1;
	}
	*lines = 0;

	text_len = strlen(text);

	// calculate line's count in text
	for (i = 0 ; i < text_len ; i++)
	{
		if (text[i] == '\n')
		{
			line_count++;
			text[i] = 0;
		}
	}

	if (line_count > 0)
	{
		// malloc line buffer
		*lines = (char**)malloc(line_count * sizeof(char*));
		if (*lines == 0)
		{
			return -1;
		}

		// get all lines in text
		line_count = 0;
		for (i = 0 ; i < text_len ; i = i + strlen(&text[i]) + 1)
		{
			(*lines)[line_count] = &text[i];
			line_count = line_count + 1;
		}

	}

	return line_count;
}

int compare_callback(const void* a1, const void* a2)
{
	const char* e1;
	const char* e2;

	e1 = *(char**)a1;
	e2 = *(char**)a2;
	return strcmp(e1, e2);
}

int main(int argc, char* argv[])
{
	int fin, fout;
	struct stat input_file_info;
	int input_file_size;
	char* input_mapped;
	int input_mapped_len, i, line_count;
	char** lines;
	char new_line_char = '\n';

	if (argc != 3)
	{
		printf("task6.exe input.txt output.txt\n");
		return -1;
	}

	// open the input file
	fin = _open(argv[1], O_RDWR, 0);
	fout = _open(argv[2], O_WRONLY | O_CREAT | O_TRUNC, S_IWRITE);
	if (-1 == fin)
	{
		printf("Can't open the input file.\n");
		return -1;
	}
	if (-1 == fout)
	{
		printf("Can't create the output file.\n");
		return -1;
	}

	fstat(fin, &input_file_info);
	input_file_size = input_file_info.st_size;
	input_mapped = (char*)mmap(0, input_file_size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fin, 0);
	if (input_mapped == MAP_FAILED)
	{
		printf("Can't map the input file.\n");
		return -1;
	}

	line_count = get_all_lines(input_mapped, &lines);
	if (line_count == -1)
	{
		printf("Can't get all lines in input file.\n");
		return -1;
	}

	qsort(lines, line_count, sizeof(char*), compare_callback);

	// write the output file
	for (i = 0 ; i < line_count ; i++)
	{
		_write(fout, lines[i], strlen(lines[i]));
		_write(fout, &new_line_char, 1);
	}

	_close(fin);
	_close(fout);
	free(lines);

	return 0;
}

