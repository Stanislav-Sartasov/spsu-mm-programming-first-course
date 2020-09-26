#include "hashTable.h"

int find(int key, node** hashTable)
{
	int flag = 0;
	node* c = hashTable[key % 13];
	while (c->link)
	{
		if (c->key == key)
		{
			flag = 1;
			break;
		}
		c = c->link;
	}
	if (flag)
		return c->value;
	else
		return NULL;
}