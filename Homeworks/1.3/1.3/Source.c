#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h> 
#include <stdlib.h>
#include <math.h>

void ins(double* a, double* b, double* c)
{
	short f = 0;

	while (f != 1)
	{
		if (!((scanf("%lf %lf %lf", a, b, c) == 3) && ((*a > 0) && (*b > 0) && (*c > 0))))
		{
			printf("Incorrect input, please try again.\n");
			int sym;
			while (!((sym = getchar()) == '\n') || (sym == '\0'));
			{
				continue;
			}
		}
		f = 1;
	}
}

void deg(double n1, double n2, double n3)
{
	double p = 180 / 3.1415926535;
	double d1, d2, d3;

	d1 = acos((n1 * n1 + n2 * n2 - n3 * n3) / (2 * n1 * n2)) * p;
	d2 = acos((n3 * n3 + n2 * n2 - n1 * n1) / (2 * n3 * n2)) * p;
	d3 = acos((n1 * n1 + n3 * n3 - n2 * n2) / (2 * n1 * n3)) * p;

	printf("%d %d' %d''\n", (int)d1, ((int)(d1 * 60)) % 60, ((int)(d1 * 360)) % 60);
	printf("%d %d' %d''\n", (int)d2, ((int)(d2 * 60)) % 60, ((int)(d2 * 360)) % 60);
	printf("%d %d' %d''\n", (int)d3, ((int)(d3 * 60)) % 60, ((int)(d3 * 360)) % 60);
}

int main()
{
	double x, y, z;
	printf("The program calculates the angles of an unborn triangle with specified sides, if it exists.\n");
	printf("Input 3 positive numbers - triangle sides: ");

	ins(&x, &y, &z);

	if ((x < y + z) && (y < x + z) && (z < y + x))
	{
		printf("The triangle exists. The angles:\n");

		deg(x, y, z);
	}
	else
		printf("The triangle doesn't exist.");
	return 0;
}