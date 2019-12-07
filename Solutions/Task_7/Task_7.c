#include <stdio.h>
#include <stdlib.h>
#include "hash.h"


int main()
{
	hash_table* head = hash_init();
	int code;
	element* el;
	for (int key = 0; key < 2000000; key++)
	{
		code = (int)hash_add(head, &key, sizeof(int), &key, sizeof(int));
		if (code)
		{
			head = (hash_table*)code;
		}
	}

	char* str_key = "hello_hash";
	char* str_data = "yes it's working";
	hash_add(head, str_key, 11, str_data, 17);	

	for (int i = 0; i < 2000000; i++)
	{
		el = hash_get(head, &i, sizeof(int));
		if (el == 0)
			printf("error\n");
		else if (*((int*)el->data) != i)
			printf("error\n");
		hash_delete(head, &i, sizeof(int));
		if (hash_get(head, &i, sizeof(int)))
			printf("error\n");
	}

	el = hash_get(head, str_key, 11);
	if (el == 0)
		printf("error\n");
	else if (compare(el->data, el->data_size, str_data, 17) == 0)
		printf("still working for any data and keys!\n");
	else
		printf("error\n");

	hash_delete(head, str_key, 11);
	if (hash_get(head, str_key, 11))
		printf("but deleting error\n");

	printf("final size in count of buckets = %u\n", head->size);
	printf("if it isn't any error messages then solution work correctly\n");
	hash_free(head);
}