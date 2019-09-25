#include <stdio.h>
#include <math.h>
#include "inputing.h"

int main()
{
	printf("This program for the entered number determines the continued fraction of its square root\n\n");
	int n;
	do
	{
		in_int("Enter number: ", &n);
		if (n <= 0)
			printf("positive value expected\n");
	}
	while (n <= 0);
	printf("\n");

	int whole_part = trunc(sqrt(n));

	printf("%d", whole_part);

	if (whole_part * whole_part == n)
	{
		printf("\n");
		return 0;
	}

	int t_fraction = 1;
	int b_fraction = 0; // top fraction, bottom fraction

	int t;	//intermediate variable

	for (;;)
	{
		t = whole_part - b_fraction;
		b_fraction = t + whole_part;
		t_fraction = (n - t * t) / t_fraction;

		t = b_fraction / t_fraction;
		printf(", %d", t);		

		if (t == whole_part * 2)
		{
			printf("\n\n");
			return 0;
		}

		b_fraction = b_fraction % t_fraction;
	}
}