#include <stdio.h>

void sort(int* a, int* b, int* c)	
{
	if ((*a > * b) && (*a > * c))
	{
		*c += *a;
		*a -= *c;
		*a = -*a;
		*c -= *a;
	}
	else if (*b > * c)
	{
		*c += *b;
		*b -= *c;
		*b = -*b;
		*c -= *b;
	}
}

int gcd(int a, int b)			// Алгоритм Евклида
{
	while (a && b)
	{
		if (a > b)
			a = a % b;
		else
			b = b % a;
	}
	if (a == 0)
		return b;
	else
		return a;
}


int inputCheck(int check)		// Проверка на корректность ввода
{
	printf("Input a positive number:\n");
	char t;
	for(;;)
	{
		if (!scanf_s("%d", &check) || check <= 0 || getchar() != '\n')
		{
			while ((t = getchar()) != '\n' && t != EOF);
			printf_s("Input error\nInput a positive number:\n");
		}
		else
			return check;
	}
}


