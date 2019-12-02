#include <stdio.h> 
#include <math.h>
int main()
{
	long a = 4;
	printf("The program outputs all simple Mersenne numbers on the segment [1; 2^31 - 1]:\n");

	for (long i = 2; i <= 31; i++)
	{
		long b = a - 1;
		int r = 0;
		for (long j = 2; j < sqrt(b) + 1; j++)
		{
			if ((b % j) == 0)
				r = 1;
		}
		if (r == 0)
			printf("%ld ", b);
		a = a * 2;
	}
	return 0;
}