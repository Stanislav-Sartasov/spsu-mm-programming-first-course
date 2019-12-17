#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>

int is_square_of_integer(int num)
{
	int sqrt_num = (int)sqrt(num*1.0);

	if (sqrt_num * sqrt_num == num)
	{
		return 1;
	}

	return 0;
}

int main()
{
	int number = -1;
	char str[1000];
	int praction_value, numerator_value, period, denominator_value;
	int sqrt_num;

	for (;;)
	{
		printf("Please enter a positive integer is not a square of an integer:");
		scanf("%s", str);

		number = atoi(str);

		if (number > 0 && !is_square_of_integer(number))
		{
			break;
		}
		printf("You entered incorrect integer\n");
	}

	sqrt_num = (int)floor(sqrt(number*1.0));
	printf("[%d;", sqrt_num);

	numerator_value = 1;
	period = 0;
	denominator_value = 0;
	for (;;)
	{
		period += 1;
		praction_value = sqrt_num - denominator_value;
		denominator_value = sqrt_num + praction_value;
		numerator_value = (number - praction_value * praction_value) / numerator_value;
		praction_value = (sqrt_num + praction_value) / numerator_value;
		printf("%d ", praction_value);
		if(praction_value == 2 * sqrt_num)
		{
			break;
		}
		denominator_value = denominator_value % numerator_value;
	}
	printf("]");
	printf(" %d\n", period);

	return 0;
}
