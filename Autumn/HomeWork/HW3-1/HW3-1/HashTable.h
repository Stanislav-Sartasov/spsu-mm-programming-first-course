#pragma once

typedef struct node
{
	int value;
	int key;
	struct node* next;
} node;

typedef struct hashTable
{
	int numOfLists;
	int maxLenOfList;
	int* lenOfList;
	node** arrayOfLists;
} hashTable;

void initHashTable(hashTable** table, int num);
int addPair(hashTable** table, int key, int value);
void deletePair(hashTable* table, int key);
int findValue(hashTable* table, int key);
void printAllPairs(hashTable* table);
void deleteHashTable(hashTable** table);