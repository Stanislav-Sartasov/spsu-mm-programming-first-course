#include "allocator.h"
#include <stdlib.h>
#include <string.h>

const size_t max_size = 16384;
unsigned int* memory_list;

void mem_block_change(unsigned int* mem_control_block, int size, char is_available)
{
	*mem_control_block = size << 1;
	*mem_control_block += is_available;
}

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
	memory_list = memory_block;
	mem_block_change(memory_list, max_size, 1);
}


void* myMalloc(size_t size)
{
	if (!memory_block)
	{
		init();
	}
	void* current_location;
	unsigned int* current_location_mcb;
	void* memory_location = 0;
	current_location = memory_list;

	size += sizeof(unsigned int);
	int current_number = 0;
	int free_size = 0;
	while (memory_location != current_location)
	{
		if ((char*)current_location + size > (char*)memory_list + max_size)
		{
			printf("Out of memory");
			exit(0);
		}
		current_location_mcb = (unsigned int*)current_location;
		if (*current_location_mcb & 1)
		{
			if ((*current_location_mcb >> 1) + sizeof(unsigned int) >= size)
			{
				mem_block_change(current_location_mcb, size, 0);
				memory_location = current_location;
				free_size = 0;
				break;
			}
			else if ((*current_location_mcb >> 1) + free_size >= size)
			{
				current_location = (char*)current_location - free_size;
				current_location_mcb = (unsigned int*)current_location;
				mem_block_change(current_location_mcb, size, 0);
				memory_location = current_location;
				free_size = 0;
				break;
			}
			free_size = free_size + (*current_location_mcb >> 1);
		}
		else
			free_size = 0;
		current_number += (*current_location_mcb >> 1);
		current_location = (char*)memory_list + current_number;
	}
	memory_location = sizeof(unsigned int) + (char*)memory_location;
	return memory_location;
}

void* myRealloc(void* ptr, size_t new_size)
{
	void* current_location = (char*)ptr - sizeof(unsigned int);
	unsigned int* current_location_mcb;
	current_location_mcb = (unsigned int*)current_location;
	if (new_size + sizeof(unsigned int) < (*current_location_mcb >> 1))
	{
		*current_location_mcb = (new_size + sizeof(unsigned)) << 1;
		return ptr;
	}
	else if (*current_location_mcb & 1)
	{
		void* new_ptr = myMalloc(new_size);
		memcpy(new_ptr, ptr, (*current_location_mcb >> 1) - sizeof(unsigned int));
		return new_ptr;
	}
	else
	{
		(*current_location_mcb)++;
		void* new_ptr = myMalloc(new_size);
		memcpy(new_ptr, ptr, (*current_location_mcb >> 1));
		if (current_location != (char*)new_ptr - sizeof(unsigned int))
		{
			*current_location_mcb >>= 1;
			*current_location_mcb <<= 1;
		}
		return new_ptr;
	}
}

void myFree(void* ptr)
{
	unsigned int* mcb;
	mcb = (char*)ptr - sizeof(unsigned int);
	*mcb = *mcb | 1;
}