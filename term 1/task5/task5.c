#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>
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

int main()
{
	int j = 0;
	int n;
	char s[21];
	printf("Enter a positive integer: ");
	while (j != 1)
	{
		(void)scanf("%s", s);
		if (correct_input(s) != 0) printf("\nIncorrect input. Enter a positive integer: ");
		else
		{
			n = atoi(s);
			if (sqrt(n) == floor(sqrt(n))) printf("\nEnter a non-square number: ");
			else j = 1;
		}
	}

	int p = 0;
	int q = 1;
	int whole_part = floor(sqrt(n));
	printf("[%d;", whole_part);
	int k = 0;
	while (1)
	{
		k += 1;
		p = whole_part * q - p;
		q = (n - p * p) / q;
		whole_part = (floor(sqrt(n)) + p) / q;
		printf("% d", whole_part);
		if (q == 1) break;
	}
	printf("]");
	printf(" %d", k);

	return 0;
}
