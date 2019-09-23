#include <stdio.h>
#include <math.h>
#include "inputing.h"

#define PI 3.141592653589793238462643383279502884

void dms_out(double degree)
{
	double minute = (degree - trunc(degree)) * 60;
	double second = (minute - trunc(minute)) * 60;
	printf("%d*%d'%d''\n", (int)trunc(degree), (int)trunc(minute), (int)trunc(second));
}

int main()
{
	double x, y, z;

	printf("This program, based on the three lengths of segments entered (x, y, z),\ndetermines whether a triangle with such sides can exist and if so, what angles it has\n\n");

	do
	{
		in_double("Enter x: ", &x);
		if (x < 0)
			printf("positive number expected\n");
	}
	while (x < 0);
	printf("\n");

	do
	{
		in_double("Enter y: ", &y);
		if (y < 0)
			printf("positive number expected\n");
	} 
	while (y < 0);
	printf("\n");

	do
	{
		in_double("Enter z: ", &z);
		if (z < 0)
			printf("positive number expected\n");
	} 
	while (z < 0);
	printf("\n");

	if ((x + y <= z) || (x + z <= y) || (y + z <= x) || !x || !y || !z)
	{
		printf("There is no such triangle\n");
		return 0;
	}
	
	printf("Such a triangle exists\n\n");
	
	double a, b, c;	//a = alpha, b = betta, c = gamma

	a = (acos((y * y + z * z - x * x) / (2 * y * z)) * 180) / PI;
	b = (acos((x * x + z * z - y * y) / (2 * x * z)) * 180) / PI;
	c = (acos((x * x + y * y - z * z) / (2 * x * y)) * 180) / PI;

	printf("Angle one   : ");
	dms_out(a);
	printf("Angle two   : ");
	dms_out(b);
	printf("Angle three : ");
	dms_out(c);

	return 0;
}
