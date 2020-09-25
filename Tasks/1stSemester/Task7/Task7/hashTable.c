#include <stdlib.h>
#include "hashTable.h"

int hash(int a, int m)
{
	return a % m;
}

void create(struct hashTable* hashTable)
{
	hashTable->maxLength = 150;
	hashTable->currentLength = 0;
	hashTable->list = (struct cell*)malloc(hashTable->maxLength * sizeof(struct cell));
	for (int i = 0; i < 150; i++)
		hashTable->list[i].key = -1;
}

void relocate(struct hashTable* hashTable)
{
	hashTable->maxLength = hashTable->maxLength * 2;
	hashTable->list = (struct cell*)realloc(hashTable->list, hashTable->maxLength * sizeof(struct cell));
	for (int i = hashTable->maxLength / 2; i < hashTable->maxLength; i++)
		hashTable->list[i].key = -1;
}

void insert(struct hashTable* hashTable, int value, int key)
{
	if (hashTable->currentLength == hashTable->maxLength)
		relocate(hashTable);

	int hashResult = hash(key, hashTable->maxLength);
	if (hashTable->list[hashResult].key == -1)
	{
		hashTable->list[hashResult].key = key;
		hashTable->list[hashResult].value = value;
	}
	else
		for (int i = 1; hashTable->list[hashResult].key != -1; i++)
		{
			hashResult = hash(hash(key, hashTable->maxLength) + i, hashTable->maxLength);
			if (hashTable->list[hashResult].key == -1)
			{
				hashTable->list[hashResult].key = key;
				hashTable->list[hashResult].value = value;
				break;
			}
		}
	hashTable->currentLength += 1;
}

int search(struct hashTable* hashTable, int key)
{
	int hashResult = hash(key, hashTable->maxLength);
	if (hashTable->list[hashResult].key == key)
	{
		return  hashResult;
	}
	else
		for (int i = 1; hashTable->list[hashResult].key != key && i <= hashTable->maxLength; i++)
		{
			hashResult = hash(hash(key, hashTable->maxLength) + i, hashTable->maxLength);
			if (hashTable->list[hashResult].key == key)
			{
				return hashResult;
			}
		}
	return NULL;
}

void deleteCell(struct hashTable* hashTable, int key)
{
	if (search(hashTable, key) != NULL)
	{
		hashTable->list[search(hashTable, key)].key = -1;
		hashTable->currentLength -= 1;
	}
}

void freeMemory(struct hashTable* hashTable)
{
	for (int i = 0; i < hashTable->currentLength; i++)
		free(&(hashTable->list[i]));
	free(hashTable->list);
}