#include <stdio.h>
#include <math.h>
#define PI 3.141592


double inputCheck(double check)
{
	printf("Input a positive number:\n");
	char t;
	for (;;)
	{
		if (!scanf_s("%lf", &check) || check <= 0 || getchar() != '\n')
		{
			while ((t = getchar()) != '\n' && t != EOF);
			printf_s("Input error\nInput a positive number:\n");
		}
		else
			return check;
	}
}


void dmsOutput(double angle)
{
	int degrees = (int)angle;
	int minutes = (int)((angle - degrees) * 60);
	int seconds = (int)(((angle - degrees) * 60 - minutes) * 60);
	printf("%d degrees %d minutes %d seconds\n", degrees, minutes, seconds);
}


int main()
{
	double p = 180 / PI;
	double a = 0, b = 0, c = 0;
	double alpha, beta, gamma;
	a = inputCheck(a);
	b = inputCheck(b);
	c = inputCheck(c);
	if ((a + b > c) && (a + c > b) && (b + c > a))
	{
		alpha = acos((a * a + b * b - c * c) / (2 * a * b)) * p;
		beta = acos((c * c + b * b - a * a) / (2 * c * b)) * (180 / PI);
		gamma = acos((a * a + c * c - b * b) / (2 * a * c)) * (180 / PI);
		dmsOutput(alpha);
		dmsOutput(beta);
		dmsOutput(gamma);
	}
	else
		printf("Triangle on these sides is impossible.");
}