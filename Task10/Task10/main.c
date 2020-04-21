#include <stdio.h>
#include <stdlib.h>


int inputCheck()
{
	printf("Input a positive number:\n");
	char t;
	int  check;
	for (;;)

	{
		if (!scanf_s("%d", &check) || check <= 0 || getchar() != '\n')
		{
			while ((t = getchar()) != '\n' && t != EOF);
			printf_s("Input error\nInput a positive number:\n");
		}
		else
			return check;
	}
}


int main()
{
	int n = inputCheck();
	int a[] = { 1, 2, 5, 10, 20, 50, 100, 200 };
	long long* block;
	block = calloc((n + 1), sizeof(long long));
	for (int i = 1; i < n; i++)
	{
		block[i] = 0;
	}
	block[0] = 1;
	for (int i = 0; i < 8; i++)
	{
		for (int j = 1; j <= n; j++)
		{
			if (j - a[i] >= 0)
				block[j] += block[j - a[i]];
		}
	}
	printf("The number of options: %lld", block[n]);
	free(block);
}