#include <stdio.h>
#include <math.h>

int prime(unsigned x)
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
	int i = 2;
	while (pow(2, i) - 1 <= pow(2, 31) - 1)
	{
		int num = pow(2, i) - 1;
		if (prime(num) == 1)
			printf("%d ", num);
		i++;
	}
	return 0;
}