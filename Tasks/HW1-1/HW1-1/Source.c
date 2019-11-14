#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

int main() 
{
	const char name[] = "Lev";
	const char surname[] = "Chernishev";
	const char fathersname[] = "Dmitrievich";

	int length;
	length = strlen(name) * strlen(surname) * strlen(fathersname);

	printf("%d\n", length);
	
	int tv = length, l;

	int bit32[32];
	for (int i = 0; i < 32; i++)
		bit32[i] = 0;

	int k;
	for (k = 31; length > 0; k--)
	{
		bit32[k] = length % 2;
		length /= 2;
	}
	int j = 31 - k;

	for (int i = 0; i < 32; i++)
	{
		if (bit32[i] == 0) bit32[i] = 1;
		else bit32[i] = 0;
	}

	for (int i = 31; i >= 0; i--)
	{
		if (bit32[i] == 0)
		{
			bit32[i] = 1;
			break;
		}
		else bit32[i] = 0;
	}

	for (int i = 0; i < 32; i++) printf("%hu", bit32[i]);
	printf("\n");



	int bin32IE[32], exponent;
	for (int i = 0; i < 32; i++) 
		bin32IE[i] = 0;
	int i = 7 + k;

	exponent = k + 126;
	for (int j = 1; j < k; j++)
	{
		bin32IE[i] = tv % 2;
		tv /= 2;
		i--;
	}

	for (int i = 8; i > 0; i--)
	{
		bin32IE[i] = exponent % 2;
		exponent /= 2;
	}

	for (int i = 0; i < 32; i++) printf("%d", bin32IE[i]);
	printf("\n");


	int bin64IE[64];
	bin64IE[0] = 1;
	for (i = 1; i < 64; i++) 
		bin64IE[i] = 0;

	int g = 11;

	exponent = k + 1022;
	while (exponent)
	{
		bin64IE[g] = exponent % 2;
		g--;
		exponent /= 2;
	}

	for (int i = 12; i - 3 < 32; i++) 
		bin64IE[i] = bin32IE[i - 3];

	for (int i = 0; i < 64; i++) 
		printf("%d", bin64IE[i]);

	printf("\n");

	return 0;
}