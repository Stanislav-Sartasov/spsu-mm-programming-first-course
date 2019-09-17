#include <stdio.h>

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
	long long a, b, c;
	printf("Please, enter three numbers separated by a space:\n");
	scanf_s("%lli %lli %lli", &a, &b, &c);

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