#include <stdio.h>
#include <malloc.h>
#include <string.h>

struct hash_table
{
	int* values;
	int* keys;
	int max_load;
	int load;
};

void create(struct hash_table* q)
{
	q->max_load = 100;
	q->load = 0;
	q->values = (int*)(malloc(sizeof(int) * 100));
	q->keys = (int*)(malloc(sizeof(int) * 100));
	memset(q->keys, -1, sizeof(int) * 100);
}

int hash(int x, int n)
{
	int v = x % n;
	return v;
}


void add(struct hash_table* q, int val, int k)
{
	int res;
	res = hash(k, q->max_load);
	if (q->keys[res] == -1)
	{
		q->values[res] = val;
		q->keys[res] = k;
	}
	else
	{
		int i = res + 1;
		int flag = 1;
		while (i < q->max_load)
		{
			if (q->keys[i] == -1)
			{
				q->values[i] = val;
				q->keys[i] = k;
				flag = 0;
				break;
			}
			i += 1;
		}
		if (flag == 1)
		{
			i = 0;
			while (i < res)
			{
				if (q->keys[i] == -1)
				{
					q->values[i] = val;
					q->keys[i] = k;
					break;
				}
				i += 1;
			}
		}
	}

}

void relocate(struct hash_table* q)
{
	int length = q->max_load;
	int** copy_hash_table = (int**)malloc(2 * sizeof(int*));
	for (int i = 0; i < 2; i++)
	{
		copy_hash_table[i] = (int*)malloc(length / 2 * sizeof(int));
	}
	for (int i = 0; i < length / 2; i++)
	{
		copy_hash_table[0][i] = q->values[i];
		copy_hash_table[1][i] = q->keys[i];

	}
	memset(q->keys, -1, sizeof(int) * length);
	for (int i = 0; i < length / 2; i++)
	{
		add(q, copy_hash_table[0][i], copy_hash_table[1][i]);
	}
	for (int i = 0; i < 2; i++) {
		free(copy_hash_table[i]);
	}
	free(copy_hash_table);
}



void insert(struct hash_table* q, int val, int k)
{

	if (q->load == q->max_load)
	{
		q->max_load *= 2;
		q->values = (int*)realloc(q->values, sizeof(int) * q->max_load);
		q->keys = (int*)realloc(q->keys, sizeof(int) * q->max_load);
		relocate(q);
	}
	add(q, val, k);
	q->load += 1;
}

int search(struct hash_table* q, int k)
{
	int res;
	res = hash(k, q->max_load);
	if (q->keys[res] == k) return res;
	else
	{
		for (int i = res + 1; i < q->max_load; i++)
		{
			if (q->keys[i] == k) return i;
		}
	}
	for (int i = 0; i < res; i++)
	{
		if (q->keys[i] == k) return i;
	}
	return -1;
}

void remove_element(struct hash_table* q, int k)
{
	if (search(q, k) != -1)
	{
		printf("\nThe element deleted is %d.\n", q->values[search(q, k)]);
		q->keys[search(q, k)] = -1;
		q->load -= 1;
	}
}
