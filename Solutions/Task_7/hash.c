#include <stdio.h>
#include <stdlib.h>
#include "crc_32.h"

#define poly 0x4C11DB7, 0, 0, 0xFFFFFFFF	//Polynomial, Initial Value, Revers Flag, Final Xor Value
#define hash_base_size 128
#define hash_base_modifier 4
#define hash_max_depht 4

typedef struct
{
	unsigned char* key;
	unsigned key_size;
	unsigned char* data;
	unsigned data_size;
	struct element* right;
	struct element* left;
} element;

char compare(unsigned char* x, unsigned x_size, unsigned char* y, unsigned y_size)
{
	if (x_size > y_size)
		return 1;
	if (x_size < y_size)
		return -1;
	for (unsigned i = 0; i < x_size; i++)
		if (x[i] > y[i])
			return 1;
		else if (x[i] < y[i])
			return -1;
	return 0;
}

element* element_add(element* step, unsigned char* key, unsigned key_size, unsigned char* data, unsigned data_size)
{
	if (step == 0)
	{
		step = (element*)malloc(sizeof(element));
		step->key = (unsigned char*)malloc(key_size);
		step->key_size = key_size;
		step->data = (unsigned char*)malloc(data_size);
		step->data_size = data_size;
		for (unsigned i = 0; i < key_size; i++)
			step->key[i] = key[i];
		for (unsigned i = 0; i < data_size; i++)
			step->data[i] = data[i];
		step->left = 0;
		step->right = 0;
		return step;
	}
	unsigned code = 0;
	switch (compare(key, key_size, step->key, step->key_size))
	{
	case 1:
		if (step->right == 0)
			step->right = element_add(0, key, key_size, data, data_size);
		else
			code = (unsigned)element_add(step->right, key, key_size, data, data_size);
		break;
	case -1:
		if (step->left == 0)
			step->left = element_add(0, key, key_size, data, data_size);
		else
			code = (unsigned)element_add(step->left, key, key_size, data, data_size);
		break;
	case 0:
		free(step->data);
		step->data = (unsigned char*)malloc(data_size);
		step->data_size = data_size;
		for (unsigned i = 0; i < data_size; i++)
			step->data[i] = data[i];
		return 0;
	}
	return (element*)(code + 1);
}

element* element_get(element* step, unsigned char* key, unsigned key_size)
{
	if (step == 0)
		return 0;
	switch (compare(key, key_size, step->key, step->key_size))
	{
	case 1:
		return element_get(step->right, key, key_size);
	case -1:
		return element_get(step->left, key, key_size);
	case 0:
		return step;
	}
}

void element_free(element* step)
{
	free(step->data);
	free(step->key);
	free(step);
}

int element_delete(element* step, unsigned char* key, unsigned key_size)
{
	if (step == 0)
		return -1;
	int code;
	char is_right;
	char comp = compare(key, key_size, step->key, step->key_size);
	switch (comp)
	{
	case 1:
		code = element_delete(step->right, key, key_size);
		is_right = 1;
		break;
	case -1:
		code = element_delete(step->left, key, key_size);
		is_right = 0;
		break;
	case 0:
		if (step->right)
		{
			if (step->left == 0)
			{
				code = (int)step->right;
				element_free(step);
				return code;
			}
			element* right_low = step->right;
			element* pre_element = 0;
			while (right_low->left)
			{
				pre_element = right_low;
				right_low = right_low->left;
			}
			if (right_low->right)
			{
				free(step->key);
				free(step->data);
				step->key = (unsigned char*)malloc(right_low->key_size);
				step->key_size = right_low->key_size;
				step->data = (unsigned char*)malloc(right_low->data_size);
				step->data_size = right_low->data_size;
				for (unsigned i = 0; i < right_low->key_size; i++)
					step->key[i] = right_low->key[i];
				for (unsigned i = 0; i < right_low->data_size; i++)
					step->data[i] = right_low->data[i];
				code = element_delete(right_low, right_low->key, right_low->key_size);
				if (code == 1)
					if (pre_element == 0)
						step->right = 0;
					else
						pre_element->left = 0;
				else if (code != -1 && code != 0)
					if (pre_element == 0)
						step->right = (element*)code;

					else
						pre_element->left = (element*)code;
				return 0;
			}
			right_low->right = step->right;
			right_low->left = step->left;
			element_free(step);
			if (pre_element == 0)
				right_low->right = 0;
			else
				pre_element->left = 0;
			return (int)right_low;
		}
		else if (step->left)
		{
			code = (int)step->left;
			element_free(step);
			return code;
		}
		else
		{
			element_free(step);
			return 1;
		}
	}
	switch (code)
	{
	case -1:
		return -1;
	case 0:
		return 0;
	case 1:
		if (is_right)
			step->right = 0;
		else
			step->left = 0;
		return 0;
	}
	if (is_right)
		step->right = (element*)code;
	else
		step->left = (element*)code;
	return 0;
}

