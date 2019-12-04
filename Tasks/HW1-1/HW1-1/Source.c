#include <stdio.h>

int main()
{
	char name[] = "Lev", surname[] = "Chernishev", fathersname[] = "Dmitrievich";
	int prod = strlen(name) * strlen(surname) * strlen(fathersname);

	int int_b[32];
	for (int i = 0; i < 32; i++)
		int_b[i] = 1;
	int b = prod, pos = 31;
	while (b)
	{
		if (b % 2)
			int_b[pos] = 0;
		else
			int_b[pos] = 1;
		pos--;
		b /= 2;
	}
	int k = 31;
	while (int_b[k])
	{
		int_b[k] = 0;
		k--;
	}
	int_b[k] = 1;

	printf("A) ");
	for (int i = 0; i < 32; i++)
		printf("%d", int_b[i]);
	printf("\n");


	int float_b[32];
	for (int i = 0; i < 32; i++)
		float_b[i] = 0;
	int n = prod, lenght = -1;
	while (n)
	{
		n /= 2;
		lenght++;
	}
	int exponent = 127 + lenght;
	for (int i = 8; i > 0; i--)
	{
		float_b[i] = exponent % 2;
		exponent /= 2;
	}
	int m = prod;
	for (int i = 8 + lenght; i > 8; i--)
	{
		float_b[i] = m % 2;
		m /= 2;
	}

	printf("B) ");
	for (int i = 0; i < 32; i++)
		printf("%d", float_b[i]);
	printf("\n");


	int double_b[64] = { 0 };
	double_b[0] = 1;
	n = prod;
	lenght = -1;
	while (n)
	{
		n /= 2;
		lenght++;
	}
	exponent = 1023 + lenght;
	for (int i = 11; i > 0; i--)
	{
		double_b[i] = exponent % 2;
		exponent /= 2;
	}
	for (int i = 11 + lenght; i > 11; i--)
	{
		double_b[i] = prod % 2;
		prod /= 2;
	}

	printf("C) ");
	for (int i = 0; i < 64; i++)
		printf("%d", double_b[i]);
	return 0;
}