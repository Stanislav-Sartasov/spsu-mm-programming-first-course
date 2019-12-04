#include <stdio.h>
#include <stdlib.h>
#include "hash.h"


int main()
{
	unsigned key;
	hash_table* head = hash_table_init();
	int code;
	element* el;
	for (int key = 0; key < 2000000; key++)
	{
		code = (int)hash_table_insert(head, &key, sizeof(int), &key, sizeof(int), 1);
		if (code)
		{
			head = (hash_table*)code;
		}
	}

	char* str_key = "hello_hash";
	char* str_data = "yes it's working";
	hash_table_insert(head, str_key, 11, str_data, 17, 0);	

	for (int i = 0; i < 2000000; i++)
	{
		el = hash_table_find(head, &i, sizeof(int));
		if (el == 0)
			printf("error\n");
		else if (*((int*)el->data) != i)
			printf("error\n");
		hash_table_delete(head, &i, sizeof(int));
		if (hash_table_find(head, &i, sizeof(int)))
			printf("error\n");
	}

	el = hash_table_find(head, str_key, 11);
	if (el == 0)
		printf("error\n");
	else if (compare(el->data, el->data_size, str_data, 17) == 0)
		printf("still working for any data and keys!\n");
	else
		printf("error\n");

	hash_table_delete(head, str_key, 11);
	if (hash_table_find(head, str_key, 11))
		printf("but deleting error\n");

	printf("final size in count of buckets = %u\n", head->size);
	printf("if it isn't any error messages then solution work correctly\n");
	hash_table_free(head);
}