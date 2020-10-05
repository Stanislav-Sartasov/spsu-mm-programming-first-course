#include "hashTable.h"

HashTable* buildTable(int items)
{
	HashTable* hashTable = malloc(sizeof(HashTable)*items);
	hashTable->elementsCount = 0;
	hashTable->tableSize = items;
	hashTable->elementInLines = calloc(items, sizeof(int));
	hashTable->table = calloc(items, sizeof(node));


	return hashTable;
}