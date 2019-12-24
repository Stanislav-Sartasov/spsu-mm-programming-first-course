#include <stdio.h>
#include <stdint.h>

uint32_t pows[32];

void calculatePow2() 
{
	pows[0] = 1;
	for(int i = 1; i < 32; ++i) 
	{
		pows[i] = pows[i - 1] * 2;
	}
}

int main() 
{
	const char* description = "This program print's all the Mersenne primes in"
								" interval [1, 2^31 - 1]\n";

	printf("%s", description);

	calculatePow2();

//    int mersenneExp[8] = {2, 3, 5, 7, 13, 17, 19, 31};

	for (int i = 0; i < 32; ++i) 
	{
		printf("%d\n", pows[i] - 1);
    }
}
