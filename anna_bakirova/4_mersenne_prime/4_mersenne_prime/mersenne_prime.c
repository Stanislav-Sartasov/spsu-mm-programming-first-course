#include <stdio.h>

int isPrime(unsigned x)
{
	if (x % 2 != 0)
	{
		for (unsigned i = 3; i * i <= x; i += 2)
		{
			if (x % i == 0)
				return -1;
		}
	}

	return 1;
}


int main()
{
	unsigned number = 4;
	for (int j = 0; j < 30; ++j)
	{
		if (isPrime(number - 1) == 1)
			printf("%u\n", number - 1);
		number = number << 1;
	}

	return 0;
}