#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>



int main()
{
	int coin[8] = { 1, 2, 5, 10, 20, 50, 100, 200 };
	long long* ways;
	int n;
	char c = 0;
	int check = 0;

	do
	{
		printf("Enter natural number: \n");
		if (scanf("%d%c", &n, &c) && c == '\n')
		{
			if (n > 0)
				check = 1;
			else
				printf("Please, enter natural numbers!\n\n");
		}
		else
		{
			printf("Invalid input! Try again! \n\n");
			scanf("%*[^\n]");
		}
	} while (!check);

	ways = (long long*)malloc((n + 1) * sizeof(long long));

	for (int i = 1; i <= n; i++)
		ways[i] = 1;
	ways[0] = 1;

	for (int i = 1; i < 8; i++)
	{
		for (int j = coin[i]; j <= n; j++)
		{
			ways[j] = ways[j - coin[i]] + ways[j];
		}
	}
	printf("There are ways to collect %d coins: %ld\n", n, ways[n]);
	free(ways);

	return 0;
}
