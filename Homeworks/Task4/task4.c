#include <stdio.h>

int is_prime_number(unsigned int num)
{
	unsigned int i;

	if (num < 2)
	{
		return 0;
	}

	for (i = 2 ; i <= num / 2 ; i++)
	{
		if ((num % i) == 0)
		{
			return 0;
		}
	}

	return 1;
}

int main()
{
	int i, j;
	unsigned int number;

	for (i = 1 ; i < 32 ; i++)
	{
		number = 1;
		for (j = 0 ; j < i ; j++)
		{
			number = number * 2;
		}
		number = number - 1;

		if (is_prime_number(number))
		{
			printf("Mersenne number(%d) is %d\n", i, number);
		}
	}

	return 0;
}