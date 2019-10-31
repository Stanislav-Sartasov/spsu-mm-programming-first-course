#include <stdio.h>
#include <stdlib.h>

char *bufer;
int *buferstat;
unsigned long long bufersize;

void init(unsigned long long x);
void* myMalloc(size_t size);
void myFree(void *ptr);

int main()
{

    init(5);
    myMalloc(4);
    return 0;
}

void myFree(void *ptr)
{
    unsigned long long start =(unsigned long long) ((char*)ptr-bufer);
    size_t size = buferstat[start];

    for(unsigned long long i = start; i < start+size; ++i)
        buferstat[i]=0;
}

void* myMalloc(size_t size)
{
    int flagtrue = 0;
    unsigned long long start = 0;
    for(unsigned long long i = 0; i < bufersize; ++i)
    {
        if(buferstat[i] == 0)
        {
            flagtrue = 1;
            for(unsigned long long k = i; k<i+size; ++k)
                if(buferstat[k] != 0)
                {
                    i = k;
                    flagtrue = 0;
                    break;
                }
        }
        if(flagtrue)
        {
            start=i;
            break;
        }
    }
    if(!start)
        {return (void*)0;}
    for(unsigned long long i=1+start; i<start+size; ++i)
        buferstat[i] = 1;

    buferstat[start] = size;
    return (void*)(bufer+start);
}

void init(unsigned long long x)
{
    bufer = (char*) malloc(sizeof(char)*x);
    buferstat = (int*) malloc(sizeof(int)*x);
    for(unsigned long long i = 0; i<x; ++i)
        buferstat[0] = 0;
    bufersize=x;
}
