#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <math.h>
#include <stdbool.h>

int isPrime(int num)
{
	int d;

	for (d = 2; d * d <= num; d++)
	{
		if (num % d == 0)
		{
			return(0);
		}
	}

	return(1);
}


int digitRoot(int num)
{
	int sum;

	while (num > 9)
	{
		sum = 0;

		while (num > 0)
		{
			sum = sum + num % 10;
			num = num / 10;
		}

		num = sum;
	}

	return(num);
}

int main()
{
	int sum_MDRS = 0;
	int* digit;
	digit = (int*)malloc(1000000 * sizeof(int));

	for (int n = 2; n < 1000000; n++) 
	{
		if ((n < 10) || isPrime(n) == 1)
		{
			digit[n] = digitRoot(n);
			sum_MDRS = sum_MDRS + digit[n];
		}
		else
		{
			int mdrs = digitRoot(n);

			for (int i = 2; i * i < n; i++)
			{
				if (n % i == 0)
					digit[n] = digit[i] + digit[n / i];

				if (digit[n] > mdrs)
					mdrs = digit[n];
			}

			digit[n] = mdrs;
			sum_MDRS = sum_MDRS + digit[n];
		}
	}

	printf("The sum of all MDRS is: %d", sum_MDRS);

	return(0);
}