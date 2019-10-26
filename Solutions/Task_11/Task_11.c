#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int mdrs(int value)
{
	int x;
	while (value >= 10)
	{
		x = 0;
		while (value > 0)
		{
			x += value % 10;
			value /= 10;
		}
		value = x;
	}
	return (value);
}

int main()
{
	int* mdrs_table;
	while (!(mdrs_table = (int*)malloc(sizeof(int) * 1000000)));
	mdrs_table[1] = 1;
	mdrs_table[2] = 2;
	mdrs_table[3] = 3;
	int sum = 0;

	for (int i = 2; i <= 999999; i++)
	{
		int mdrs_max = 0;
		int sum_mdrs;
		for (int j = 2; j * j <= i; j++)
			if (i % j == 0)
			{
				sum_mdrs = mdrs_table[j] + mdrs_table[i / j];
				if (sum_mdrs > mdrs_max)
					mdrs_max = sum_mdrs;
			}
		sum_mdrs = mdrs(i);
		if (mdrs_max > sum_mdrs)
		{
			mdrs_table[i] = mdrs_max;
			sum += mdrs_max;
		}
		else
		{
			mdrs_table[i] = sum_mdrs;
			sum += sum_mdrs;
		}
	}
	printf("%d\n", sum);
}