#include <stdio.h>
#include <stdlib.h>

int digitalRoot(int number)
{
	int digitalRoot = ((number - 1) % 9) + 1;
	return digitalRoot;
}

int main()
{
	long long sum = 0;
	int* mdrs = (int*)calloc(1000000, sizeof(int));

	if (mdrs == NULL)
	{
		printf("Allocation failure.");
		exit(1);
	}

	for (int i = 2; i < 1000000; i++)
	{
		mdrs[i] = digitalRoot(i);
		for (int j = 2; j * j <= i; j++)
		{
			if (i % j == 0)
				mdrs[i] = max(mdrs[i], mdrs[j] + mdrs[i / j]);
		}
		sum += mdrs[i];
	}

	free(mdrs);
	printf("%lli", sum);
	return 0;
}