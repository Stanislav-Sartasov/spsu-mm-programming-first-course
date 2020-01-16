#pragma once

typedef struct Node 
{
	int value;               //значение
	int key;                 //ключ
	struct Node* next;       //адрес следующего узла
} Node;

typedef struct hashTable
{
	int NumOfLists;          //количество списков
	int MaxLenOfList;        //лимит длины списка
	int* LenOfList;          //длина каждого списка
	Node** arrayOfLists;     //массив списков
} hashTable;

void initHashTable(hashTable** table, int num);

void TableResize(hashTable** table);

int addPair(hashTable** table, int key, int value);

void deletePair(hashTable* table, int key);

int findValue(hashTable* table, int key);

void printAllPairs(hashTable* table);

void deleteHashTable(hashTable** table);


