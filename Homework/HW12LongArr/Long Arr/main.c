#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <stdint.h>

void powerArithmetics (uint8_t number, unsigned short int power)
{
	unsigned short int hexDigitsCount = (1 + (log(number) / log(256)) * power);
	uint8_t* hexDigits;
	hexDigits = (uint8_t*)malloc(hexDigitsCount);
	unsigned int temp, hexDigitTail = 0;

	for (unsigned short int i = 0; i < hexDigitsCount; i++)
	{
		hexDigits[i] = 0;
	} 

	hexDigits[0] = 1;

	for (unsigned short int i = 0; i < power; i++)
	{
		for (unsigned short int j = 0; j < hexDigitsCount; j++)
		{
			if ((unsigned int)hexDigits[j] * 3 + hexDigitTail > 255)
			{
				temp = hexDigits[j] * 3 + hexDigitTail;
				hexDigits[j] = temp % 256;
				hexDigitTail = temp / 256;
			}
			else
			{
				hexDigits[j] = hexDigits[j] * 3 + hexDigitTail;
				hexDigitTail = 0;
			}
		}
	}
	for (short int i = hexDigitsCount - 1; i >= 0; i--)
	{
		printf("%x", hexDigits[i] / 16);
		printf("%x", hexDigits[i] % 16);
	}
	free(hexDigits);
}


int main()
{
	powerArithmetics(3, 5000);
	return 0;
}