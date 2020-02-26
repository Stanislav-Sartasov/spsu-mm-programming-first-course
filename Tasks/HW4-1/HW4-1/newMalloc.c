#include "newMalloc.h"

int amount_of_space = 10000;
void* memory;
int has_initialized = 0;

void init()
{
	has_initialized = 1;
	memory = malloc(amount_of_space);
	mem_block* block = (mem_block*)memory;
	block->next = NULL;
	block->previous = NULL;
	block->size_of_block = amount_of_space;
	stack = block;
}


void delete_block(mem_block* block)
{
	if (block == stack)
	{
		stack = block->next;
		return 0;
	}
	else if (block->next == NULL)
	{
		block->previous->next = NULL;
		return 0;
	}
	block->previous->next = block->next;
	block->next->previous = block->previous;
}

void* newMalloc(size_t size)
{
	if (has_initialized == 0) init();
	int k = 0;

	mem_block* block = stack;
	while (block != NULL)
	{
		if (block->size_of_block >= size + sizeof(size_t))
		{
			k = 1;
			break;
		}

		block = block->next;
	}

	if (k == NULL) return NULL;
	if (size == block->size_of_block)
	{
		delete_block(block);
		return (char*)block + sizeof(size_t);
	}

	block->size_of_block = block->size_of_block - sizeof(size_t) - size;
	mem_block* new = (mem_block*)((char*)block + block->size_of_block);
	new->size_of_block = size + sizeof(size_t);
	return (char*)new + sizeof(size_t);

}

void newFree(void* ptr)
{
	mem_block* block = (mem_block*)((char*)ptr - sizeof(size_t));
	if (stack == NULL)
	{
		block->previous = NULL;
		block->next = NULL;
		stack = block;
		return 0;
	}

	mem_block* tmp = stack;
	while ((tmp->next != NULL) && (tmp->next < block))	tmp = tmp->next;

	if ((tmp->previous != NULL) && (tmp->next != NULL))
	{
		tmp->next->previous = block;
		tmp->previous->next = block;
	}
	else if (tmp->previous != NULL)
	{
		tmp->previous->next = block;
	}
	else if (tmp->next != NULL)
	{
		tmp->next->previous = block;
	}
	if ((block->next != NULL) && (block->next == (char*)block + sizeof(block)))
	{

		(block)->next = (block->next)->next;
		if ((block->next)->next != NULL)
		{
			(block->next)->next->previous = (block);
		}
		(block)->size_of_block = (block)->size_of_block + (block->next)->size_of_block;

	}
	if ((block->previous != NULL) && (block->previous == (char*)block - sizeof(block)))
	{

		(block->previous)->next = (block)->next;
		if ((block)->next != NULL)
		{
			(block)->next->previous = (block->previous);
		}
		(block->previous)->size_of_block = (block->previous)->size_of_block + (block)->size_of_block;

	}


}

void* newRealloc(void* ptr, size_t newSize)
{
	mem_block* block = (char*)ptr - sizeof(size_t);

	if (block->size_of_block - sizeof(size_t) >= newSize) return ptr;

	mem_block* new = newMalloc(newSize);
	if (new == NULL) return NULL;

	memcpy(new, ptr, block->size_of_block - sizeof(size_t));
	newFree(ptr);
	return new;
}