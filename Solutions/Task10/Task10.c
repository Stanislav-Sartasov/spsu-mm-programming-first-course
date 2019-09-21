#include <stdio.h>

void in_int(char* massage, int* variable_int)	//function for int input
{
	char c;
in_int_begin:
	printf("%s", massage);
	do
	{								//handling spaces at the beginning of a line
		c = getchar();
	} while (c == ' ' || c == '\t');

	if (c == '\n')
		goto in_int_begin;		//handling empty input

	char minus;						//handling negative input
	if (c == '-')
	{
		c = getchar();
		minus = -1;
	}
	else
		minus = 1;

	while (c == ' ' || c == '\t')
		c = getchar();

	*variable_int = 0;

	while (c >= '0' && c <= '9')
	{
		if (*variable_int * 10 + c - '0' < 0)		//handling int overflow
		{
			while (getchar() != '\n');
			printf("invalid input, input is too big\n");
			goto in_int_begin;
		}
		*variable_int = *variable_int * 10 + c - '0';
		c = getchar();
	}

	while (c == ' ' || c == '\t')		//post processing and validation of input
		c = getchar();

	if (c != '\n')
	{
		while (getchar() != '\n');
		printf("invalid input\n");
		goto in_int_begin;
	}
	*variable_int *= minus;
}

int find_amount(int number, int money_biggest_type, int* money_types)
{
	if (money_biggest_type == 0)
		return 1;
	if (!number)
		return 1;
	int count = 0;
	for (int type_count = 1; money_types[money_biggest_type] * type_count <= number; type_count++)
		count += find_amount(number - money_types[money_biggest_type] * type_count, money_biggest_type - 1, money_types);
	return count;
}

int main()
{
	int money;
	printf("This program for the entered amount of money in pensions\nis determined by how many ways you can collect this amount using standard English coins\n\n");
	do
	{
		in_int("Enter amount of money: ", &money);
		if (money <= 0)
			printf("Please enter a natural value\n");
	} 
	while (money <= 0);

	int money_types[8] = { 1, 2, 5, 10, 20, 50, 100, 200 };

	long long count = 0;

	for (int i = 0; (money / money_types[i] > 0) && (i < 8); i++)
		count += find_amount(money, i, money_types);

	printf("%d\n", count);

	return 0;
}
