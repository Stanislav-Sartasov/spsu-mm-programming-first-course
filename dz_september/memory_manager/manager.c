#include "manager.h"

char *bufer;
char *buferstat;
unsigned long long bufersize;


void initstop(void)
{
    free(bufer);
    free(buferstat);
    bufersize = 0;
}

void* myRealloc(void* ptr, int size)
{
    unsigned long long newstart = 0, start = (unsigned long long)( ((char*)ptr) - bufer);
    int flag = 0;

    unsigned long long stop = start;
    do
    {
        pushBuferStat(stop++, 0);
    }
    while (getBuferStat(stop) != 2);
    pushBuferStat(stop, 0);

    for (unsigned long long i = 0; i < bufersize; ++i)
    {
        if (getBuferStat(i) == 0)
        {
            flag = 1;
            for (unsigned long long k = i; k < i + (unsigned long long)size; ++k)
                if ((getBuferStat(k) != 0) || (bufersize < i + (unsigned long long)size))
                {
                    flag = 0;
                    i = k;
                    break;
                }
        }
        if (flag)
        {
            newstart = i;
            break;
        }
    }

    if(!flag)
    {
        pushBuferStat(stop, 2);
        pushBuferStat(start, 2);
        for (unsigned long long i = start + 1; i < stop; ++i)
            pushBuferStat(i, 1);

        return (void *)0;
    }

    for (unsigned long long i = newstart + 1; i < newstart + (unsigned long long)size - 1; ++i)
        pushBuferStat(i, 1);
    pushBuferStat(newstart, 2);
    pushBuferStat(newstart + (unsigned long long)size - 1, 2);

    for (unsigned long long i = start, k = newstart; i < stop + 1; ++k, ++i)
        bufer[k]=bufer[i];
    return (void*)(bufer + newstart);
}

void myFree(void *ptr)
{
    unsigned long long start = (unsigned long long) ((char*)ptr - bufer);
    pushBuferStat(start, 0);
    unsigned long long i = start + 1;
    while (getBuferStat(i) != 2)
        pushBuferStat(i++, 0);
    pushBuferStat(i, 0);
}

void* myMalloc(size_t size)
{
    int flagtrue = 0;
    unsigned long long start = 0;
    for (unsigned long long i = 0; i < bufersize; ++i)
    {
        if (i + size > bufersize)
            break;
        if (getBuferStat(i) == 0)
        {
            flagtrue = 1;
            for (unsigned long long k = i; k < i + size; ++k)
                if ((getBuferStat(k) != 0) || (bufersize < i + size))
                {
                    i = k;
                    flagtrue = 0;
                    break;
                }

        }
        if (flagtrue)
        {
            start = i;
            break;
        }
    }
    if (!flagtrue)
        return (void*)0;
    if (size == 1)
    {
        pushBuferStat(start, 1);
        return (void*)(bufer + start);
    }

    for (unsigned long long i = 1 + start; i < start + size - 1; ++i)
        pushBuferStat(i, 1);
    pushBuferStat(start, 2);
    pushBuferStat(start + size - 1, 2);
    return (void*)(bufer+start);
}

void init(unsigned long long x)
{
    x = x + (x % 4 == 0 ? 0 : 4 - (x % 4));
    bufer = (char*) malloc(sizeof(char) * x);
    buferstat = (char*) malloc(sizeof(char) * x / 4);
    //for(unsigned long long i = 0; i<x; ++i)
    //    buferstat[i] = 0;
    memset(buferstat, 0, sizeof(char) * x / 4);
    bufersize=x;
}

unsigned long long getBuferStat(unsigned long long i)
{
    unsigned long long byte = i / 4;
    i = i - byte * 4;
    short n1, n2;
    switch (i)
    {
    case 0:
          n1 = buferstat[byte] & 128;
          n2 = buferstat[byte] & 64;
    break;
    case 1:
          n1 = buferstat[byte] & 32;
          n2 = buferstat[byte] & 16;
    break;
    case 2:
          n1 = buferstat[byte] & 8;
          n2 = buferstat[byte] & 4;
    break;
    case 3:
          n1 = buferstat[byte] & 2;
          n2 = buferstat[byte] & 1;
    break;
    }
    return (n1 != 0 ? 1 : 0) + (n2 != 0 ? 1 : 0);
}

void pushBuferStat(unsigned long long i, short n)
{
    unsigned long long byte = i / 4;
    i = i - byte * 4;
    short n1, n2;
    switch (i)
    {
    case 0:
          n1 = 128;
          n2 = 64;
          buferstat[byte] &= 63;
    break;
    case 1:
          n1 = 32;
          n2 = 16;
          buferstat[byte] &= 207;
    break;
    case 2:
          n1 = 8;
          n2 = 4;
          buferstat[byte] &= 243;
    break;
    case 3:
          n1 = 2;
          n2 = 1;
          buferstat[byte] &= 252;
    break;
    }

    switch (n)
    {
    case 1:
        buferstat[byte] += n1;
    break;
    case 2:
        buferstat[byte] += n1+n2;
    }
}
