#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

int correct_input(char str[])
{
	char digits[11] = "0123456789";
	int digit_true, exit = 0;
	if (str[0] != '0')
	{

		for (int l = 0; (l < strlen(str)) && (exit != 1); l++)
		{
			digit_true = 0;
			for (int n = 0; (n < 10) && (exit != 1); n++)
			{
				if (str[l] == digits[n]) digit_true = 1;
			}
			if (digit_true == 0) exit = 1;
		}
	}
	else exit = 1;
	if (exit == 1) return 1;
	else return 0;
}

// (Кол-во способов представить сумму total всеми видами монет) ==
// ((представить total без монеты coin) + ( представить (total - coin) всеми видами монет)) 
int count(int total, int coins[], int amount) 
{
	if (total == 0) return 1;
	if (total < 0) return 0;
	if ((amount <= 0) && (total >= 1)) return 0;
	return (count(total, coins, (amount - 1)) + count((total - coins[(amount - 1)]), coins, amount));
}

int main()
{
	int a[8] = { 1, 2, 5, 10, 20, 50, 100, 200 };
	int sum = 0;
	int j = 0;
	char s[21];	
	while (j !=	1)
	{
		(void)scanf("%s", s);
		if (correct_input(s) != 0) printf("Incorrect input. Enter a positive integer.\n");
		else j = 1;
	}
	sum = atoi(s);
	printf("%d", count(sum, a, 8));
	return 0;
}