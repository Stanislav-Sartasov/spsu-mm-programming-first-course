#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "HashTable.h"

int main()
{
	hashTable* test;
	initHashTable(&test, 2);
	printf("%d\n", test->numOfLists);
	for (int i = 0; i < 10000; i++)
	{
		addPair(&test, i, i);
	}
	printf("%d\n", test->numOfLists);
	for (int i = 500; i < 700; i++)
		printf("%d\n", findValue(test, i));
	for (int i = 0; i < 9999; i++)
	{
		deletePair(test, i);
	}
	deletePair(test, 5);
	printAllPairs(test);
	deleteHashTable(&test);
	initHashTable(&test, 2);
	for (int i = 0; i < 100; i++)
	{
		addPair(&test, i, i);
	}
	printAllPairs(test);
	deleteHashTable(&test);
	printAllPairs(test);
	addPair(&test, 1, 1);
	return 0;
}