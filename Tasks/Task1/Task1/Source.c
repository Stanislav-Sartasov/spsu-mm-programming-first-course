#include <stdio.h>


void print(int* bit, int length)
{
	for (int i = 0; i < length; i++)
		printf("%d", bit[i]);
	printf("\n");
}

void convertToBin(int* bit, long long num, int length)
{
	bit[0] = num < 0 ? 1 : 0;
	num = num < 0 ? -num : num;

	for (int i = length - 1; i > 0; i--)
	{
		bit[i] = num % 2;
		num /= 2;
	}
}

int main()
{
	char name[] = "David", surname[] = "Bykov", patronymic[] = "Vadimovich";
	int multiple = strlen(name) * strlen(surname) * strlen(patronymic);
	printf("The multiple of name, surname and patronymic is %d\n", multiple);

	int bit[32];

	convertToBin(&bit, -(4294967296 - multiple), 32);
	printf("-%d in binary is                        ", multiple);
	print(&bit, 32);

	float floatMultiple = multiple;

	convertToBin(&bit, *((int*)&floatMultiple), 32);
	printf(" %d in IEEE754 with single precision is ", multiple);
	print(&bit, 32);

	int bit64[64];
	double doubleMultiple = multiple;
	convertToBin(&bit64, -*((long long*)&doubleMultiple), 64);
	printf("-%d in IEEE754 with double precision is ", multiple);
	print(&bit64, 64);

	return 0;
}