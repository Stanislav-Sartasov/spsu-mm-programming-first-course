#include <stdio.h>
#include <math.h>

// Тест Люка-Лемера 
void check(long long x, long n)
{
	for (int i = 2; i <= sqrt(n); i++)
	{
		if (n % i == 0)
		{
			return;
		}
	}
	
	long long u = 4;
	for (int i = 1; i <= (n - 2); i++)
	{
		u = ((u * u - 2) % x);
	}
	if (u == 0)
	{
		printf("%lld\n", x);
		u = 4;
	}
}




int main()
{
	int x = pow(2, 2) - 1;
	printf("This program prints Prime numbers Mersenne\n");
	printf("%d\n",x);
	long i = 0;

	for (i = 3; i < 32; i++)
	{
		check(pow(2, i) - 1, i);
	}
}