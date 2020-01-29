#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>

const double M_PI = 3.14159265358979323846;;

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

double getDouble(double* number)
{
	double result = scanf("%lf", number);
	int extra = extraSymbols();

	while (result != 1 || extra == 1)
	{
		printf("Wrong input, try again:\n");
		result = scanf("%lf", number);
		extra = extraSymbols();
	}

	return *number;
}

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
	printf("Please, enter the length of the first side:\n");
	double a = getDouble(&a);
	printf("Please, enter the length of the second side:\n");
	double b = getDouble(&b);
	printf("Please, enter the length of the third side:\n");
	double c = getDouble(&c);


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