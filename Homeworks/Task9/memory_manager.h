#ifndef PROJECT_MEMORY_MANAGER_H
#define PROJECT_MEMORY_MANAGER_H

#include <stddef.h>

#define MAX_MEMORY_SIZE				(160)
#pragma pack(1)
typedef struct Meta
{
    size_t size;
    unsigned char allocated;
    struct Block *next;
    struct Block *prev;

} Meta;

typedef struct Block 
{
    struct Meta meta;
    char data[];
} Block;

struct Block *mem_block;

void *myMalloc(size_t size);

void myFree(void *ptr);

void *myRealloc(void *ptr, size_t size);

void init();

#endif //PROJECT_MEMORY_MANAGER_H
