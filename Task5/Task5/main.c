#include <stdio.h>
#include <math.h>


int IntChecking(int check)		// Проверка на корректность ввода
{
	printf("Input a positive number:\n");
	char t;
	for (;;)

	{
		if (!scanf_s("%d", &check) || sqrt(check) == (int)(sqrt(check)) || getchar() != '\n')
		{
			while ((t = getchar()) != '\n' && t != EOF);
			printf_s("Input error\nInput a positive number:\n");
		}
		else
			return check;
	}
}


void ChainFraction(int x)
{
	int t = sqrt(x);
	int intPart = t, numerator = 0, denominator = 1, count = 0;
	printf("[%d, (", intPart);
	while (intPart != 2 * t)
	{
		numerator = numerator - intPart * denominator;
		denominator = (x - numerator * numerator) / denominator;
		numerator = -numerator;
		intPart = (sqrt(x) + numerator) / denominator;
		count++;
		if (intPart != 2 * t)
			printf("%d, ", intPart);
		else
			printf("%d)]\nPeriod is %d", intPart, count);
	}
}


int main()
{
	int x = 0;
	x = IntChecking(x);
	ChainFraction(x);
}