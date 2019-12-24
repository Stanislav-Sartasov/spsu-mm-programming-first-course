#ifndef PROJECT_HASHMAP_H
#define PROJECT_HASHMAP_H

typedef struct Entry
{
	int key;
	int value;
	struct Entry *next;
} Entry;

typedef struct HashTable 
{
	int size;
	int defaultValue;
	struct Entry **baskets;
} HashTable;

int hashCode(int key, int n);

void insert(HashTable *hashTable, int key, int value);

int lookup(HashTable *hashTable, int key);

void delete(HashTable *hashTable, int key);

HashTable *createHashTable(int size, int defaultValue);

void freeTable(HashTable *hashTable);


#endif //PROJECT_HASHMAP_H
