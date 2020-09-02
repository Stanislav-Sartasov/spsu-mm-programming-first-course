#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "HashTable.h"

int main()
{
	hashTable* hash;
	initHashTable(&hash, 4);
	printf("%d\n", hash->numOfLists);
	for (int i = 0; i < 1000; i++)
	{
		addPair(&hash, i, i);
	}
	printf("%d\n", hash->numOfLists);
	for (int i = 50; i < 70; i++)
	{
		printf("%d\n", findValue(hash, i));
	}
	for (int i = 0; i < 999; i++)
	{
		deletePair(hash, i);
	}
	deletePair(hash, 10);
	printAllPairs(hash);
	deleteHashTable(&hash);
	initHashTable(&hash, 4);
	for (int i = 0; i < 10; i++)
	{
		addPair(&hash, i, i);
	}
	printAllPairs(hash);
	deleteHashTable(&hash);
	printAllPairs(hash);
	addPair(&hash, 1, 1);
	return 0;
}