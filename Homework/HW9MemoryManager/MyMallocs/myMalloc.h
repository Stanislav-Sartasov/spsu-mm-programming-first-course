#include <stdio.h>

typedef struct
{
	char* startBytePtr;
	size_t size;
	void* nextBlockPtr;
} MemoryBlock;

typedef struct
{
	char* startBytePtr;
	char* freeAreaFirstBytePtr;

	MemoryBlock* startBusyBlocksPtr;
	MemoryBlock* startFreeBlocksPtr;
	MemoryBlock* lastBusyBlockPtr;
	MemoryBlock* lastFreeBlockPtr;

	unsigned int bytesCount;

} MyMemory;

MyMemory memory;

void init();
void* myMalloc(size_t size);
void* myRealloc(void* ptr, size_t size);
void myFree(void* ptr);