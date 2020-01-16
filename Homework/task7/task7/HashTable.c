#define _CRT_SECURE_NO_WARNINGS
#include "HashTable.h"
#include <string.h>
#include <stdio.h>
#include <stdlib.h>


void initHashTable(hashTable** table, int num)
{
	(*table) = (hashTable*)malloc(sizeof(hashTable));
	(*table)->NumOfLists = num;
	(*table)->MaxLenOfList = num / 4;
	(*table)->arrayOfLists = (Node**)calloc((*table)->NumOfLists, sizeof(Node*));
	(*table)->LenOfList = (int*)calloc((*table)->NumOfLists, sizeof(int));
}

unsigned int hash(int key, int size)
{
	return key % size;
}

void TableResize(hashTable** table)
{
	hashTable* newTable;
	initHashTable(&newTable, (*table)->NumOfLists * 2);
	for (int i = 0; i < (*table)->NumOfLists; i++)
	{
		Node* pointer = ((*table)->arrayOfLists)[i];
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
		int pos = hash(key, (*table)->NumOfLists);
		Node* new = (Node*)malloc(sizeof(Node));
		new->key = key;
		new->value = value;
		new->next = NULL;

		if ((*table)->arrayOfLists[pos] == NULL)
		{
			(*table)->arrayOfLists[pos] = new;
			(*table)->LenOfList[pos]++;
		}
		else if (findValue(*table, key))
			return 0;
		else
		{
			new->next = (*table)->arrayOfLists[pos];
			(*table)->arrayOfLists[pos] = new;
			(*table)->LenOfList[pos]++;
		}
		if (((*table)->LenOfList)[pos] >= (*table)->MaxLenOfList)
			TableResize(table);
	}
	else
		printf("initialize the table first!\n\n");
}

void deletePair(hashTable* table, int key)
{
	if (table)
	{
		int pos = hash(key, table->NumOfLists);
		Node* pointer = table->arrayOfLists[pos];
		if (pointer->key == key)
		{
			table->arrayOfLists[pos] = pointer->next;
			table->LenOfList[pos]--;
			free(pointer);
		}
		else
		{
			Node* next = pointer->next;
			while (next)
			{
				if (next->key == key)
				{
					pointer->next = next->next;
					table->LenOfList[pos]--;
					free(next);
					break;
				}
				pointer = next;
				next = next->next;
			}
		}
	}
}

int findValue(hashTable* table, int key)
{
	if (table)
	{
		Node* pointer = (table->arrayOfLists)[hash(key, table->NumOfLists)];
		while (pointer)
		{
			if (pointer->key == key)
				return pointer->value;
			pointer = pointer->next;
		}
		return NULL;
	}
}

void printAllPairs(hashTable* table)
{
	if (table)
	{
		printf("list of pairs:\n\n");
		for (int i = 0; i < table->NumOfLists; i++)
		{
			Node* pointer = (table->arrayOfLists[i]);
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
	for (int i = 0; i < (*table)->NumOfLists; i++)
	{
		Node* pointer = (*table)->arrayOfLists[i];
		if (pointer)
		{
			while (pointer)
			{
				Node* prevPointer = pointer;
				pointer = pointer->next;
				free(prevPointer);
			}
		}
	}
	free((*table)->LenOfList);
	free((*table)->arrayOfLists);
	free((*table));
	*table = NULL;
	printf("the table has been deleted!\n\n");
}