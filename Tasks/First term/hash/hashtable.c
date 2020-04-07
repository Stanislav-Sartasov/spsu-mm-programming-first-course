#include "hashtable.h"

int control_lenght;
int index_max_lenght;
int* lenght;

int get_hash(int a, int m)
{
	return (a % m);
}

list* get_last(list* head)
{
	while (head->next)
		head = head->next;
	return head;
}

void add(list** head, int k, int x)
{
	if (*head == NULL)
	{
		*head = (list*)malloc(sizeof(list));
		(*head)->key = k;
		(*head)->data = x;
		(*head)->next = NULL;
	}
	else
	{
		list* last = get_last(*head);
		list* new_last = (list*)malloc(sizeof(list));
		new_last->key = k;
		new_last->data = x;
		new_last->next = NULL;
		last->next = new_last;
	}
}

void print(list* head)
{
	while (head)
	{
		printf("key %d : value %d\n", head->key, head->data);
		head = head->next;
	}
	printf("\n");
}

void print_table(hash_table* hash)
{
	printf("Start of table output\n");
	for (int i = 0; i < hash->size * hash->amount_blocks; i++)
	{
		printf("%d block\n", i);
		print(hash->table[i]);
	}
	printf("End of table output\n");
}

void free_memory(list* head)
{
	while (head)
	{
		list* next = head->next;
		free(head);
		head = next;
	}
}

int find(list* head, int k)
{
	while (head)
	{
		if (head->key == k)
			return (head->data);
		head = head->next;
	}
	return -1;
}


int delete_by_key(list** head, int k)
{
	if (*head)
	{
		if ((*head)->key == k)
		{
			list* curr = *head;
			*head = (*head)->next;
			free(curr);
			return 1;
		}
		list* prev = (*head);
		list* curr = (*head)->next;
		while (curr)
		{
			if (curr->key == k)
			{
				prev->next = curr->next;
				free(curr);
				return 1;
			}
			prev = curr;
			curr = curr->next;
		}
	}
	return 0;
}

hash_table* rebalance(hash_table* hash)
{
	(hash->amount_blocks)++;
	int m = hash->size * hash->amount_blocks;
	hash_table* new_hash = (hash_table*)malloc(sizeof(hash_table));
	new_hash->size = hash->size;
	new_hash->amount_blocks = hash->amount_blocks;
	new_hash->table = (list**)malloc(sizeof(list*) * m);
	for (int i = 0; i < m; i++)
		new_hash->table[i] = NULL;
	free(lenght);
	lenght = (int*)calloc(m, sizeof(int));
	control_lenght = 0;
	index_max_lenght = 0;
	for (int i = 0; i < m - hash->size; i++)
	{
		list* head = hash->table[i];
		while (head)
		{
			int k = head->key;
			int hash_k = get_hash(k, hash->size);
			int d = head->data;
			int curr_box = rand() % hash->amount_blocks;
			add(&new_hash->table[hash_k + hash->size * curr_box], k, d);
			lenght[hash_k + hash->size * curr_box]++;
			if (lenght[hash_k + hash->size * curr_box] > control_lenght)
			{
				control_lenght = lenght[hash_k + hash->size * curr_box];
				index_max_lenght = hash_k + hash->size * curr_box;
			}
			head = head->next;
		}
	}
	for (int i = 0; i < m - hash->size; i++)
		free_memory(hash->table[i]);
	free(hash);
	return new_hash;
}

void find_and_delete(hash_table* hash, int key)
{
	int k = get_hash(key, hash->size);
	for (int i = 0; i < hash->amount_blocks; i++)
	{
		if (delete_by_key(&hash->table[hash->size * i + k], key))
		{
			lenght[hash->size * i + k]--;
			if (hash->size * i + k == index_max_lenght)
				control_lenght--;
		}
	}
}

hash_table* insert(hash_table* hash, int key, int x)
{
	int k = get_hash(key, hash->size);
	int curr_box = rand() % hash->amount_blocks;
	find_and_delete(hash, key);
	add(&hash->table[k + hash->size * curr_box], key, x);
	lenght[k + hash->size * curr_box]++;
	if (lenght[k + hash->size * curr_box] > control_lenght)
	{
		control_lenght = lenght[k + hash->size * curr_box];
		index_max_lenght = k + hash->size * curr_box;
	}
	while (control_lenght > hash->size)
		hash = rebalance(hash);
	return hash;
}

hash_table* init()
{
	hash_table* hash = (hash_table*)malloc(sizeof(hash_table));
	hash->size = 7;
	hash->amount_blocks = 1;
	hash->table = (list**)malloc(sizeof(list*) * (hash->size) * (hash->amount_blocks));
	lenght = (int*)malloc(sizeof(int) * (hash->size) * (hash->amount_blocks));
	for (int i = 0; i < (hash->size) * (hash->amount_blocks); i++)
	{
		hash->table[i] = NULL;
		lenght[i] = 0;
	}
	return hash;
}

int search(hash_table* hash, int input_key)
{
	int k = get_hash(input_key, hash->size);
	for (int i = 1; i < hash->amount_blocks + 1; i++)
		return find(hash->table[k * i], input_key);
}

void free_table(hash_table* hash)
{
	for (int i = 0; i < hash->size * hash->amount_blocks; i++)
		free_memory(hash->table[i]);
	free(hash);
	free(lenght);
}
