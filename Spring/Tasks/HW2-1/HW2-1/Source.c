#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <malloc.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <string.h>
#include "mman.h"

int cmp(const void* p1, const void* p2)
{
    char* s1, * s2;

    s1 = *(char**)p1;
    s2 = *(char**)p2;
    while (*s1 == ' ') s1++;
    while (*s2 == ' ') s2++;
    return strcmp(s1, s2);
}

int main(int argc, char* argv[])
{
    int fin, fout, i, stringNumber = 1;
    struct stat statbuf;
    char* map;
    char** strings;

    if (argc != 3)
    {
        printf("insufficiently passed parameters");
        return 1;
    }

    if ((fin = _open(argv[1], O_RDWR)) == -1)
    {
        printf("unable to open file");
        return 1;
    }

    if ((fout = _open(argv[2], O_RDWR | O_CREAT | O_TRUNC, S_IWRITE)) == -1)
    {
        printf("unable create for recording");
        return 1;
    }

    fstat(fin, &statbuf);

    map = mmap(0, statbuf.st_size, PROT_READ | PROT_WRITE, MAP_SHARED, fin, 0);

    if (map == MAP_FAILED)
    {
        printf("map error");
        return 1;
    }

    for (i = 0; i < statbuf.st_size; i++)
    {
        if (map[i] == '\n' || map[i] == '\r\n')
            stringNumber++;
    }

    strings = (char**)malloc(sizeof(char*) * stringNumber);

    strings[0] = &map[0];

    int j = 0;

    for (i = 0; i < statbuf.st_size; i++)
    {
        if (map[i] == '\n' || map[i] == '\r\n')
        {
            j++;
            strings[j] = &map[i + 1];
        }
    }

    qsort(strings, stringNumber, sizeof(char*), cmp);

    char slashN = '\n';

    int len;

    for (i = 0; i < stringNumber; i++)
    {
        len = 0;
        while (*(strings[i] + len) != '\r' && *(strings[i] + len) != '\n' && *(strings[i] + len) != '\0')
        {
            if (&(*(strings[i] + len)) == &map[statbuf.st_size - 1])
            {
                len++;
                break;
            }
            len++;
        }
        write(fout, strings[i], len);
        write(fout, &slashN, 1);
    }
    free(strings);
    munmap(map, statbuf.st_size);
    close(fin);
    close(fout);
    system("pause");
    return 0;
}