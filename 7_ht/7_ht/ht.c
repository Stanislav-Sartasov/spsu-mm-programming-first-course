#include "ht.h"

int* lengths;
int prime = 1009;

void allocationFailure()
{
	printf("Allocation failure.");
	exit(-1);
}

void printList(listNode* head)
{
	while (head != NULL)
	{
		printf("%d ", head->key);
		head = head->nextNode;
	}
	printf("\n");
}

listNode* createList(int headKey, int headData)
{
	listNode* head = (listNode*)malloc(sizeof(listNode));
	if (head == NULL) allocationFailure();
	head->key = headKey;
	head->data = headData;
	head->nextNode = NULL;
	head->previousNode = NULL;
	return head;
}

void insertAtHead(listNode** head, int key, int data)
{
	listNode* newNode = (listNode*)malloc(sizeof(listNode));
	if (newNode == NULL) allocationFailure();
	newNode->key = key;
	newNode->data = data;
	newNode->nextNode = (*head);
	newNode->previousNode = NULL;
	if ((*head) != NULL)
	{
		(*head)->previousNode = newNode;
	}
	(*head) = newNode;
}

listNode* findNode(listNode* head, int key)
{
	listNode* tmp = head;
	while (tmp != NULL)
	{
		if (tmp->key == key) return tmp;
		tmp = tmp->nextNode;
	}
	return NULL;
}

void deleteNode(listNode** head, int key)
{
	listNode* node = findNode(*head, key);
	if (node != NULL)
	{
		if (node->previousNode != NULL) node->previousNode->nextNode = node->nextNode;
		else *head = (*head)->nextNode;
		if (node->nextNode != NULL) node->nextNode->previousNode = node->previousNode;
		free(node);
		node = NULL;
	}
}

hashTable* createHT()
{
	hashTable* table = (hashTable*)malloc(sizeof(hashTable));
	if (table == NULL) allocationFailure();
	table->size = 15;
	table->ht = (listNode**)malloc(sizeof(listNode*) * table->size);
	if (table->ht == NULL) allocationFailure();
	lengths = (int*)calloc(table->size, sizeof(int));
	if (lengths == NULL) allocationFailure;
	table->a = rand() % prime + 1;
	table->b = rand() % prime;
	for (int i = 0; i < table->size; i++)
	{
		table->ht[i] = NULL;
	}
	return table;
}

int getHash(int key, int size, int a, int b)
{
	return ((a * key + b) % prime) % size;
}

int searchByKey(hashTable* table, int key)
{
	int hash = getHash(key, table->size, table->a, table->b);
	listNode* node = findNode(table->ht[hash], key);
	if (node != NULL)
		return 1;
	return -1;
}

hashTable* rebalanceHT(hashTable* oldHT)
{
	hashTable* table = (hashTable*)malloc(sizeof(hashTable));
	if (table == NULL) allocationFailure();
	table->size = 2 * oldHT->size;

	table->ht = (listNode**)malloc(sizeof(listNode*) * table->size);
	for (int i = 0; i < table->size; i++)
		table->ht[i] = NULL;
	table->a = rand() % prime + 1;
	table->b = rand() % prime;

	free(lengths);
	lengths = (int*)calloc(table->size, sizeof(int));
	if (lengths == NULL) allocationFailure;

	for (int i = 0; i < oldHT->size; i++)
	{
		listNode* head = oldHT->ht[i];
		while (head != NULL)
		{
			int newHash = getHash(head->key, table->size, table->a, table->b);
			if (table->ht[newHash] == NULL)
				table->ht[newHash] = createList(head->key, head->data);
			else
				insertAtHead(&table->ht[newHash], head->key, head->data);
			lengths[newHash]++;
			deleteNode(&head, head->key);
		}
	}
	free(oldHT);
	return table;
}

hashTable* insert(hashTable* table, int key, int data)
{
	int hash = getHash(key, table->size, table->a, table->b);
	if (table->ht[hash] == NULL)
		table->ht[hash] = createList(key, data);
	else
		insertAtHead(&table->ht[hash], key, data);

	lengths[hash]++;
	while (lengths[hash] > table->size)
	{
		table = rebalanceHT(table);
	}
	return table;
}

void deleteFromHT(hashTable* table, int key, int data)
{
	int hash = getHash(key, table->size, table->a, table->b);
	if (findNode(table->ht[hash], key) != NULL)
		deleteNode(&table->ht[hash], key);
}

void printHT(hashTable* table)
{
	for (int i = 0; i < table->size; i++)
		printList(table->ht[i]);
}

void freeHT(hashTable* mine)
{
	for (int i = 0; i < mine->size; i++)
	{
		listNode* head = mine->ht[i];
		while (head != NULL)
			deleteNode(&head, head->key);
	}
	free(mine);
	mine = NULL;
}