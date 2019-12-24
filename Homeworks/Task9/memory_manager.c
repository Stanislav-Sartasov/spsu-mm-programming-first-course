#include <stdlib.h>
#include <stdio.h>
#include <memory.h>
#include "memory_manager.h"

void fillMeta(Meta *meta, size_t size, Block *next, Block *prev, unsigned char allocated);

void merge(Block *block);

void init()
{
    mem_block = (Block *) malloc(MAX_MEMORY_SIZE);
    mem_block->meta.size = MAX_MEMORY_SIZE - sizeof(Meta);
    mem_block->meta.next = NULL;
    mem_block->meta.prev = NULL;
    mem_block->meta.allocated = 0;
}

void *myMalloc(size_t size) 
{
    if (size == 0)
        return NULL;
    Block *block = mem_block;
    while (block) {
        if (block->meta.allocated) 
		{
            block = block->meta.next;
            continue;
        }
        size_t needed_space = size;
        if (block->meta.size < needed_space) 
		{
            block = block->meta.next;
            continue;
        }
        size_t remaining_space = block->meta.size - needed_space;
        fillMeta(&block->meta, size, block->meta.next, block->meta.prev, 1);
        if (remaining_space > sizeof(Meta))
		{
            Block *remainedBlock = (Block *) block->data + size;
            fillMeta(&remainedBlock->meta, remaining_space - sizeof(Meta), block->meta.next, block, 0);
            block->meta.next = remainedBlock;
            if(remainedBlock->meta.next)
			{
                Block *next = remainedBlock->meta.next;
                next->meta.prev = remainedBlock;
            }
        }
        return block->data;
    }
    printf("No space available to allocate\n");
    return NULL;
}

void myFree(void *ptr)
{
    if (!ptr)
        return;
    Block *block = mem_block;
    while (block)
	{
        if (block->data == ptr) 
		{
            block->meta.allocated = 0;
            merge(block);
            return;
        }
        block = block->meta.next;
    }
}

void *myRealloc(void *ptr, size_t size) 
{
    myFree(ptr);
    void *newPtr = myMalloc(size);
    memcpy(newPtr, ptr, size);
    return newPtr;

}


void fillMeta(Meta *meta, size_t size, Block *next, Block *prev, unsigned char allocated) {
    meta->allocated = allocated;
    meta->size = size;
    meta->next = next;
    meta->prev = prev;
}

void merge(Block *block) {
    if (block->meta.next) {
        Block *next = block->meta.next;
        if (!next->meta.allocated) {
            block->meta.next = next->meta.next;
            block->meta.size = next->meta.size + sizeof(Meta);
            if (next->meta.next) {
                next = next->meta.next;
                next->meta.prev = block;
            }

        }
    }
    if (block->meta.prev) {
        Block *prev = block->meta.prev;
        if (!prev->meta.allocated) {
            prev->meta.next = block->meta.next;
            prev->meta.size = block->meta.size + sizeof(Meta);
            if (block->meta.next) {
                block = block->meta.next;
                block->meta.prev = prev;
            }
        }
    }
}