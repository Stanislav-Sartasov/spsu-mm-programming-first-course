#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <malloc.h>
#include <sys/stat.h>
#include <sys/types.h>
#include "sys/mman.h"
#include <string.h>

//void qsortt(char **, long long len);

int strcmpr(const void *ptr1, const void *ptr2);
unsigned long long lengthstr(char *str);

int main(int argc, char* argv[])
{
    int fIn, fOut;
    if (argc != 3)
    {
        printf("supposed to be 3 parametrs");
        return 0;
    }
    else
        if ((fIn = open(argv[1], O_RDWR)) == -1)
        {
            printf("can not open file");
            return 0;
        }
        else
            if ((fOut = open(argv[2], O_RDWR | O_CREAT | O_TRUNC, S_IWRITE)) == -1)
            {
                printf("cosyak with out file");
                return 0;
            }

//    char str[10];
//    scanf("%s", str);
//    printf("-%s-\n", str);
//    fIn = open(str, O_RDWR);
//    fOut = open("out.txt", O_RDWR | O_CREAT | O_TRUNC, S_IWRITE);
    fIn = open(argv[1], O_RDWR);
    fOut = open(argv[2], O_RDWR | O_CREAT | O_TRUNC, S_IWRITE);

    struct stat stat;
    fstat(fIn, &stat);

    char* map = mmap(0, stat.st_size, PROT_READ | PROT_WRITE, MAP_PRIVATE, fIn, 0);
    if (map == MAP_FAILED)
    {
        printf("Error when calling the mmap function");
        return 0;
    }
    //printf("scaning\n");
    long long numberStr = 0, maxLen = 0, len = 0;
    for (int i = 0; i < stat.st_size; ++i)
    {
        ++len;
        if (map[i] == '\n')
        {
            ++numberStr;
            maxLen = maxLen < len ? len : maxLen;
            len = 0;
        }
        else
            if (i + 1 == stat.st_size)
            {
                ++numberStr;
                maxLen = maxLen < len ? len : maxLen;
                len = 0;
            }
    }

    char **strs = (char **)malloc(sizeof (char*) * (unsigned long long)numberStr);

    strs[0] = strtok(map, "\n");
    char *prev = map + strlen(strs[0]);
    *prev = '\n';
    for (int i = 1; i < numberStr; ++i)
    {
        strs[i] = strtok(prev + 1, "\n");
        prev = prev + 1 + strlen(strs[i]);
        *prev = '\n';
    }


    //qsortt(strs, numberStr);
    qsort(strs, (size_t)numberStr, sizeof(char*), strcmpr);

    char endl = '\n';
    for(int i = 0; i < numberStr; ++i)
    {
        write(fOut, strs[i], lengthstr(strs[i]));
        write(fOut, &endl, 1);
    }

    munmap(map, stat.st_size);
    close(fIn);
    close(fOut);

    printf("Sorted)");
    return 0;
}

unsigned long long lengthstr(char *str)
{
    unsigned long long i = 0;
    while ((str[i] != '\n') && (str[i] != '\r') && (str[i] != '\0'))
        ++i;
    return i;
}

int strcmpr(const void *ptr1, const void *ptr2)
{
    char* str1 = *(char**)ptr1;
    char* str2 = *(char**)ptr2;
    unsigned long long l1 = lengthstr(str1), l2 = lengthstr(str2);
    char c1 = str1[l1], c2 = str2[l2];
    str1[l1] = '\0';
    str2[l2] = '\0';
    int result = strcmp(str1, str2);
    str1[l1] = c1;
    str2[l2] = c2;
    if (result > 0)
        result = 1;
    else
        if (result < 0)
            result = -1;
    return result;
}
