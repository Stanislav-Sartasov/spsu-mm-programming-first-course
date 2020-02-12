#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include "ht.h"


int main()
{
	hashTable* mine = createHT();
	for (int i = 0; i < 1000; i++)
		mine = insert(mine, i, i);
	for (int i = 20; i < 70; i++)
		deleteFromHT(mine, i, i);
	printHT(mine);
	if (searchByKey(mine, 20) == 1)
		printf("Element with a key %d is in a hash table.\n", 20);
	else
		printf("Element with a key %d is not found.\n", 20);
	freeHT(mine);

	return 0;
}