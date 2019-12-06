#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include "hashtable.h"

int main()
{
	hash_table* head = init();

	for (int i = 0; i < 5; i++)
		head = insert(head, i, i);
	print_table(head);
	for (int i = 0; i < 100; i++)
		head = insert(head, i, i);
	print_table(head);

	int input_key = 13;
	int value = search(head, input_key);
	if (value == -1)
		printf("Key not found\n");
	else
		printf("Find value is %d\n", value);

	for (int j = 0; j < 5; j++)
	{
		int delete_key = rand() % 100 + 1;
		printf("delete key %d\n", delete_key);
		find_and_delete(head, delete_key);
	}
	print_table(head);

	free_table(head);

	return 0;
}
