#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

unsigned int Trian(float a, float b, float c)       // is possible to bild a triangle if yes isTrian = 1
{
	if ((a + b > c) && (a + c > b) && (b + c > a)) return 1;
	else  return 0;
}

void angle(float a, float b, float c)
{
	float cosa;
	cosa = (pow(a, 2) + pow(b, 2) - pow(c, 2)) / (-2)*a*b;
	printf("%f\n", acos(cosa));
}

int main()
{
	system("chcp 1251");
	float a, b, c;
	int res;
	unsigned int isTrian = 0;
	printf("%s", "Enter lengths of sides of the triangle ");
	do
	{
		res = scanf("%f%f%f", &a, &b, &c);
		if (a <= 0 || b <= 0 || c <= 0) res = 0;
		while (getchar() != '\n');
		if (res == 3) break;
		else printf("%s", "Invalid numbers entered, try again\n");
	} while (res != 3);

	if (Trian(a, b, c) == 0) printf("%s\n", "Is not tiangle");
	else
	{
		float cosa, cosb, cosc;
		cosa = (a*a - b * b - c * c) / ((-2)*b*c);
		cosa = acos(cosa) * 180 / 3.14159265359;

		cosb = (b*b - a * a - c * c) / ((-2)*a*c);
		cosb = acos(cosb) * 180 / 3.14159265359;

		cosc = (c*c - b * b - a * a) / ((-2)*b*a);
		cosc = acos(cosc) * 180 / 3.14159265359;
		printf("\n%f", cosa);
		printf("\n%f", cosb);
		printf("\n%f", cosc);
	}

	system("pause");
	return 0;
}