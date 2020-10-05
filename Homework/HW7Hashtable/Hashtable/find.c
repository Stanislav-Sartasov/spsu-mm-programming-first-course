#include "hashTable.h"

int find(int key, HashTable* hashTable)
{
	
	node* c = hashTable->table[key % hashTable->tableSize];

	do
	{
		if (c && c->key == key)
			return c->value;
		else if(c)
			c = c->link;
	} while (c);
	return -1;
}