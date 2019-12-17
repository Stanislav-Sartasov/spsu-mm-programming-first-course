#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <malloc.h>
#include <string.h>
#include <stdlib.h>

int g_number;

struct Entry
{
	int		key;
	int		value;
};

int hash(int key)
{
	return (key % g_number);
}

int insert(int key, int value, struct Entry** table)
{
	int hash_value = hash(key);
	struct Entry* new_entry;
	int init_value = hash_value;

	while(table[hash_value] != NULL)
	{
		hash_value = (hash_value + 1) % g_number;
		if (hash_value == init_value)
		{
			return 0;
		}
	}

	new_entry = (struct Entry*)malloc(sizeof(struct Entry));
	new_entry->key = key;
	new_entry->value = value;

	table[hash_value] = new_entry;
	return 1;
}

int search(int key, struct Entry** table, int* pValue)
{
	int hash_value = hash(key);

	while (table[hash_value] != NULL)
	{
		if (key == table[hash_value]->key)
		{
			*pValue = table[hash_value]->value;
			return 1;
		}
		else
		{
			hash_value = (hash_value + 1) % g_number;
		}
	}

	return 0;
}

int delete_entry(int key, struct Entry** table)
{
	int hash_value = hash(key);

	while (table[hash_value] != NULL)
	{
		if (key == table[hash_value]->key)
		{
			free(table[hash_value]);
			table[hash_value] = NULL;
			return 1;
		}
		else
		{
			hash_value = (hash_value + 1) % g_number;
		}
	}

	return 0;
}

int main()
{
	struct Entry** table;
	char str[1000];
	int key, value, i;

	for (;;)
	{
		printf("please input the number for hash table(this number need to be grater than all elements):");
		scanf("%s", str);

		g_number = atoi(str);

		if (g_number > 0)
		{
			break;
		}
		printf("You entered incorrect numbers\n");
	}

	table = (struct Entry**)malloc(sizeof(struct Entry*) * g_number);
	if (!table)
	{
		printf("Cant' alloc the hash's table.\n");
		return -1;
	}
	memset(table, 0, sizeof(struct Entry*) * g_number);
	
	//srand(time(NULL));
	
	for (i = 0; i < 10 ; i++)
	{
		key = rand() % g_number;
		value = rand();
		
		if (insert(key, value, table))
		{
			printf("The insertion is successful: Key => %d, Value => %d\n", key, value);
		}
		else
		{
			printf("The insertion(key=%d, value=%d) failed.\n", key, value);
		}
	}
	
	// Search and delete records for all possible keys
	for (i = 0; i < g_number ; ++i)
	{
		if (search(i, table, &value))
		{
			printf("Deleting: Key => %i, Value => %i\n", i, value);
			delete_entry(key, table);
		}
	}
	
	return 0;
}

