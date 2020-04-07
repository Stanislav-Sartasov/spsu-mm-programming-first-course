#ifndef HASHTABLE_H_INCLUDED
#define HASHTABLE_H_INCLUDED
#include <stdio.h>
#include <stdlib.h>

typedef struct element
{
	int key;
	int data;
	struct element* next;
} list;

typedef struct
{
	int size;
	int amount_blocks;
	list** table;
} hash_table;

void print_table(hash_table* hash);

void find_and_delete(hash_table* hash, int key);

hash_table* insert(hash_table* hash, int key, int x);

hash_table* init();

int search(hash_table* hash, int input_key);

void free_table(hash_table* hash);

#endif // HASHTABLE_H_INCLUDED
