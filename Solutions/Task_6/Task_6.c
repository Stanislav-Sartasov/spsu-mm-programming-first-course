#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include "mman.h"
#include <fcntl.h>
#include <sys/stat.h>

char** strgs(char** str_in, int* len)
{
	char* str = *str_in;
	char** arr = (char**)malloc(sizeof(char*));
	int ln = 0;
	for (int i = 0; str[i] != '\0' && str[i] != '\n';)
	{
		if (str[i] != ' ' && str[i] != '\t')
		{
			if (str[i] == '"')
			{
				++i;
				ln++;
				arr = (char**)realloc(arr, sizeof(char*) * ln);
				arr[ln - 1] = str + i;
				for (i; str[i] != '"' && str[i] != '\0' && str[i] != '\n'; i++);
				if (str[i] == '\0' || str[i] == '\n')
				{
					*len = 0;
					free(arr);
					return 0;
				}
				str[i] = '\0';
				++i;
			}
			else
			{
				ln++;
				arr = (char**)realloc(arr, sizeof(char*) * ln);
				arr[ln - 1] = str + i;
				for (; str[i] != '\0' && str[i] != '\n' && str[i] != ' ' && str[i] != '\t'; i++);
				if (str[i] != '\0')
				{
					str[i] = '\0';
					++i;
				}
			}
		}
		else
			++i;
	}
	*len = ln;
	return arr;
}

int compare(const void** arg_1, const void** arg_2)
{
	char* in_1 = (char*)(*arg_1);
	char* in_2 = (char*)(*arg_2);
	for (int i = 0; (in_1[i] != '\0' && in_1[i] != '\n' && in_1[i] != '\r') || (in_2[i] != '\0' && in_2[i] != '\n' && in_2[i] != '\r'); i++)
		if ((in_1[i] == '\0' || in_1[i] == '\n' || in_1[i] == '\r') && (in_2[i] != '\0' && in_2[i] != '\n' && in_2[i] != '\r'))
			return -1;
		else if ((in_2[i] == '\0' || in_2[i] == '\n' || in_2[i] == '\r') && (in_1[i] != '\0' && in_1[i] != '\n' && in_1[i] != '\r'))
			return 1;
		else if (in_1[i] > in_2[i])
			return 1;
		else if (in_1[i] < in_2[i])
			return -1;
	return 0;
}

int main(int argc, char** argv)
{
	char* str = (char*)malloc(sizeof(char) * 3);
	str[0] = 'a';
	str[1] = ' ';

	if (argc == 1)
	{
		printf("\tinputing format: <input_file_name> <output_file_name>\n > ");
		for (;;)
		{
			for (int i = 2;; i++)
			{
				str[i] = getchar();
				if (str[i] == '\n')
				{
					str[i] = '\0';
					break;
				}
				str = (char*)realloc(str, sizeof(char) * (i + 2));
			}
			argv = strgs(&str, &argc);
			if (argc != 3)
			{
				printf(" > invalid input\n > ");
				str[1] = ' ';
			}
			else
				break;
		}
	}

	if (argc != 3)
	{
		printf(" > invalid input\n\tinputing format: <input_file_name> <output_file_name>");
		return -1;
	}

	int file_in = _open(argv[1], O_RDWR, 0);
	if (file_in == -1)
	{
		printf(" > invalid inputing file name");
		return -1;
	}
	struct stat file_in_stat;
	fstat(file_in, &file_in_stat);

	int file_in_size = file_in_stat.st_size;
	char* file_in_mmap = mmap(0, file_in_size, PROT_READ | PROT_WRITE, MAP_PRIVATE, file_in, 0);
	if (file_in_mmap == MAP_FAILED)
	{
		printf(" > mmap fail");
		_close(file_in);
		return -1;
	}

	FILE* file_out = fopen(argv[2], "w");	

	int arr_size = 0;

	for (int i = 0; i <= file_in_size; i++)
		if (file_in_mmap[i] == '\n' || (file_in_mmap[i] == '\0' && file_in_mmap[i - 1] != '\n'))
			++arr_size;

	char** arr_str = (char**)malloc(sizeof(char*) * arr_size);
	arr_str[0] = file_in_mmap;
	arr_size = 1;

	for (int i = 0; i < file_in_size; i++)
		if (file_in_mmap[i] == '\n' && i + 1 < file_in_size)
			arr_str[arr_size++] = file_in_mmap + i + 1;

	qsort(arr_str, arr_size, sizeof(char*), compare);

	for (int i = 0; i < arr_size; i++)
	{
		for (int j = 0; arr_str[i][j] != '\0' && arr_str[i][j] != '\n' && arr_str[i][j] != '\r'; j++)
			fputc(arr_str[i][j], file_out);
		fputc('\n', file_out);
	}

	for (int j = 0; arr_str[0][j] != '\0' && arr_str[0][j] != '\n' && arr_str[0][j] != '\r'; j++)
		printf("%c", arr_str[0][j]);

	munmap(file_in_mmap, file_in_size);

	_close(file_in);
	fclose(file_out);
	return 0;
}