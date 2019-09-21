#include <stdio.h>
#include <math.h>
#define LDBL_TRUE_MIN  4.9406564584124654E-324

void in_double(char* massage, double* variable_double)
{
	char c;
in_double_begin:
	printf("%s", massage);

	do
	{
		c = getchar();
	} while (c == ' ' || c == '\t');

	if (c == '\n')
		goto in_double_begin;

	short minus;
	if (c == '-')
	{
		c = getchar();
		minus = -1;
	}
	else
		minus = 1;

	while (c == ' ' || c == '\t')
		c = getchar();

	*variable_double = 0.0;
	char whole_part_flag = (c >= '0' && c <= '9') ? 1 : 0;

	while (c >= '0' && c <= '9')
	{
		*variable_double = *variable_double * 10 + c - '0';
		c = getchar();
	}

	while (c == ' ' || c == '\t')
		c = getchar();

	if ((c == '.') && whole_part_flag)
	{
		c = getchar();

		while (c == ' ' || c == '\t')
			c = getchar();

		if (c < '0' || c > '9')
		{
			if (c != '\n')
				while (getchar() != '\n');
			printf("invalid input\n");
			goto in_double_begin;
		}
		for (int i = 10; c >= '0' && c <= '9'; i *= 10)
		{
			*variable_double = *variable_double + ((double)c - (double)'0') / (double)i;
			c = getchar();
		}
	}

	while (c == ' ' || c == '\t')
		c = getchar();

	if (c != '\n')
	{
		while (getchar() != '\n');
		printf("invalid input\n");
		goto in_double_begin;
	}

	*variable_double *= minus;
}

long double f(int* array, int i, int size)
{
	if (!i)
		return array[size];
	return array[size - i] + 1 / f(array, i - 1, size);
}

long double cfin(int* array, int size)	//continued fraction in number
{
	return f(array, size, size);
}

int main()
{
	printf("This program for the entered number determines the continued fraction of its square root\n\n");
	double n;
	in_double("Enter number: ", &n);
	printf("\n");
	long double in = sqrtl(n);
	int a[100];
	a[0] = truncl(in);
	in -= a[0];
	printf("%d", a[0]);
	for (int i = 1; i < 100; i++)
	{
		if (fabsl(cfin(a, i - 1) - sqrtl(n)) <= LDBL_TRUE_MIN)
		{
			printf("\n\nProgram can't find continued fraction for nuber you entered because of long double accuracy,\n");
			printf("Or number you entered is squared number, and this is final continued fraction for it\n");
			return 0;
		}
		a[i] = truncl(1 / in);
		in = 1 / in - a[i];
		printf(", %d", a[i]);		
		if (a[i] == a[0] * 2)
		{
			char flag = 1;
			for (int j = 1; j <= i / 2; j++)
			{
				if (a[j] != a[i - j])
				{
					flag = 0;
					break;
				}
			}
			if (flag)
			{
				printf("\n");
				return 0;
			}
		}
	}
	printf("\nProgram can't find continued fraction for nuber you entered because of long double accuracy\n");
	return -1;
}