#pragma once

typedef struct Node 
{
	int value;               //��������
	int key;                 //����
	struct Node* next;       //����� ���������� ����
} Node;

typedef struct hashTable
{
	int NumOfLists;          //���������� �������
	int MaxLenOfList;        //����� ����� ������
	int* LenOfList;          //����� ������� ������
	Node** arrayOfLists;     //������ �������
} hashTable;

void initHashTable(hashTable** table, int num);

void TableResize(hashTable** table);

int addPair(hashTable** table, int key, int value);

void deletePair(hashTable* table, int key);

int findValue(hashTable* table, int key);

void printAllPairs(hashTable* table);

void deleteHashTable(hashTable** table);


