#include <stdio.h>
#include <stdlib.h>
#include "alloc.h"

#define memory_size 2048

typedef struct mem_block mem_block;

struct mem_block
{
	int size;
	unsigned char is_used;
	mem_block* prev_block;
	mem_block* next_block;
};

mem_block* current_block = NULL;
mem_block end_block;
void* mem_start = NULL;

void insert_new_block(mem_block* left_block, mem_block* right_block, size_t size)
{
	mem_block* new_block = (mem_block*)((int*)left_block + left_block->size - size);

	new_block->is_used = 0;
	new_block->size = size;
	new_block->prev_block = left_block;
	new_block->next_block = right_block;

	left_block->next_block = new_block;
	right_block->prev_block = new_block;
}

mem_block* get_free_space(size_t size)
{
	mem_block* block_ptr = (mem_block*)mem_start;
	mem_block* new_block;

	size_t size_diff, block_size = sizeof(mem_block) + size;

	while (block_ptr->next_block)
	{
		if ((!block_ptr->is_used) && (block_ptr->size >= block_size))
		{
			size_diff = block_ptr->size - block_size;

			if (size_diff > sizeof(mem_block))
				insert_new_block(block_ptr, block_ptr->next_block, size_diff);

			block_ptr->is_used = 1;
			block_ptr->size = block_size;

			return block_ptr;
		}
		block_ptr = block_ptr->next_block;
	}
	return NULL;
}

void merge_blocks(mem_block* left_block, mem_block* right_block)
{
	left_block->size += right_block->size;
	left_block->next_block = right_block->next_block;
	right_block->next_block->prev_block = left_block;
}

void merge_free_blocks()
{
	mem_block* block_ptr = (mem_block*)mem_start;

	while (block_ptr->next_block)
	{
		if ((!block_ptr->is_used) && (!block_ptr->next_block->is_used))
			merge_blocks(block_ptr, block_ptr->next_block);
		else
			block_ptr = block_ptr->next_block;
	}
}

void init()
{
	mem_start = malloc(memory_size * sizeof(int)); 

	if (!mem_start)
	{
		printf("Malloc error!\n");
		exit(-1);
	}

	current_block = (mem_block*)mem_start;
	current_block->size = memory_size * sizeof(int); 
	current_block->is_used = 0;
	current_block->prev_block = NULL;
	current_block->next_block = &end_block;

	end_block.size = sizeof(mem_block);
	end_block.is_used = 1;
	end_block.prev_block = NULL;
	end_block.next_block = NULL;
}

void* myMalloc(size_t size)
{
	current_block = get_free_space(size);

	if (!current_block)
	{
		printf("Could not allocate memory!\n");
		return NULL;
	}

	return ((int*)current_block) + sizeof(mem_block);
}

void myFree(void* ptr)
{
	mem_block* block_ptr = (mem_block*)((int*)ptr - sizeof(mem_block));

	if (!block_ptr->is_used)
		return;

	block_ptr->is_used = 0;

	merge_free_blocks();

}

void* myRealloc(void* ptr, size_t size)
{
	void* ret_ptr;
	mem_block* block_ptr = (mem_block*)((int*)ptr - sizeof(mem_block));
	mem_block* temp_block_ptr;

	int* source, *dest;
	int csize, new_block_size;
	int size_diff = size - block_ptr->size;

	if ((block_ptr->next_block) && (!block_ptr->next_block->is_used) && (block_ptr->next_block->size >= size_diff))
	{
		new_block_size = block_ptr->size + block_ptr->next_block->size - (size + sizeof(mem_block));
		merge_blocks(block_ptr, block_ptr->next_block);

		if (new_block_size > (int)sizeof(mem_block))
		{
			insert_new_block(block_ptr, block_ptr->next_block, new_block_size);
			block_ptr->size = size + sizeof(mem_block);
		}

		ret_ptr = ptr;
	}
	else 
		if ((block_ptr->prev_block) && (!block_ptr->prev_block->is_used) && (block_ptr->prev_block->size >= size_diff))
		{
			temp_block_ptr = block_ptr->prev_block;
			
			source = ((int*)block_ptr) + sizeof(mem_block);
			dest = ((int*)block_ptr->prev_block) + sizeof(mem_block);
			csize = block_ptr->size - sizeof(mem_block);

			block_ptr->prev_block->is_used = 1;

			merge_blocks(block_ptr->prev_block, block_ptr);
			
			memcpy(dest, source, csize);
			size_diff = temp_block_ptr->size - (size + sizeof(mem_block));
			
			if (size_diff > (int)sizeof(mem_block))
			{
				insert_new_block(temp_block_ptr, temp_block_ptr->next_block, size_diff);
				temp_block_ptr->size = size + sizeof(mem_block);
			}

			ret_ptr = dest;
		}
		else
		{
			dest = (int*)myMalloc(size);

			if (!dest)
				printf("Could not reallocate memory!\n");

			block_ptr->is_used = 0;
			source = ((int*)block_ptr) + sizeof(mem_block);
			csize = block_ptr->size - sizeof(mem_block);

			memcpy(dest, source, csize);

			ret_ptr = dest;
		}
	merge_free_blocks();
	return ret_ptr;
}

void print_blocks()
{
	mem_block* block_ptr = (mem_block*)mem_start;
	int ptr_diff;

	while (block_ptr->next_block)
	{
		ptr_diff = (unsigned char*)block_ptr->next_block - (unsigned char*)block_ptr;
		printf("Start: %p, size: %d, prev: %p, next: %p, used: %d\n", (void*)block_ptr, block_ptr->size, (void*)block_ptr->prev_block, (void*)block_ptr->next_block, block_ptr->is_used);
		if ((block_ptr->size != ptr_diff) && (block_ptr->next_block->next_block))
			printf("Size != ptr_diff (%d)!\n", ptr_diff);
		block_ptr = block_ptr->next_block;
	}
}