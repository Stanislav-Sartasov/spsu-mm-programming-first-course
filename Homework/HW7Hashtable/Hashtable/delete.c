#include "hashTable.h"

int delete(int key, HashTable* hashTable)
{
	int index = key % hashTable->tableSize;
	if (hashTable->table[index]->key == key)
	{
		node* temp = hashTable->table[index];
		hashTable->table[index] = hashTable->table[index]->link;
		free(temp);
		hashTable->elementsCount--;
		hashTable->elementInLines[index]--;
		return 0;
	}
	else
	{
		node* temp = hashTable->table[index];
		node* prevTemp = hashTable->table[index];
		while (temp->link)
		{
			if (temp->key == key)
			{
				prevTemp->link = temp->link;
				temp->key = NULL;
				free(temp);
				hashTable->elementsCount--;
				hashTable->elementInLines[index]--;
				return 0;
			}
			prevTemp = temp;
			temp = temp->link;
		}
		if (temp->key == key)
		{
			prevTemp->link = NULL;
			free(temp);
			hashTable->elementsCount--;
			hashTable->elementInLines[index]--;
			return 0;
		} 
	}
	return -1;
}