#include "myMalloc.h"

void init()
{
	if (memory.startBytePtr)
	{
		free(memory.startBytePtr);
		return;
	}
	memory.bytesCount = 300;

	memory.startBytePtr = (char*)malloc(memory.bytesCount);
	memory.freeAreaFirstBytePtr = memory.startBytePtr;

	memory.startBusyBlocksPtr = NULL;
	memory.startFreeBlocksPtr = NULL;
	memory.lastBusyBlockPtr = NULL;
	memory.lastFreeBlockPtr = NULL;
}