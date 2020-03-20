#define _CRT_SECURE_NO_WARNINGS
#include "HashTable.h"
#include <string.h>
#include <stdio.h>
#include <stdlib.h>


void initHashTable(hashTable** table, int num)
{
	(*table) = (hashTable*)malloc(sizeof(hashTable));
	(*table)->numOfLists = num;
	(*table)->maxLenOfList = num / 4;
	(*table)->arrayOfLists = (node**)calloc((*table)->numOfLists, sizeof(node*));
	(*table)->lenOfList = (int*)calloc((*table)->numOfLists, sizeof(int));
}

unsigned int hash(int key, int size)
{
	return key % size;
}

void tableResize(hashTable** table)
{
	hashTable* newTable;
	initHashTable(&newTable, (*table)->numOfLists * 2);
	for (int i = 0; i < (*table)->numOfLists; i++)
	{
		node* pointer = ((*table)->arrayOfLists)[i];
		if (pointer)
		{
			while (pointer)
			{
				addPair(&newTable, pointer->key, pointer->value);
				pointer = pointer->next;
			}
		}
	}
	free(*table);
	*table = newTable;
}

int addPair(hashTable** table, int key, int value)
{
	if (*table)
	{
		int pos = hash(key, (*table)->numOfLists);
		node* new = (node*)malloc(sizeof(node));
		new->key = key;
		new->value = value;
		new->next = NULL;

		if ((*table)->arrayOfLists[pos] == NULL)
		{
			(*table)->arrayOfLists[pos] = new;
			(*table)->lenOfList[pos]++;
		}
		else if (findValue(*table, key))
			return 0;
		else
		{
			new->next = (*table)->arrayOfLists[pos];
			(*table)->arrayOfLists[pos] = new;
			(*table)->lenOfList[pos]++;
		}
		if (((*table)->lenOfList)[pos] >= (*table)->maxLenOfList)
			tableResize(table);
	}
	else
		printf("initialize the table first!\n\n");
	return 0;
}

void deletePair(hashTable* table, int key)
{
	if (table)
	{
		if (findValue(table, key))
		{
			int pos = hash(key, table->numOfLists);
			node* pointer = table->arrayOfLists[pos];
			if (pointer->key == key)
			{
				table->arrayOfLists[pos] = pointer->next;
				table->lenOfList[pos]--;
				free(pointer);
			}
			else
			{
				node* next = pointer->next;
				while (next)
				{
					if (next->key == key)
					{
						pointer->next = next->next;
						table->lenOfList[pos]--;
						free(next);
						break;
					}
					pointer = next;
					next = next->next;
				}
			}
		}
	}
}

int findValue(hashTable* table, int key)
{
	if (table)
	{
		node* pointer = (table->arrayOfLists)[hash(key, table->numOfLists)];
		while (pointer)
		{
			if (pointer->key == key)
				return pointer->value;
			pointer = pointer->next;
		}
		return 0;
	}
	return 0;
}

void printAllPairs(hashTable* table)
{
	if (table)
	{
		printf("list of pairs:\n\n");
		for (int i = 0; i < table->numOfLists; i++)
		{
			node* pointer = (table->arrayOfLists[i]);
			while (pointer)
			{
				printf("key: %d \nvalue: %d\n\n", pointer->key, pointer->value);
				pointer = pointer->next;
			}
		}
	}
	else
		printf("table not found!\n\n");
}

void deleteHashTable(hashTable** table)
{
	for (int i = 0; i < (*table)->numOfLists; i++)
	{
		node* pointer = (*table)->arrayOfLists[i];
		if (pointer)
		{
			while (pointer)
			{
				node* prevPointer = pointer;
				pointer = pointer->next;
				free(prevPointer);
			}
		}
	}
	free((*table)->lenOfList);
	free((*table)->arrayOfLists);
	free((*table));
	*table = NULL;
	printf("the table has been deleted!\n\n");
}