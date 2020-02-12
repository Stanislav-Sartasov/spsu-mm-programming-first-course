#define _CRT_SECURE_NO_WARNINGS

#include <stdio.h>
#include <stdlib.h>
#include <math.h> 

void ins(double* p)
{
	char sres[15]; // inputed string
	double a; // number
	short check = 0;
	while (check == 0)
	{
		short fnum = 0;
		for (int k = 0; k < 1; k++)
		{
			scanf("%s", &sres);
			a = 0;
			for (int l = 0; sres[l] != '\0'; l++)
			{
				if (!((sres[l] >= '0') && (sres[l] <= '9')))
					check = 1;
				if (fnum == 0)
					a = a * 10 + (sres[l] - '0');
				if (sres[l] == '\0')
					fnum = 0;
			}
		}
		fnum = 1;
		for (int k = 0; k < 1; k++)
		{
			if ( (a <= 0) || ( (sqrt(a) - trunc(sqrt(a))) == 0) )
				check = 1;
		}
		if (check == 0)
			break;
		else
		{
			printf("Incorrect input, please try again.\n");
			check = 0;
		}
	}
	*p = a;
}

void main()
{
	printf("This program calculates the sequence and period for chain shots.\n");
	printf("Insert number: ");
	double num;

	ins(&num);

	int a = 0;
	int b = 1;
	int n0 = sqrt(num); // zero number in sequence
	printf("[%d;", n0);

	int n = n0; // current number in sequence
	short f = 0;
	int count = 0;

	while (f == 0)
	{
		a = n * b - a;
		b = (num - a * a) / b;
		n = (n0 + a) / b;

		if (n == 2 * n0)
			f = 1;
		count++;

		printf(" %d", n);
	}

	printf("]\n");
	printf("Period: %d", count);
}