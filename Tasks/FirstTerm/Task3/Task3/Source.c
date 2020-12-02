#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>

const float def = 57.2958;
const float eps = 0.00001;


int check()
{
	int c, flag = 1;
	while ((c = getchar()) != '\n' && c != EOF)
		if (c != '\n' && c != ' ' && c != EOF)
			flag = 0;
	return flag;
}

void input(float* x, float* y, float* z)
{
	int result = scanf("%f%f%f", x, y, z);
	int flag = check();

	while ((flag == 0) || (result - 3) || (*x < 0) || (*y < 0) || (*z < 0))
	{
		printf("Invalid Input.Try Again.\n");
		result = scanf("%f%f%f", x, y, z);
		flag = check();
	}

}

float solve2(float a, float b, float c)
{
	return (a * a + b * b - c * c) / (2 * a * b);
}

void radToAngle(float x)
{
	float now;
	now = acos(x) * def;
	double angle, minutes, seconds;

	modf(now, &angle);
	now = (now - angle) * 60;

	modf(now, &minutes);
	now = (now - minutes) * 60;

	modf(now, &seconds);
	printf("%d Degrees\n%d Minutes\n%d Seconds\n\n", (int)angle, (int)minutes, (int)seconds);
}

int main()
{
	float side1;, side2;, side3;
	float angle1, angle2, angle3;

	input(&side1, &side2, &side3);

	if ((side1 + side2 - side3 < eps) || (side1 + side3 - side2 < eps) || (side2 + side3 - side1 < eps))
	{
		printf("It is impossible to construct a non-degenerate triangle.");
		return 0;
	}

	angle1 = solve2(side2, side3, side1);
	angle2 = solve2(side1, side3, side2);
	angle3 = solve2(side1, side2, side3);

	printf("Angle 1\n");
	radToAngle(angle1);

	printf("Angle 2\n");
	radToAngle(angle2);

	printf("Angle 3\n");
	radToAngle(angle3);
	return 0;
}