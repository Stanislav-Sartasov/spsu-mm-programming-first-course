#include <stdio.h> 
#include <string.h>
#include <math.h>

int num_IEEE754(long aa, int ff)
{
	long b = aa;
	int p1, p2, i, s;
	printf("Presentation as a ");

	if (ff == 1)
	{
		printf("positive");
	}
	else
	{
		printf("negative");
	}
	printf(" floating point number with ");

	if (ff == 1)
	{
		printf("unit accuracy");
	}
	else
	{
		printf("double precision");
	}
	printf(" according to IEEE 754: ");

	if (ff == 1)
	{
		p1 = 8;
		p2 = 23;
		printf("%d", 0);
	}
	else
	{
		p1 = 11;
		p2 = 52;
		printf("%d", 1);
	}

	int array_2[52];
	int r = (int)trunc(log(b) / log(2));
	b = b - pow(2, r);

	for (int i = 0; i <= r - 1; i++)
	{
		if (i == r - 1)
		{
			array_2[i] = b;
		}
		else
		{
			if (b >= pow(2, r - (1 + i)))
			{
				b = b - pow(2, r - (1 + i));
				array_2[i] = 1;
			}
			else
				array_2[i] = 0;

		}
	}

	int c = r + pow(2, p1 - 1);
	for (i = 0; i <= p1 - 1; i++)
	{
		if (i == p1 - 1)
			printf("%d", c);
		else
		{
			if (c >= pow(2, p1 - (i + 1)))
			{
				c = c - pow(2, p1 - (i + 1));
				printf("%d", 1);
			}
			else
				printf("%d", 0);
		}
	}

	for (i = 0; i <= p2 - 1; i++)
	{
		if (i >= r)
			array_2[i] = 0;
		printf("%d", array_2[i]);
	}

	printf(".\n");
	return 0;
}

int num_32_Negative(long aa)
{
	long b = aa;
	int array_1[32];

	printf("Presentation as a negative 32-bit integer: ");
	printf("%d", 1); //Требуемый формат - отрицательное число.

	for (int i = 1; i <= 31; i++)
	{
		if (i == 31)
		{
			array_1[i] = b;
		}
		else
		{
			if (b >= pow(2, 31 - i))
			{
				b = b - pow(2, 31 - i);
				array_1[i] = 0;
			}
			else
				array_1[i] = 1;

		}
		printf("%d", array_1[i]);
	}

	printf(".\n");
	return 0;
}

int main()
{
	long a = strlen("Kuznetsov") * strlen("Dmitriy") * strlen("Vladimirovich");

	printf("The program calculates the lengths of my name, surname and patronymic and displays its binary representation in certain data formats.\n");
	printf("The product is equal to %ld.\n", a);

	num_32_Negative(a);

	num_IEEE754(a, 1);

	num_IEEE754(a, 2);

	return 0;
}

