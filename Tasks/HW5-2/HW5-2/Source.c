#include <stdio.h>
#include <stdlib.h>

int main()
{

	int* Mdrs = (int*)malloc(sizeof(int) * 1000000);
	int Sum = 0;
	for (int i = 2; i < 1000000; i++)
		Mdrs[i] = (i - 1) % 9 + 1;
	for (int i = 2; i < 1000000; i++)
	{
		int j = 1;
		Sum += Mdrs[i];
		while (j * i < 1000000)
		{
			if (Mdrs[i * j] < Mdrs[i] + Mdrs[j])
				Mdrs[i * j] = Mdrs[i] + Mdrs[j];
			j++;
		}
	}
	free(Mdrs);
	printf("Sum of all Mdrs is %d\n", Sum);

	return 0;
}