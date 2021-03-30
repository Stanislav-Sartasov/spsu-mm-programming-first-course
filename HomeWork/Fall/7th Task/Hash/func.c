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
	tmp->array_size = (int*)calloc(tmp->limit, sizeof(int));;
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
		(*table)->array_size[index] ++;
	}
	else
	{
		hash* tmp = (hash*)malloc(sizeof(hash));
		tmp->next = (*table)->array_list[index];
		tmp->value = value;
		tmp->key = key;
		tmp->next = (*table)->array_list[index];
		(*table)->array_list[index] = tmp;
		(*table)->array_size[index] ++;
	}



	(*table)->size++;
	//printf("%d %c", (*table)->size, ' ');


}

int find(hash_table** table, int key)
{
	hash* pointer = ((*table)->array_list)[hash_func(key, (*table)->limit)];
	while (pointer != NULL)
	{
		if (pointer->key == key)
			printf("%d%c", pointer->value, ' ');
		pointer = pointer->next;
	}

	return NULL;
}

void delete (hash_table* table, int key)
{
	int index = hash_func(key, table->limit);
	//printf(" %d", table->array_size[index], " ");
	if (table->array_list[index] != NULL)
	{
		if (table->array_list[index]->key == key)
		{
			table->array_list[index] = table->array_list[index]->next;
			table->array_size[index]--;
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
					table->array_size[index]--;
					table->size--;
					free(next);
					break;
				}
				next = next->next;
			}
			
		}
	}
	//printf(" %d", table->array_size[index], " ");
}

int rebalance(hash_table** table, int value, int key)
{
	hash_table* new_table = init((*table)->limit + 10);
	hash* pointer = NULL;
	int limit = (*table)->limit;
	for (int i = 0; i < limit; i++)
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
	//free((*table)->array_list);
	//free(*table);
	delete_hash_table(&(*table));
	*table = new_table;
	return NULL;
}

void delete_hash_table(hash_table** table)
{
	//printf("\n %d %s", (*table)->limit, "		");
	hash* pointer = NULL;
	hash* tmp = NULL;
	int mas_size = (*table)->limit;
	for (int i = 0; i < mas_size; i++)
	{
		pointer = (*table)->array_list[i];
		if (pointer)
		{
			while (pointer)
			{
				tmp = pointer;
				pointer = pointer->next;
				free(tmp);
			}
		}
		free(pointer);
	}
	free((*table)->array_list);
	free((*table)->array_size);
	free((*table));
	*table = NULL;
	//printf("Deleted\n");
}
