#include <stdio.h>
#include <math.h>

int is_prime(long long x)
{
	if (x < 2) return 0;
	for (int i = 2; i < (int)sqrt(x) + 1; i++)
		if (x % i == 0) return 0;

	return 1;
}

int main()
{
	long long mersens;
	for (int n = 1; n <= 31; n++)
	{
		mersens = pow(2, n) - 1;
		if (is_prime(mersens))
		{
			printf("%lld\n", mersens);
		}
	}

	return 0;
}