#include "allocator.h"

char* heap = NULL;
unsigned char* blockInfo;
size_t memorySize = 128;
size_t numberOfBlocks;
size_t memoryBeginning;
size_t initFlag = 0;
size_t newArray;

struct node
{
	struct node* next;
	struct node* previous;
	size_t blocks;
	size_t indexOfFirst;
	size_t address;
};

typedef struct node listNode;

listNode* head = NULL;
listNode* insertion = NULL;

size_t findNextFreeBlock(size_t size)
{
	for (size_t firstFreeBlock = 0; firstFreeBlock < numberOfBlocks; firstFreeBlock++)
	{
		if (blockInfo[firstFreeBlock] == 0)
		{
			size_t freeSize = 1;
			size_t nextBlock = firstFreeBlock + 1;
			while (nextBlock < numberOfBlocks && blockInfo[nextBlock] == 0 && freeSize < size)
			{
				nextBlock++;
				freeSize++;
			}

			if (freeSize < size)
			{
				if (nextBlock == numberOfBlocks)
					return -1;
				firstFreeBlock = nextBlock;
				continue;
			}
			return firstFreeBlock;
		}
	}
	return -1;
}

void* myMalloc(size_t size)
{
	newArray = 0;
	init();

	size_t blocks = (size % 4 == 0) ? (size / 4) : (size / 4 + 1);
	size_t firstFreeBlock = findNextFreeBlock(blocks);
	while (firstFreeBlock == -1)
	{
		initFlag = 2;
		init();
		firstFreeBlock = findNextFreeBlock(blocks);
	}

	listNode* node;
	node = insertion;
	node->blocks = blocks;
	node->indexOfFirst = firstFreeBlock;
	node->address = memoryBeginning + firstFreeBlock * 4;
	listNode* next = head;
	listNode* current = NULL;
	while (next != NULL && next->indexOfFirst < node->indexOfFirst)
	{
		current = next;
		next = next->next;
	}
	node->next = next;
	node->previous = current;
	if (node->previous)
		node->previous->next = node;
	else
		head = node;
	if (node->next)
		node->next->previous = node;
	insertion = NULL;

	for (long long i = 0; i < blocks; i++)
		blockInfo[firstFreeBlock + i] = 1;
		
	return (void*)&heap[node->indexOfFirst * 4];
}

void myFree(void* ptr)
{
	if (ptr == NULL) return;
	size_t address = (size_t)ptr;
	listNode* temp = head;
	while (temp != NULL && temp->address != address)
		temp = temp->next;

	if (temp != NULL)
	{
		for (size_t i = 0; i < temp->blocks; i++)
			blockInfo[temp->indexOfFirst + i] = 0;

		if (temp->next)
			temp->next->previous = temp->previous;
		if (!temp->previous)
			head = temp->next;
		else
			temp->previous->next = temp->next;
		ptr = NULL;
	}
	else
	{
		printf("Incorrect pointer value.\n");
		exit(-1);
	}
}

void* myRealloc(void* ptr, size_t size)
{
	if (ptr == NULL) return myMalloc(size);
	if (size == 0)
	{
		myFree(ptr);
		return;
	}

	size_t address = (size_t)ptr;
	listNode* temp = head;
	while (temp != NULL && temp->address != address)
		temp = temp->next;

	if (temp != NULL)
	{
		size = (size % 4 == 0) ? size : (size / 4 + 1);
		size_t firstFreeBlock = findNextFreeBlock(size / 4);
		while (firstFreeBlock == -1)
		{
			initFlag = 2;
			init();
			firstFreeBlock = findNextFreeBlock(size / 4);
		}
		for (size_t i = 0; i < temp->blocks * 4; i++)
		{
			heap[firstFreeBlock * 4 + i] = heap[temp->indexOfFirst * 4 + i];
		}
		for (size_t i = 0; i < temp->blocks; i++)
		{
			blockInfo[temp->indexOfFirst + i] = 0;
		}
		for (size_t i = 0; i < size / 4; i++)
		{
			blockInfo[firstFreeBlock + i] = 1;
		}
		temp->blocks = size / 4;
		temp->indexOfFirst = firstFreeBlock;
		temp->address = memoryBeginning + firstFreeBlock * 4;

		return (void*)&heap[temp->indexOfFirst * 4];
	}
	else
	{
		printf("Incorrect pointer value.\n");
		exit(-1);
	}
}

void init()
{
	if (!newArray)
	{
		listNode* temp = (listNode*)malloc(sizeof(listNode));
		insertion = temp;
		newArray = 1;
	}

	if (initFlag == 0)
	{
		initFlag = 1;
		heap = (char*)malloc(sizeof(char) * memorySize);
		memoryBeginning = heap;
		numberOfBlocks = memorySize / 4;
		blockInfo = (unsigned char*)malloc(sizeof(unsigned char) * numberOfBlocks);
		for (size_t i = 0; i < numberOfBlocks; i++)
			blockInfo[i] = 0;
	}
	else  if (initFlag == 2)
	{
		initFlag = 1;
		memorySize += 128;
		char* newHeap = (char*)realloc(heap, sizeof(char) * memorySize);
		if (newHeap)
		{
			heap = newHeap;
			memoryBeginning = heap;
			size_t oldBlockNum = numberOfBlocks;
			numberOfBlocks = memorySize / 4;
			unsigned char* newBlockInfo = (unsigned char*)realloc(blockInfo, sizeof(unsigned char) * numberOfBlocks);
			if (newBlockInfo)
			{
				blockInfo = newBlockInfo;
				for (size_t i = oldBlockNum; i < numberOfBlocks; i++)
					blockInfo[i] = 0;
			}
		}
		else
		{
			memorySize -= 128;
		}
	}
}