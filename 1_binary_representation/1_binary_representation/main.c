#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

void printIEEE754(int exp, int mantissa, int firstOne, int* exponent, int* reversedMantissa)
{
	for (int i = 0; i < exp; i++)
		printf("%d", exponent[i]);
	for (int j = firstOne - 1; j >= 0; j--)
		printf("%d", reversedMantissa[j]);
	for (int j = firstOne; j < mantissa; j++)
		printf("0");
	printf("\n");
}

void IEEE754(int product, int mantissa, int exp, int bias)
{
	int* reversedMantissa = (int*)calloc(mantissa, sizeof(int));
	int* exponent = (int*)calloc(exp, sizeof(int));

	if (reversedMantissa == NULL || exponent == NULL)
	{
		printf("Allocation failure.");
		exit(-1);
	}

	int x = product;
	int previous = 0;
	int firstOne = -1;

	for (int i = 0; x > 1 && i < mantissa; i++)	
	{
		(reversedMantissa[i]) = x % 2;
		x /= 2;
		firstOne = i + 1;
	}

	int biasedExp = bias + firstOne;
	for (int k = exp - 1; biasedExp > 0 && k >= 0; k--)
	{
		exponent[k] = biasedExp % 2;
		biasedExp /= 2;
	}

	printIEEE754(exp, mantissa, firstOne, exponent, reversedMantissa);

	free(reversedMantissa);
	free(exponent);
}

int main()
{
	char name[] = "Anna";
	char surname[] = "Bakirova";
	char patronymic[] = "Abdusattorovna";
	int product = strlen(name) * strlen(surname) * strlen(patronymic);

	int* twosComplement = (int*)calloc(32, sizeof(int));
	if (twosComplement == NULL)
	{
		printf("Allocation failure.");
		exit(-1);
	}
	long long complement = pow(2, 32) - product;
	for (int i = 31; complement > 0 && i >= 0; i--)
	{
		twosComplement[i] = complement % 2;
		complement /= 2;
	}
	printf("Two's complement of %d is: ", product);
	for (int j = 0; j < 32; j++)
		printf("%d", twosComplement[j]);
	printf("\n");
	free(twosComplement);

	printf("IEEE 754 single-precision positive floating representation of %d: 0", product);
	IEEE754(product, 23, 8, 127);
	printf("IEEE 754 double-precision negative floating representation of %d: 1", product);
	IEEE754(product, 52, 11, 1023);
	return 0;
}