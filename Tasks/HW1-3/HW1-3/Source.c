#define M_PI 3.14159265358979323846
#include <stdio.h>
#include <math.h>


void degrees(float a, float b, float c) 
{
	printf("degenerate triangle\n");
	float alpha = acos((pow(a, 2.0) + pow(b, 2.0) - pow(c, 2.0)) / (2.0 * a * b)) * (180.0 / M_PI);
	float beta = acos((pow(a, 2.0) + pow(c, 2.0) - pow(b, 2.0)) / (2.0 * a * c)) * (180.0 / M_PI);
	float gamma = acos((pow(c, 2.0) + pow(b, 2.0) - pow(a, 2.0)) / (2.0 * c * b)) * (180.0 / M_PI);

	printf("%f deg, %f min, %f sec;\n%f deg, %f min, %f sec;\n%f deg, %f min, %f sec;", alpha, (int)alpha * 60.0, (int)alpha * 3600.0, beta, (int)beta * 60.0, (int)beta * 3600.0, gamma, (int)gamma * 60.0, (int)gamma * 3600.0);
}

int main() 
{
	printf("Enter sides of triangle to find angles : \n");
	float a, b, c;
	scanf_s("%f%f%f", &a, &b, &c);
	printf("%f %f %f\n\n", a, b, c);

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

