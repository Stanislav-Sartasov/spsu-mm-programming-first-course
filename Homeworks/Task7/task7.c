#define _CRT_SECURE_NO_WARNINGS
#include "hashmap.h"
#include <stdio.h>
#include <stdlib.h>
#include <time.h>


int main() {
	char str[1000];
	int initialSize;
	int dataSize;
	int max = 20;
	srand(time(0));
	while (1) {
		printf("Enter hash table initial size: ");
		scanf("%s", str);

		initialSize = atoi(str);

		if (initialSize > 0) {
			break;
		}
		printf("You entered incorrect value\n");
	}
	while (1) {
		printf("Enter random data size: ");
		scanf("%s", str);

		dataSize = atoi(str);

		if (dataSize > 0) {
			break;
		}
		printf("You entered incorrect value\n");
	}
	HashTable *table = createHashTable(initialSize, 0);

	for (int i = 0; i < dataSize; i++) {
		int key = rand() % max;
		int value = rand();
		printf("Inserting key=%d value=%d in table\n", key, value);
		insert(table, key, value);
		print(table);
	}

	for (int i = 0; i < max; ++i) {
		if (lookup(table, i) != 0) {
			printf("Deleting key=%d from table\n", i);
			delete(table, i);
			print(table);
		}
	}
	return 0;
}

