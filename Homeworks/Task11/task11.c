#include <stdio.h>
#include <math.h>
#include <malloc.h>
#include <stdlib.h>

#define MAX_NUM		1000000

int digital_root(int num)
{
	int sum;

	while (num > 9)
	{
		sum = 0;

		while (num > 0)
		{
			sum = sum + (num % 10);
			num = num / 10;
		}

		num = sum;
	}

	return num;
}

int mdrs(int num, int* mdrs_table)
{
	int num_sqrt, i, sum, max_val;

	num_sqrt = (int)sqrt(num * 1.0);
	sum = digital_root(num);
	for (i = 2 ; i <= num_sqrt ; i = i + 1)
	{
		if ((num % i) == 0)
		{
			max_val = digital_root(i) + mdrs_table[num / i];
			if (sum < max_val)
				sum = max_val;
		}
	}

	return sum;
}

int main()
{
	int i, sum;
	int* mdrs_table;

	mdrs_table = (int*)malloc(MAX_NUM * sizeof(int));
	if (!mdrs_table)
	{
		printf("Can't alloc the mdrs's table.\n");
		return -1;
	}

	for (i = 0 ; i < MAX_NUM ; i++)
	{
		mdrs_table[i] = mdrs(i, mdrs_table);
	}

	sum = 0;
	for (i = 2 ; i < MAX_NUM ; i++)
	{
		sum = sum + mdrs_table[i];
	}

	printf("the sum of all MDRS (n) for n = [2; %d] is %d\n", MAX_NUM - 1, sum);
		
	free(mdrs_table);

	return 0;
}