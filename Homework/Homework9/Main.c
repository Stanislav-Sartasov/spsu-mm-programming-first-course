#include <stdlib.h>
#include <time.h>
#include <stdio.h>
#include "allocator.h"

#define MAX_NODES      (100005)
#define MIN_ALLOC_SIZE (1)
#define MAX_ALLOC_SIZE (1005)

// stores allocated memory regions
void* memptrs[MAX_NODES];

// fi points to a free index in memptrs
int fi = 0;

// ui points to an oldest allocated memory in memptrs
int ui = 0;

int mallocSuccessCount   = 0;
int mallocFailCount      = 0;
int deallocCount         = 0;
int reallocSuccessCount  = 0;
int reallocFailCount     = 0;

int genRandom(int a, int b) 
{
    int len = (b - a + 1);
    return a + (rand() % len);
}

// allocate a random sized memory, and store it in memptrs[fi]
void mallocTest() 
{
    int random_size = genRandom(MIN_ALLOC_SIZE, MAX_ALLOC_SIZE);
    void* addr = myMalloc(random_size);
    if (addr)
	{
        ++mallocSuccessCount;
        memptrs[fi] = addr;
        fi = (fi + 1) % MAX_NODES;
    } else
	{
        ++mallocFailCount;
    }
}

// deallocate memptrs[ui], memptrs[ui] points to an oldest allocated memory
void deallocTest()
{
    myFree(memptrs[ui]);
    ++deallocCount;
    memptrs[ui] = NULL;
    ui = (ui + 1) % MAX_NODES;
}

// realloc memptrs[ui], with a random size
void reallocTest() 
{
    int random_size = genRandom(MIN_ALLOC_SIZE, MAX_ALLOC_SIZE);
    void* addr = myRealloc(memptrs[ui], random_size);
    if (addr)
	{
        ++reallocSuccessCount;
        memptrs[ui] = addr;
    } else 
	{
        ++reallocFailCount;
    }
}

// perform a random task, either malloc or free or realloc
void testcase()
{
    // can allocate, free, realloc, select a random one
    if (memptrs[fi] == NULL && memptrs[ui] != NULL) {
        int r = genRandom(0, 2);
        if (r == 0)
            mallocTest();
        else if (r == 1)
            deallocTest();
        else
            reallocTest();
    } else if(memptrs[fi] == NULL) 
	{
        // can only allocate
        mallocTest();
    } else
	{
        // can free, realloc, select a random one
        if (genRandom(0, 1))
            reallocTest();
        else
            deallocTest();
    }
}

int readNumberStdin(int max_digits, int* ret)
{
    int digits = 0;
    int c = 0;
    (*ret) = 0;

    while ((c = getchar()) != '\n')
	{
        if (!(c >= '0' && c <= '9') || (++digits > max_digits))
		{
            while (getchar() != '\n');
            return 1;
        }
        (*ret) = (*ret) * 10 + (c - '0');
    }

    return !digits;
}

void getNumber(const char* prompt, int* number)
{

    do 
	{
        printf("%s", prompt);
    } while(readNumberStdin(8, number) || !*number);
}

int main()
{
    srand(time(NULL));

    const char description[] = ""
                               "This program checks the performance of custom"
                               " my_malloc, my_free, my_realloc functions\n\n";

    printf("%s", description);

    int iterations = 0;
    getNumber("number of iterations(> 0): ", &iterations);
    printf("\n");

    int base_memory_size = 0;
    getNumber("base memory size(> 0 bytes): ", &base_memory_size);
    printf("\n");

    init(base_memory_size);

    printf("[*] running...\n");
    for(int i=0; i<iterations; ++i)
	{
        testcase();
    }
    printf("[*] done...\n\n");

    printf("\tResults: \n");
    printf("\nmy_malloc: \n");
    printf("\trequested: %8d\n", mallocSuccessCount + mallocFailCount);
    printf("\tsuccess  : %8d\n", mallocSuccessCount);
    printf("\tfailed   : %8d\n", mallocFailCount);

    printf("\nmy_free: \n");
    printf("\trequested: %8d\n", deallocCount);
    printf("\tsuccess  : %8d\n", deallocCount);
    printf("\tfailed   : %8d\n", 0);

    printf("\nmy_realloc: \n");
    printf("\trequested: %8d\n", reallocSuccessCount + reallocFailCount);
    printf("\tsuccess  : %8d\n", reallocSuccessCount);
    printf("\tfailed   : %8d\n", reallocFailCount);
}