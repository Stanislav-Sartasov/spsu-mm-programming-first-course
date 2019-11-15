#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>

int is_int_square(int n)
{
	for (int i = (int)sqrt(n) - 1; i * i <= (n + 1); i++)
		if (i * i == n)
			return 0;
	return 1;
}

int main()
{
	int check = 0, period = 0;
	int n;
	char c = 0;

	do
	{
		printf("Enter natural non-square integer number: \n");
		if (scanf("%d%c", &n, &c) && c == '\n')
		{
			if (n > 0 && is_int_square(n))
				check = 1;
			else
				printf("Please, enter natural non-square integer number!\n\n");
		}
		else
		{
			printf("Invalid input! Try again! \n\n");
			scanf("%*[^\n]");
		}
	} while (!check);

	double x = sqrt(n);
	double m = 0, s = 1, a = (int)sqrt(n);

	printf("Continued fractional representation of square root of the entered number: [%.0f; ", a);
	while(1)
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
	printf("%.0f]\n", a);
	printf("Period of quadratic irrationality: %d", period);

	return 0;
}