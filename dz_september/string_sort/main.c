#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <malloc.h>
#include <sys/stat.h>
#include <sys/types.h>
#include "sys/mman.h"
#include <string.h>

void qsortt(char **, long long len);

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
    if (strs[0][strlen(strs[0]) - 1] == '\r')
        strs[0][strlen(strs[0]) - 1] = '\0';
    for (int i = 1; i < numberStr; ++i)
    {
        strs[i] = strtok(NULL, "\n");
        if (strs[i][strlen(strs[i]) - 1] == '\r')
            strs[i][strlen(strs[i]) - 1] = '\0';
    }

    qsortt(strs, numberStr);

    char endl = '\n';
    for(int i = 0; i < numberStr; ++i)
    {
        write(fOut, strs[i], maxLen);
        write(fOut, &endl, 1);
    }

    munmap(map, stat.st_size);
    close(fIn);
    close(fOut);

    printf("Sorted)");
    return 0;
}

void qsortt(char **strs, long long numberStr)
{
    long long opr = (numberStr - 1) / 2, flagi = 0, flagj = 0, lasti = -1, oprflag = 0;
    for (long long i = 0, j = numberStr - 1; i <= j; )
    {
        if ((strcmp(strs[i], strs[opr]) == 0) && (!(lasti + 1)))
        {
            lasti = i;
        }
        if (strcmp(strs[i], strs[opr]) > 0)
        {
            flagi = 1;
        }
        else
        {
            ++i;
        }

        if (strcmp(strs[opr], strs[j]) >= 0)
        {
            flagj = 1;
        }
        else
        {
            --j;
        }
        if (flagi && flagj)
        {
            char *temp = strs[i];
            strs[i] = strs[j];
            strs[j] = temp;
            flagi = 0;
            flagj = 0;
            if (opr == i)
                opr = j;
            else
                if (opr == j)
                    opr = i;
        }
        if (i == j)
        {
            if (strcmp(strs[opr], strs[j]) >= 0)
            {
                flagj = 1;
            }
            if ((flagj) && (lasti + 1))
            {
                char *temp = strs[lasti];
                strs[lasti] = strs[j];
                strs[j] = temp;
                flagj = 0;
            }
            opr = i;
            break;
        }
        if (i > j)
        {
            opr = j;
            oprflag = 1;
            break;
        }
    }
    ///////////////////////////////
    if (oprflag)
    {
        if (opr + 1 > 1)
            qsortt(strs, opr + 1);
        if (numberStr - opr - 1 > 1)
            qsortt(strs + opr + 1, numberStr - opr - 1);
    }
    else
    {
        if ((flagj) && (numberStr > opr + 1))
            ++opr;
        if (opr > 1)
            qsortt(strs, opr);
        if (numberStr - opr > 1)
            qsortt(strs + opr, numberStr - opr);
    }
    return ;
}
