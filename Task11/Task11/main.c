#include <stdio.h>
#include <stdlib.h>


int dig_root(int num)
{
	if (num % 9 == 0)
		return 9;
	return num % 9;
}


int main()
{
	int i, j;
	int sum = 0;
	int* block;
	block = malloc(sizeof(int) * 1000000);
	for (i = 2; i < 1000000; i++)
		block[i] = dig_root(i);
	for (i = 2; i < 1000000; i++)
	{
		for (j = 2; i * j < 1000000; j++)
		{
			if (block[i * j] < block[i] + block[j])
				block[i * j] = block[i] + block[j];
		}
		sum += block[i];
	}
	printf("Sum of all mdrs(n): %d", sum);
}