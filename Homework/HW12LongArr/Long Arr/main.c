#include <stdio.h>
#include <stdlib.h>
#include <math.h>

void powerArithmetics (int number, int power)
{
	int hexDigitsCount = (1 + (log(number) / log(16)) * power);
	int* hexDigits;
	hexDigits = (int*)malloc(hexDigitsCount * sizeof(int));
	
	for (int i = 0; i < hexDigitsCount; i++)
	{
		hexDigits[i] = 0;
	} 
	hexDigits[0] = 1;
	for (int i = 0; i < power; i++)
	{
		for (int j = 0; j < hexDigitsCount; j++)
			hexDigits[j] = hexDigits[j] * 3;

		for (int j = 0; j < hexDigitsCount - 1; j++)
		{
			if (hexDigits[j] > 15)
			{
				hexDigits[j + 1] += hexDigits[j] / 16;
				hexDigits[j] %= 16;
			}
		}
	}
	for (int i = hexDigitsCount - 1; i > -1; i--)
		printf("%x", hexDigits[i]);
	free(hexDigits);
}


int main()
{
	int n = 3, power = 5000;
	powerArithmetics(3, 5000);

	return 0;
}