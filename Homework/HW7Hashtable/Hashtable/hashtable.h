#ifndef HEADER_H
#define HEADER_H

#include <stdio.h>
#include <math.h>
typedef struct p
{
	int key;
	int value;
	struct p* link;
} node;


typedef struct
{
	node**  table;
	int elementsCount, tableSize;
	

	int* elementInLines;


} HashTable;

node* createNode(int key, int value);

HashTable* buildTable(int items);

int find(int key, HashTable* hashTable);
int delete(int key, HashTable* hashTable);
int insert(int key, int value, HashTable* hashTable);

#endif