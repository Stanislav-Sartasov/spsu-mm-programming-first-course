#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

int toBin(unsigned int *mas, int n, int length)
{
	int k;
	int i = n - 1;
	while (length > 0)
	{
		mas[i] = length % 2;
		length /= 2;
		i--;
	}
	return k = 31 - i;
}

int main()
{
	const char name[] = "Danil";
	const char surname[] = "Kostennikov";
	const char fathersname[] = "Vyacheslavovich";
	int length, i;
	length = strlen(name) * strlen(surname) * strlen(fathersname);
	printf("%d\n", length);
	int tmp = length, k = 0;
	unsigned int bit32[32];
	for (i = 0; i < 32; i++) bit32[i] = 0;
	k = toBin(bit32, 32, length);
	for (i = 0; i < 32; i++)
	{
		if (bit32[i] == 0) bit32[i] = 1;
		else bit32[i] = 0;
	}
	for (i = 31; i >= 0; i--)
	{
		if (bit32[i] == 0)
		{
			bit32[i] = 1;
			break;
		}
		else bit32[i] = 0;
	}
	for (i = 0; i < 32; i++) printf("%hu", bit32[i]);
	printf("\n");


	int bin32IE[32], exponent;
	for (i = 0; i < 32; i++) bin32IE[i] = 0;
	i = 7 + k;
	exponent = k + 126;
	for (int j = 1; j < k; j++)
	{
		bin32IE[i] = tmp % 2;
		tmp /= 2;
		i--;
	}
	for (i = 8; i > 0; i--)
	{
		bin32IE[i] = exponent % 2;
		exponent /= 2;
	}
	for (i = 0; i < 32; i++) printf("%d", bin32IE[i]);
	printf("\n");


	int bin64IE[64];
	bin64IE[0] = 1;
	for (i = 1; i < 64; i++) bin64IE[i] = 0;
	i = 11;
	exponent = k + 1022;
	while (exponent)
	{
		bin64IE[i] = exponent % 2;
		i--;
		exponent /= 2;
	}
	for (i = 12; i - 3 < 32; i++) bin64IE[i] = bin32IE[i - 3];
	for (i = 0; i < 64; i++) printf("%d", bin64IE[i]);
	printf("\n");

	system("pause");
	return 0;
}