#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>

int extraSymbols()
{
	char symbol;
	int extra = 0;
	while ((symbol = getchar()) != '\n' && symbol != EOF)
	{
		if (symbol != ' ' && (symbol < '0' || symbol > '9'))
			extra = 1;
	}

	return extra;
}

int getPositiveInteger(int* number)
{
	int result = scanf("%d", number);
	int extra = extraSymbols();
	double intPart = 0;

	while (result != 1 || extra == 1 || *number <= 0 || modf(sqrt((double)*number), &intPart) == 0.0)
	{
		printf("Wrong input, try again:\n");
		result = scanf("%d", number);
		extra = extraSymbols();
	}

	return *number;

}

void printContinuedFraction(int number)
{
	int x = 0;
	int numerator = 1;
	int wholePart = floor(sqrt(number));
	int counter = 0;
	printf("[%d;", wholePart);
	while (1)
	{
		counter += 1;
		x = wholePart * numerator - x;
		numerator = (number - x * x) / numerator;
		wholePart = (floor(sqrt(number)) + x) / numerator;
		printf("%d ", wholePart);
		if (numerator == 1) 
			break;
	}
	printf("\b]\n");
	printf("The length of a period is %d", counter);
}

int main()
{
	int number = 0;
	printf("Please, enter one positive integer:\n");
	getPositiveInteger(&number);
	printContinuedFraction(number);
	return 0;
}
