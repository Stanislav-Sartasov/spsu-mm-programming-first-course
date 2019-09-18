#include <stdio.h>

void in_int(char* massage, int* variable_int)	//function for int input
{
	char c;
in_int_begin:
	printf("%s", massage);
	do
	{								//handling spaces at the beginning of a line
		c = getchar();
	}
	while (c == ' ');

	if (c == '\n')
		goto in_int_begin;		//handling empty input

	short minus;						//handling negative input
	if (c == '-')
	{
		c = getchar();
		minus = -1;
	}
	else
		minus = 1;

	while (c == ' ')
		c = getchar();

	*variable_int = 0;

	while (c >= '0' && c <= '9')
	{
		if (*variable_int * 10 + c - '0' < 0)		//handling int overflow
		{
			while (getchar() != '\n');
			printf("invalid input, input is too big\n");
			goto in_int_begin;
		}
		*variable_int = *variable_int * 10 + c - '0';
		c = getchar();
	}

	while (c == ' ')		//post processing and validation of input
		c = getchar();

	if (c != '\n')
	{
		while (getchar() != '\n');
		printf("invalid input\n");
		goto in_int_begin;
	}
	*variable_int *= minus;
}

int gcd(int a, int b)			//greatest common divisor
{
	while (a > 0 && b > 0)
	{
		if (a > b)
			a = a % b;
		else
			b = b % a;
	}
	return a + b;
}

int main()
{
	int x, y, z;

	do 
	{												//data input
		in_int("Enter first number: ", &x);
		if (x < 0)
			printf("invalid input, natural number expacted\n");
	}
	while (x < 0);
	printf("\n");

	do
	{
		in_int("Enter second number: ", &y);
		if (y < 0)
			printf("invalid input, natural number expacted\n");
	}
	while (y < 0);
	printf("\n");

	do
	{
		in_int("Enter third number: ", &z);
		if (z < 0)
			printf("invalid input, natural number expacted\n");
	}
	while (z < 0);
	printf("\n");
	
	if (x > y)
	{
		if (x > z)
		{
			int t = x;
			x = z;
			z = t;
		}
	}
	else
	{
		if (y > z)
		{
			int t = y;
			y = z;
			z = t;
		}
	}

	if (x * x + y * y == z * z)
		printf("Pythagorean triple\n");
	else
	{
		printf("Not Pythagorean triple\n");
		return 0;
	}

	if (gcd(x, y) == 1 && gcd(x, z) == 1 && gcd(y, z) == 1)
		printf("Simple\n");
	else
		printf("Not simple\n");

	return 0;
}