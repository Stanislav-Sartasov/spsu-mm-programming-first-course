#define _CRT_SECURE_NO_WARNINGS
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>

typedef struct hash {
	int key;
	int value;
	struct hash* next;
} hash;


typedef struct hash_table {
	int size;
	int* array_size;
	int limit;
	hash** array_list;
} hash_table;

hash_table* init(int limit)
{
	hash_table* tmp = (hash_table*)malloc(sizeof(hash_table));
	tmp->size = 0;
	tmp->limit = limit;
	tmp->array_size = limit;
	tmp->array_list = (hash**)calloc(tmp->array_size, sizeof(hash*));
	return tmp;
}

int hash_func(int value)
{
	int key = value * 100;
	return key % 100;
}

void add(hash_table** table, int value, int key)
{
	int index = hash_func(key);
	
	if (((*table)->size) >= (*table)->limit)
		rebalance(table);

		if ((*table)->array_list[index] == NULL)
		{
			hash* element = (hash*)malloc(sizeof(hash));
		    element->next = NULL;
			element->key = key;
			element->value = value;
			(*table)->array_list[index] = element;
		}
		else
		{
			hash* tmp = (hash*)malloc(sizeof(hash));
			tmp->next = (*table)->array_list[index];
			tmp->value = value;
			tmp->key = key;
			tmp->next = (*table)->array_list[index];
			(*table)->array_list[index] = tmp;

		}

	(*table)->size++;
	

}

int find(hash_table** table, int key)
{
	hash* pointer = ((*table)->array_list)[hash_func(key)];
	while (pointer)
	{
		if (pointer->key == key)
			printf("%d%c", pointer->value, ' ');
		pointer = pointer->next;
	}
	return NULL;
}

void delete (hash_table* table, int key)
{
	int index = hash_func(key);
	if (table->array_list[index] != NULL)
	{
		if (table->array_list[index]->key == key)
		{
			table->array_list[index] = table->array_list[index]->next;
			table->size--;
		}
		else
		{
			hash* next = table->array_list[index]->next;
			while (next)
			{
				if (next->key == key)
				{
					table->array_list[index]->next = next->next;
					table->size--;
					break;
				}
				next = next->next;
			}
		}
	}
}

int rebalance(hash_table** table)
{
	hash_table* new_table = init((*table)->limit + 1);
	hash* pointer = NULL;
	for (int i = 0; i < (*table)->limit; i++)
	{
		pointer = (*table)->array_list[i];
		while (pointer)
		{
			add(&new_table, pointer->key, pointer->value);
			pointer = pointer->next;
		}
	}
	return new_table;
}

int main()
{
	hash_table* map = init(1);
	add(&map, 15, 10);
	add(&map, 5, 10);
	add(&map, 10, 20);

	find(&map, 10);
	printf("\n");
	find(&map, 20);
	delete(map, 10);
	
	find(&map, 10);
	delete(map, 10);
	find(&map, 10);

	return 0;
}