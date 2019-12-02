#include <stdio.h>
#include <math.h>
#include <time.h>

void LLT(int p)								//Lucas-Lehmer test
{
	long long seq = 4;
	long long mers = pow(2, p) - 1;
	for (int i = 2; i < sqrt(p); i++)		//If p is composite, then mers is also composite
		if (p % i == 0)
			return 0;
	for (int i = 1; i < p - 1; i++)
	{
		seq = (seq * seq - 2) % mers;
	}
	if (seq == 0 || p == 2)
		printf("%lld\n", mers);
}

int main()
{
	for (int i = 1; i < 32; i++)
	{
		LLT(i);
	}
}