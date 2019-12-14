#include <unistd.h>
#include <malloc.h>
#include <stdlib.h>
#include <string.h>

#include "allocators.h"

int memory_size = 1e5;
int has_initialized = 0;

void init()
{
    last_valid_address = (void*) malloc(memory_size);
    managed_memory_start = last_valid_address;
    has_initialized = 1;
}

void uninit(void *ptr)
{
    free(ptr);
}

void* myMalloc(size_t size)
{
    void *current_location;
    mem_control_block *current_location_mcb;
    void *memory_location;
    if (!has_initialized)
        init();
    size = size + sizeof(mem_control_block);
    memory_location = 0;
    current_location = managed_memory_start;

    while(current_location != last_valid_address)
    {
        current_location_mcb = (mem_control_block*) current_location;
        if(current_location_mcb->is_available)
        {
            if(current_location_mcb->size >= size)
            {
                current_location_mcb->is_available = 0;
                memory_location = current_location;
                break;
            }
        }
        current_location = current_location + current_location_mcb->size;
    }

    if(!memory_location)
    {
        init();
        memory_location = last_valid_address;
        last_valid_address = last_valid_address + size;

        current_location_mcb = memory_location;
        current_location_mcb->is_available = 0;
        current_location_mcb->size = size;
    }

    memory_location = memory_location + sizeof(mem_control_block);
    return memory_location;
}

void myFree(void* ptr)
{
    mem_control_block *mcb;
    mcb = ptr - sizeof(mem_control_block);
    mcb->is_available = 1;
    uninit(ptr);
    return;
}

void* myRealloc(void* ptr, size_t size)
{
    void *memory_location;
    memory_location = myMalloc(size);
    memset(memory_location, ptr, size + sizeof(mem_control_block));
    myFree(ptr);
    return memory_location;
}
