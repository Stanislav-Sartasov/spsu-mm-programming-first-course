#include <stdio.h>
#include <math.h>

const double M_PI = 3.14159265358979323846;;

int isATriangle(double x, double y, double z)
{
	if (x - y - z < 0.0)
		return 1;
	return -1;
}

double cosOfAnAngle(double x, double y, double z)
{
	double result = (-x * x + y * y + z * z) / (2 * y * z);
	return result;
}

double gettingAnAngle(double x, double y, double z)
{
	return acos(cosOfAnAngle(x, y, z));
}

int getDegrees(double rads)
{
	int result = (int)(rads * 180 / M_PI);
	return result;
}

int getMinutes(double rads)
{
	int result = (int)(rads * 180 * 60 / M_PI) % 60;
	return result;
}

int getSeconds(double rads)
{
	int result = (int)(rads * 180 * 60 * 60 / M_PI) % 60;
	return result;
}

void printTheAngle(double x, double y, double z)
{
	double angle = gettingAnAngle(x, y, z);
	printf("%d degrees, %d minutes, %d seconds\n", getDegrees(angle), getMinutes(angle), getSeconds(angle));
}


int main()
{
	double a, b, c;

	printf("Please, enter three numbers separated by a space:\n");
	scanf_s("%lf %lf %lf", &a, &b, &c);

	if (isATriangle(a, b, c) == 1 && isATriangle(b, a, c) == 1 && isATriangle(c, a, b) == 1)
	{
		printTheAngle(a, b, c);
		printTheAngle(b, a, c);
		printTheAngle(c, a, b);
	}
	else
		printf("A triangle with sides %.1lf %.1lf %.1lf can't be formed.\n", a, b, c);

	return 0;
}