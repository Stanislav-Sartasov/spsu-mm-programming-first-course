#pragma once

typedef struct node 
{
	int value;               //значение
	int key;                 //ключ
	struct node* next;       //адрес следующего узла
} node;

typedef struct hashTable
{
	int numOfLists;          //количество списков
	int maxLenOfList;        //лимит длины списка
	int* lenOfList;          //длина каждого списка
	node** arrayOfLists;     //массив списков
} hashTable;

void initHashTable(hashTable** table, int num);

void tableResize(hashTable** table);

int addPair(hashTable** table, int key, int value);

void deletePair(hashTable* table, int key);

int findValue(hashTable* table, int key);

void printAllPairs(hashTable* table);

void deleteHashTable(hashTable** table);


