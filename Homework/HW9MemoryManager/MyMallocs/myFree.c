#include "myMalloc.h"

void myFree(void* ptr)
{
	MemoryBlock* currentBlock = memory.startBusyBlocksPtr;
	MemoryBlock* prevBlock = NULL;

	while (currentBlock)
	{
		if (ptr == currentBlock->startBytePtr)
			break;

		prevBlock = currentBlock;
		currentBlock = currentBlock->nextBlockPtr;
	}

	if (prevBlock)
		prevBlock->nextBlockPtr = currentBlock->nextBlockPtr;
	else
		memory.startBusyBlocksPtr = currentBlock->nextBlockPtr;

	if (memory.lastBusyBlockPtr == currentBlock)
		memory.lastBusyBlockPtr = prevBlock;

	currentBlock->nextBlockPtr = NULL;

	if (memory.lastFreeBlockPtr)
	{
		memory.lastFreeBlockPtr->nextBlockPtr = currentBlock;
		memory.lastFreeBlockPtr = memory.lastFreeBlockPtr->nextBlockPtr;
	}
	else
	{
		memory.startFreeBlocksPtr = currentBlock;
		memory.lastFreeBlockPtr = currentBlock;
	}

	for (int i = 0; i < currentBlock->size; ++i)
		*(currentBlock->startBytePtr + i) = '\0';

	ptr = NULL;
}