#pragma once
#include <stdlib.h>
#include <stdio.h>

struct node
{
	int data;
	int key;
	struct node* nextNode;
	struct node* prevNode;
};

typedef struct node listNode;

struct table
{
	int size;
	listNode** lst;
};

typedef struct table hashTable;

hashTable* hashCreate();

hashTable* hashInsert(hashTable* table, int key, int data);

hashTable* hashRebalance(hashTable* oldTable);

int searchKey(hashTable* table, int key);

void hashDelete(hashTable* table, int key, int data);

void hashPrint(hashTable* table);

void hashFree(hashTable* table);