#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>

int inputCheck(int check);
void sort(int *a, int *b, int *c);
int gcd(int a, int b);
	
int main()
{
	int a = 0, b = 0, c = 0;
	a = inputCheck(a);
	b = inputCheck(b);
	c = inputCheck(c);
	sort(&a, &b, &c);
	if (a * a + b * b == c * c)
	{
		if (gcd(a, b) * gcd(b, c) * gcd(a, c) == 1)
			printf("(%d, %d, %d) is primitive Pythagorean triple\n", a, b, c);
		else
			printf("(%d, %d, %d) is non-primitive Pythagorean triple\n", a, b, c);
	}
	else
		printf("(%d, %d, %d) isn`t Pythagorean triple\n", a, b, c);
	return 0;
}