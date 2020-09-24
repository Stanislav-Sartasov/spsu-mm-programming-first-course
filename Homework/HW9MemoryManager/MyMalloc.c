#include "MyMalloc.h"

static memDict blockHead = {0, 0};
static const size_t alignTo = 16;
static const size_t overhead = sizeof(size_t);



void *myMalloc(size_t size)
{
    size = (size+sizeof(size_t) + (alignTo - 1));
    memDict *block = blockHead.next;
    memDict **head = &(blockHead.next);
    while (block != 0)
    {
        if (block->size >= size)
        {
            *head = block->next;
            return ((char*)block) + sizeof(size_t);
        }
        head = &(block->next);
        block = block->next;
    }
    block = (memDict*)sbrk(size);
    block->size = size;
    return ((char*)block + sizeof(size_t));
}

