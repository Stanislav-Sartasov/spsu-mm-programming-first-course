#include <stdio.h>
#include <string.h>
#include <fcntl.h>
#include <sys/stat.h>
#include "mman.h"

int comparator(const void* a, const void* b)
{
	const char* s1, * s2;
	s1 = *(char**)a;
	s2 = *(char**)b;
	int i = 0;
	while (s1[i] == s2[i] && s1[i] != '\n' && s2[i] != '\n')
		i++;
	return (s1[i] - s2[i]);
}

int main()
{
	int file_in = _open("input.txt", O_RDWR, 0);
	FILE* file_out = fopen("output.txt", "w");
	if (file_in == -1 || file_out == NULL)
	{
		printf("Opening error\n");
		return 1;
	}
	struct stat data;
	fstat(file_in, &data);
	int size_file = data.st_size;
	char* text = mmap(0, size_file, PROT_READ, MAP_PRIVATE, file_in, 0);
	if (text == MAP_FAILED)
	{
		printf("Mapping error\n");
		return 1;
	}
	int len_input_file = strlen(text);
	int count_of_string = 1;
	for (int i = 0; i < len_input_file; i++)
		if (text[i] == '\n')
			count_of_string++;
	char** strings = (char**)malloc(count_of_string * sizeof(char*));
	if (strings == NULL)
	{
		printf("Memory allocation error\n");
		return 1;
	}
	int curr_string = 0;
	int i = 0;
	while (text[i])
	{
		if (text[i] != '\n')
		{
			int j = 0;
			while (text[i + j] != '\n' && text[i + j] != '\0')
				j++;
            if (text[i + j] == '\0')
				j++;
			strings[curr_string] = malloc(j);
			snprintf(strings[curr_string], j, &text[i]);
			i += j;
			curr_string++;
		}
		i++;
	}
	qsort(strings, count_of_string, sizeof(char*), comparator);
	for (i = 0; i < count_of_string; i++)
	{
		int pos = 0;
		while (strings[i][pos] != '\0')
		{
			fputc(strings[i][pos], file_out);
			pos++;
		}
		fputc('\n', file_out);
	}
	fputc('\0', file_out);
	munmap(text, len_input_file);
	for (i = 0; i < count_of_string; i++)
		free(strings[i]);
	free(strings);
	_close(file_in);
	fclose(file_out);
	return 0;
}
