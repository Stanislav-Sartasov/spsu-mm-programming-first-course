#pragma once
#include <stdio.h>
#include <stdlib.h>
#include "hash_table.h"

//’еш-функции построены таким образом, что элемент пройдет все €чейки таблицы
//это возможно благодар€ взаимной простоте размера массива и значени€ hash_function_step

int hash_function(int value, int size)
{
	int hash_result = (value % 132) % size;
	return hash_result;
}

int hash_function_step(int value, int size)
{
	int hash_result = value % (size - 1) + 1;
	return hash_result;
}

struct Hash_table initialization(int size)
{
	struct Hash_table table;
	table.size = size;
	table.full = 0;
	table.hash_list = (struct Cell*) calloc (size, sizeof(struct Cell));
	return table;
}

unsigned int sieve(int size)  // –ешето Ёратосфена
{
	unsigned int* block = (unsigned int*)calloc(size * 4, sizeof(unsigned int));
	for (int i = 2; i < size * 4; i++)
	{
		block[i] = i;
	}
	for (int i = 2; i < size * 4; i++)
	{
		if (block[i] != 0)
		{
			for (int j = i * 2; j < size * 4; j += i)
			{
				block[j] = 0;
			}
		}
	}
	for (int i = size * 2; i < size * 4; i++)
	{
		if (block[i] != 0)
		{
			unsigned int k = block[i];
			free(block);
			return k;
		}
	}
}

struct Hash_table balancing(struct Hash_table* table)
{
	if ((table->full + 1) * 4 >= table->size * 3)
	{
		int new_size = sieve(table->size);
		struct Hash_table new_table;
		new_table = initialization(new_size);
		for (int i = 0; i <= table->size; i++)
		{
			if (table->hash_list[i].remote == 2)
			{
				add_value(&new_table, table->hash_list[i].value);
			}
		}
		free(table->hash_list);
		return new_table;
	}
}

void add_value(struct Hash_table *table, int value)
{
	struct Cell new_cell;
	new_cell.value = value;
	new_cell.remote = 2;
	*table = balancing(table);
	int h1 = hash_function(new_cell.value, table->size);
	if (table->hash_list[h1].remote == 2)
	{
		int t = hash_function_step(new_cell.value, table->size);
		int k = 1;
		while (table->hash_list[(h1 + k * t) % table->size].remote == 2)
			k++;
		table->hash_list[(h1 + k * t) % table->size] = new_cell;
		table->full++;
	}
	else
	{
		table->hash_list[h1] = new_cell;
		table->full++;
	}
}

void delete_value(struct Hash_table* table, int value)
{
	int h1 = hash_function(value, table->size);
	int t = hash_function_step(value, table->size);
	int k = 0;
	while (table->hash_list[(h1 + t * k) % table->size].value != value)
	{
		if (table->hash_list[(h1 + t * k) % table->size].remote == 0)
			return printf("value not found\n");
		k++;
	}
	table->hash_list[(h1 + t * k) % table->size].remote = 1;
	return printf("value deleted\n");
}

void find_value(struct Hash_table *table, int value)
{
	int h1 = hash_function(value, table->size);
	int t = hash_function_step(value, table->size);
	int k = 0;
	while (table->hash_list[(h1 + t * k) % table->size].value != value)
	{
		if (table->hash_list[(h1 + t * k) % table->size].remote == 0)
			return printf("value not found\n");
		k++;
	}
	if (table->hash_list[(h1 + t * k) % table->size].remote == 2)
		return printf("value found and it is in cell number %d\n", (h1 + t * k) % table->size);
	return printf("value not found\n");
}

void output(struct Hash_table* table)
{
	for (int i = 0; i <= table->size - 1; i++)
	{
		printf("%d value: %d ", i, table->hash_list[i].value );
		printf("remote: %d \n", table->hash_list[i].remote);
	}
	return;
}
