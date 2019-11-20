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

int relocate(int** q, int length)
{
	int res;
	int** copy_hash_table = (int**)malloc(2 * sizeof(int*));
	for (int i = 0; i < 2; i++)
	{
		copy_hash_table[i] = (int*)malloc(length / 2 * sizeof(int));
	}
	for (int i = 0; i < length / 2; i++)
	{
		copy_hash_table[0][i] = q[0][i];	
		copy_hash_table[1][i] = q[1][i];

	}
	memset(q[1], -1, sizeof(int) * length);
	for (int i = 0; i < length / 2; i++)
	{
		res = copy_hash_table[1][i] % length;
		q[0][res] = copy_hash_table[0][i];
		q[1][res] = copy_hash_table[1][i];
	}
	for (int i = 0; i < 2; i++) {
		free(copy_hash_table[i]);
	}
	free(copy_hash_table);
}

void insert(int** p, int val, int k, int mod)
{
	int res;
	int i = 0;
	res = k % mod;
	if (p[1][res] == -1)
	{
		p[0][res] = val;
		p[1][res] = k;
	}
	else
	{
		int i = res + 1;
		int flag = 1;
		while (i < mod)
		{
			if (p[1][i] == -1)
			{
				p[0][i] = val;
				p[1][i] = k;
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
				if (p[1][i] == -1)
				{
					p[0][i] = val;
					p[1][i] = k;
					break;
				}
				i += 1;
			}
		}
	}
}

int search(int** q, int key, int mod)
{
	int res;
	
	res = key % mod;
	if (q[1][res] == key) return res;
	else
	{
		for (int i = res + 1; i < mod; i++)
		{
			if (q[1][i] == key) return i;
		}
	}
	if (1)
	{
		for (int i = 0; i < res; i++)
		{
			if (q[1][i] == key)	return i;
		}
	}
	return -1;
}

void delete(int** q, int key, int mod)
{
	if (search(q, key, mod) != -1)
	{
		printf("\nThe element deleted is %d.\n", q[0][search(q, key, mod)]);
		q[1][search(q, key, mod)] = -1;
	}
}


int main()
{
	int n = 100;
	int exit = 1;
	int choice = 0;
	int amount = 0;
	int key;
	int value;
	char s[21];

	int** hash_table = (int**)malloc(2 * sizeof(int*));
	for (int i = 0; i < 2; i++) 
	{
		hash_table[i] = (int*)malloc(n * sizeof(int));
	}
	memset(hash_table[1], -1, n * sizeof(int));

	printf("\nMENU \n1.Insert \n2.Search \n3.Delete \n4.Exit \n");
	while (exit == 1)
	{
		printf("\nSelect menu item ");
		choice = input(s);
		if (amount == n)
		{
			n *= 2;
			for (int i = 0; i < 2; i++)
			{
				hash_table[i] = realloc(hash_table[i], n * sizeof(int));

			}
			relocate(hash_table, n);
		}

		if (choice == 1)
		{
			printf("\nEnter the value ");
			value = input(s);
			printf("\nEnter the key ");
			key = input(s);
			insert(hash_table, value, key, n);
			amount += 1;
		}
		else if (choice == 2)
		{
			printf("\nEnter the key: ");
			key = input(s);
			if (search(hash_table, key, n) == -1) printf("\nThe key searched was not found in the hash table");
			else printf("Searched element = %d\n", hash_table[0][search(hash_table, key, n)]);
		}
		else if (choice == 3)
		{
			printf("\nEnter the key: ");
			key = input(s);
			delete(hash_table, key, n);
		}
		else if (choice == 4)
		{
			exit = 0;
		}
		else printf("Incorrrect input.\n");
	}

	for (int i = 0; i < 2; i++) {
		free(hash_table[i]);
	}
	free(hash_table);

	return 0;
}