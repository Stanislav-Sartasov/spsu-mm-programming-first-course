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

int main()
{
	printf("This program calculates the sequence and period for chain shots.\n");
	printf("Insert number: ");
	double n;

	ins(&n);

	n = sqrt(n);
	double n0 = trunc(n);
	printf("[%d;", (int)n0);

	short f = 0;
	n = n - n0;
	int count = 0;

	while (f == 0)
		{
			n = 1 / n;
			if (trunc(n) == 2*n0)
				f = 1;
			printf(" %d", (int)trunc(n));
			count++;
			n = n - trunc(n);
		}
		printf("]\n");
		printf("Period: %d", count);
	return 0;
}