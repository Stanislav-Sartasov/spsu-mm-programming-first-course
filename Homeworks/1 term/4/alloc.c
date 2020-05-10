#include <stdlib.h>
#include <string.h>
#include "alloc.h"

char* heap = NULL;
unsigned char* inf;

size_t memSize = 128, fl = 0;
size_t numOfNodes, memBegin, isNewArray;

struct node
{
	struct node* nextNode;
	struct node* previousNode;
	size_t nodes;
	size_t address;
	size_t indexFirst;
};

typedef struct node listNode;

listNode* head = NULL;
listNode* insert = NULL;

size_t nextFree(size_t size)
{
	for (size_t firstFreeNode = 0; firstFreeNode < numOfNodes; firstFreeNode++)
	{
		if (inf[firstFreeNode] == 0)
		{
			size_t freeSize = 1;
			size_t nextNode = firstFreeNode + 1;

			while (nextNode < numOfNodes && inf[nextNode] == 0 && freeSize < size)
			{
				nextNode++;
				freeSize++;
			}

			if (freeSize < size)
			{
				if (nextNode == numOfNodes)
				{
					return -1;
				}

				firstFreeNode = nextNode;
				continue;
			}
			return firstFreeNode;
		}
	}
	return -1;
}

void init()
{
	if (isNewArray == NULL)
	{
		listNode* temp = (listNode*)malloc(sizeof(listNode));
		insert = temp;
		isNewArray = 1;
	}

	switch (fl)
	{
	case 0:
		fl = 1;

		heap = (char*)malloc(sizeof(char) * memSize);
		memBegin = heap;
		numOfNodes = memSize / 4;

		inf = (unsigned char*)malloc(sizeof(unsigned char) * numOfNodes);
		for (size_t i = 0; i < numOfNodes; i++)
		{
			inf[i] = 0;
		}
		break;
	case 1:
		memSize = memSize - 128;
		break;
	case 2:
		fl = 1;

		memSize = memSize + 128;
		char* newHeap = (char*)realloc(heap, sizeof(char) * memSize);

		if (newHeap)
		{
			heap = newHeap;
			memBegin = heap;

			size_t oldNodeNum = numOfNodes;
			numOfNodes = memSize / 4;

			unsigned char* newInf = (unsigned char*)realloc(inf, sizeof(unsigned char) * numOfNodes);
			if (newInf != NULL)
			{
				inf = newInf;
				for (size_t i = oldNodeNum; i < numOfNodes; i++)
				{
					inf[i] = 0;
				}
			}
		}
		break;
	default:
		break;
	}
}

void* myMalloc(size_t size)
{
	isNewArray = 0;

	init();

	size_t nodes;

	if (size % 4 == 0)
	{
		nodes = size / 4;
	}
	else
	{
		nodes = size / 4 + 1;
	}

	size_t firstFreeNode = nextFree(nodes);

	while (firstFreeNode == -1)
	{
		fl = 2;

		init();

		firstFreeNode = nextFree(nodes);
	}

	listNode* node;
	node = insert;
	node->nodes = nodes;
	node->indexFirst = firstFreeNode;
	node->address = memBegin + firstFreeNode * 4;

	listNode* next = head;
	listNode* current = NULL;

	while (next != NULL && next->indexFirst < node->indexFirst)
	{
		current = next;
		next = next->nextNode;
	}

	node->nextNode = next;
	node->previousNode = current;

	if (node->previousNode)
	{
		node->previousNode->nextNode = node;
	}
	else
	{
		head = node;
	}
	if (node->nextNode)
	{
		node->nextNode->previousNode = node;
	}
	insert = NULL;

	for (long long i = 0; i < nodes; i++)
	{
		inf[firstFreeNode + i] = 1;
	}

	return (void*)&heap[node->indexFirst * 4];
}

void myFree(void* ptr)
{
	if (ptr == NULL)
	{
		return NULL;
	}

	size_t address = (size_t)ptr;
	listNode* temp = head;

	while (temp != NULL && temp->address != address)
	{
		temp = temp->nextNode;
	}

	if (temp != NULL)
	{
		for (size_t i = 0; i < temp->nodes; i++)
		{
			inf[temp->indexFirst + i] = 0;
		}

		if (temp->nextNode)
		{
			temp->nextNode->previousNode = temp->previousNode;
		}
		if (!temp->previousNode)
		{
			head = temp->nextNode;
		}
		else
		{
			temp->previousNode->nextNode = temp->nextNode;
		}
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
	if (ptr == NULL)
	{
		return myMalloc(size);
	}
	if (size == 0)
	{
		myFree(ptr);
		return NULL;
	}

	size_t address = (size_t)ptr;
	listNode* temp = head;
	while (temp != NULL && temp->address != address)
	{
		temp = temp->nextNode;
	}

	if (temp != NULL)
	{
		if (size % 4 != 0)
		{
			size = size / 4 + 1;
		}

		size_t firstFreeNode = nextFree(size / 4);
		while (firstFreeNode == -1)
		{
			fl = 2;

			init();

			firstFreeNode = nextFree(size / 4);
		}

		for (size_t i = 0; i < temp->nodes * 4; i++)
		{
			heap[firstFreeNode * 4 + i] = heap[temp->indexFirst * 4 + i];
		}

		for (size_t i = 0; i < temp->nodes; i++)
		{
			inf[temp->indexFirst + i] = 0;
		}
		for (size_t i = 0; i < size / 4; i++)
		{
			inf[firstFreeNode + i] = 1;
		}

		temp->nodes = size / 4;
		temp->indexFirst = firstFreeNode;
		temp->address = memBegin + firstFreeNode * 4;

		return (void*)&heap[temp->indexFirst * 4];
	}
	else
	{
		printf("Error: the pointer value is incorrect!\n");
		exit(-1);
	}
}
