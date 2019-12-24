#include "hashmap.h"
#include <stdlib.h>
#include <stdio.h>
#include <math.h>


void rebalance(HashTable *hashTable);

int hashCode(int key, int n) 
{
	return key % n;
}

void insert(HashTable *hashTable, int key, int value) 
{
	int hash = hashCode(key, hashTable->size);
	int basketSize = 0;
	Entry *basket = hashTable->baskets[hash];
	Entry *entry = basket;
	while (entry)
	{
		if (entry->key == key) 
		{
			entry->value = value;
			return;
		}
		entry = entry->next;
		basketSize++;
	}
	Entry *newEntry = (Entry *)malloc(sizeof(Entry));
	newEntry->key = key;
	newEntry->value = value;
	newEntry->next = basket;
	hashTable->baskets[hash] = newEntry;
	basketSize++;
	if (basketSize > log2(hashTable->size)) 
	{
		rebalance(hashTable);
	}
}


int lookup(HashTable *hashTable, int key) 
{
	int hash = hashCode(key, hashTable->size);
	Entry *entry = hashTable->baskets[hash];
	while (entry) 
	{
		if (entry->key == key)
			return entry->value;
		entry = entry->next;
	}
	return hashTable->defaultValue;

}

HashTable *createHashTable(int size, int defaultValue)
{
	HashTable *hashTable = (HashTable *)malloc(sizeof(HashTable));
	hashTable->size = size;
	hashTable->defaultValue = defaultValue;
	hashTable->baskets = (Entry **)malloc(sizeof(Entry) * size);
	for (int i = 0; i < size; ++i)
	{
		hashTable->baskets[i] = NULL;
	}
	return hashTable;
}

void delete(HashTable *hashTable, int key)
{
	int hash = hashCode(key, hashTable->size);
	Entry *entry = hashTable->baskets[hash];
	Entry *prev = NULL;
	while (entry) 
	{
		if (entry->key == key)
		{
			if (!prev) {
				hashTable->baskets[hash] = entry->next;
				free(entry);
				return;
			}
			prev->next = entry->next;
			free(entry);
			return;
		}
		prev = entry;
		entry = entry->next;
	}

}

void rebalance(HashTable *hashTable)
{
	int newSize = hashTable->size * 2;
	HashTable *newHashTable = createHashTable(newSize, hashTable->defaultValue);
	for (int i = 0; i < hashTable->size; ++i) 
	{
		Entry *entry = hashTable->baskets[i];
		while (entry) 
		{
			insert(newHashTable, entry->key, entry->value);
			entry = entry->next;
		}
	}
	freeTable(hashTable);
	hashTable->size = newSize;
	hashTable->baskets = newHashTable->baskets;
}

void freeTable(HashTable *hashTable)
{
	for (int i = 0; i < hashTable->size; ++i)
	{
		Entry *entry = hashTable->baskets[i];
		Entry *nextEntry = NULL;
		while (entry) 
		{
			nextEntry = entry->next;
			free(entry);
			entry = nextEntry;
		}
	}
	free(hashTable->baskets);
}

