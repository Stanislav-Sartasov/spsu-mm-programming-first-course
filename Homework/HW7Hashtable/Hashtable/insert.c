#include "hashTable.h"
int isPrimeNumber(int numb)
{
	for (int n = 2; n < sqrt(numb)+1; n++)
		if (numb % n == 0)
			return -1;
	return 0;
}


int insert(int key, int value, HashTable* hashTable)
{
	int index = key % hashTable->tableSize;
	if (hashTable->elementInLines[index] > hashTable->elementsCount / 2)
	{
		hashTable->tableSize++;
		while (isPrimeNumber(hashTable->tableSize) == -1)
		{
			hashTable->tableSize++;
		}

		node* tempList = NULL;
		node* currentNode = NULL;

		for (int i = 0; i < hashTable->tableSize; i++)
		{
			node* c = hashTable->table[i];

			while (c)
			{
				if (currentNode)
				{
					currentNode->link = c;
					currentNode = currentNode->link;
				}
				else
				{
					currentNode = c;
					tempList = currentNode;
				}
				c = c->link;
			}
			
		}
		free(hashTable->table);
		free(hashTable->elementInLines);
		hashTable->elementsCount = 0;

		
		hashTable->elementInLines = calloc(hashTable->tableSize, sizeof(int));

		/*
		printf("\n\n");
		for (int i = 0; i < hashTable->tableSize; i++)
		{
			printf("%d ", hashTable->elementInLines[i]);
			
		}
		printf("\n\n");*/

		hashTable->table = calloc(hashTable->tableSize, sizeof(node));
		currentNode = tempList;
		while (currentNode)
		{
			insert(currentNode->key, currentNode->value, hashTable);
			currentNode = currentNode->link;
			
		}
		currentNode = NULL;
		tempList = NULL;
		index = key % hashTable->tableSize;

	}

	if (hashTable->table[index])
	{
		
		node* c = hashTable->table[index];
		while (c->link)
		{
			if (c->key == key)
			{
				return -1;
			}
			c = c->link;
			
		} 
		
		c->link = createNode(key, value);
		
	}
	else
		hashTable->table[index] = createNode(key, value);

	hashTable->elementsCount++;
	hashTable->elementInLines[index]++;

	return 0;
}