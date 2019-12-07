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

char compare(unsigned char* x, unsigned x_size, unsigned char* y, unsigned y_size);

hash_table* hash_init();

hash_table* hash_add(hash_table* hash, unsigned char* key, unsigned key_size, unsigned char* data, unsigned data_size);

element* hash_get(hash_table* hash, unsigned char* key, unsigned key_size);

void hash_delete(hash_table* hash, unsigned char* key, unsigned key_size);

void hash_free(hash_table* hash);