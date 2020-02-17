#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int main()
{
	int power = 5000;
	int numberOfDigits = (log(3) / log(16)) * power + 1;
	unsigned char* longHex = (unsigned char*)calloc(numberOfDigits, sizeof(unsigned char));
	if (longHex == NULL)
	{
		printf("Allocation failure.");
		exit(-1);
	}
	longHex[numberOfDigits - 1] = 1;

	for (int i = 1; i <= power; i++)
	{
		unsigned char carry = 0;
		unsigned char current = 0;
		for (int j = numberOfDigits - 1; j >= 0; j--)
		{
			current = 3 * longHex[j] + carry;
			longHex[j] = current % 16;
			carry = current / 16;
		}
	}

	printf("The hexadecimal representation of 3^5000: ");
	for (int i = 0; i < numberOfDigits; i++)
		printf("%x", longHex[i]);

	free(longHex);
	return 0;
}