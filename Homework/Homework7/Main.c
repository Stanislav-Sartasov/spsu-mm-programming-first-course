#include "Header.h"


// The program starts here.
int main() {
	struct hashTable* newHashTable = initHashTable(20);

	insert(newHashTable, 1, 20);
	insert(newHashTable, 2, 70);
	insert(newHashTable, 42, 80);
	insert(newHashTable, 4, 25);
	insert(newHashTable, 12, 44);
	insert(newHashTable, 14, 32);
	insert(newHashTable, 17, 11);
	insert(newHashTable, 13, 78);
	insert(newHashTable, 37, 97);

	printf("\nThe current hash table:\n");
	show(newHashTable);
	struct data* item = search(newHashTable, 37);

	if (item != NULL) {
		printf("Element found: %d\n", item->data);
	}
	else {
		printf("Element not found\n");
	}

	deleteData(newHashTable, item);
	item = search(newHashTable, 37);

	if (item != NULL) {
		printf("Element found: %d\n", item->data);
	}
	else {
		printf("Element not found\n");
	}
	getchar();
}