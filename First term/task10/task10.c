#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>

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

int main()
{
	int coins[8] = { 1, 2, 5, 10, 20, 50, 100, 200 };
	int sum;
	int j = 0;
	int amount;
	int* table;
	char s[21];	
	
	amount = sizeof(coins) / sizeof(coins[0]);
	printf("Enter the required amount: ");
	
	while (j !=	1)
	{
		(void)scanf("%s", s);
		if (correct_input(s) != 0) printf("Incorrect input. Enter a positive integer.\n");
		else j = 1;
	}

	sum = atoi(s);
	table = (int*)malloc((sum + 1) * sizeof(int));
	if (table == NULL)
	{
		printf("Memory not allocated.\n");
		exit(0);
	}
	else
	{
		memset(table, 0, (sum + 1) * sizeof(int));
		table[0] = 1;

		// ( ол-во способов представить сумму S всеми видами монет) ==
		// ((представить сумму S без монеты coins[i]) + ( представить (S - coins[i]) всеми видами монет)) 
		for (int i = 0; i < amount; i++)
		{
			for (int k = coins[i]; k <= sum; k++)
			{
				table[k] += table[k - coins[i]];
			}
		}
		printf("%d\n", table[sum]);
	}
	free(table);
	return 0;
}