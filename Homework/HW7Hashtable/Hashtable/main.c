#include "hashTable.h"
void elementscount(HashTable* hashTable )
{
	printf("commandcount: %d\n", hashTable->elementsCount);
	for (int i = 0; i < hashTable->tableSize; i++)
		printf("line %d: %d\n", i, hashTable->elementInLines[i]);

}

int main()
{

	HashTable* hashTable = buildTable(1);

	int action;
	int key, value;
	do
	{
		printf("this prog...\n choose your action\n 1- add element\n 2- remove element\n 3- find element\n 4- exit\n");
		scanf_s("%d", &action);

		switch (action)
		{
		case 1:
			printf("input key\n");
			scanf_s("%d", &key);
			printf("input value\n");
			scanf_s("%d", &value);
			if (insert(key, value, hashTable) == -1)
				printf("this key exists\n");
			else
				printf("node inserted\n");

			//elementscount(hashTable);
			break;
		case 2:
			printf("input key\n");
			scanf_s("%d", &key);
			if (delete(key, hashTable) == -1)
				printf("not found\n");
			else
			{
				printf("node removed\n");
				//elementscount(hashTable);
			}
			break;
		case 3:
			printf("input key\n");
			scanf_s("%d", &key);
			int foundValue = find(key, hashTable);
			if (foundValue == -1)
				printf("key not found\n");
			else
				printf("%d\n", foundValue);
			break;
		}
	} while (action != 4);

	return 0;
}