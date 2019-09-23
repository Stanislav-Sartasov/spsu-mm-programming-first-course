#include <stdio.h>
#include "inputing.h"

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

	printf("This program for the entered three numbers determines\nwhether they are a Pythagorean triple and if they are, simple or compound\n\n");

	do 
	{												//data input
		in_int("Enter first number: ", &x);
		if (x <= 0)
			printf("invalid input, natural number expacted\n");
	}
	while (x <= 0);
	printf("\n");

	do
	{
		in_int("Enter second number: ", &y);
		if (y <= 0)
			printf("invalid input, natural number expacted\n");
	}
	while (y <= 0);
	printf("\n");

	do
	{
		in_int("Enter third number: ", &z);
		if (z <= 0)
			printf("invalid input, natural number expacted\n");
	}
	while (z <= 0);
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