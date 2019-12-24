#include "allocator.h"
#include "pool.h"
#include <stdio.h>

typedef uint8_t u8;

typedef struct 
{
    // pool id
    int poolID;

    // number of (1 << poolID) blocks
    int numOfBlocks;

    // size of memory user requested
    size_t reqUserSize;

} MemHeader;

static uint64_t roundNextPow2(uint64_t v) 
{
    v -= 1;
    v |= v >> 1u;
    v |= v >> 2u;
    v |= v >> 4u;
    v |= v >> 8u;
    v |= v >> 16u;
    v |= v >> 32u;
    v += 1;
    return v;
}

// find's n in (2 ^ n = num)
static int whichPower(uint64_t num) 
{
    for(int i=0; i<64; ++i) 
	{
        if (num & (1ULL << i))
            return i;
    }
    return 0;
}

static size_t max(size_t a, size_t b) 
{
    return (a >= b) ? a : b;
}

// round size to pool requirements
static void roundPC(size_t size, int* poolID, int* numOfBlocks) 
{
    size = max((1ULL << MIN_POOL_ID), roundNextPow2(size));

    (*poolID) = whichPower(size);
    (*numOfBlocks) = 1;
    if ((*poolID) > MAX_POOL_ID)
	{
        (*numOfBlocks) = (1 << ((*poolID) - MAX_POOL_ID));
        (*poolID) = MAX_POOL_ID;
    }
}

void* allocate(size_t size)
{
    int poolID, numOfBlocks;

    // apart from user memory, we want extra memory region to store header
    roundPC(sizeof(MemHeader) + size, &poolID, &numOfBlocks);

    MemHeader* header = (MemHeader*)poolAllocate(poolID, numOfBlocks);
    if (header == NULL)
        return NULL;

    header->poolID = poolID;
    header->numOfBlocks = numOfBlocks;
    header->reqUserSize = size;

    void* userMem = (u8*)header + sizeof(MemHeader);
    return userMem;
}

void deallocate(void* addr) 
{
    MemHeader* header = (MemHeader*)((u8*)addr - sizeof(MemHeader));
    poolDeallocate(header->poolID, header->numOfBlocks, header);
}

void* reallocate(void* addr, size_t newSize)
{
    MemHeader* header = (MemHeader*)((u8*)addr - sizeof(MemHeader));
    size_t tsize = (1ULL << header->poolID) * header->numOfBlocks;
    size_t usize = tsize - sizeof(MemHeader);

    // old memory location is capable of holding newSize elements
    if (newSize <= usize)
        return addr;

    // allocate new block of memory
    void* newAddr = allocate(newSize);
    if(newAddr == NULL)
        return NULL;

    // copy old data to new memory location
    for(size_t i=0; i<header->reqUserSize; ++i)
	{
        *((u8*)newAddr + i) = *((u8*)addr + i);
    }

    // delete old memory location
    deallocate(addr);
    return newAddr;
}

void* myMalloc(size_t size) 
{
    if (size)
        return allocate(size);
    return NULL;
}

void myFree(void* ptr) 
{
    if(ptr)
        deallocate(ptr);
}

void* myRealloc(void* ptr, size_t size) 
{
    if(ptr)
        return reallocate(ptr, size);
    return NULL;
}

// initialize a base memory block, to be used by pool
void init(size_t size) 
{
    int poolID, numOfBlocks;

    printf("[*] rounding %lu to ", size);

    roundPC(size, &poolID, &numOfBlocks);
    size = (1 << poolID) * numOfBlocks;

    printf("%lu\n", size);
    printf("[*] allocating %lld bytes of memory\n", size * 1ULL);

    void* memory = malloc(size);

    if (memory == NULL) 
	{
        printf("[*] failed to allocate initial memory\n");
        exit(-1);
    }

    poolInit(poolID, numOfBlocks, memory);
}