typedef struct
{
	unsigned max_depth;
	unsigned size;
	element** table;
} hash_table;

hash_table* hash_init()
{
	hash_table* hash = (hash_table*)malloc(sizeof(hash_table));
	hash->max_depth = 0;
	hash->size = hash_base_size;
	hash->table = (element * *)malloc(sizeof(element*) * hash->size);
	for (unsigned i = 0; i < hash->size; i++)
		hash->table[i] = 0;
	return hash;
}

unsigned hash_func(unsigned char* key, unsigned key_size)
{
	if (!crc_set_flag)
		crc_set(poly);
	return crc_calc(key, key_size);
}

hash_table* hash_add_back(hash_table* hash, unsigned char* key, unsigned key_size, unsigned char* data, unsigned data_size, char balance_key);

void hash_reinit_element(element* old_hash, hash_table* new_hash)
{
	if (old_hash == 0)
		return;
	if (old_hash->left)
		hash_reinit_element(old_hash->left, new_hash);
	if (old_hash->right)
		hash_reinit_element(old_hash->right, new_hash);
	if (new_hash)
		hash_add_back(new_hash, old_hash->key, old_hash->key_size, old_hash->data, old_hash->data_size, 0);
	element_free(old_hash);
}

hash_table* hash_balance(hash_table* hash_old, double modifier)
{
	hash_table* hash_new = (hash_table*)malloc(sizeof(hash_table));
	hash_new->max_depth = 0;
	hash_new->size = (unsigned)(hash_old->size * hash_base_modifier);
	hash_new->table = (element * *)malloc(sizeof(element*) * hash_new->size);
	for (unsigned i = 0; i < hash_new->size; i++)
		hash_new->table[i] = 0;
	for (unsigned i = 0; i < hash_old->size; i++)
		hash_reinit_element(hash_old->table[i], hash_new);
	free(hash_old->table);
	free(hash_old);
	return hash_new;
}

hash_table* hash_add_back(hash_table* hash, unsigned char* key, unsigned key_size, unsigned char* data, unsigned data_size, char balance_key)
{
	unsigned hash_key = (unsigned)hash_func(key, key_size) % hash->size;
	if (hash->table[hash_key])
	{
		unsigned depth = (unsigned)element_add(hash->table[hash_key], key, key_size, data, data_size);
		if (depth > hash->max_depth)
			hash->max_depth = depth;
		if (hash->max_depth > hash_max_depht && balance_key)
			return hash_balance(hash, hash_base_modifier);
	}
	else
		hash->table[hash_key] = element_add(hash->table[hash_key], key, key_size, data, data_size);
	return 0;
}

hash_table* hash_add(hash_table* hash, unsigned char* key, unsigned key_size, unsigned char* data, unsigned data_size)
{
	hash_add_back(hash, key, key_size, data, data_size, 1);
}

element* hash_get(hash_table* hash, unsigned char* key, unsigned key_size)
{
	unsigned hash_key = hash_func(key, key_size) % hash->size;
	return element_get(hash->table[hash_key], key, key_size);
}

void hash_delete(hash_table* hash, unsigned char* key, unsigned key_size)
{
	unsigned hash_key = hash_func(key, key_size) % hash->size;
	int code = (int)element_delete(hash->table[hash_key], key, key_size);
	switch (code)
	{
	case -1:
		break;
	case 0:
		break;
	case 1:
		hash->table[hash_key] = 0;
		break;
	default:
		hash->table[hash_key] = (element*)code;
	}
}

void hash_free(hash_table* hash)
{
	for (unsigned i = 0; i < hash->size; i++)
		hash_reinit_element(hash->table[i], 0);
	free(hash->table);
	free(hash);
}
