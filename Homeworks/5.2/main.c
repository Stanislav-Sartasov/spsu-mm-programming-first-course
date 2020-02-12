#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <malloc.h>

long sumNum(long a)
{
	return (((a - 1) % 9) + 1);
}

void main()
{
	printf("The program calculates the sum of all maximal sums of digital roots among all number multiplier expansions from the interval [2; 999999].\n");
	long sumResult = 0;
	long n = 999999;
	long i;
	long* sumDiv = malloc(sizeof(long) * (n + 1));

	for (i = 2; i <= n; i++)
	{
		sumDiv[i] = sumNum(i);
	}

	for (i = 2; i <= n; i++)
	{
		long j = 1;
		long r;
		sumResult = sumResult + sumDiv[i];
		while ((r = ++j * i) <= n)
		{
			sumDiv[r] = max(sumDiv[r], (sumNum(j) + sumDiv[i]));
		}
	}

	printf("The answer is %ld.\n", sumResult);
	free(sumDiv);
}