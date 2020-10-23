#define _CRT_SECURE_NO_WARNINGS
#include <sys/stat.h>
#include "mman.c"
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
//#include <string.h>

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
    int flag = 0; int n = 0; char eol = 0;
    int fd_src;
    void* addr_src;
    struct stat file_stat;
    FILE* fd_dst;

    fd_src = _open(argv[1], O_RDWR);
    fd_dst = fopen(argv[2], "w+");

    if (fd_src < 0)
    {
        printf("Check the input file");
        exit(EXIT_FAILURE);
    }
    if (fd_dst < 0)
    {
        printf("Check the output file");
        exit(EXIT_FAILURE);
    }
    if (argc != 3)
    {
        printf("Check the number of arguments\n");
        exit(EXIT_FAILURE);
    }

    printf("The program sorts the lines in the file. Choose a sort order. To sort in ascending order, enter 1.\n"
        "To sort in descending order, enter 2.\n");
    
    while (n == 0)
    {
        scanf("%d%c", &flag, &eol);
        if ((flag == 1 || flag == 2) && eol == '\n')
        {
            n = 1;
        }
        else
        {
            printf("Error. Try again\n");
        }
    }
    
    fstat(fd_src, &file_stat);
    
    addr_src = mmap(0, file_stat.st_size, PROT_READ, MAP_SHARED, fd_src, 0);
    if (addr_src == MAP_FAILED)
    {
        printf("Error mmap function");
        exit(EXIT_FAILURE);
    }
    unsigned int number_count = file_stat.st_size;
    unsigned int number_line = 0;

    // подсчёт колличества строк
    for (int i = 0; i < number_count; i++)
    {
        const char a = ((char*)addr_src)[i];
        const char b = 10;
        if (!(a - b))
        {
            number_line++;
        }
    }

    //  подсчет колличества символов в каждой строке
    unsigned int* number_of_characters_per_line = (unsigned int*)malloc(sizeof(unsigned int) * (number_line + 1));
    int y = 0, x = 0;
    for (int i = 0; i < number_count; i++)
    {
        x++;
        const char a = ((char*)addr_src)[i];
        const char b = 10;
        if (!(a - b))
        {
            number_of_characters_per_line[y] = x;
            y++;
            x = 0;
        }
        else if (i == number_count - 1)
        {
            number_line = number_line + 1;
            number_of_characters_per_line[y] = x;
            x = 0;
        }
    }

    // создание массива
    char** mas = (char**)malloc(sizeof(char*) * number_line + 1);
    for (int i = 0; i < number_line; i++)
    {
        mas[i] = (char*)malloc(sizeof(char) * number_of_characters_per_line[i] + 1);
    }


    // копирование из addr_src в массив
    int len_str = 0;
    int num_str = 0;
    for (int i = 0; i < number_count; i++)
    {
        mas[num_str][len_str] = ((char*)(addr_src))[i];
        len_str++;
        if (len_str == number_of_characters_per_line[num_str])
        {
            mas[num_str][len_str] = '\0';
            len_str = 0;
            num_str++;
        }
    }

    // сортировка
    if (flag == 1)
    {
        qsort(mas, number_line, sizeof(char*), compare_ascending);
    }
    if (flag == 2)
    {
        qsort(mas, number_line, sizeof(char*), compare_decending);
    }

    for (int i = 0; i < number_line; i++)
    {
        fprintf(fd_dst, "%s", mas[i]);
    }

    _close(fd_src);
    fclose(fd_dst);
    free(number_of_characters_per_line);
    for (int i = 0; i < number_line; i++)
    {
        free(mas[i]);
    }
    free(mas);
    munmap(addr_src, file_stat.st_size);
    printf("successfully\n");
    return 0;
}
