#include "hash_table.h"
#include <stdio.h>

void insert(HashTable *table, int key, int v) 
{
	printf("[*] inserting k: %2d, v: %2d, status = ", key, v);
	if (hashTableInsertKey(table, key, v))
		printf("fail\n");
	else
		printf("ok\n");
}

void delete(HashTable *table, int key) 
{
	printf("[*] deleting  k: %2d"
			"         "
			"status = ok\n", key);
	hashTableDeleteKey(table, key);
}

void search(HashTable *table, int key) 
{
	printf("[*] searching k: %2d, ", key);
	int v = hashTableSearchKey(table, key, -1);

	if (v == -1)
		printf( "       "
				"status = key not found\n");
	else
		printf( "       "
				"status = key found v: %2d\n", v);
}

int main() 
{
	const char description[] = "This program implements a hash table "
								"data structure, with following 3 operations\n"
								"\t1. insert a key, value pair\n"
								"\t2. delete a key\n"
								"\t3. search a key\n\n";
	printf("%s", description);


	HashTable *table = hashTableCreate();

	if (!table) 
	{
		printf("[*] failed to create hash table\n");
		return -1;
    }

	insert(table, 1, 23);
	insert(table, 2, 45);
	insert(table, 3, 56);
	search(table, 2);
	delete(table, 3);
	search(table, 3);
	insert(table, 4, 65);
	search(table, 34);
	insert(table, 5, 76);
	delete(table, 1);
	search(table, 4);
	search(table, 5);
	search(table, 1);
	search(table, 2);

	hashTableDelete(table);
}