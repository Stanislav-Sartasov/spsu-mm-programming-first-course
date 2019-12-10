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
        *str_array = realloc(*str_array, (i+1) * sizeof(char*));
        (*str_array)[i] = sub_str;
        i++;

        sub_str = strtok(NULL, "\n");
    }

    *str_array_size = i;
}

void sort_str_array(char** str_array, int n)
{
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n-1-i; j++)
        {
            if (strcmp(str_array[j], str_array[j+1]) > 0)
            {
                char* buf = str_array[j];
                str_array[j] = str_array[j+1];
                str_array[j+1] = buf;
            }
        }
    }
}

void print_array(char** array, int n)
{
    for (int i = 0; i < n; i++)
    {
        printf("%s\n", array[i]);
    }
    printf("\n");
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

    print_array(str_array, str_array_size);

    sort_str_array(str_array, str_array_size);

    print_array(str_array, str_array_size);

    free(str_array);

    munmap(mapped, file_size);

    return 0;
}

