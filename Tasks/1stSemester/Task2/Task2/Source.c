#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#define swap(x, y) {x = x + y; y = x - y; x = x - y;}


int gcd(int a, int b)
{
	if (!b)
		return abs(a);
	gcd(b, a % b);
}

int check()
{
	int c, flag = 1;
	while ((c = getchar()) != '\n' && c != EOF)
		if (c != '\n' && c != ' ' && c != EOF)
			flag = 0;
	return flag;
}

void input(int* x, int* y, int* z)
{
	int result = scanf("%d%d%d", x, y, z);
	int flag = check();

	while ((flag == 0) || (result - 3) || (*x < 1) || (*y < 1) || (*z < 1))
	{
		printf("Invalid Input.Try Again.\n");
		result = scanf("%d%d%d", x, y, z);
		flag = check();
	}

}

int main()
{
	int x, y, z;

	input(&x, &y, &z);

	if (min(x, y) == y)
		swap(x, y);
	
	if (min(x, z) == z)
		swap(x, z);
	
	if (min(y, z) == z)
		swap(y, z);

	if (x * x + y * y == z * z)
	{
		if (gcd(gcd(x, y), z) == 1)
			printf("The triple of numbers is a primitive Pythagorean triple.");
		else
			printf("The triple of numbers is the Pythagorean triple, but not the primitive Pythagorean triple.");
	}
	else
		printf("The triple of numbers is not the Pythagorean triple.");

	return 0;
}