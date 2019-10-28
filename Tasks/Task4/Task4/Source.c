#include <stdio.h>
#include <time.h>

int isPrime(unsigned int x)
{
	int d = 2;
	if (x % 2 == 0)
		return x == 2;
	d = 3;
	while (d * d <= x)
	{
		if (x % d == 0)
		{
			return 0;
		}
		d = d + 2;
	}
	return 1;
}

int main()
{
	clock_t begin = clock();
	unsigned int x = 2;

	for (int i = 1; i < 32; i++) {
		x = x << 1;
		if (isPrime(x - 1))
		{
			printf("Number %u is prime.\n", x - 1);
		}
	}
	clock_t end = clock();
	double time_spent = (double)(end - begin) / CLOCKS_PER_SEC;
	printf("%f", time_spent);
	return 0;
}