#include <stdio.h>
#include <math.h>
#define M_PI 3.14159265358979323846

void degrees(double a, double b, double c) 
{
	printf("degenerate triangle\n");
	double alpha = acos((pow(a, 2.0) + pow(b, 2.0) - pow(c, 2.0)) / (2.0 * a * b)) * (180.0 / M_PI);
	double beta = acos((pow(a, 2.0) + pow(c, 2.0) - pow(b, 2.0)) / (2.0 * a * c)) * (180.0 / M_PI);
	double gamma = acos((pow(c, 2.0) + pow(b, 2.0) - pow(a, 2.0)) / (2.0 * c * b)) * (180.0 / M_PI);

	printf("%lf deg, %lf min, %f sec;\n%lf deg, %lf min, %lf sec;\n%lf deg, %lf min, %lf sec;", alpha, alpha * 60.0, alpha * 3600.0, beta, beta * 60.0, beta * 3600.0, gamma, gamma * 60.0, gamma * 3600.0);
}

int main() 
{
	double a, b, c;
	scanf_s("%lf%lf%lf", &a, &b, &c);
	printf("%lf %lf %lf\n\n", a, b, c);

	if (a + b > c && a + c > b && c + b + a) 
	{
		degrees(a, b, c);
	} 
	else
	{
		printf("non-degenerate triangle");
	}
	return 0;
}