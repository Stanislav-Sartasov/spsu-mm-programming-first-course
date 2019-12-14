#ifndef _HASHTABLE_H
#define _HASHTABLE_H

struct cell
{
	int key;
	int value;
};

struct hashTable
{
	int currentLength;
	int maxLength;
	struct cell* list;
};


void create(struct hashTable* hashTable);

void insert(struct hashTable* hashTable, int value, int key);

int search(struct hashTable* hashTable, int key);

void deleteCell(struct hashTable* hashTable, int key);

void freeMemory(struct hashTable* hashTable);

#endif