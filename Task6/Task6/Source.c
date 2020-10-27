#define _CRT_SECURE_NO_WARNINGS
#include <sys/stat.h>
#include "mman.c"
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int check_in_space(char* str)
{
	int count = 0;
	int flag = 0;
	while (str[count] != '\n')
	{
		if (str[count] != ' ')
			flag = 1;
		count++;
	}

	if (flag)
		return 1;
	else
		return 0;
}

int chars_count(char* str)
{
	int count = 0;
	while (str[count] != '\n')
	{
		count++;
	}
	return count;
}

int compare_ascending(const void* ptr1, const void* ptr2)
{
	return strcmp(*(char**)ptr1, *(char**)ptr2);
}

int compare_decending(const void* ptr1, const void* ptr2)
{
	return strcmp(*(char**)ptr2, *(char**)ptr1);
}

int main(int argc, char* argv[])
{
	char* flag;
	int line = 0;
	int fd_src;
	char* addr_src;
	struct stat file_stat;

	if (argc != 4)
	{
		printf("Check the number of arguments\n");
		exit(EXIT_FAILURE);
	}

	fd_src = _open(argv[1], O_RDWR);

	if (fd_src < 0)
	{
		printf("Check the input file");
		exit(EXIT_FAILURE);
	}

	if (!strcmp(argv[3], "ascending"))
	{
		flag = "ascending";
	}
	else if (!strcmp(argv[3], "decending"))
	{
		flag = "decending";
	}
	else
	{
		printf("Sorting direction not selected");
		exit(EXIT_FAILURE);
	}



	fstat(fd_src, &file_stat);

	addr_src = mmap(0, file_stat.st_size, PROT_READ, MAP_SHARED, fd_src, 0);
	if (addr_src == MAP_FAILED)
	{
		printf("Error mmap function");
		exit(EXIT_FAILURE);
	}
	unsigned int number_character = file_stat.st_size;
	unsigned int number_line = 0;

	for (int i = 0; i < number_character; i++)
	{
		const char a = ((char*)addr_src)[i];
		const char b = 10;
		if (!(a - b))
		{
			number_line++;
		}
	}

	char** mas = (char**)malloc((number_line + 1) * sizeof(char*));

	int j = 0;
	while (j < number_character)
	{
		mas[line] = &addr_src[j];
		while (addr_src[j] != '\n' && j < number_character)
		{
			j++;
		}
		j++;
		line++;
	}

	if (flag == "ascending")
	{
		qsort(mas, line, sizeof(char*), compare_ascending);
	}
	if (flag == "decending")
	{
		qsort(mas, line, sizeof(char*), compare_decending);
	}

	int fd_dst = _open(argv[2], O_RDWR | O_CREAT);
	if (fd_dst < 0)
	{
		printf("Check the output file");
		exit(EXIT_FAILURE);
	}

	for (int i = 0; i < line; i++)
	{
		if (check_in_space(mas[i]))
		{
			_write(fd_dst, mas[i], chars_count(mas[i]) + 1);
		}
	}

	munmap(addr_src, file_stat.st_size);
	_close(fd_dst);
	_close(fd_src);
	printf("Successfully");
	return 0;
}



