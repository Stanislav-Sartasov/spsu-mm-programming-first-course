#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>

int is_pythagorean_triple(int x, int y, int z)
{
	return ((x * x + y * y == z * z) || (x * x + z * z == y * y) 
		|| (z * z + y * y == x * x));
}
int gcd(int a, int b)
{
	while (a && b)
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
	char c = 0;
	int check = 0;

	do
	{
		printf("Enter 3 natural numbers: \n");
		if (scanf("%d", &x) && scanf("%d", &y) && scanf("%d%c", &z, &c) && c == '\n')
		{
			if (x > 0 && y > 0 && z > 0)
				check = 1;
			else
				printf("Please, enter natural numbers!\n\n");
		}
		else
		{
			printf("Invalid input! Try again! \n\n");
			scanf("%*[^\n]");
		}
	} while (!check);
	
	if (is_pythagorean_triple(x, y, z))
	{
		if ((gcd(x, y) + gcd(y, z) + gcd(x, z)) == 3)
			printf("%s\n", "The numbers are primitive pythagorean triple");
		else
			printf("%s\n", "The entered numbers are not-primitive pythagorean triple");
	}
	else
	{
		printf("%s\n", "The entered numbers aren't pythagorean triple");
	}

	return 0;
}

