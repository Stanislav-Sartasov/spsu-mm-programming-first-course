#pragma once

typedef struct
{
	unsigned char* key;
	unsigned key_size;
	unsigned char* data;
	unsigned data_size;
	struct element* right;
	struct element* left;
} element;

typedef struct
{
	unsigned max_depth;
	unsigned size;
	element** table;
} hash_table;

hash_table* hash_table_init();

hash_table* hash_table_insert(hash_table* hash, unsigned char* key, unsigned key_size, unsigned char* data, unsigned data_size, char balance_key);

element* hash_table_find(hash_table* hash, unsigned char* key, unsigned key_size);

void hash_table_delete(hash_table* hash, unsigned char* key, unsigned key_size);

void hash_table_free(hash_table* hash);