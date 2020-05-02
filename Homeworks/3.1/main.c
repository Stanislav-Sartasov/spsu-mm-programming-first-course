#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include "hash.h"

int main()
{
	int startSize = 20;
	hashTable* table = hashCreate(startSize);

	for (int i = 0; i < 1500; i++) 
	{
		table = hashInsert(table, i, i);
	}

	for (int i = 50; i < 135; i++)
	{
		hashDelete(table, i);
	}

	hashPrint(table);

	int num = 60;
	if (searchKey(table, num))
	{
		printf("The element with a key %d is in a hash table\n", num);
	}
	else
	{
		printf("The element with a key %d is not in a hash table\n", num);
	}

	hashFree(table);

	return 0;
}