#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <malloc.h>

int main()
{
	int coin_type[] = { 1, 2, 5, 10, 20, 50, 100, 200 };
	int type_count, amount, i, j;
	int* ways;

	type_count = sizeof(coin_type) / sizeof(int);

	for (;;)
	{
		printf("Please a amount of money in pensions:");
		scanf("%d", &amount);

		if (amount > 0)
		{
			break;
		}
		printf("You entered incorrect numbers\n");
	}

	ways = (int*)calloc(amount + 1, sizeof(int));
	if (ways == NULL)
	{
		printf("Can't malloc memory.\n");
		return -1;
	}

	ways[0] = 1;
	for (i = 0; i < type_count; i++)
	{
		for (j = coin_type[i] ; j <= amount ; j = j + 1)
		{
			ways[j] = ways[j] + ways[j - coin_type[i]];
		}
	}

	printf("the number of ways in which this amount can be dialed using any number of any English coins is %d\n", ways[amount]);

	free(ways);

	return 0;
}