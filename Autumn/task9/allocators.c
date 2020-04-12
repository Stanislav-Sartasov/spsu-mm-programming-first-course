#include <unistd.h>
#include <malloc.h>
#include <stdlib.h>
#include <string.h>

#include "allocators.h"

int memory_size = (1 << 16);
int has_initialized = 0;

void *managed_memory_start;
void *last_valid_address;

typedef struct mem_control_block mem_control_block;

struct mem_control_block
{
	int is_available;
	int is_start;
	int size;
	void * address;
	mem_control_block *next;
	mem_control_block *prev;
};

mem_control_block *memory;

void init()
{
    memory = (mem_control_block *) malloc(sizeof(mem_control_block));
    memory->is_available = 1;
    memory->is_start = 1;
    memory->size = memory_size;
    memory->address = (void*) malloc(memory_size);
    memory->next = NULL;
    memory->prev = NULL;
    has_initialized = 1;
}

void newMemory()
{
    mem_control_block *new_mcb = (mem_control_block *) malloc(sizeof(mem_control_block));
    new_mcb->is_available = 1;
    new_mcb->is_start = 1;
    new_mcb->size = memory_size;
    new_mcb->address = (void*) malloc(memory_size);
    new_mcb->next = NULL;
    mem_control_block *current_location_mcb;
    current_location_mcb = memory;
    while (current_location_mcb->next)
        current_location_mcb = current_location_mcb->next;
    current_location_mcb->next = new_mcb;
    new_mcb->prev = current_location_mcb;
}

void *findMemory(size_t size)
{
    mem_control_block *current_location_mcb;
    void *memory_location;
    if (!has_initialized)
        init();
    memory_location = NULL;
    current_location_mcb = memory;

    while(current_location_mcb->next)
    {
        if(current_location_mcb->is_available)
        {
            if(current_location_mcb->size >= size)
            {
                if (current_location_mcb->size > size)
                {
                    mem_control_block *new_mcb = (mem_control_block *) malloc(sizeof(mem_control_block));
                    new_mcb->is_available = 1;
                    new_mcb->is_start = 0;
                    new_mcb->size = current_location_mcb->size - size;
                    new_mcb->address = current_location_mcb->address + size;
                    new_mcb->next = current_location_mcb->next;
                    new_mcb->prev = current_location_mcb;
                    if (current_location_mcb->next)
                        current_location_mcb->next->prev = new_mcb;
                    current_location_mcb->next = new_mcb;

                }
                current_location_mcb->is_available = 0;
                memory_location = current_location_mcb->address;
                break;
            }
        }
        current_location_mcb = current_location_mcb->next;
    }
    return memory_location;
}

void* myMalloc(size_t size)
{
    void *memory_location;
    memory_location = findMemory(size);

    if(!memory_location)
    {
        newMemory();
        memory_location = findMemory(size);
    }
    return memory_location;
}

void myFree(void* ptr)
{
    if (ptr == NULL)
        return;
    mem_control_block *current_location_mcb;
    current_location_mcb = memory;
    while (current_location_mcb->next)
    {
        if (current_location_mcb->address == ptr)
        {
            if (current_location_mcb->next && current_location_mcb->next->is_available && !current_location_mcb->next->is_start)
            {
                current_location_mcb->size += current_location_mcb->next->size;
                current_location_mcb->next = current_location_mcb->next->next;
            }
            if (current_location_mcb->prev && current_location_mcb->prev->is_available && !current_location_mcb->is_start)
            {
                current_location_mcb->prev->size += current_location_mcb->size;
                current_location_mcb->prev->next = current_location_mcb->next;
            }
            break;
        }
        current_location_mcb = current_location_mcb->next;
    }
    return;
}

void* myRealloc(void* ptr, size_t size)
{
    void *memory_location;
    memory_location = (void *)myMalloc(size);
    memcpy(memory_location, ptr, size);
    myFree(ptr);
    return memory_location;
}

void totalFree()
{
    mem_control_block *current_location_mcb;
    current_location_mcb = memory;
    while (current_location_mcb->next)
    {
        if (current_location_mcb->is_start)
        {
            free(current_location_mcb->address);
            free(current_location_mcb);
        }
        current_location_mcb = current_location_mcb->next;
    }
}
