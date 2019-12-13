#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

double get_angle(double side1, double side2, double side3)
{
	double PI = 3.141592;

	return acos((side1 * side1 + side2 * side2 - side3 * side3) / (2 * side1 * side2)) / PI * 180.0;
}

int main()
{
	int sides[3] = {-1, -1, -1};
	int i;
	double angle[3], degree, minute, second;

	for (;;)
	{
		printf("Please enter three numbers( for example, enter 1, 2, 3):");
		scanf("%d %d %d", &sides[0], &sides[1], &sides[2]);

		if (sides[0] > 0 && sides[1] > 0 && sides[2] > 0)
		{
			break;
		}
		printf("You entered incorrect numbers\n");
	}

	if (sides[0] + sides[1] > sides[2] && 
		sides[1] + sides[2] > sides[0] && 
		sides[2] + sides[0] > sides[1])
	{
		printf("The numbers are possible as triangle's side.\n");

		angle[0] = get_angle(sides[0], sides[1], sides[2]);
		angle[1] = get_angle(sides[1], sides[2], sides[0]);
		angle[2] = 180.0 - angle[0] - angle[1];
		for (i = 0; i < 3; i++)
		{
			minute = modf(angle[i], &degree);
			second = modf(minute * 60, &minute);
			second = floor(second * 60);
			printf("angle(%d): %f degrees %f minutes %f seconds\n", i + 1, degree, minute, second);
		}
	}
	else
	{
		printf("The numbers are impossible as triangle's side.\n");
	}

	return 0;
}
