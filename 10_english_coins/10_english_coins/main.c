#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>

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

long long getInteger(long long* number)
{
	long long result = scanf("%lli", number);
	int extra = extraSymbols();

	while (result != 1 || extra == 1)
	{
		printf("Wrong input, try again:\n");
		result = scanf("%lli", number);
		extra = extraSymbols();
	}

	return *number;

}

int main()
{
	printf("Enter the number:\n");
	long long number = getInteger(&number);

	long long* options = (long long*)calloc(number + 1, sizeof(long long));
	if (options == NULL)
	{
		printf("Allocation failure.");
		exit(-1);
	}
	options[0] = 1;
	int coins[8] = { 1, 2, 5, 10, 20, 50, 100, 200 };

	for (int j = 0; j < 8; j++)
		for (int i = coins[j]; i < number + 1; i++)
			options[i] += options[i - coins[j]];

	printf("You can get %lli pence in %lli ways", number, options[number]);
	free(options);
	return 0;
}