#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <malloc.h>
#include <string.h>
#include <stdlib.h>

int correct_input(char str[])
{
	char digits[11] = "0123456789";
	int digit_true;
	if (str[0] != '0')
	{

		for (int l = 0; (l < strlen(str)); l++)
		{
			digit_true = 0;
			for (int n = 0; (n < 10); n++)
			{
				if (str[l] == digits[n]) digit_true = 1;
			}
			if (digit_true == 0) return 1;
		}
	}
	else return 1;
	return 0;
}

int input(char string[])
{
	int j = 0;
	while (j != 1)
	{
		(void)scanf("%s", string);
		if (correct_input(string) != 0) printf("Incorrect input. Enter a positive integer.\n");
		else j = 1;
	}
	return atoi(string);
}

struct hash_table
{
	int* values;
	int* keys;
	int max_load;
	int load;
};

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
	int res;
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
		relocate(q, q->max_load);
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
	if (1)
	{
		for (int i = 0; i < res; i++)
		{
			if (q->keys[i] == k) return i;
		}
	}
	return -1;
}

void delete(struct hash_table* q, int k)
{
	if (search(q, k) != -1)
	{
		printf("\nThe element deleted is %d.\n", q->values[search(q, k)]);
		q->keys[search(q, k)] = -1;
		q->load -= 1;
	}
}


int main()
{
	int exit = 1;
	int choice = 0;
	int key;
	int value;
	char s[21];

	struct hash_table p;
	p.max_load = 3;
	p.load = 0;
	p.values = (int*)(malloc(sizeof(int) * 3));
	p.keys = (int*)(malloc(sizeof(int) * 3));
	memset(p.keys, -1, sizeof(int) * 3);
	printf("\nMENU \n1.Insert \n2.Search \n3.Delete \n4.Exit \n");
	while (exit == 1)
	{
		printf("\nSelect menu item ");
		choice = input(s);
		if (choice == 1)
		{
			printf("\nEnter the value ");
			value = input(s);
			printf("\nEnter the key ");
			key = input(s);
			insert(&p, value, key, p.max_load);
		}
		else if (choice == 2)
		{
			printf("\nEnter the key: ");
			key = input(s);
			if (search(&p, key, p.max_load) == -1) printf("\nThe key searched was not found in the hash table");
			else printf("Searched element = %d\n", p.values[search(&p, key, p.max_load)]);
		}
		else if (choice == 3)
		{
			printf("\nEnter the key: ");
			key = input(s);
			delete(&p, key, p.max_load);
		}
		else if (choice == 4)
		{
			exit = 0;
		}
		else printf("Incorrrect input.\n");
	}

	free(p.keys);
	free(p.values);

	return 0;
}