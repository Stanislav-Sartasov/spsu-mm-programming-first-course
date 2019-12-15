#pragma once
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <stdbool.h>

#define UNDEFINED_DATA -1

/*
	A data object saved in the hash table.
	@attribute data:	The data that the object holds.
	@attribute key:		The key of the object data.
*/
typedef struct data {
	int data;
	int key;
};


/*
	A hash table that can contain multiple integers.
	@attribute hashArray:	The array that holds multiple data objects.
	@attribute maxSize:		The maximum number a hash table can currently hold, it can be change.
*/
typedef struct hashTable {
	struct data* hashArray;
	int maxSize;
};


/*
	Calculates the hash code from a key.
	@param m_hashTable:	The hash table whoes hash needs to be calculated.
	@param m_key:		The key that needs to be converted.
	@return int:		The hash code for the hash table converted from the key.
*/
int hashCode(struct hashTable* m_hashTable, int m_key);


/*
	Search an element in the hash table.
	@param m_hashTable:	The hash table in which the item is being searched.
	@param m_key:		The key of the item that needs to be searched.
	@return data*:		The items address that matches the being looked item.
*/
struct data* search(struct hashTable* m_hashTable, int m_key);


/*
	Inserts an item into the hash table.
	@param m_hashTable:	The hash table to which the item is beeing inserted.
	@param m_key:		The key of the item that needs to be inserted.
	@param m_data:		The data of the item that needs to be inserted.
*/
void insert(struct hashTable* m_hashTable, int m_key, int m_data);


/*
	Deletes an item from the hash table.
	@param m_hashTable:	The hash table from which the item is being deleted.
	@param m_item:		The item that needs to be deleted.
	@return data*:
*/
struct data* deleteData(struct hashTable* m_hashTable, struct data* item);


/*
	Prints out all the elements in the hash table.
	@param m_hashTable:	The hash table which is being showed.
*/
void show(struct hashTable* m_hashTable);


/*
	Initializes a hash table.
	@param m_maxSize:	The maximum number of elements the hashTable can hold.
	@return hashTable*:	The hash table that has just been created.
*/
struct hashTable* initHashTable(int m_maxSize);


/*
	Free the allocated memories.
	@param m_hashTable:	The hash table that is being freed.
*/
void freeHashTable(struct hashTable* m_hashTable);