#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <stdint.h>

void powerArithmetics (uint8_t number, unsigned short int power)
{
	unsigned short int hexDigitsCount = (1 + (log(number) / log(16)) * power);
	uint8_t* hexDigits;
	hexDigits = (uint8_t*)malloc(hexDigitsCount * sizeof(uint8_t));
	
	for (unsigned short int i = 0; i < hexDigitsCount; i++)
	{
		hexDigits[i] = 0;
	} 
	hexDigits[0] = 1;
	for (unsigned short int i = 0; i < power; i++)
	{
		for (unsigned short int j = 0; j < hexDigitsCount; j++)
			hexDigits[j] = hexDigits[j] * 3;

		for (unsigned short int j = 0; j < hexDigitsCount - 1; j++)
		{
			if (hexDigits[j] > 15)
			{
				hexDigits[j + 1] += hexDigits[j] / 16;
				hexDigits[j] %= 16;
			}
		}
	}
	for (short int i = hexDigitsCount - 1; i >= 0; i--)
		printf("%x", hexDigits[i]);
	free(hexDigits);
}


int main()
{
	powerArithmetics(3, 5000);
	return 0;
}