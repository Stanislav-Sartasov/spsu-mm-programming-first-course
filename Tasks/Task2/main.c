#include "stdio.h"
#include "sys/stat.h"
#include "sys/mman.h"
#include "stdlib.h"
#include "fcntl.h"
#include "string.h"

void str_strip(char* init_str, int n, char*** str_array, int *str_array_size)
{
	*str_array = calloc(1, sizeof(char **));
	int str_count = 1;

	(*str_array)[0] = &init_str[0];

	int data_length = n;

	int i = 0;
	while (i < data_length - 1)
	{
		if (init_str[i] == '\n' && i != data_length)
		{
			i++;
			*str_array = realloc(*str_array, (str_count + 1) * sizeof(char **));
			(*str_array)[str_count] = &init_str[i];
			str_count++;
		}
		i++;
	}
	*str_array_size = str_count;
}

int cmp_str(const void *a, const void *b)
{
	const char *first_str = *(const char **) a;
	const char *second_str = *(const char **) b;

	while (*first_str != '\n' && *second_str != '\n' && *first_str == *second_str)
	{
		first_str++;
		second_str++;
	}

	if (*first_str == *second_str)
	{
		return 0;
	}
	return *first_str - *second_str;
}

void print_array(char** array, int n)
{
	for (int i = 0; i < n; i++)
	{
		int j = 0;
		do
		{
			printf("%c", array[i][j]);
			j++;
		} while (array[i][j] != '\n');
		printf("\n");
	}
}

void write_to_file(FILE* file, char** array, int n)
{
	for (int i = 0; i < n; i++)
	{
		int j = 0;
		do
		{
			fwrite(&array[i][j], sizeof(char), 1, file);
			j++;
		} while (array[i][j] != '\n');
		fwrite("\n", sizeof(char), 1, file);
	}
}

int main(int argv, char* argc[])
{
	if (argv != 3)
	{
		printf("Usage: ./start <input_filename> <output_filename>\n");
		return 0;
	}

	int fd;
	struct stat file_data;

	fd = open(argc[1], O_RDWR);

	if (fd == -1)
	{
		printf("Unable to open input file\n");
		return 0;
	}

	FILE* out = fopen(argc[2], "w");

	if (out == NULL)
	{
		printf("Unable to open output file\n");
		return 0;
	}

	fstat(fd, &file_data);

	int file_size = file_data.st_size;

	char* mapped = mmap(0, file_size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fd, 0);

	char** str_array;
	int str_array_size;

	str_strip(mapped, file_size, &str_array, &str_array_size);

	qsort(str_array, str_array_size, sizeof(char *), cmp_str);

	write_to_file(out, str_array, str_array_size);

	fclose(out);

	free(str_array);

	munmap(mapped, file_size);

	return 0;
}
