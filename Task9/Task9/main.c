#include <stdio.h>
#include <string.h>
#include <stdlib.h>

#pragma pack(push, 1)

typedef struct mem_control_block
{
	int size;
	char is_available;
}mem_control_block;

#pragma pack(pop)

const size_t max_size = 16384;
void* memory_block;
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

void* my_malloc(size_t size)
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

void* my_realloc(void* ptr, size_t newsize)
{
	void* current_location = (char*)ptr - sizeof(mem_control_block);
	mem_control_block* current_location_mcb;
	current_location_mcb = (mem_control_block*)current_location;
	if (newsize + sizeof(mem_control_block) < current_location_mcb->size)
	{
		current_location_mcb->size = newsize + sizeof(mem_control_block);
		return ptr;
	}
	else if (current_location_mcb->is_available == 1)
	{
		void* newptr = my_malloc(newsize);
		memcpy(newptr, ptr, current_location_mcb->size - sizeof(mem_control_block));
		return newptr;
	}
	else
	{
		current_location_mcb->is_available = 1;
		void* newptr = my_malloc(newsize);
		memcpy(newptr, ptr, current_location_mcb->size);
		if (current_location != (char*)newptr - sizeof(mem_control_block))
			current_location_mcb->is_available = 0;
		return newptr;
	}
}

void my_free(void* ptr)
{
	struct mem_control_block* mcb;
	mcb = (char*)ptr - sizeof(mem_control_block);
	mcb->is_available = 1;
}
int main()
{
	char* a = my_malloc(10);
	for (int i = 0; i < 10; i++)
	{
		a[i] = i;
	}

	a = my_realloc(a, 13);
	for (int i = 10; i < 13; i++)
	{
		a[i] = -i;
	}

	for (int i = 0; i < 13; i++)
	{
		printf("a[%d] = %d\n", i, a[i]);
	}
	int* c = my_malloc(4 * sizeof(int));
	for (int i = 0; i < 4; i++)
	{
		c[i] = i * 5;
		printf("c[%d] = %d\n", i, c[i]);
	}
	my_free(a);
	char* b = my_malloc(13);
	for (int i = 0; i < 13; i++)
	{
		b[i] = 2 * i;
		printf("b[%d] = %d\n", i, b[i]);
	}
	char* d = my_malloc(16341);		//Out of memory
	free(memory_block);
	return 0;
}