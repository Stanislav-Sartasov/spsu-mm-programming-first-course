#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int compare_sides(const void* a1, const void* a2)
{
	int * pSide1 = (int*)a1;
	int * pSide2 = (int*)a2;

	if (*pSide1 > *pSide2)
		return 1;
	if (*pSide1 == *pSide2)
		return 0;
	return -1;
}

double get_angle(double side1, double side2, double side3)
{
	double PI = 3.141592;

	return acos((side1 * side1 + side2 * side2 - side3 * side3) / (2 * side1 * side2)) / PI * 180.0;
}

int main()
{
	int sides[3] = {-1, -1, -1};
	char strs[3][1000];
	int i;
	double angle[3], degree, minute, second;
	int sum_degree, sum_minute, sum_second;

	for (;;)
	{
		printf("Please enter three numbers( for example, enter 1, 2, 3):");
		scanf("%s %s %s", strs[0], strs[1], strs[2]);

		sides[0] = atoi(strs[0]);
		sides[1] = atoi(strs[1]);
		sides[2] = atoi(strs[2]);

		qsort(sides, 3, sizeof(int), compare_sides);

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

		sum_degree = 0;
		sum_minute = 0;
		sum_second = 0;
		for (i = 0; i < 2 ; i++)
		{
			minute = modf(angle[i], &degree);
			second = modf(minute * 60, &minute);
			second = floor(second * 60);
			printf("angle(%d): %f degrees %f minutes %f seconds\n", i + 1, degree, minute, second);
			sum_second += (int)second;
			if (sum_second >= 60)
			{
				sum_second -= 60;
				sum_minute += 1;
			}
			sum_minute += (int)minute;
			if (sum_minute >= 60)
			{
				sum_minute -= 60;
				sum_degree += 1;
			}
			sum_degree += degree;
		}
		second = 60 - sum_second;
		minute = 59 - sum_minute;
		degree = 179 - sum_degree;
		if (second >= 60)
		{
			second -= 60;
			minute += 1;
		}
		if(minute >= 60)
		{
			minute -= 60;
			degree += 1;
		}
		printf("angle(%d): %f degrees %f minutes %f seconds\n", 3, degree, minute, second);
	}
	else
	{
		printf("The numbers are impossible as triangle's side.\n");
	}

	return 0;
}
