#include "stdio.h"
#include "sys/stat.h"
#include "sys/mman.h"
#include "stdlib.h"
#include "fcntl.h"
#include "string.h"

void str_strip(char* init_str, int n, char*** str_array, int *str_array_size)
{
	*str_array = calloc(1, sizeof(char*));

	char* sub_str = strtok(init_str, "\n");

	int i = 0;
	while (sub_str != NULL)
	{
		*str_array = realloc(*str_array, (i + 1) * sizeof(char*));
		(*str_array)[i] = sub_str;
		i++;

		sub_str = strtok(NULL, "\n");
	}

	*str_array_size = i;
}

int cmp_str(const void *a, const void *b)
{
	const char **first_str = (const char **) a;
	const char **second_str = (const char **) b;

	return strcmp(*first_str, *second_str);
}

void print_array(char** array, int n)
{
	for (int i = 0; i < n; i++)
	{
		printf("%s\n", array[i]);
	}
	printf("\n");
}

void write_to_file(char** array, int n)
{
	FILE *out = fopen("out.txt", "w");

	for (int i = 0; i < n; i++)
	{
		fwrite(array[i], strlen(array[i]), 1, out);
		fwrite("\n", sizeof(char), 1, out);
	}
	fclose(out);
}

int main()
{
	int fd;
	struct stat file_data;

	char* file_name = "text.txt";

	fd = open(file_name, O_RDWR);

	fstat(fd, &file_data);

	int file_size = file_data.st_size;

	char* mapped = mmap(0, file_size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fd, 0);

	char** str_array;
	int str_array_size;

	str_strip(mapped, file_size, &str_array, &str_array_size);

	qsort(str_array, str_array_size, sizeof(char *), cmp_str);

	write_to_file(str_array, str_array_size);

	free(str_array);

	munmap(mapped, file_size);

	return 0;
}
