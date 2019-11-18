#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include<math.h>

int **hashTable;
const int n = 10;
int ptr = 1;

struct hash
{
	int key;
	int value;
};

void init(int a, int b)
{
	hashTable = (int**)malloc(a * sizeof(int*));
	for (int i = 0; i < a; i++) hashTable[i] = (int*)malloc(b * sizeof(int));

	for (int i = 0; i < a; i++)
	{
		for (int j = 0; j < b; j++) hashTable[i][j] = 0;
	}
}


int hash_func(int key)
{
	if (key < 0) key *= -1;
	if (key > n)
	{
		int tmp = key, k = 0;

		while (tmp > 0)
		{
			tmp /= n;
			k++;
		}
		k = pow(10, k - 1) ; // k - (amount of signs of n/10)
 		key /= k;
	}
	return key - 1;
}

void add(struct hash a)
{
	int key;
	key = hash_func(a.key);
	for (int j = 0; j < ptr; j++)
	{
		if (hashTable[key][j] == 0)
		{
			hashTable[key][j] = a.value;
			return 1;
		}
	}
	rebalance(0);
	hashTable[key][ptr - 1] = a.value;
	return 1;
}

int rebalance(unsigned int choice)
{
	int i, j;
	if (choice == 0)
	{
		int **tmp = (int**)malloc(n * sizeof(int*));
		for (i = 0; i < n; i++) tmp[i] = (int*)malloc(ptr * sizeof(int));

		for (i = 0; i < n; i++)
		{
			for (j = 0; j < ptr; j++) tmp[i][j] = hashTable[i][j];
		}

		for (i = 0; i < n; i++) free(hashTable[i]);

		ptr++;

		hashTable = (int**)malloc(n * sizeof(int*));
		for (i = 0; i < n; i++) hashTable[i] = (int*)malloc(ptr * sizeof(int));

		for (i = 0; i < n; i++)
		{
			for (j = 0; j < ptr - 1; j++) hashTable[i][j] = tmp[i][j];
		}

		for (i = 0; i < n; i++) hashTable[i][ptr - 1] = 0;

		for (i = 0; i < n; i++) free(tmp[i]);
		free(tmp);

		return 0;
	}

	int k = 0;
	if (choice == 1)
	{
		for (i = 0; i < n; i++)
		{
			if (hashTable[i][ptr - 1] != 0) k++;    // checks whether to reduce the array of pointers
		}
		if (k > 1) return 1;

		ptr--;

		int **tmp = (int**)malloc(n * sizeof(int*));
		for (i = 0; i < n; i++) tmp[i] = (int*)malloc(ptr * sizeof(int));
		
		for (i = 0; i < n; i++)
		{
			for (j = 0; j < ptr; j++) tmp[i][j] = hashTable[i][j];
		}

		//for (i = 0; i < n; i++) free(hashTable[i]);

		//hashTable = (int**)malloc(n * sizeof(int*));
		for (i = 0; i < n; i++) hashTable[i] = (int*)malloc(ptr * sizeof(int));

		for (i = 0; i < n; i++)
		{
			for (j = 0; j < ptr; j++) hashTable[i][j] = tmp[i][j];
		}

		for (i = 0; i < n; i++) free(tmp[i]);
		free(tmp);

		return 0;
	}
}

void find(struct hash a)
{
	int key;
	key = hash_func(a.key);
		for (int j = 0; j < ptr; j++)
		{
			if (hashTable[key][j] != 0)
			{
				printf("%d", hashTable[key][j]);
				printf("%c", ' ');
			}
			else break;
		}
}

void delite(struct hash a)
{
	int key, x;
	key = hash_func(a.key);
	for (int i = 0; i < n; i++)
	{
		if (hashTable[key][i] == a.value)
		{
			hashTable[key][i] = 0;
			for (int j = i; j < n - 1; j++)
			{
				hashTable[key][j] = hashTable[key][j + 1];
			}
		}
	}
	hashTable[key][ptr - 1] = a.value;
	x = rebalance(1);
	if (x == 1) hashTable[key][ptr - 1] = 0;
}

int main()
{
	init(n, ptr);

	struct hash a, b, c, d, e, f, g;
	a.key = 1; a.value = 20;
	b.key = 1; b.value = 11;
	c.key = 2; c.value = 92;
	d.key = 2; d.value = 14;
	e.key = -14; e.value = 9;
	f.key = 387654; f.value = 41;
	g.key = 1; g.value = 31;

	add(a);
	add(b);

	find(a);

	printf("\n");

	add(c);
	add(d);
	add(e);
	add(f);
	add(g);

	find(a);

	delite(a);

	printf("\n");

	find(a);

	for (int i = 0; i < n; i++)
	{
		free(hashTable[i]);
	}
	free(hashTable);

	system("pause");
	return 0;
}