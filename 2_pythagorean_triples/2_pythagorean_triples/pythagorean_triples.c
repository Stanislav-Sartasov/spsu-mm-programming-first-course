#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>

int extraSymbols()
{
	char symbol;
	int extra = 0;
	while ((symbol = getchar()) != '\n' && symbol != EOF)
	{
		if (symbol != ' ' && (symbol < '0' || symbol > '9'))
			extra = 1;
	}

	return extra;
}

int getInteger(int* number)
{
	int result = scanf("%d", number);
	int extra = extraSymbols();

	while (result != 1 || extra == 1)
	{
		printf("Wrong input, try again:\n");
		result = scanf("%d", number);
		extra = extraSymbols();
	}

	return *number;

}

long long max(long long x, long long y)
{
	if (x > y)
		return x;
	return y;
}

long long gcd(long long x, long long y)
{
	for (long long temp; y; )
	{
		temp = x % y;
		x = y;
		y = temp;
	}
	return x;
}

int main()
{
	printf("Please, enter the first number:\n");
	int a = getInteger(&a);
	printf("Please, enter the second number:\n");
	int b = getInteger(&b);
	printf("Please, enter the third number:\n");
	int c = getInteger(&c);

	if (a == max(a, (b, c)))
	{
		long long temp = a;
		a = c;
		c = temp;
	}
	else if (b > max(a, (b, c)))
	{
		long long temp = b;
		b = c;
		c = temp;
	}

	long long divisor = gcd(a, b);
	if (a * a + b * b == c * c)
	{
		if (divisor == 1)
			printf("That's a primitive Pythagorean triple.\n");
		else
			printf("That's a Pythagorean triple, but not the primitive one.\n");
	}
	else printf("That's not a Pythagorean triple.\n");

	return 0;
}