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
	int maxLength;
	int maxLengthIndex;
	listNode** list;
};

typedef struct table hashTable;

hashTable* hashCreate(int startSize);

hashTable* hashInsert(hashTable* table, int key, int data);

int searchKey(hashTable* table, int key);

void hashDelete(hashTable* table, int key);

void hashPrint(hashTable* table);

void hashFree(hashTable* table);