#include <stdio.h>

void Sort(int* a, int* b, int* c)			/* Не совсем сортировка,
											ставим на последнее место максимальный из тройки элемент */
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

int Gcd(int a, int b)			// Алгоритм Евклида
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


int IntChecking(int check)		// Проверка на корректность ввода
{
	printf("Input a positive number:\n");
	char t;
	for(;;)
	{
		if (!scanf_s("%d", &check) || check == 0 || getchar() != '\n')
		{
			while ((t = getchar()) != '\n' && t != EOF);
			printf_s("Input error\nInput a positive number:\n");
		}
		else
			return check;
	}
}


