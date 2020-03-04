#include "allocator.h"

const size_t sizeToAllocate = (1 << 16);

struct memoryBlock
{
	size_t blockSize;
	struct memoryBlock* previous;
	struct memoryBlock* next;
};

typedef struct memoryBlock memoryBlock;
memoryBlock* memory;
void* memoryStart = NULL;

void init()
{
	memoryStart = malloc(sizeToAllocate * sizeof(size_t));
	if (!memoryStart)
	{
		printf("Allocation failure.");
		exit(-1);
	}
	memory = (memoryBlock*)memoryStart;
	memory->blockSize = sizeToAllocate;
	memory->previous = NULL;
	memory->next = NULL;
}

void myFree(void* ptr)
{
	if (!memoryStart)
		exit(-1);

	memoryBlock* block = (memoryBlock*)((unsigned char*)ptr - sizeof(size_t));
	if (!memory)
	{
		memory = block;
		block->previous = NULL;
		block->next = NULL;
		return;
	}

	memoryBlock* current = memory;
	memoryBlock* prev = NULL;
	while (current && current->next < block)
	{
		prev = current;
		current = current->next;
	}
	if (!prev)
	{
		block->next = memory;
		memory = block;
	}
	else
	{
		prev->next = block;
		block->previous = prev;
	}
	if (current)
		current->previous = block;
	block->next = current;

	if (((unsigned char*)block + block->blockSize) == (unsigned char*)current)
	{
		block->next = current->next;
		block->blockSize = block->blockSize + current->blockSize;
	}
	if (prev && (((unsigned char*)prev + prev->blockSize) == (unsigned char*)block))
	{
		prev->next = block->next;
		prev->blockSize = prev->blockSize + block->blockSize;
	}
}

memoryBlock* findNextFreeBlock(size_t size)
{
	memoryBlock* block = memory;
	while (block && block->blockSize < size + sizeof(size_t))
		block = block->next;
	return block;
}

void deleteBlock(memoryBlock* block)
{
	if (block->next && block->previous)
	{
		block->next->previous = block->previous;
		block->previous->next = block->next;
	}
	if (!block->next && block->previous)
	{
		block->previous->next = NULL;
	}
	if (block->next && !block->previous)
	{
		block->next->previous = NULL;
	}
}

void* myMalloc(size_t size)
{
	if (!memoryStart)
		init();

	memoryBlock* block = findNextFreeBlock(size + sizeof(size_t));
	if (!block)
		return NULL;

	if (block->blockSize < size + sizeof(size_t) + sizeof(memoryBlock))
	{
		deleteBlock(block);
		return (unsigned char*)block + sizeof(size_t);
	}

	block->blockSize = block->blockSize - size - sizeof(size_t);
	memoryBlock* ptr = (memoryBlock*)((unsigned char*)block + block->blockSize);
	ptr->blockSize = size + sizeof(size_t);
	return (unsigned char*)ptr + sizeof(size_t);
}

void* myRealloc(void* ptr, size_t newSize)
{
	if (!memoryStart)
		exit(-1);

	memoryBlock* block = (memoryBlock*)((unsigned char*)ptr - sizeof(size_t));

	if (block->blockSize < newSize + sizeof(size_t))
	{
		void* newPtr = myMalloc(newSize);
		if (!newPtr)
			return NULL;
		memcpy(newPtr, ptr, block->blockSize - sizeof(size_t));
		myFree(ptr);
		return newPtr;
	}
	else
	{
		return ptr;
	}
}