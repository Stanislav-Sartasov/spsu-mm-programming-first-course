#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>

int IntChecking(int check);
void Sort(int *a, int *b, int *c);
int Gcd(int a, int b);
	
int main()
{
	int a = 0, b = 0, c = 0;
	a = IntChecking(a);
	b = IntChecking(b);
	c = IntChecking(c);
	Sort(&a, &b, &c);
	if (a * a + b * b == c * c)
	{
		if (Gcd(a, b) * Gcd(b, c) * Gcd(a, c) == 1)
			printf("(%d, %d, %d) is primitive Pythagorean triple\n", a, b, c);
		else
			printf("(%d, %d, %d) is non-primitive Pythagorean triple\n", a, b, c);
	}
	else
		printf("(%d, %d, %d) isn`t Pythagorean triple\n", a, b, c);
	return 0;
}