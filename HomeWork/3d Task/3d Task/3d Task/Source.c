#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

unsigned int trian(float a, float b, float c)
{
	if ((a + b > c) && (a + c > b) && (b + c > a)) return 1;
	else  return 0;
}

float degree(float a, float b, float c)
{
	float deg;
	deg = (pow(a, 2) - pow(b, 2) - pow(c, 2)) / ((-2) * b * c);
	deg = acos(deg) * 180 / 3.14159265359;
	return deg;
}


int main()
{
	float a, b, c;
	int res;
	printf("%s", "Enter lengths of sides of the triangle ");
	do
	{
		res = scanf("%f%f%f", &a, &b, &c);
		if (a <= 0 || b <= 0 || c <= 0) res = 0;
		while (getchar() != '\n');
		if (res == 3) break;
		else printf("%s", "Invalid numbers entered, try again\n");
	} while (res != 3);

	if (trian(a, b, c) == 0) printf("%s\n", "Is not tiangle");
	else
	{
		float alpha_deg, alpha_min, alpha_sec, beta_deg, beta_min, beta_sec, gamma_deg, gamma_min, gamma_sec;

		alpha_deg = degree(a, b, c);
		alpha_min = (alpha_deg - floor(alpha_deg)) * 60;
		alpha_sec = (alpha_min - floor(alpha_min)) * 60;

		beta_deg = degree(b, c, a);
		beta_min = (beta_deg - floor(beta_deg)) * 60;
		beta_sec = (beta_min - floor(beta_min)) * 60;

		gamma_deg = degree(c, b, a);
		gamma_min = (gamma_deg - floor(gamma_deg)) * 60;
		gamma_sec = (gamma_min - floor(gamma_min)) * 60;

		printf("%d degree %d minutes %d seconds \n", (int)alpha_deg, (int)alpha_min, (int)alpha_sec);
		printf("%d degree %d minutes %d seconds \n", (int)beta_deg, (int)beta_min, (int)beta_sec);
		printf("%d degree %d minutes %d seconds \n", (int)gamma_deg, (int)gamma_min, (int)gamma_sec);
	}

	system("pause");
	return 0;
}
