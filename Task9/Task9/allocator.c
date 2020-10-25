#include "allocator.h"
#include <stdlib.h>
#include <string.h>

const size_t max_size = 16384;
mem_control_block* memory_list;

void init()
{
	if (memory_block)
	{
		printf("Error of memory size");
		exit(-1);
	}
	memory_block = malloc(max_size);
	if (!memory_block)
	{
		exit(-1);
	}
	memory_list = (mem_control_block*)memory_block;
	memory_list->size = max_size;
	memory_list->is_available = 1;
}

void* myMalloc(size_t size)
{
	if (!memory_block)
	{
		init();
	}
	void* current_location;
	mem_control_block* current_location_mcb;
	void* memory_location = 0;
	current_location = memory_list;

	size += sizeof(mem_control_block);
	int current_number = 0;
	int free_size = 0;
	while (memory_location != current_location)
	{
		if ((char*)current_location + size > (char*)memory_list + max_size)
		{
			printf("Out of memory");
			exit(0);
		}
		current_location_mcb = (mem_control_block*)current_location;
		if (current_location_mcb->is_available)
		{
			if (current_location_mcb->size + sizeof(mem_control_block) >= size)
			{
				current_location_mcb->is_available = 0;
				current_location_mcb->size = size;
				memory_location = current_location;
				free_size = 0;
				break;
			}
			else if (current_location_mcb->size + free_size >= size)
			{
				current_location = (char*)current_location - free_size;
				current_location_mcb = (mem_control_block*)current_location;
				current_location_mcb->is_available = 0;
				current_location_mcb->size = size;
				memory_location = current_location;
				free_size = 0;
				break;
			}
			free_size = free_size + current_location_mcb->size;
		}
		else
			free_size = 0;
		current_number += current_location_mcb->size;
		current_location = (char*)memory_list + current_number;
	}
	memory_location = sizeof(mem_control_block) + (char*)memory_location;
	return memory_location;
}

void* myRealloc(void* ptr, size_t new_size)
{
	void* current_location = (char*)ptr - sizeof(mem_control_block);
	mem_control_block* current_location_mcb;
	current_location_mcb = (mem_control_block*)current_location;
	if (new_size + sizeof(mem_control_block) < current_location_mcb->size)
	{
		current_location_mcb->size = new_size + sizeof(mem_control_block);
		return ptr;
	}
	else if (current_location_mcb->is_available == 1)
	{
		void* new_ptr = myMalloc(new_size);
		memcpy(new_ptr, ptr, current_location_mcb->size - sizeof(mem_control_block));
		return new_ptr;
	}
	else
	{
		current_location_mcb->is_available = 1;
		void* new_ptr = myMalloc(new_size);
		memcpy(new_ptr, ptr, current_location_mcb->size);
		if (current_location != (char*)new_ptr - sizeof(mem_control_block))
			current_location_mcb->is_available = 0;
		return new_ptr;
	}
}

void myFree(void* ptr)
{
	struct mem_control_block* mcb;
	mcb = (char*)ptr - sizeof(mem_control_block);
	mcb->is_available = 1;
}