#include <stdio.h>

void bin(char* str, int value, int size, char sign_bit_on)
{
	char minus = 0;				//conversion to binary notation with writing to a string
	if (value < 0)				//size is bit number
	{							//if size is negative, the least bit corresponds to the largest index
		minus = 1;				//str - result string
		value = -1 - value;
	}
	int i;
	if (size > 0)
	{
		for (i = 0; (i < size) && value; i++)
		{
			str[i] = (value % 2) ^ minus + '0';
			value /= 2;
		}
		for (i; i < size; i++)
			str[i] = minus + '0';
		if (sign_bit_on)
			str[size - 1] = minus + '0';
	}
	else
	{
		size = -size;
		for (i = size - 1; (i >= 0) && value; i--)
		{
			str[i] = (value % 2) ^ minus + '0';
			value /= 2;
		}
		for (i; i >= 0; i--)
			str[i] = minus + '0';
		if (sign_bit_on)
			str[0] = minus + '0';
	}
	str[size] = '\0';
}

int dimension(int value)		//number of bits for conversion to binary
{
	int i;
	value = value ? value : 1;
	for (i = 0; value; i++)
		value >>= 1;
	return i;
}

int main()
{
	const char name[] = "Anton";
	const char surname[] = "Kazancev";
	const char patron[] = "Alexeyevich";

	int multiple = strlen(name) * strlen(surname) * strlen(patron);

	printf("Multiple of %s * %s * %s is %d\n", surname, name, patron, multiple);

	//return of negative 32 bit number
	
	char bit32[33];
	bin(bit32, -multiple, -32, 1);
	printf("-%d in binary:       %s\n", multiple, bit32);	
	//in bit32 we have (-1) * multiple in binary represented in string,
	//the least bit corresponds to the largest index for easy printf

	int std_bin_count = dimension(multiple);
	char mantissa[65];
	bin(mantissa, multiple, 1 - std_bin_count, 0);

	// return of positive number in IEEE 754 binary32

	char IEEE754bin32[33];
	IEEE754bin32[32] = '\0';
	bin(IEEE754bin32, std_bin_count + 126, -9, 1);	
	for (int i = 9; i < 8 + std_bin_count; i++)
		IEEE754bin32[i] = mantissa[i - 9];
	for (int i = 8 + std_bin_count; i < 32; i++)
		IEEE754bin32[i] = '0';

	printf("%d in IEEE754bin32:  %s\n", multiple, IEEE754bin32);	
	//in IEEE754bin32 we have multiple in IEEE 754 binary 32
	//the least bit corresponds to the largest index for easy printf

	// return of negative number in IEEE 754 binary64

	char IEEE754bin64[65];
	IEEE754bin64[64] = '\0';
	bin(IEEE754bin64, std_bin_count + 1022, -12, 0);
	IEEE754bin64[0] = '1';
	for (int i = 12; i < 11 + std_bin_count; i++)
		IEEE754bin64[i] = mantissa[i - 12];
	for (int i = 11 + std_bin_count; i < 64; i++)
		IEEE754bin64[i] = '0';

	printf("-%d in IEEE754bin64: %s\n", multiple, IEEE754bin64);	
	//in IEEE754bin64 we have multiple in IEEE 754 binary 64
	//the least bit corresponds to the largest index for easy printf

	return 0;
}
