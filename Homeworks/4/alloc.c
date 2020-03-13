#include "alloc.h"

int sizeAlloc = (1 << 16);

typedef struct memBlock memBlock;

void* memStart = NULL;

struct memBlock
{
	size_t blSize;
	memBlock* prev;
	memBlock* next;
};

memBlock* memory;

void init()
{
	memStart = malloc(sizeAlloc * sizeof(size_t));
	if (!memStart)
	{
		exit(-1);
	}

	memory = (memBlock*)memStart;
	memory->blSize = sizeAlloc;

	memory->prev = NULL;
	memory->next = NULL;
}

memBlock* nextFreeBlock(size_t size)
{
	memBlock* block = memory;

	while (block && block->blSize < size + sizeof(size_t))
	{
		block = block->next;
	}

	return block;
}

void* myMalloc(size_t size)
{
	if (!memStart)
		init();

	memBlock* block = nextFreeBlock(size + sizeof(size_t));
	if (!block)
		return NULL;

	if (block->blSize < size + sizeof(size_t) + sizeof(memBlock))
	{
		if (block->next && block->prev)
		{
			block->next->prev = block->prev;
			block->prev->next = block->next;
		}
		if (!block->next && block->prev)
		{
			block->prev->next = NULL;
		}
		if (block->next && !block->prev)
		{
			block->next->prev = NULL;
		}
		return (unsigned char*)block + sizeof(size_t);
	}
	block->blSize = block->blSize - size - sizeof(size_t);

	memBlock* ptr = (memBlock*)((unsigned char*)block + block->blSize);
	ptr->blSize = size + sizeof(size_t);

	return ((unsigned char*)ptr + sizeof(size_t));
}

void* myRealloc(void* ptr, size_t newSize)
{
	if (!memStart)
		exit(-1);

	memBlock* block = (memBlock*)((unsigned char*)ptr - sizeof(size_t));

	if (block->blSize < newSize + sizeof(size_t))
	{
		void* newPtr = myMalloc(newSize);
		if (!newPtr)
		{
			return NULL;
		}
		memcpy(newPtr, ptr, block->blSize - sizeof(size_t));

		myFree(ptr);

		return newPtr;
	}
	else
	{
		return ptr;
	}
}

void myFree(void* ptr)
{
	if (!memStart)
		exit(-1);

	memBlock* block = (memBlock*)((unsigned char*)ptr - sizeof(size_t));

	if (!memory)
	{
		memory = block;
		block->prev = NULL;
		block->next = NULL;
		return;
	}

	memBlock* curr = memory;
	memBlock* prev = NULL;

	while (curr && curr->next < block)
	{
		prev = curr;
		curr = curr->next;
	}

	if (!prev)
	{
		block->next = memory;
		memory = block;
	}
	else
	{
		prev->next = block;
		block->prev = prev;
	}

	if (curr)
		curr->prev = block;
	block->next = curr;

	if (((unsigned char*)block + block->blSize) == (unsigned char*)curr)
	{
		block->next = curr->next;
		block->blSize = block->blSize + curr->blSize;
	}

	if (prev && (((unsigned char*)prev + prev->blSize) == (unsigned char*)block))
	{
		prev->next = block->next;
		prev->blSize = prev->blSize + block->blSize;
	}
}

