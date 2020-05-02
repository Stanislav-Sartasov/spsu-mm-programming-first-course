#include "hash.h"

hashTable* hashRebalance(hashTable* oldTable);

int hashGet(int key, int size)
{
	return (key % size);
}

listNode* findNode(listNode* head, int key)
{
	listNode* temp = head;

	while (temp != NULL)
	{
		if (temp->key == key)
		{
			return temp;
		}
		temp = temp->nextNode;
	}
	return NULL;
}

void deleteNode(listNode** head, int key)
{
	listNode* node = findNode(*head, key);

	if (node != NULL)
	{
		if (node->prevNode != NULL)
		{
			node->prevNode->nextNode = node->nextNode;
		}
		else
		{
			*head = (*head)->nextNode;
		}

		if (node->nextNode != NULL)
		{
			node->nextNode->prevNode = node->prevNode;
		}

		free(node);
		node = NULL;
	}
}

hashTable* hashCreate(int startSize)
{
	hashTable* table = (hashTable*)malloc(sizeof(hashTable));

	table->size = startSize;
	table->list = (listNode**)malloc(sizeof(listNode*) * table->size);
	table->maxLength = 0;
	table->maxLengthIndex = -1;

	for (int i = 0; i < table->size; i++)
	{
		table->list[i] = NULL;
	}
	return table;
}

hashTable* hashInsert(hashTable* table, int key, int data)
{
	int hash = hashGet(key, table->size);

	int currLength = 1;

	if (table->list[hash] == NULL)
	{
		listNode* head = (listNode*)malloc(sizeof(listNode));

		head->key = key;
		head->data = data;
		head->nextNode = NULL;
		head->prevNode = NULL;

		table->list[hash] = head;
	}
	else
	{
		listNode* newNode = (listNode*)malloc(sizeof(listNode));

		newNode->key = key;
		newNode->data = data;
		newNode->nextNode = table->list[hash];
		newNode->prevNode = NULL;

		table->list[hash]->prevNode = newNode;
		table->list[hash] = newNode;

		listNode* p = table->list[hash];

		while (p->nextNode != NULL)
		{
			p = p->nextNode;
			currLength++;
		}

		p = NULL;
	}

	if (currLength > table->maxLength)
	{
		table->maxLength = currLength;
		table->maxLengthIndex = hash;
	}

	while (table->maxLength >= table->size)
	{
		table = hashRebalance(table);
	}
	return table;
}

int searchKey(hashTable* table, int key)
{
	int hash = hashGet(key, table->size);

	listNode* node = findNode(table->list[hash], key);

	if (node != NULL)
	{
		return 1;
	}
	return 0;
}

void hashDelete(hashTable* table, int key)
{
	int hash = hashGet(key, table->size);

	if (findNode(table->list[hash], key) != NULL)
	{
		deleteNode(&table->list[hash], key);
	}

	if (hash == table->maxLengthIndex)
	{
		table->maxLength--;
	}
}

hashTable* hashRebalance(hashTable* oldTable)
{
	hashTable* newTable = hashCreate(2 * oldTable->size);

	for (int i = 0; i < oldTable->size; i++)
	{
		listNode* head = oldTable->list[i];
		while (head != NULL)
		{
			newTable = hashInsert(newTable, head->key, head->data);

			deleteNode(&head, head->key);
		}
	}

	free(oldTable);
	oldTable = NULL;

	return newTable;
}

void hashPrint(hashTable* table)
{
	for (int i = 0; i < table->size; i++)
	{
		while ((table->list[i]) != NULL)
		{
			printf("%d ", table->list[i]->key);
			table->list[i] = table->list[i]->nextNode;
		}
		printf("\n");
	}
}

void hashFree(hashTable* table)
{
	for (int i = 0; i < table->size; i++)
	{
		listNode* head = table->list[i];

		while (head != NULL)
		{
			deleteNode(&head, head->key);
		}
	}

	free(table);
	table = NULL;
}