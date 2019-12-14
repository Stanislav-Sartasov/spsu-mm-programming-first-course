#include "stdio.h"
#include "stdlib.h"

#define REBALLANCE_SIZE 4
#define MAX_ELEM 10
#define NOT_FOUND -100

struct list_t {
	int key;
	int value;
	struct list_t *next;
};

typedef struct list_t node_t;


int hash_func(int num, int table_size)
{
	return num % table_size;
}

void init_table(node_t*** hash_table, int table_size)
{
	*hash_table = (node_t**) malloc(table_size * sizeof(node_t*));

	for (int i = 0; i < table_size; i++)
	{
		(*hash_table)[i] = NULL;
	}
}

void free_table(node_t** hash_table, int table_size)
{
	for (int i = 0; i < table_size; i++)
	{
		node_t* list = hash_table[i];
		node_t* list_next = NULL;

		while (list != NULL)
		{
			list_next = list->next;
			free(list);
			list = list_next;
		}
	}
	free(hash_table);
}

void insert_element_raw(node_t** hash_table, int table_size, int key, int value)
{
	int index = hash_func(key, table_size);

	node_t* list = hash_table[index];

	node_t* elem = (node_t*) malloc(sizeof(node_t));
	elem->next = NULL;
	elem->key = key;
	elem->value = value;

	if (list == NULL)
	{
		hash_table[index] = elem;
	}
	else
	{
		while (list->next != NULL)
		{
			list = list->next;
		}
		list->next = elem;
	}
}

void reballance(node_t*** hash_table, int* table_size)
{
	*table_size = *table_size + REBALLANCE_SIZE;
	node_t** new_hash_table = NULL;

	init_table(&new_hash_table, *table_size);

	for (int i = 0; i < *table_size - REBALLANCE_SIZE; i++)
	{
		node_t* list = (*hash_table)[i];

		while (list != NULL)
		{
			insert_element_raw(new_hash_table, *table_size, list->key, list->value);
			list = list->next;
		}
	}

	free_table(*hash_table, *table_size - REBALLANCE_SIZE);

	*hash_table = new_hash_table;
}

void insert_element(node_t*** hash_table, int* table_size, int key, int value)
{
	int index = hash_func(key, *table_size);

	node_t* list = (*hash_table)[index];

	node_t* new_element = (node_t*) malloc(sizeof(node_t));
	new_element->key = key;
	new_element->value = value;
	new_element->next = NULL;

	int count_elem = 0;

	if (list == NULL)
	{
		(*hash_table)[index] = new_element;
		count_elem++;
	}
	else
	{
		while (list->next != NULL)
		{
			list = list->next;
			count_elem++;
		}
		count_elem = count_elem + 2;
		list->next = new_element;
	}

	if (count_elem > MAX_ELEM)
	{
		reballance(hash_table, table_size);
	}
}

int delete_element(node_t** hash_table, int table_size, int key, int value)
{
	int index = hash_func(key, table_size);

	node_t* list = hash_table[index];

	if (list == NULL)
	{
		return 0;
	}

	if (list->key == key && list->value == value)
	{
		hash_table[index] = list->next;
		free(list);
		return 1;
	}

	node_t* prev_node = list;
	list = list->next;
	while (list != NULL)
	{
		if (list->key == key && list->value == value)
		{
			prev_node->next = list->next;
			free(list);
			return 1;
		}
		prev_node = list;
		list = list->next;
	}
	return 0;
}

int find_element(node_t** hash_table, int table_size, int key)
{
	int index = hash_func(key, table_size);

	node_t* list = hash_table[index];

	while (list != NULL)
	{
		if (list->key == key)
		{
			return list->value;
		}
		list = list->next;
	}
	return NOT_FOUND;
}

void print_hash_table(node_t** hash_table, int table_size)
{
	for (int i = 0; i < table_size; i++)
	{
		node_t* list = hash_table[i];

		while (list != NULL)
		{
			printf("(%d %d) ", list->key, list->value);
			list = list->next;
		}

		printf("\n");
	}
}

void consume_input()
{
	int symb;
	while ((symb = getchar()) != '\n' && symb != EOF);
}

int main()
{
	node_t** hash_table;

	int table_size = 3;

	init_table(&hash_table, table_size);

	int choice = -1;
	while (1)
	{
		printf("1 - Insert element\n2 - Delete element\n3 - Find element\n4 - Print table\n5 - Exit\n\n");
		printf("Your choice: ");
		
		if (scanf("%d", &choice) != 1)
		{
			printf("Incorrect input\n\n");
			consume_input();
			continue;
		}

		if (choice == 1)
		{
			int key, value;
			printf("Input key and value (int int): ");
			if (scanf("%d %d", &key, &value) != 2)
			{
				printf("Incorrect input\n\n");
				consume_input();
				continue;
			}
			insert_element(&hash_table, &table_size, key, value);
			printf("Element was inserted\n\n");
		}
		else if (choice == 2)
		{
			int key, value;
			printf("Input key and value (int int): ");
			if (scanf("%d %d", &key, &value) != 2)
			{
				printf("Incorrect input\n\n");
				consume_input();
				continue;
			}
			if (delete_element(hash_table, table_size, key, value) == 0)
			{
				printf("Element wasn't deleted\n\n");
			}
			else
			{
				printf("Element was deleted\n\n");
			}
		}
		else if (choice == 3)
		{
			int key;
			printf("Input key: ");
			if (scanf("%d", &key) != 1)
			{
				printf("Incorrect input\n\n");
				consume_input();
				continue;
			}
			int value = find_element(hash_table, table_size, key);
			if (value == NOT_FOUND)
			{
				printf("Element wasn't found\n\n");
			}
			else
			{
				printf("Element was found: %d %d\n\n", key, value);
			}
		}
		else if (choice == 4)
		{
			print_hash_table(hash_table, table_size);
		}
		else if (choice == 5)
		{
			break;
		}
		else
		{
			printf("Incorrect choice\n\n");
		}
	}

	free_table(hash_table, table_size);

	return 0;
}
