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

double angCalc(double a, double b, double c)
{
	double p = 180 / 3.141592653589793;
	return (acos((a * a + b * b - c * c) / (2 * a * b)) * p);
}

void angPrint(double ang)
{
	double deg = (int)ang;
	double min = (ang - (int)ang) * 60;
	double sec = round((min - (int)min) * 60);

	if ((int)sec == 60)
	{
		sec = 0;
		min++;
	}
	if (min == 60)
	{
		min = 0;
		deg++;
	}
	printf("%d %d' %d''\n", (int)deg, (int)min, (int)sec);
}

void ang(double side1, double side2, double side3)
{
	double ang1 = angCalc(side1, side2, side3);

	double ang2 = angCalc(side3, side2, side1);

	double ang3 = angCalc(side3, side1, side2);

	angPrint(ang1);

	angPrint(ang2);

	angPrint(ang3);
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

		ang(x, y, z);
	}
	else
		printf("The triangle doesn't exist.");
	return 0;
}