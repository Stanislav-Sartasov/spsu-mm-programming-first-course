#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>

int is_square(int n)
{
	for (int i = (int)sqrt(n) - 1; i * i <= (n + 1); i++)
		if (i * i == n)
			return 0;
	return 1;
}

int main()
{
	int n, period = 0;

	printf("Enter the natural number whose continued fraction you want to get\n");
	while ((!scanf_s("%d", &n)) || (is_square(n) == 0) || (n < 0))
	{
		while (getchar() != '\n');
		printf("Input Error\nEnter not square natural number\ntry again: ");
	}

	double x = sqrt(n);
	double m = 0, s = 1, a = (int)sqrt(n);

	printf("Continued fraction is: [%.0f; ", a);

	while (1)
	{
		period++;
		m = a * s - m;
		s = (n - m * m) / s;
		x = (m + sqrt(n)) / s;
		a = (int)x;
		if (s != 1 && a != 0)
			printf("%.0f, ", a);
		else
			break;
	}

	printf("%.0f] period is %d\n", a, period);

	return 0;
}