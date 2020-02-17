#pragma once
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
typedef struct hash {
	int key;
	int value;
	struct hash* next;
} hash;


typedef struct hash_table {
	int size;
	int* array_size;
	int limit;
	hash** array_list;
} hash_table;

void add(hash_table** table, int value, int key);
int find(hash_table** table, int key);
void delete (hash_table* table, int key);