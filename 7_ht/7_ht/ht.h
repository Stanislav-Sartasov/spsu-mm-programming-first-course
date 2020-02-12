#pragma once
#include <stdlib.h>
#include <stdio.h>

struct node
{
	int data;
	int key;
	struct node* nextNode;
	struct node* previousNode;
};
typedef struct node listNode;

struct table
{
	int size;
	int a;
	int b;
	listNode** ht;
};
typedef struct table hashTable;

hashTable* createHT();

hashTable* insert(hashTable* table, int key, int data);

int searchByKey(hashTable* table, int key);

hashTable* rebalanceHT(hashTable* oldHT);

void deleteFromHT(hashTable* table, int key, int data);

void printHT(hashTable* table);

void freeHT(hashTable* mine);