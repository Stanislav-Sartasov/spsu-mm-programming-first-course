#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>

int make_triangle(double a, double b, double c)
{
	return (a + b > c) && (a + c > b) && (b + c > a);
}

int main()
{
	double x, y, z;
	char c = 0;
	int check = 0;

	do
	{
		printf("Enter 3 positive real numbers: \n");
		if (scanf("%lf", &x) && scanf("%lf", &y) && scanf("%lf%c", &z, &c) && c == '\n')
		{
			if (x > 0 && y > 0 && z > 0)
				check = 1;
			else
				printf("Please, enter positive numbers!\n\n");
		}
		else
		{
			printf("Invalid input! Try again! \n\n");
			scanf("%*[^\n]");
		}
	} while (!check);

	if (make_triangle(x, y, z))
	{
		double pi = 3.141592653589793238462643383279502884;
		double alpha_deg = 0, alpha_minutes = 0, alpha_sec = 0,
			beta_deg = 0, beta_minutes = 0, beta_sec = 0,
			gamma_deg = 0, gamma_minutes = 0, gamma_sec = 0;

		alpha_deg = acos((x * x + y * y - z * z) / (2 * x * y)) * (180 / pi);
		alpha_minutes = (alpha_deg - floor(alpha_deg)) * 60;
		alpha_sec = (alpha_minutes - floor(alpha_minutes)) * 60;

		beta_deg = acos((x * x + z * z - y * y) / (2 * x * z)) * (180 / pi);
		beta_minutes = (beta_deg - floor(beta_deg)) * 60;
		beta_sec = (beta_minutes - floor(beta_minutes)) * 60;

		gamma_deg = acos((y * y + z * z - x * x) / (2 * y * z)) * (180 / pi);
		gamma_minutes = (gamma_deg - floor(gamma_deg)) * 60;
		gamma_sec = (gamma_minutes - floor(gamma_minutes)) * 60;
		printf("%d deg %d' %d''\n", (int)alpha_deg, (int)alpha_minutes, (int)alpha_sec);
		printf("%d deg %d' %d''\n", (int)beta_deg, (int)beta_minutes, (int)beta_sec);
		printf("%d deg %d' %d''\n", (int)gamma_deg, (int)gamma_minutes, (int)gamma_sec);
	}
	else
	{
		printf("Unable to construct a non-degenerate triangle");
	}
	return 0;
}