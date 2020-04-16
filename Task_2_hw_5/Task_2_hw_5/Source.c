#include <stdio.h>
#include <math.h>
#include <stdlib.h>

int root(int num)
{
	int root = 0;
	return root = (num % 9 == 0) ? 9 : (num % 9);
}

int main()
{
	printf("The program solves the task of \"digital root\"\n");
	int sum_mdrs = 0, sum_root = 0;

	int *mas = (int*)calloc(1000000, sizeof(int));
	for (int i = 2; i <= 999999; i++)
	{
		if (mas) // предупреждение С6011 в 30, 32,37,38 строчках
		{
			for (int j = 2; j < sqrt(i) + 1; j++)
			{
				if (i % j == 0)
				{
					mas[i] = mas[i / j] + mas[j];
				}
				if (mas[i] > sum_root)
				{
					sum_root = mas[i];
				}
			}
			mas[i] = (sum_root > root(i)) ? sum_root : root(i);
			sum_mdrs = sum_mdrs + mas[i];
			sum_root = 0;
		}
	}
	free(mas);
	printf("%d", sum_mdrs);
	return 0;
}