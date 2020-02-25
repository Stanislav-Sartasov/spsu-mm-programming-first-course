#pragma once

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
void delete_hash_table(hash_table** table);
hash_table* init(int limit);