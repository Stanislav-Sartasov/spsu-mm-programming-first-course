#include "hashTable.h"

void nodePushback(int key, int value, node* fNode)
{

	node* begin = fNode;
	while (begin->link)
	{
		begin = begin->link;
	}
	node* nNode = createNode(key, value);
	begin->link = nNode;
}

