#include <stdio.h>
#include <math.h>
#include <malloc.h>
#include <stdlib.h>

int sum_digit(int x)
{
	int sum = 0;
	while (x > 0)
	{
		sum += x % 10;
		x /= 10;
	}
	return sum;
}

int mdrs(int n, int* q)
{
	int max = 0;
	int k = n;
	int rest = k % 2;
	int j = 2 + rest;

	while (n > 10) n = sum_digit(n);
	max = sum_digit(n);
	while (j < sqrt(k) + 1)
	{
		if ((k % j) == 0)
		{
			if (max < (*(q + j - 2) + *(q + (k / j) - 2)))
			{
				max = *(q + j - 2) + *(q + (k / j) - 2);
				
			}
		}
		j += rest + 1;
	}

	return max;
}

int main()
{
	
	int* p;
	p = (int*)malloc(999999 * sizeof(int));
	if (p == NULL)
	{
		printf("Memory not allocated.\n");
		exit(0);
	}
	else
	{
		int sum = 0;
		for (int i = 0; i < 8; i++)
		{
			*(p + i) = i + 2;
			sum += *(p + i);
		}
		for (int i = 8; i < 999998; i++)
		{

			*(p + i) = mdrs(i + 2, p);
			sum += *(p + i);
		}

		printf("%d\n", sum);
	}
	free(p);
	return 0;
}