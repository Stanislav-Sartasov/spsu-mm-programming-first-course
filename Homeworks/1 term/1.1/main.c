#include <stdio.h> 
#include <string.h>
#include <math.h>

int numRepresentInFloat(long num, int flag) //ѕредставление в формате вещественного числа по IEEE754 
{
	long a = num;
	int exp, m, i, s;
	printf("Presentation as a ");

	if (flag == 1)
	{
		printf("positive");
	}
	else
	{
		printf("negative");
	}
	printf(" floating point number with ");

	if (flag == 1)
	{
		printf("unit accuracy");
	}
	else
	{
		printf("double precision");
	}
	printf(" according to IEEE 754: ");

	if (flag == 1)
	{
		exp = 8;
		m = 23;
		printf("%d", 0);
	}
	else
	{
		exp = 11;
		m = 52;
		printf("%d", 1);
	}

	int arrayBits[52];
	int r = (int)trunc(log(a) / log(2));
	a = a - pow(2, r);

	for (int i = 0; i <= r - 1; i++)
	{
		if (i == r - 1)
		{
			arrayBits[i] = a;
		}
		else
		{
			if (a >= pow(2, r - (1 + i)))
			{
				a = a - pow(2, r - (1 + i));
				arrayBits[i] = 1;
			}
			else
				arrayBits[i] = 0;

		}
	}

	int b = r + pow(2, exp - 1);
	for (i = 0; i <= exp - 1; i++)
	{
		if (i == exp - 1)
			printf("%d", b);
		else
		{
			if (b >= pow(2, exp - (i + 1)))
			{
				b = b - pow(2, exp - (i + 1));
				printf("%d", 1);
			}
			else
				printf("%d", 0);
		}
	}

	for (i = 0; i <= m - 1; i++)
	{
		if (i >= r)
			arrayBits[i] = 0;
		printf("%d", arrayBits[i]);
	}

	printf(".\n");
	return 0;
}

int numRepresentIn32Bit(long num) //ѕредставление в формате отрицательного 32-битного целого числа 
{
	long a = num;
	int arrayBits[32];

	printf("Presentation as a negative 32-bit integer: ");
	printf("%d", 1); //“ребуемый формат - отрицательное число.

	for (int i = 1; i <= 31; i++)
	{
		if (i == 31)
		{
			arrayBits[i] = a;
		}
		else
		{
			if (a >= pow(2, 31 - i))
			{
				a = a - pow(2, 31 - i);
				arrayBits[i] = 0;
			}
			else
				arrayBits[i] = 1;

		}
		printf("%d", arrayBits[i]);
	}

	printf(".\n");
	return 0;
}

int main()
{
	long a = strlen("Kuznetsov") * strlen("Dmitriy") * strlen("Vladimirovich");

	printf("The program calculates the lengths of my name, surname and patronymic and displays its binary representation in certain data formats.\n");
	printf("The product is equal to %ld.\n", a);

	numRepresentIn32Bit(a);

	numRepresentInFloat(a, 1);

	numRepresentInFloat(a, 2);

	return 0;
}

