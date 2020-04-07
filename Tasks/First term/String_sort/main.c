#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <fcntl.h>
#include "mman.h"
#include <string.h>

int compare(const void* ptr1, const void* ptr2)
{
    return strcmp(*(char**)ptr1, *(char**)ptr2);
}

int len(char* string)
{
    int counter = 1;
    for (int i = 2; string[i] != '\n'; i++)
        counter++;
    return counter;
}

int main()
{
    int a = 2, b = 3, c = 4;
    int *p = &a, *q = &b, *r = &c;
    printf("%d %d %d\n", &p, &q, &r);
    printf("%d %d %d\n", p, q, r);
//    printf("%d\n", (p+q));
//    printf("%d\n", (p*q));
//    printf("%d\n", (p/q));
    printf("%d\n", (p-q));
    printf("%d\n", (p-r));
    printf("%d\n", (q - r));

    return 0;
}
