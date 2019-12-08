#pragma once

struct hash_table
{
	int* values;
	int* keys;
	int max_load;
	int load;
};

void create(struct hash_table* q);

void relocate(struct hash_table* q);

void insert(struct hash_table* q, int val, int k);

int search(struct hash_table* q, int k);

void remove_element(struct hash_table* q, int k);