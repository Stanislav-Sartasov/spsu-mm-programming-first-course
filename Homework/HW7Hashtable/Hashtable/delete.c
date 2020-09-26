#include "hashTable.h"

void delete(int key, node** hashTable)
{

	if (hashTable[key % 13]->key == key)
		hashTable[key % 13] = hashTable[key % 13]->link;
	else
	{
		node* temp = hashTable[key % 13];
		node* prevTemp = hashTable[key % 13];
		while (temp->link)
		{
			if (temp->key == key)
			{
				prevTemp->link = temp->link;
				temp->key = NULL;
				break;
			}
			prevTemp = temp;
			temp = temp->link;
		}
		if (temp->key == key)
			prevTemp->link = NULL;
	}
}