#include <stdio.h>
#include "hashTable.h"

int main()
{
	struct hashTable hashTable;
	create(&hashTable);

	int result = search(&hashTable, 1234);
	if (result == NULL)
		printf("Not found\n");
	else
		printf("%d\n", hashTable.list[result].value);

	insert(&hashTable, 100, 1234);
	result = search(&hashTable, 1234);
	if (result == NULL)
		printf("Not found\n");
	else
		printf("%d\n", hashTable.list[result].value);

	deleteCell(&hashTable, 1234);
	result = search(&hashTable, 1234);
	if (result == NULL)
		printf("Not found\n");
	else
		printf("%d\n", hashTable.list[result].value);

	freeMemory(&hashTable);

	return 0;
}