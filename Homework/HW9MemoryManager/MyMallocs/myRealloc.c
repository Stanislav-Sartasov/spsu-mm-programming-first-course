#include "myMalloc.h"

void* myRealloc(void* ptr, size_t size)
{
	if (!size && !ptr)
		return NULL;

	if (size == 0 && ptr)
	{
		myFree(ptr);
		return NULL;
	}

	if (!ptr)
		return myMalloc(size);

	MemoryBlock* currentBlock = memory.startBusyBlocksPtr;
	MemoryBlock* prevBlock = NULL;

	while (currentBlock)
	{
		if (ptr == currentBlock->startBytePtr)
			break;

		prevBlock = currentBlock;
		currentBlock = currentBlock->nextBlockPtr;
	}

	if (currentBlock->size == size)
		return currentBlock->startBytePtr;
	else if (currentBlock->size > size)
	{
		if (currentBlock->size - size <= sizeof(MemoryBlock))
			return currentBlock->startBytePtr;

		void* startNewBlockPtr = currentBlock->startBytePtr + size;

		MemoryBlock newBlock;
		newBlock.size = currentBlock->size - size;
		newBlock.nextBlockPtr = NULL;
		newBlock.startBytePtr = startNewBlockPtr;

		*(MemoryBlock*)startNewBlockPtr = newBlock;

		if (memory.lastFreeBlockPtr)
		{
			memory.lastFreeBlockPtr->nextBlockPtr = startNewBlockPtr;
			memory.lastFreeBlockPtr = memory.lastFreeBlockPtr->nextBlockPtr;
		}
		else
		{
			memory.startFreeBlocksPtr = startNewBlockPtr;
			memory.lastFreeBlockPtr = startNewBlockPtr;
		}

		currentBlock->size = size;

		return currentBlock->startBytePtr;
	}

	MemoryBlock* relevantFreeBlock = memory.startFreeBlocksPtr;
	MemoryBlock* prevRelevantFreeBlock = NULL;


	while (relevantFreeBlock)
	{
		if (currentBlock->startBytePtr + currentBlock->size == relevantFreeBlock)
			break;

		relevantFreeBlock = (MemoryBlock*)relevantFreeBlock->nextBlockPtr;
	}

	if (relevantFreeBlock && relevantFreeBlock->size + sizeof(MemoryBlock) + currentBlock->size >= size)
	{
		if (prevRelevantFreeBlock)
			prevRelevantFreeBlock->nextBlockPtr = relevantFreeBlock->nextBlockPtr;
		else
			memory.startFreeBlocksPtr = relevantFreeBlock->nextBlockPtr;

		if (memory.lastFreeBlockPtr == relevantFreeBlock)
			memory.lastFreeBlockPtr = prevRelevantFreeBlock;

		unsigned int newBlockSize = relevantFreeBlock->size - (size - currentBlock->size);
		MemoryBlock* newBlockPtr = currentBlock->startBytePtr + size;

		MemoryBlock newBlock;
		newBlock.size = newBlockSize;
		newBlock.nextBlockPtr = NULL;
		newBlock.startBytePtr = newBlockPtr + sizeof(MemoryBlock);

		*newBlockPtr = newBlock;

		if (memory.lastFreeBlockPtr)
		{
			memory.lastFreeBlockPtr->nextBlockPtr = newBlockPtr;
			memory.lastFreeBlockPtr = memory.lastFreeBlockPtr->nextBlockPtr;
		}
		else
		{
			memory.startFreeBlocksPtr = newBlockPtr;
			memory.lastFreeBlockPtr = newBlockPtr;
		}

		currentBlock->size = size;
	}
	else
	{
		if (memory.bytesCount - (memory.freeAreaFirstBytePtr - memory.startBytePtr) < size + sizeof(MemoryBlock))
			return NULL;

		MemoryBlock newBlock;
		newBlock.size = size;
		newBlock.nextBlockPtr = NULL;
		newBlock.startBytePtr = memory.freeAreaFirstBytePtr + sizeof(MemoryBlock);

		*((MemoryBlock*)memory.freeAreaFirstBytePtr) = newBlock;

		memory.lastBusyBlockPtr->nextBlockPtr = memory.freeAreaFirstBytePtr;
		memory.lastBusyBlockPtr = (MemoryBlock*)memory.lastBusyBlockPtr->nextBlockPtr;

		for (int i = 0; i < currentBlock->size; ++i)
		{
			*(memory.lastBusyBlockPtr->startBytePtr + i) = *(currentBlock->startBytePtr + i);
			*(currentBlock->startBytePtr) = '\0';
		}

		myFree(currentBlock->startBytePtr);

		currentBlock = memory.lastBusyBlockPtr;
	}

	return currentBlock->startBytePtr;
}