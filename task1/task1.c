#include <stdio.h>

void IEEE754_Single(int list, int* r, int left)
{
	int exp;
	int bit_exp[8];
	exp = 127 + 64 - left - 1;
	for (int i = 7; i >= 0; i--)
	{
		bit_exp[i] = exp % 2;
		exp /= 2;
	}
	printf("\nIEEE 754 Single precision positive: ");
	printf("0");
	for (int i = 0; i < 8; i++)
	{
		printf("%d", bit_exp[i]);
	}
	for (int i = 1; i < 24; i++)
	{
		if ((left + i) < 64) printf("%d", *(r + left + i));
		else printf("0");
	}
}

void IEEE754_Double(int list, int* r, int left)
{
	int exp;
	int n;
	int bit_exp[11];
	n = sizeof(list) / sizeof(*r);
	exp = 1023 + 64 - left - 1;
	for (int i = 10; i >= 0; i--)
	{
		bit_exp[i] = exp % 2;
		exp /= 2;
	}
	printf("\nIEEE 754 Double precision negative: ");
	printf("1");
	for (int i = 0; i < 11; i++)
	{
		printf("%d", bit_exp[i]);
	}
	for (int i = 1; i < 52; i++)
	{
		if ((left + i) < 64) printf("%d", *(r + left + i));
		else printf("0");
	}
}

int main()
{
	const char name[] = "Evgeniy";
	const char surname[] = "Bakaev";
	const char patronymic[] = "Vladimirovich";
	int c = strlen(name) * strlen(surname) * strlen(patronymic);
	int bit_int[64];
	int right_zero = -1;
	int left_unit = -1;
	int* p;
	int right_unit = -1;
	
	

	for (int k = 63; k >= 0; k--)
	{
		bit_int[k] = c % 2;
		if (right_zero == -1 && bit_int[k] == 0) right_zero = k;
		if (bit_int[k] == 1) left_unit = k;
		if (right_unit == -1  && bit_int[k] == 1) right_unit = k;
		c /= 2;

	}

	printf("Two's complement: ");
	for (int i = 32; i < 64; i++) // Отрицательное 32-битное целое, модуль которого равен найденному произведению
	{
		if (i < right_zero - 1) printf("%d", (bit_int[i] + 1) % 2);
		else if (i == right_zero - 1) printf("%d", bit_int[i]);
		else printf("0");
	}

	p = bit_int;
	IEEE754_Single(bit_int, p, left_unit);
	IEEE754_Double(bit_int, p, left_unit);
	return 0;
}