#include <stdio.h>
#include <stdlib.h>

char *bufer;
int *buferstat;
unsigned long long bufersize;

void init(unsigned long long x);
void initstop(void);
void* myMalloc(size_t size);
void myFree(void *ptr);
void* myRealloc(void* ptr, int size);

int main()
{
    unsigned long long memsize = 8;
    int *number, *next;
    unsigned int amount;
    //scanf("%lld", &memsize);
    //printf("%lld", memsize);

    init(memsize);
    printf("how many elements? ");
    scanf("%d", &amount);
    number = (int*)  myMalloc((size_t)(sizeof(int)*amount));
    if(number)
    {
        for(int i = 0; i < (int)amount; ++i)
        {
            printf("put your %d number ", i);
            scanf("%d", &number[i]);
        }

        for(int i = 0; i < (int)amount; ++i)
            printf("%d ", number[i]);
        printf("\n");
        next = (int*)myMalloc(sizeof (int));
        if(!next)
            printf("memory full, can not give space for *next\n");

        myFree(number);

        next = (int*)myMalloc(sizeof (int));
        if(next)
        {
            printf("memory is given for *next, put your number: \n");
            scanf("%d",next);
            printf("number is %d", *next);

            myFree(next);
        }



    }
    else
    {
        printf("memory full\n");
    }
    initstop();
    init(memsize+4);

    next = (int*)myMalloc(sizeof (int));
    number = (int*)myMalloc(sizeof (int)*2);
    printf("\n");
    if(number)
        for(int i = 0; i < 2; ++i)
        {
            printf("put your %d number ", i);
            scanf("%d", &number[i]);
        }

    int *real = myRealloc(number, sizeof(int)*3);
    if(!real)
        printf("memory full for realloc\n");

    myFree(next);
    real = myRealloc(number, sizeof(int)*3);

    printf("add number: ");
    scanf("%d", real+2);


    if(real)
        for(int i=0; i<3; ++i)
            printf("%d ", real[i]);

    initstop();
    return 0;
}

void initstop(void)
{
    free(bufer);
    free(buferstat);
}

void* myRealloc(void* ptr, int size)
{
    unsigned long long newstart = 0, start = (unsigned long long)( ((char*)ptr) - bufer);
    int flag = 0, freesize = buferstat[start];
    for(unsigned long long i = start; i < start+(unsigned long long)freesize; ++i)
        buferstat[i] = 0;

    for(unsigned long long i = 0; i < bufersize; ++i)
    {
        if (buferstat[i] == 0)
        {
            flag = 1;
            for(unsigned long long k = i; k < i+(unsigned long long)size; ++k)
                if((buferstat[k] != 0) || (bufersize < i+(unsigned long long)size))
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
        for(unsigned long long i = start+1; i < start+(unsigned long long)freesize; ++i)
            buferstat[i] = 1;

        buferstat[start] = freesize;
        return (void *)0;
    }

    for(unsigned long long i = newstart+1; i < newstart+(unsigned long long)size; ++i)
        buferstat[i] = 1;
    buferstat[newstart] = size;

    for(unsigned long long i = start, k = newstart; i < start+(unsigned long long)freesize; ++k, ++i)
        bufer[k]=bufer[i];
    return (void*)(bufer + newstart);
}

void myFree(void *ptr)
{
    unsigned long long start =(unsigned long long) ((char*)ptr-bufer);
    size_t size = (size_t)buferstat[start];

    for(unsigned long long i = start; i < start+size; ++i)
        buferstat[i]=0;
}

void* myMalloc(size_t size)
{
    int flagtrue = 0;
    unsigned long long start = 0;
    for(unsigned long long i = 0; i < bufersize; ++i)
    {
        if(i+size > bufersize)
            break;
        if(buferstat[i] == 0)
        {
            flagtrue = 1;
            for(unsigned long long k = i; k<i+size; ++k)
                if((buferstat[k] != 0) || (bufersize < i+size))
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
    if(!flagtrue)
        {return (void*)0;}

    for(unsigned long long i=1+start; i<start+size; ++i)
        buferstat[i] = 1;

    buferstat[start] = (int)size;
    return (void*)(bufer+start);
}

void init(unsigned long long x)
{
    bufer = (char*) malloc(sizeof(char)*x);
    buferstat = (int*) malloc(sizeof(int)*x);
    for(unsigned long long i = 0; i<x; ++i)
        buferstat[i] = 0;
    bufersize=x;
}
