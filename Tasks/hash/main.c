#include <stdio.h>
#include <stdlib.h>

typedef struct element
{
	int key;
	int data;
	struct element* next;
} list;

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

list** rebalance(list** hash, int size_block, int* amount_blocks, int** lenght, int* control_lenght, int* index_max_lenght)
{
	(*amount_blocks)++;
	int m = size_block * (*amount_blocks);
	list** new_hash = (list**)malloc(sizeof(list) * m);
	for (int i = 0; i < m; i++)
		new_hash[i] = NULL;
	free(*lenght);
	*lenght = (int*)calloc(m, sizeof(int));
	(*control_lenght) = 0;
	(*index_max_lenght) = 0;
	for (int i = 0; i < m - size_block; i++)
	{
		list* head = hash[i];
		while (head)
		{
			int k = head->key;
			int hash_k = get_hash(k, size_block);
			int d = head->data;
			int curr_box = rand() % (*amount_blocks);
			add(&new_hash[hash_k + size_block * curr_box], k, d);
			(*lenght)[hash_k + size_block * curr_box]++;
			if ((*lenght)[hash_k + size_block * curr_box] > (*control_lenght))
			{
				*control_lenght = (*lenght)[hash_k + size_block * curr_box];
				*index_max_lenght = hash_k + size_block * curr_box;
			}
			head = head->next;
		}
	}
	for (int i = 0; i < m - size_block; i++)
		free_memory(hash[i]);
	free(hash);
	return new_hash;
}

void find_and_delete(list** hash, int key, int size_block, int amount_blocks, int* control_lenght, int index_max_lenght, int* lenght)
{
	int k = get_hash(key, size_block);
	int i;
	for (i = 0; i < amount_blocks; i++)
	{
		if (delete_by_key(&hash[size_block * i + k], key))
		{
			lenght[size_block * i + k]--;
			if (size_block * i + k == index_max_lenght)
				(*control_lenght)--;
		}
	}
}

list** random_number_padding(list** hash, int number_of_value, int sampling_range, int size_block, int* amount_blocks, int* control_lenght, int* index_max_lenght, int** lenght)
{
	for (int i = 0; i < number_of_value; i++)
	{
		int k = get_hash(i, size_block);
		int x = rand() % sampling_range + 1;
		int curr_box = rand() % (*amount_blocks);
		find_and_delete(hash, i, size_block, *amount_blocks, control_lenght, *index_max_lenght, (*lenght));
		list* a = hash[k + size_block * curr_box];
		add(&hash[k + size_block * curr_box], i, x);
		(*lenght)[k + size_block * curr_box]++;
		if ((*lenght)[k + size_block * curr_box] > * control_lenght)
		{
			*control_lenght = (*lenght)[k + size_block * curr_box];
			*index_max_lenght = k + size_block * curr_box;
		}
		while (*control_lenght > size_block)
			hash = rebalance(hash, size_block, amount_blocks, lenght, control_lenght, index_max_lenght);
	}
	return hash;
}

int main()
{
	int size_block = 5;
	int amount_blocks = 1;
	list** hash = (list**)malloc(sizeof(list) * size_block * amount_blocks);
	int* lenght = (int*)calloc(size_block * amount_blocks, sizeof(int));
	int control_lenght = 0;
	int index_max_lenght = 0;
	for (int i = 0; i < size_block * amount_blocks; i++)
	{
		hash[i] = NULL;
		lenght[i] = 0;
	}
	hash = random_number_padding(hash, size_block * 8, 100, size_block, &amount_blocks, &control_lenght, &index_max_lenght, &lenght);

	for (int i = 0; i < size_block * amount_blocks; i++)
	{
		printf("%d block\n", i);
		print(hash[i]);
	}

	int input_key = 13;
	int find_flag = 1;
	int k = get_hash(input_key, size_block);
	for (int i = 1; i < amount_blocks + 1; i++)
	{
		int value = find(hash[k * i], input_key);
		if (value != -1)
		{
			printf("Find value is %d\n", value);
			find_flag = 0;
		}
	}
	if (find_flag)
		printf("Key not found\n");

	for (int j = 0; j < 5; j++)
	{
		int delete_key = rand() % 30 + 1;
		printf("delete key %d\n", delete_key);
		find_and_delete(hash, delete_key, size_block, amount_blocks, &control_lenght, index_max_lenght, lenght);
	}

	//hash = rebalance(hash, size_block, &amount_blocks, &lenght, &control_lenght, &index_max_lenght);

	for (int i = 0; i < size_block * amount_blocks; i++)
	{
		printf("%d block\n", i);
		print(hash[i]);
	}
	for (int i = 0; i < size_block * amount_blocks; i++)
		free_memory(hash[i]);
	free(hash);
	printf("\nEnd\n");
	return 0;
}