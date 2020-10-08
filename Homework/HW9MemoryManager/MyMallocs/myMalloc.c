#include "myMalloc.h"

void* myMalloc(size_t size)
{
	MemoryBlock newBlock;
	newBlock.size = size;
	newBlock.nextBlockPtr = NULL;
	newBlock.startBytePtr = memory.freeAreaFirstBytePtr + sizeof(MemoryBlock);

	if (!memory.startBusyBlocksPtr && !memory.startFreeBlocksPtr)
	{
		memory.startBusyBlocksPtr = memory.startBytePtr;
		*(memory.startBusyBlocksPtr) = newBlock;
		memory.freeAreaFirstBytePtr += sizeof(MemoryBlock) + size;

		memory.lastBusyBlockPtr = memory.startBusyBlocksPtr;

		return memory.startBusyBlocksPtr->startBytePtr;
	}

	MemoryBlock* relevantFreeBlock = memory.startFreeBlocksPtr;
	MemoryBlock* prevRelevantFreeBlock = NULL;


	MemoryBlock* currentBlock = relevantFreeBlock;

	while (currentBlock && currentBlock->nextBlockPtr)
	{
		if (((MemoryBlock*)currentBlock->nextBlockPtr)->size < relevantFreeBlock->size
			&& ((MemoryBlock*)currentBlock->nextBlockPtr)->size >= size)
		{
			prevRelevantFreeBlock = currentBlock;
			relevantFreeBlock = (MemoryBlock*)currentBlock->nextBlockPtr;

			if (relevantFreeBlock->size == size)
				break;
		}

		currentBlock = (MemoryBlock*)currentBlock->nextBlockPtr;
	}

	if (relevantFreeBlock && relevantFreeBlock->size >= size)
	{
		if (prevRelevantFreeBlock)
			prevRelevantFreeBlock->nextBlockPtr = relevantFreeBlock->nextBlockPtr;
		else
			memory.startFreeBlocksPtr = relevantFreeBlock->nextBlockPtr;

		if (memory.lastFreeBlockPtr == relevantFreeBlock)
			memory.lastFreeBlockPtr = prevRelevantFreeBlock;

		if (memory.lastBusyBlockPtr)
		{
			memory.lastBusyBlockPtr->nextBlockPtr = relevantFreeBlock;
			memory.lastBusyBlockPtr = memory.lastBusyBlockPtr->nextBlockPtr;
		}
		else
		{
			memory.lastBusyBlockPtr = relevantFreeBlock;
			memory.startBusyBlocksPtr = relevantFreeBlock;
		}

		relevantFreeBlock->nextBlockPtr = NULL;
	}
	else
	{
		if (memory.bytesCount - (memory.freeAreaFirstBytePtr - memory.startBytePtr) <= size + sizeof(MemoryBlock))
			return NULL;

		*((MemoryBlock*)memory.freeAreaFirstBytePtr) = newBlock;

		memory.lastBusyBlockPtr->nextBlockPtr = memory.freeAreaFirstBytePtr;
		memory.lastBusyBlockPtr = (MemoryBlock*)memory.lastBusyBlockPtr->nextBlockPtr;

		memory.freeAreaFirstBytePtr += sizeof(MemoryBlock) + size;

		relevantFreeBlock = memory.lastBusyBlockPtr;
	}

	return relevantFreeBlock->startBytePtr;
}