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

double degCalcBase(double a, double b, double c)
{
	double p = 180 / 3.1415926535;
	return (acos((a * a + b * b - c * c) / (2 * a * b)) * p);
}

double degCalcThird(double a, double b)
{
	printf("%d %d' %d''\n", (int)a, ((int)(a * 60)) % 60, ((int)(a * 360)) % 60);
	printf("%d %d' %d''\n", (int)b, ((int)(b * 60)) % 60, ((int)(b * 360)) % 60);
	int c = (180 * 3600) - (((int)a) * 3600 + (((int)(a * 60)) % 60) * 60 + ((int)(a * 360)) % 60) - (((int)b) * 3600 + (((int)(b * 60)) % 60) * 60 + ((int)(b * 360)) % 60);
	printf("%d %d' %d''\n", (int)(c / 3600), (int)((c % 3600) / 60), (c % 60));
}

void deg(double side1, double side2, double side3)
{
	double deg1 = degCalcBase(side1, side2, side3);

	double deg2 = degCalcBase(side3, side2, side1);

	degCalcThird(deg1, deg2);

}

void main()
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
}