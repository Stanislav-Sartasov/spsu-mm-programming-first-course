#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>


int check()
{
	int c, flag = 1;
	while ((c = getchar()) != '\n' && c != EOF)
		if (c != '\n' && c != ' ' && c != EOF)
			flag = 0;
	return flag;
}

void input(int* x)
{
	int result = scanf("%d", x);
	int flag = check();

	while ((flag == 0) || (result - 1) || (*x < 1) || ((int)sqrt(*x) * (int)sqrt(*x) == *x))
	{
		if ((int)sqrt(*x) * (int)sqrt(*x) == *x)
			printf("You entered a square of a natural number. Repeat Input.\n");
		else
			printf("Invalid Input.Try Again.\n");
		result = scanf("%d", x);
		flag = check();
	}
}

int main()
{
	int n;
	input(&n);

	int wholePart = sqrt(n);
	printf("%d representable as a continued fraction ( %d { ", n, wholePart);

	int topFrac = 1, lowFrac = 0, nextValue;

	for (int period = 1;; period++)
	{
		nextValue = wholePart - lowFrac;
		lowFrac = nextValue + wholePart;
		topFrac = (n - nextValue * nextValue) / topFrac;
		nextValue = lowFrac / topFrac;

		printf("%d ", nextValue);
		
		if (nextValue == wholePart * 2)
		{
			printf("} ), period = %d", period);
			break;
		}
		
		lowFrac = lowFrac % topFrac;
	}
	return 0;
}