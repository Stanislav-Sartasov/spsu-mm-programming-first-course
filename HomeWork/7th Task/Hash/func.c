#include"Header.h"
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>

hash_table* init(int limit)
{
	hash_table* tmp = (hash_table*)malloc(sizeof(hash_table));
	tmp->size = 0;
	tmp->limit = limit;
	tmp->array_size = limit;
	tmp->array_list = (hash**)calloc(tmp->array_size, sizeof(hash*));
	return tmp;
}

int hash_func(int value, int size)
{
	return value % size;
}

void add(hash_table** table, int value, int key)
{
	int index = hash_func(key,(*table)->limit);

	if (((*table)->size) >= (*table)->limit)
		rebalance(table, value, key);

	else if ((*table)->array_list[index] == NULL)
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
	//printf("%d %c", (*table)->size, ' ');


}

int find(hash_table** table, int key)
{
	hash* pointer = ((*table)->array_list)[hash_func(key, (*table)->array_size)];
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
	int index = hash_func(key, table->array_size);
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

int rebalance(hash_table** table, int value, int key)
{
	hash_table* new_table = init((*table)->limit + 10);
	hash* pointer = NULL;
	for (int i = 0; i < (*table)->limit; i++)
	{
		pointer = (*table)->array_list[i];
		while (pointer)
		{
			add(&new_table, pointer->value, pointer->key);
			pointer = pointer->next;
		}
		free(pointer);
	}
	add(&new_table, value, key);
	free((*table)->array_list);
	free(*table);
	*table = new_table;
}

void delete_hash_table(hash_table** table)
{
	//printf("\n %d %s", (*table)->limit, "		");
	hash* pointer = NULL;
	int mas_size = (*table)->array_size;
	for (int i = 0; i < mas_size; i++)
	{
		pointer = (*table)->array_list[i];
		if (pointer)
		{
			while (pointer)
			{
				pointer = pointer->next;
			}
		}
		free(pointer);
	}
	free((*table)->array_list);
	free((*table));
	*table = NULL;
	//printf("Deleted\n");
}
