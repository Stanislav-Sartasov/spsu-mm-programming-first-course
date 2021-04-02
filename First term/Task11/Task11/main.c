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
	int n = 1000000;
	int* block;
	block = malloc(sizeof(int) * n);
	for (i = 2; i < n; i++)
		block[i] = dig_root(i);
	for (i = 2; i < n; i++)
	{
		for (j = 2; i * j < n; j++)
		{
			if (block[i * j] < block[i] + block[j])
				block[i * j] = block[i] + block[j];
		}
		sum += block[i];
	}
	printf("Sum of all mdrs(n): %d", sum);
}