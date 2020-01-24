#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <malloc.h>

long sumNum(long a)
{
	while (!(a < 10))
	{
		int s = 0;
		while (a > 0)
		{
			s = s + (a % 10);
			a = a / 10;
		}
		a = s;
	}
	return a;
}

int main()
{
	printf("The program calculates the sum of all maximal sums of digital roots among all number multiplier expansions from the interval [2; 999999].\n");
	long sumResult = 0;
	long* sumDiv = malloc(sizeof(long) * (999999 + 1));

	for (long i = 2; i <= 999999; i++)
	{
		sumDiv[i] = sumNum(i);

		for (long j = 2; j <= (long)(trunc(sqrt(i))) + 1; j++)
		{
			if (i % j == 0)
			{
				sumDiv[i] = max(sumDiv[i], (sumDiv[j] + sumDiv[i / j]));
			}
		}
		sumResult = sumResult + sumDiv[i];
	}
	printf("The answer is %ld.\n", sumResult);

	free(sumDiv);
	return 0;
}