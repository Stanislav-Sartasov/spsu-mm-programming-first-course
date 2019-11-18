#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>
#include <stdlib.h>

int main()
{
	int period = 0;
	double main_num;

	scanf_s("%lf", &main_num);

	if (sqrt(main_num) == (int)sqrt(main_num)) 
	{
		printf("number is rational, try again\n");
		return main();
	}

	main_num = sqrt(main_num);

	int whole = main_num;
	double divider = pow(main_num - whole, -1);

	double a = main_num;

	while (1) 
	{
		int b = a;
		if (period == 0)
			printf("[%d;", b);
		else
			printf(" %d", b);
		double c = a - b;
		a = pow(c, -1);
		if ((float)a == (float)divider && period != 0)
		{
			printf("]\nperiod = %d", period);
			break;
		}

		period++;
	}
	return 0;
}