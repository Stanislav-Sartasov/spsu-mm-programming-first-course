#include <stdio.h>
#include <stdlib.h>

int main()
{

	int* mdrs = (int*)malloc(sizeof(int) * 1000000);
	int sum = 0;
	for (int i = 2; i < 1000000; i++)
		mdrs[i] = (i - 1) % 9 + 1;
	for (int i = 2; i < 1000000; i++)
	{
		int j = 1;
		sum += mdrs[i];
		while (j * i < 1000000)
		{
			if (mdrs[i * j] < mdrs[i] + mdrs[j])
				mdrs[i * j] = mdrs[i] + mdrs[j];
			j++;
		}
	}
	free(mdrs);
	printf("Sum of all mdrs is %d\n", sum);

	return 0;
}