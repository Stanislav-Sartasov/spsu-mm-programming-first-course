#include <stdio.h>

#define NUM			5000
#define BASE_NUM	16

int g_digit[2000];

int main()
{
	int i, j;

	for (i = 0 ; i < sizeof(g_digit) / sizeof(g_digit[0]) ; i++)
	{
		g_digit[i] = 0;
	}
	g_digit[0] = 1;

	for (i = 0 ; i < NUM ; i++)
	{
		for (j = 0 ; j < sizeof(g_digit) / sizeof(g_digit[0]) ; j++)
		{
			g_digit[j] = g_digit[j] * 3;
		}

		for (j = 0 ; j < sizeof(g_digit) / sizeof(g_digit[0]) - 1 ; j++)
		{
			g_digit[j + 1] = g_digit[j + 1] + g_digit[j] / BASE_NUM;
			g_digit[j] = g_digit[j] % BASE_NUM;
		}
	}

	printf("the sum for 3 ^ %d is \n", NUM);
	for (i = sizeof(g_digit) / sizeof(g_digit[0]) - 1 ; i >= 0 ; i = i - 1)
	{
		if (g_digit[i] > 0)
		{
			printf("%X", g_digit[i]);
		}
	}
	printf("\n");
	
	return 0;
}