#include <stdio.h>
#include <stdlib.h>

int main()
{

	int* MDRS = (int*)malloc(sizeof(int) * 1000000);
	int sum = 0;
	for (int i = 2; i < 1000000; i++)
		MDRS[i] = (i - 1) % 9 + 1;
	for (int i = 2; i < 1000000; i++)
	{
		int j = 1;
		sum += MDRS[i];
		while (j * i < 1000000)
		{
			if (MDRS[i * j] < MDRS[i] + MDRS[j])
				MDRS[i * j] = MDRS[i] + MDRS[j];
			j++;
		}
	}
	free(MDRS);
	printf("Sum of all MDRS is %d\n", sum);

	return 0;
}