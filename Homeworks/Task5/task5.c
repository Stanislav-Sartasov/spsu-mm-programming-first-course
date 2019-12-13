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
	int p, q, k, d;
	double sqrt_num;

	for (;;)
	{
		printf("Please enter a positive integer is not a square of an integer:");
		scanf("%d", &number);

		if (number > 0 && !is_square_of_integer(number))
		{
			break;
		}
		printf("You entered incorrect integer\n");
	}

	sqrt_num = floor(sqrt(number*1.0));
	printf("[%d;", (int)sqrt_num);

	q = 1;
	k = 0;
	d = 0;
	for (;;)
	{
		k += 1;
		p = sqrt_num - d;
		d = sqrt_num + p;
		q = (number - p * p) / q;
		p = (sqrt_num + p) / q;
		printf("%d ", p);
		if(p == 2 * sqrt_num)
		{
			break;
		}
		d = d % q;
	}
	printf("]");
	printf(" %d\n", k);
	getchar();
	return 0;
}
