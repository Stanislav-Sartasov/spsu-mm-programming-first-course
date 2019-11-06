#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include<math.h>

int hashTable[10][10];
int n=10;

struct hash
{
	int key;
	int value;
};

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
	return key-1;
}

void add(struct hash a, int key)
{
	for (int i = 0; i < n; i++)
	{
		if (hashTable[key][i] == 0)
		{
			hashTable[key][i] = a.value;
			return 1;
		}
	}
	printf("%s", "The number of items exceeded");
}

void find(int key)
{
	key = hash_func(key);
		for (int j = 0; j < n; j++)
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
	int x;
	x = hash_func(a.key);
	for (int i = 0; i < n; i++)
	{
		if (hashTable[x][i] == a.value)
		{
			hashTable[x][i] = 0;
			for (int j = i; j < n-1; j++)
			{
				hashTable[x][j] = hashTable[x][j + 1];
			}
		}
	}
}

int main()
{
	int i;
	for (i = 0; i < 10; i++)
	{
		for (int j = 0; j < 10; j++) hashTable[i][j] = 0;
	}
	int x;
	struct hash a, b, c, d, e, f;
	a.key = 1; a.value = 20;
	b.key = 1; b.value = 11;
	c.key = 2; c.value = 92;
	d.key = 2; d.value = 14;
	e.key = -14; e.value = 9;
	f.key = 387654; f.value = 41;
	

	x = hash_func(a.key);
	add(a, x);

	x = hash_func(b.key);
	add(b, x);

	x = hash_func(c.key);
	add(c, x);

	x = hash_func(d.key);
	add(d, x);

	x = hash_func(e.key);
	add(e, x);

	x = hash_func(f.key);
	add(f, x);

	find(a.key);

	delite(a);
	
	printf("\n");
	find(a.key);

	system("pause");
	return 0;
}