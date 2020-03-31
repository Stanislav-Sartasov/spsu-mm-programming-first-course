#include "hash.h"

int* len;

int hashGet(int key, int size)
{
	return (key % size);
}

listNode* createList(int headKey, int headData)
{
	listNode* head = (listNode*)malloc(sizeof(listNode));

	head->key = headKey;
	head->data = headData;
	head->nextNode = NULL;
	head->prevNode = NULL;

	return head;
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

hashTable* hashCreate()
{
	hashTable* table = (hashTable*)malloc(sizeof(hashTable));

	table->size = 20;
	table->lst = (listNode**)malloc(sizeof(listNode*) * table->size);

	len = (int*)calloc(table->size, sizeof(int));

	for (int i = 0; i < table->size; i++)
	{
		table->lst[i] = NULL;
	}
	return table;
}

void hashHeadInsert(listNode** head, int key, int data)
{
	listNode* newNode = (listNode*)malloc(sizeof(listNode));

	newNode->key = key;
	newNode->data = data;
	newNode->nextNode = (*head);
	newNode->prevNode = NULL;

	if ((*head) != NULL)
	{
		(*head)->prevNode = newNode;
	}
	(*head) = newNode;
}

hashTable* hashInsert(hashTable* table, int key, int data)
{
	int hash = hashGet(key, table->size);

	if (table->lst[hash] == NULL)
	{
		table->lst[hash] = createList(key, data);
	}
	else
	{
		hashHeadInsert(&table->lst[hash], key, data);
	}

	len[hash]++;

	while (len[hash] > table->size)
	{
		table = hashRebalance(table);
	}
	return table;
}

int searchKey(hashTable* table, int key)
{
	int hash = hashGet(key, table->size);

	listNode* node = findNode(table->lst[hash], key);

	if (node != NULL)
	{
		return 1;
	}
	return 0;
}

void hashDelete(hashTable* table, int key, int data)
{
	int hash = hashGet(key, table->size);

	if (findNode(table->lst[hash], key) != NULL)
	{
		deleteNode(&table->lst[hash], key);
	}
}

hashTable* hashRebalance(hashTable* oldTable)
{
	hashTable* table = (hashTable*)malloc(sizeof(hashTable));

	table->size = 2 * oldTable->size;
	table->lst = (listNode**)malloc(sizeof(listNode*) * table->size);

	for (int i = 0; i < table->size; i++)
	{
		table->lst[i] = NULL;
	}

	free(len);
	len = (int*)calloc(table->size, sizeof(int));

	for (int i = 0; i < oldTable->size; i++)
	{
		listNode* head = oldTable->lst[i];
		while (head != NULL)
		{
			int newHash = hashGet(head->key, table->size);

			if (table->lst[newHash] == NULL)
			{
				table->lst[newHash] = createList(head->key, head->data);
			}
			else
			{
				hashHeadInsert(&table->lst[newHash], head->key, head->data);
			}

			len[newHash]++;

			deleteNode(&head, head->key);
		}
	}

	free(oldTable);
	return table;
}

void hashPrint(hashTable* table)
{
	for (int i = 0; i < table->size; i++)
	{
		while ((table->lst[i]) != NULL)
		{
			printf("%d ", table->lst[i]->key);
			table->lst[i] = table->lst[i]->nextNode;
		}
		printf("\n");
	}
}

void hashFree(hashTable* table)
{
	for (int i = 0; i < table->size; i++)
	{
		listNode* head = table->lst[i];

		while (head != NULL)
		{
			deleteNode(&head, head->key);
		}
	}

	free(table);
	table = NULL;
}
